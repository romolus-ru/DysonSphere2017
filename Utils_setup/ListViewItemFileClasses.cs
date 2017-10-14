using Engine.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Utils_setup
{
	class ListViewItemFileClasses:ListViewItem
	{
		public CollectClass Collect1;
		/// <summary>
		/// Присутствует ли класс в DLL - иногда классы могут быть удалены из проекта
		/// </summary>
		public bool FoundInFile;
		/// <summary>
		/// Ид в базе
		/// </summary>
		public int id = -1;
		public string InDB { get {
				return !FoundInFile ? "Not exist" :
					id == -1 ? "-" : "present";
			} }
		public string FileName;
		public string ClassName;

		public ListViewItemFileClasses(string fileName, Type type)
		{
			FoundInFile = true;
			FileName = fileName;
			ClassName = type.FullName;
			RefreshInfo();
		}
		public ListViewItemFileClasses(CollectClass collect)
		{
			FoundInFile = false;
			Collect1 = collect;
			FileName = collect.FileName;
			ClassName = collect.ClassName;
			RefreshInfo();
		}

		public void InitWithDataFromDB(CollectClass collect)
		{
			Collect1 = collect;
			id = collect.Id;
			RefreshInfo();
		}

		public void RefreshInfo()
		{
			Text = "";
			SubItems.Clear();
			Text = InDB;
			SubItems.Add(FileName);
			SubItems.Add(ClassName);
		}
	}
}
