using Engine.Data;
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

        /// <summary>
        /// Сообщение от сборщика, в основном что класс не найден
        /// </summary>
        public MsgFromCollector Msg1;
        public delegate void MsgFromCollector(string msg);

        public object GetObject(int ID)
        {
            object o = null;// создаём объект активатором или возвращаем нул
            if (_collection.ContainsKey(ID))
            {
                var type = _collection[ID];
                o = Activator.CreateInstance(type);
            }
            return o;
        }

        public Type GetObjectType(int ID)
        {
            Type type = null;// создаём объект активатором или возвращаем нул
            if (_collection.ContainsKey(ID))
            {
                type = _collection[ID];
            }
            return type;
        }

        public ushort GetClassID(object obj)
        {
            var t = obj.GetType();
            ushort ret = 0;
            foreach (var type1 in _collection)
            {
                if (t == type1.Value)
                {
                    ret = (ushort)type1.Key;
                    break;
                };
            }
            return ret;
        }

        /// <summary>
        /// получаем список классов и путей к ним и загружаем сборки и типы классов
        /// </summary>
        public void LoadClasses(List<CollectClass> collect)
        {
            foreach (var cl in collect)
            {
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
            try
            {
                assembly = Assembly.Load(assemblyName);
            }
            catch (Exception e)
            {
                var a = e.HResult;
                a++;
                // если не загрузилось то показываем что к чему
                return; // и выходим
            }
            // ищем нужные типы в объектах и сохраняем их для последующего использования
            var type = SearchType(assembly, className);
            if (type != null)
            {
                _collection.Add(ID, type);
            }
            else
                Msg1a(ID.ToString() + " " + fileName + " " + className);
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
            foreach (var assembly in assemblies)
            {
                if (!assembly.FullName.StartsWith("Engine,")) continue;
                type = SearchType(assembly, className);
                if (type != null) break;
            }
            if (type != null)
            {
                _collection.Add(ID, type);
            }
            else
                Msg1a(ID.ToString() + " " + fileName + " " + className);
        }

        public Type SearchType(Assembly assembly, string className)
        {
            Type ret = null;
            Type[] types = assembly.GetTypes();
            foreach (Type type in types)
            {
                if (type.FullName == className)
                {
                    ret = type; break;
                }
            }
            return ret;
        }

        private void Msg1a(string msg)
        {
            Msg1?.Invoke(msg);
        }

    }
}
