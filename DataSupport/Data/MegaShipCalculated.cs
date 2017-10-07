using Engine.EventSystem.Event;
using System;
using System.Collections.Generic;

namespace DataSupport.Data
{
    public partial class MegaShipCalculated:EventBase
    {
        public long MegaShipId { get; set; }
        public System.DateTime Data { get; set; }
        public long Hull { get; set; }
        public long Attack { get; set; }
        public long Speed { get; set; }
        public long Armor { get; set; }
        public long Mobility { get; set; }
        public long Precision { get; set; }
        public long Protection { get; set; }
        public long Recharge { get; set; }
        public string ListPlayers { get; set; }
        public long RocketDistance { get; set; }
        public long RocketDamage { get; set; }
        public long RocketAmmo { get; set; }
        public long RocketRecharge { get; set; }
        public System.TimeSpan PocketShootPause { get; set; }
        public long RocketCost { get; set; }
        public long CloseDistance { get; set; }
        public long CloseDamage { get; set; }
        public long CloseAmmo { get; set; }
        public long CloseRecharge { get; set; }
        public System.TimeSpan CloseShootPause { get; set; }
        public long CloseCost { get; set; }
        public long LaserDistance { get; set; }
        public long LaserDamage { get; set; }
        public long LaserAmmo { get; set; }
        public long LaserRecharge { get; set; }
        public System.TimeSpan LaserShootPause { get; set; }
        public long LaserCost { get; set; }
        public long TeleportDistance { get; set; }
        public long TeleportDamage { get; set; }
        public long TeleportAmmo { get; set; }
        public long TeleportRecharge { get; set; }
        public System.TimeSpan TeleportShootPause { get; set; }
        public long TeleportCost { get; set; }
        public long Wenergy { get; set; }
        public long DefCost { get; set; }
        public long DefRecharge { get; set; }
        public System.TimeSpan DefShootPause { get; set; }
        public long DefAmmo { get; set; }
        public long DefQuality { get; set; }
        public long Shield { get; set; }
        public long ShieldCost { get; set; }
        public long ShieldTime { get; set; }
    }
}
