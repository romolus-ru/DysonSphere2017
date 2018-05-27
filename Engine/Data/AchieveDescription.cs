using Engine.Enums;
using Engine.EventSystem.Event;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Engine.Data
{
	[DebuggerDisplay("Code={Code} Group={Group} Description={Description}")]
    public class AchieveDescription:EventBase
    {
        public long IdAchieve { get; set; }
        public string Group { get; set; }
        public long Count { get; set; }
		/// <summary>
		/// ��������� ��� ������
		/// </summary>
		public string Code { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public AchievementClassTypeEnum Type { get; set; }
        public string DescriptionReward { get; set; }
		/// <summary>
		/// ���������� ������, ������� ������ ���� ������� ��� ��������� ���� (���� ��������� �� ����� ����� � �������
		/// </summary>
		public string PreviousAchievements { get; set; }
        public bool Private { get; set; }
        public string ValuePath { get; set; }
        public long ItemId { get; set; }
    }
}
