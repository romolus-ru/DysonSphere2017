﻿using Engine.Data;
using Engine.Enums;
using Engine.EventSystem.Event;
using Engine.TCPNet;
using Engine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Engine.Models
{
	public class TCPClientModel : Model
	{
		private TCPClient _tcpClient;
		private bool _stopTCPClient = false;
		/// <summary>
		/// Обработать клиентов, у которых есть необработанные данные
		/// </summary>
		public Action<List<TCPMessage>> OnProcessPlayer;
		/// <summary>
		/// Установлено ли соединение с сервером
		/// </summary>
		public bool IsConnected { get; private set; } = false;

		public TCPClientModel(Collector collector)
		{
			_tcpClient = new TCPClient();
			_tcpClient.SetCollector(collector);
		}

		public void Connect(string server, int serverPort)
		{
			_tcpClient.Init();
			_tcpClient.ConnectToServer(server, serverPort);
			IsConnected = true;
			StateClient.ConnectionState = true;
			new Thread(() => ProcessData()).Start();
		}

		public void SendMessage(TCPOperations opCode, EventBase msg)
		{
			_tcpClient.SendMSGData(opCode, msg);
		}

		private HashSet<int> PlayerId = new HashSet<int>();
		public override void Tick()
		{
			List<TCPMessage> messages = null;
			lock (_tcpClient.Messages) {
				if (_tcpClient.Messages.Count == 0) return;
				messages = new List<TCPMessage>(_tcpClient.Messages);
				_tcpClient.Messages.Clear();
			}
			// отправляем на обработку
			OnProcessPlayer?.Invoke(messages);
		}

		public override void Stop()
		{
			_stopTCPClient = true;
		}

		private void ProcessData()
		{
			while (!_stopTCPClient)
				_tcpClient.ProcessData();
		}
	}
}