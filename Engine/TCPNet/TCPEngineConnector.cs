using Engine.Data;
using Engine.Enums;
using Engine.EventSystem.Event;
using Engine.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Engine.TCPNet
{
	/// <summary>
	/// Соединение для обработки информации от движка, с двоичной сериализацией
	/// </summary>
	public class TCPEngineConnector : TCPConnector
	{
		private const int NoPlayerId = -1;
		/// <summary>
		/// Идентификатор пользователя в системе сетевого соединения
		/// </summary>
		public int playerId = NoPlayerId;
		public bool Authorized = false;
		// Код операции ushort
		private const int LengthCodeOperation = 2;
		// Код класса ushort
		private const int LengthCodeClass = 2;
		// Длина информации ushort
		private const int LengthRecievedData = 2;
		public List<TCPMessage> Messages = new List<TCPMessage>();

		// (2 байта) код типа команды от пользователя или от сервера пользователю
		//    пункт назначения передаваемой информации
		// (2 байта) код пришедшего класса. если не хватит - придётся делать расширение протокола, или параметры будут содержаться в сообщении
		//    в зависимости от типа сообщения код пришедшего класса может быть зашифрован
		// (2 байта) длина класса. лучше ограничить, а то и использовать ещё меньше
		//    при соединении желательно проверять разрядность системы, упоминали что из-за разрядности могут быть нюансы

		/// <summary>
		/// Ссылка на сборщик информации о классах
		/// </summary>
		private Collector _collector;
		public void SetCollector(Collector collector)
		{
			if (_collector == null) _collector = collector;
		}

		public byte[] StrToBytes(string msg)
		{
			return Encoding.Unicode.GetBytes(msg);
		}

		/// <summary>
		/// преобразуем объект в массив байт
		/// </summary>
		/// <returns></returns>
		public byte[] ConvertToBytes(TCPOperations opCode, ushort classId, EventBase obj)
		{
			byte[] result = null;
			using (var ms = new MemoryStream()) {
				var bf = new BinaryFormatter();
				bf.Serialize(ms, obj);
				var bytes = ms.ToArray();
				// ошибку потом не выбрасывать а передавать сообщение выше - именно эта ошибка не критичная
				if (ms.Length > ushort.MaxValue) throw new Exception("Большая длина передаваемых данных " + ms.Length + " байт для объекта с кодом "
					+ classId.ToString() + " класс " + obj.GetType().ToString());
				var len = (ushort)ms.Length;
				result = BitConverter.GetBytes((ushort)opCode) //2
					.Concat(BitConverter.GetBytes(classId)) //2
					.Concat(BitConverter.GetBytes(len)) // 2
					.Concat(bytes)
					.ToArray();
			}
			return result;
		}

		/// <summary>
		/// Получить код операции
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public uint GetOpCode(byte[] data, int offset = 0)
		{
			return BitConverter.ToUInt16(data, 0 + offset);
		}

		/// <summary>
		/// Получить код класса
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public int GetClassCode(byte[] data, int offset = 0)
		{
			return BitConverter.ToInt16(data, 2 + offset);
		}

		/// <summary>
		/// Получить длину сериализованного объекта
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public ushort GetDataLength(byte[] data, int offset = 0)
		{
			return BitConverter.ToUInt16(data, 4 + offset);
		}

		/// <summary>
		/// Десериализуем в объект класса EventBase и наследников
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="obj"></param>
		/// <param name="classId"></param>
		/// <param name="data"></param>
		public void ConvertFromBytes<T>(out T obj, int classId, byte[] data, int offset = 0) where T : EventBase
		{
			using (var ms = new MemoryStream(data)) {
				var bf = new BinaryFormatter();
				ms.Seek(LengthCodeClass + LengthCodeOperation + LengthRecievedData + offset, SeekOrigin.Begin);// пропускаем начальные байты
				obj = (T)bf.Deserialize(ms);
			}
		}

		/// <summary>
		/// Получаем присланные данные и сохраняем в RecievedData
		/// </summary>
		public int ProcessData()
		{
			GetData();
			if (DataLoaded.Count < 1) return NoPlayerId;
			List<byte[]> data1;
			lock (DataLoaded) {
				data1 = new List<byte[]>(DataLoaded);
				DataLoaded.Clear();
			}
			List<TCPMessage> msgs = new List<TCPMessage>();
			foreach (var bytes in data1) {
				var offset = 0;
				do {
					var code = GetClassCode(bytes, offset);
					var opCode = GetOpCode(bytes, offset);
					var len = GetDataLength(bytes, offset);
					var obj = (EventBase)_collector.GetObject(code);
					ConvertFromBytes(out obj, code, bytes, offset);
					var msg = new TCPMessage();
					msg.PlayerId = playerId;
					msg.opCode = (TCPOperations)opCode;
					msg._msg = obj;
					msgs.Add(msg);
					offset += len + LengthCodeClass + LengthCodeOperation + LengthRecievedData;
				} while (offset < bytes.Length);
			}
			lock (Messages) Messages.AddRange(msgs);
			return playerId;
		}

		public void SendMSGData(TCPOperations opCode, EventBase msg)
		{
			var classId = _collector.GetClassID(msg);
			var bytes = ConvertToBytes(opCode, classId, msg);
			SendMsg(bytes);
		}
	}
}
