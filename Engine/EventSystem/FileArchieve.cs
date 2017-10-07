using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.EventSystem
{
    /// <summary>
    /// Работа с архивным файлом (zip, поддерживается .net по умолчанию)
    /// </summary>
    public class FileArchieve : IDisposable
    {
        /// <summary>
        /// Открываемый архив
        /// </summary>
        private FileStream _zipToOpen;

        private ZipArchive _archive;

        /// <summary>
        /// Сброшена ли информация на диск
        /// </summary>
        private bool _disposed;

        /// <summary>
        /// Всё равно "только для чтения", так что изменить эту переменную просто так не получится
        /// </summary>
        public ReadOnlyCollection<ZipArchiveEntry> Files;


        public FileArchieve(string fileName, bool createMode = true)
        {
            _zipToOpen = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            if (createMode)
            {
                _archive = new ZipArchive(_zipToOpen, ZipArchiveMode.Create);
                Files = null;// нельзя обращаться к entities в момент создания
            }
            else
            {
                _archive = new ZipArchive(_zipToOpen, ZipArchiveMode.Read);
                Files = _archive.Entries;
            }
        }

        /// <summary>
        /// Добавить поток к архиву
        /// </summary>
        /// <param name="fName"></param>
        /// <param name="ms"></param>
        public void AddStream(string fName, MemoryStream ms)
        {
            ZipArchiveEntry fileEntry = _archive.CreateEntry(fName);
            using (var s = fileEntry.Open())
            {
                ms.WriteTo(s);
            }
        }

        /// <summary>
        /// Получить файл из архива как поток
        /// </summary>
        /// <param name="fName"></param>
        /// <returns>поток или null</returns>
        public MemoryStream GetStream(string fName)
        {
            MemoryStream ms = null;
            foreach (ZipArchiveEntry entry in _archive.Entries)
            {
                if (entry.FullName == fName)
                {
                    ms = new MemoryStream();
                    var stream = entry.Open();
                    stream.CopyTo(ms);
                    ms.Seek(0, SeekOrigin.Begin);
                    stream.Close();
                    break;
                }
            }
            return ms;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (_archive != null) _archive.Dispose();
                _archive = null;
                try
                {
                    //_zipToOpen.Flush();
                    _zipToOpen.Close();
                    _zipToOpen.Dispose();
                    _zipToOpen = null;
                }
                catch (Exception e)
                {
                    throw new Exception("Ошибка в классе FileArchieve " + e.Message);
                }
                _disposed = true;
            }
        }


        public void Dispose()
        {
            Dispose(true);
        }

        ~FileArchieve()
        {
            Dispose(false);
        }

        /// <summary>
        /// Записываем тестовую строку
        /// </summary>
        public void AddString(MemoryStream ms, string s)
        {
            var enc = Encoding.UTF8;
            var bytes = enc.GetBytes(s);
            ms.Write(bytes, 0, bytes.Length);
        }

    }
}