using System.Collections.Generic;

namespace Submarines.Items
{
	/// <summary>
	/// Стоимостной контейнер - и деньги игрока и стоимость вещей хранятся в таком контейнере
	/// (не обязательно currency, некоторые вещи можно купить за уважение или за наличие определенных предметов)
	/// </summary>
	/// <remarks>Для лута похожий тип контейнера. но он не поддерживает операции сравнения</remarks>
	internal class ItemsCostContainer
	{
		private static int _counter;
		private string _costStr;

		/// <summary>
		/// Создать контейнер стоимости
		/// </summary>
		public void Init(string costStr, List<ItemBase> items)
		{

		}
	}
}