using Engine.Enums;
using Engine.EventSystem.Event;
using System;
using System.Collections.Generic;

namespace Engine.Data
{
	public partial class UserRegistration : EventBase
	{
		public long UserId { get; set; }
		/// <summary>
		/// �������� ������������� ��� ������������ ��� ������ ������� ����
		/// </summary>
		public string UserGUID { get; set; }
		/// <summary>
		/// ��� ������ ������ � ����
		/// </summary>
		public string NickName { get; set; }
		/// <summary>
		/// ����������� ���, ������� ����� �������������� ��������������
		/// </summary>
		public string OfficialName { get; set; }
		/// <summary>
		/// ����� ��� ������������� � ��������������
		/// </summary>
		public string Mail { get; set; }
		/// <summary>
		/// ������
		/// </summary>
		/// <remarks>
		/// ������������ ������ ������, ������ �������� � ������������ �� ������. ��� �� ������������� � ������������/����������� � ��� ��� ����
		/// ������ ������� ������ ����� ����������� ������ � ��������� ��������
		/// </remarks>
		public string HSPassword { get; set; }
		/// <summary>
		/// ���� ������������
		/// </summary>
		public Role UserRole { get; set; }
	}
}
