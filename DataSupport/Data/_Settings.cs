using Engine.EventSystem.Event;
using System;
using System.Collections.Generic;

namespace DataSupport.Data
{
    public partial class _Settings:EventBase
    {
        public int IdSettings { get; set; }
        public int ClassId { get; set; }
        public string TargetSubSys { get; set; }
    }
}
