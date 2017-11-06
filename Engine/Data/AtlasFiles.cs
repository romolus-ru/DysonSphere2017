using Engine.EventSystem.Event;
using System;
using System.Collections.Generic;

namespace Engine.Data
{
    public class AtlasFiles:EventBase
    {
        public long IdAtlasFile { get; set; }
        public string AtlasName { get; set; }
        public string AtlasFile { get; set; }
        public long Width { get; set; }
        public long Height { get; set; }
    }
}
