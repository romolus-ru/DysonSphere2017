using Engine.EventSystem.Event;

namespace Engine.Data
{
	/// <summary>
	/// Инициализация последовательности случайных чисел с заданными свойствами
	/// </summary>
	public partial class RandomSequences : EventBase
	{
		public int IdSequenceParams { get; set; }
		public long Seed { get; set; }
		/// <summary>
		/// Пропускаем столько значений перед сохранением последовательности
		/// </summary>
		public long Shift { get; set; }
		/// <summary>
		/// Длина последовательности
		/// </summary>
		public int Length { get; set; }
		/// <summary>
		/// Нижняя граница последовательности (обычно 0)
		/// </summary>
		public int LimMin { get; set; }
		/// <summary>
		/// Максимальное значение последовательности (в последовательности значения меньше чем это)
		/// </summary>
		public int LimMax { get; set; }
		/// <summary>
		/// Код генератора (примерно как версия или используемый генератор самопальный)
		/// </summary>
		public long GeneratorCode { get; set; }
		/// <summary>
		/// Имя последовательности если она какая то особенная
		/// </summary>
		public string SequenceName { get; set; }
	}
}