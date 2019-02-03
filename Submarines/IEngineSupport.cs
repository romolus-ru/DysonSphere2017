namespace Submarines
{
	/// <summary>
	/// Позволяет взаимодействовать двигателю с объектом и учитывать его текущие параметры
	/// </summary>
	public interface IEngineSupport
	{
		/// <summary>
		/// Текущая скорость
		/// </summary>
		float VCurrent { get; }
		/// <summary>
		/// Текущая масса
		/// </summary>
		float Mass { get; }
		/// <summary>
		/// Выставленная мощность двигателя
		/// </summary>
		float EnginePercent { get; }
		/// <summary>
		/// Коэффициент сопротивления силе движения
		/// </summary>
		float OpposingCoefficient { get; }
	}
}
