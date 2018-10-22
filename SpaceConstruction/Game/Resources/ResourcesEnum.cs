namespace SpaceConstruction.Game.Resources
{
	public enum ResourcesEnum
	{
		Error = 0,
		// Конструкционные материалы (древесина, камень бетон, кирпич, сталь)
		StrWood,
		StrStone,
		StrConcrete,
		StrBrick,
		StrSteel,

		// Материалы для отделки - плитка, обои, штукатурка, шпаклевка, стекло, краска, ленолиум, фарфор
		/// <summary>
		/// Плитка
		/// </summary>
		DecTile,
		/// <summary>
		/// Обои
		/// </summary>
		DecPaper,
		/// <summary>
		/// Штукатурка
		/// </summary>
		DecPlaster,
		/// <summary>
		/// Шпаклевка
		/// </summary>
		DecPutty,
		/// <summary>
		/// Стекло
		/// </summary>
		DecGlass,
		/// <summary>
		/// Краска
		/// </summary>
		DecPaint,
		/// <summary>
		/// Линолеум
		/// </summary>
		DecLinoleum,
		/// <summary>
		/// Фарфор
		/// </summary>
		DecPorcelain,

		// Мебель, сантехника, посуда, кухня
		/// <summary>
		/// Мебель
		/// </summary>
		FurFurniture,
		/// <summary>
		/// Сантехника
		/// </summary>
		FurPlumbing,
		/// <summary>
		/// Посуда
		/// </summary>
		FurDishes,
		/// <summary>
		/// Кухня
		/// </summary>
		FurKitchen,

		// Механизмы (лопаты, инструменты для отделки, эскаватор, кран, сваезабивалка)
		/// <summary>
		/// Лопаты
		/// </summary>
		MechShovels,
		/// <summary>
		/// Инструменты для отделки
		/// </summary>
		MechFinishingTools,
		/// <summary>
		/// Эскаватор
		/// </summary>
		MechExcavator,
		/// <summary>
		/// Кран
		/// </summary>
		MechCrane,
		/// <summary>
		/// Сваезабивалка
		/// </summary>
		MechPileDriving,
	}
}