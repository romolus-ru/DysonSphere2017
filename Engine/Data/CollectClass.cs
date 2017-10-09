using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Data
{
    /// <summary>
    /// Загружаемый класс
    /// </summary>
    /// <remarks>Аналог ProtocolClasses но совсем на него не похож</remarks>
    public class CollectClass
    {
        public int IdFilesClasses;
        public string FileName;
        public string ClassName;

        public CollectClass(int ID, string FileName, string ClassName)
        {
            this.IdFilesClasses = ID;
            this.FileName = FileName;
            this.ClassName = ClassName;
        }
    }
}
