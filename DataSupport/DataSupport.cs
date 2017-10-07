using DataSupport.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSupport
{
    /// <summary>
    /// Основной класс. Организует доступ к данным
    /// </summary>
    public class DataSupport
    {
        public virtual List<AtlasFiles> AtlasFilesGetAll()
        {
            return null;
        }

        public virtual void SetLog(Action<string> log1)
        {

        }

    }
}
