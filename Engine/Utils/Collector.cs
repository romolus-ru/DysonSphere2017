﻿using Engine.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

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

		public object GetObject(int ID)
		{
			object o = null;// создаём объект активатором или возвращаем нул
			if (_collection.ContainsKey(ID)) {
				var type = _collection[ID];
				o = Activator.CreateInstance(type);
			}
			return o;
		}

		public Type GetObjectType(int ID)
		{
			Type type = null;
			if (_collection.ContainsKey(ID)) {
				type = _collection[ID];
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

		private void LoadClass(int ID, string fileName, string className)
		{
			Assembly assembly; // объявляем сборку
							   // ищем имя сборки чтоб её загрузить
			if (!File.Exists(fileName)) { return; }
			AssemblyName assemblyName = AssemblyName.GetAssemblyName(fileName);
			// пробуем загрузить
			try {
				assembly = Assembly.Load(assemblyName);
			}
			catch (Exception e) {
				var a = e.HResult;
				a++;
				// если не загрузилось то показываем что к чему
				return; // и выходим
			}
			// ищем нужные типы в объектах и сохраняем их для последующего использования
			var type = SearchType(assembly, className);
			if (type != null) {
				_collection.Add(ID, type);
			} else
				AddLog("not found " + ID.ToString() + " " + fileName + " " + className);
		}

		/// <summary>
		/// Сохранить класс, находящийся в движке
		/// </summary>
		/// <param name="ID"></param>
		/// <param name="fileName"></param>
		/// <param name="className"></param>
		private void LoadClassEngine(int ID, string fileName, string className)
		{
			var assemblies = AppDomain.CurrentDomain.GetAssemblies();
			Type type = null;
			// ищем нужные типы в объектах и сохраняем их для последующего использования
			foreach (var assembly in assemblies) {
				if (!assembly.FullName.StartsWith("Engine,")) continue;
				type = SearchType(assembly, className);
				if (type != null) break;
			}
			if (type != null) {
				_collection.Add(ID, type);
			} else
				AddLog("not found " + ID.ToString() + " " + fileName + " " + className);
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