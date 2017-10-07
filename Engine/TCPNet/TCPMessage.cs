using Engine.EventSystem.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.TCPNet
{
    /// <summary>
    /// Сообщение от сервера/ от клиента
    /// </summary>
    /// <remarks>Содержит код операции и присланный класс</remarks>
    public class TCPMessage
    {
        /// <summary>
        /// Код операции
        /// </summary>
        public uint opCode = 0;
        public EventBase _msg = null;
    }
}
