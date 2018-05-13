using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DysonSphereClient.Game
{
	/// <summary>
	/// Для вывода одного ресурса в виде строки (Количество+Иконка)
	/// </summary>
	public class ResourceViewInfo
	{
		public ResourcesEnum Resource;
		public String ResourceTexture;
		public int Amount;
	}
}