namespace Submarines.Items
{
	/// <summary>
	/// Хранит ссылку на предмет и дополнительно содержит дополнительные характеристики
	/// </summary>
	internal class ItemHolder
	{
		/// <summary>
		/// Предмет
		/// </summary>
		public ItemBase Item { get; private set; }

		public int Count { get; private set; }



	}
}