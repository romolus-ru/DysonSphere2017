using Engine.EventSystem.Event;
using System;
using System.Collections.Generic;

namespace DataSupport.Data
{
    public partial class MedalDescription:EventBase
    {
        public long IdMedalDescription { get; set; }
        public bool IsShowAlways { get; set; }
        public string Group { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public long ItemId { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string NameUnity { get; set; }
    }
}
