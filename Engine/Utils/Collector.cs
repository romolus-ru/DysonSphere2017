using Engine.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Engine.Utils
{
	/// <summary>
	/// Важный класс, собирает и хранит всю информацию
	/// </summary>
	public class Collector
	{
		/// <summary>
		/// Словарь кодов и типов классов
		/// </summary>
		private readonly Dictionary<int, Type> _collection = new Dictionary<int, Type>();

		public object GetObject(int id)
		{
			object o = null;// создаём объект активатором или возвращаем нул
			if (_collection.ContainsKey(id)) {
				var type = _collection[id];
				o = Activator.CreateInstance(type);
			}
			return o;
		}

		public Type GetObjectType(int id)
		{
			Type type = null;
			if (_collection.ContainsKey(id)) {
				type = _collection[id];
			}
			return type;
		}

		public ushort GetClassID(object obj)
		{
			var t = obj.GetType();
			return GetClassID(t);
		}

		public ushort GetClassID(Type type)
		{
			foreach (var type1 in _collection) {
				if (type == type1.Value) {
					return (ushort)type1.Key;
				}
			}
			return 0;
		}
		/// <summary>
		/// получаем список классов и путей к ним и загружаем сборки и типы классов
		/// </summary>
		public void LoadClasses(List<CollectClass> collect)
		{
			foreach (var cl in collect) {
				if (cl.FileName.ToLower() == "engine.dll")
					LoadClassEngine(cl.Id, cl.FileName, cl.ClassName);
				else
					LoadClass(cl.Id, cl.FileName, cl.ClassName);
				//или отдельный метод или этот же метод, но что бы использовалась текущая сборка
			}
		}

		private void LoadClass(int id, string fileName, string className)
		{
			Assembly assembly; // объявляем сборку
							   // ищем имя сборки чтоб её загрузить
			if (!File.Exists(fileName)) { return; }
			AssemblyName assemblyName = AssemblyName.GetAssemblyName(fileName);
			// пробуем загрузить
			try {
				assembly = Assembly.Load(assemblyName);
			}
			catch {
				// не загрузилось
				AddLog("not found assembly " + id + " " + fileName);
				return; // и выходим
			}
			// ищем нужные типы в объектах и сохраняем их для последующего использования
			var type = SearchType(assembly, className);
			if (type != null) {
				_collection.Add(id, type);
			} else
				AddLog("not found " + id + " " + fileName + " " + className);
		}

		/// <summary>
		/// Сохранить класс, находящийся в движке
		/// </summary>
		/// <param name="id"></param>
		/// <param name="fileName"></param>
		/// <param name="className"></param>
		private void LoadClassEngine(int id, string fileName, string className)
		{
			var assemblies = AppDomain.CurrentDomain.GetAssemblies();
			Type type = null;
			// ищем нужные типы в объектах и сохраняем их для последующего использования
			foreach (var assembly in assemblies) {
				if (!assembly.FullName.StartsWith("Engine,", StringComparison.Ordinal)) continue;
				type = SearchType(assembly, className);
				if (type != null) break;
			}
			if (type != null) {
				_collection.Add(id, type);
			} else
				AddLog("not found " + id + " " + fileName + " " + className);
		}

		public Type SearchType(Assembly assembly, string className)
		{
			Type ret = null;
			Type[] types = assembly.GetTypes();
			foreach (Type type in types) {
				if (type.FullName == className) {
					ret = type; break;
				}
			}
			return ret;
		}

		private void AddLog(string msg)
		{
			StateEngine.Log?.AddLog(msg);
		}

		public List<int> GetRegisteredSubClasses(Type baseType)
		{
			var ret = new List<int>();
			foreach (var typeRow in _collection) {
				var type = typeRow.Value;
				if (type.IsSubclassOf(baseType))
					ret.Add(typeRow.Key);
			}
			return ret;
		}
	}
}