using Engine.EventSystem.Event;
using System;
using System.Collections.Generic;

namespace Engine.Data
{
    public partial class MiniGameDescription_old:EventBase
    {
        public long IdMiniGameDescription { get; set; }
        public long Version { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public System.DateTime LastUpdate { get; set; }
        public string Genre { get; set; }
        public string Pic1 { get; set; }
        public string Pic2 { get; set; }
        public string Pic3 { get; set; }
        public string MainFile { get; set; }
        public string MainClass { get; set; }
        public Nullable<long> PackId { get; set; }
    }
}
