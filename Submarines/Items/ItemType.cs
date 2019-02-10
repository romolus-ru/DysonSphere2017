namespace Submarines.Items
{
	/// <summary>
	/// Тип предмета (оружие, корпус, и т.п.)
	/// </summary>
	internal enum ItemType
	{
		Unknown,
		NonTypical,
		Submarine,// описание целой подлодки, которую можно купить в магазине. после покупки можно поменять составляющие
		Hull,// он же корпус
		Engine,
		ManeuverDevice,
		Radar,
		Radio,
		Currency,
		EnergyShield,
		ActiveShield,

		EngineUpgrade,
		EngineBullet,
		EngineRocket,
		EngineTorpedo,


		Weapon,// для пуль
		RocketLauncher,
		TorpedoLauncher,
		Bullet,
		Rocket,
		Torpedo,
		Cargo,// для перевозки между городами
		QuestCargo,// для квестов
		CollectionCargo,// коллекционный предмет, просто чтоб на полке стоял
		PortRoomItem,// предмет для расположения в порту
		PortRoomItemUpgrade,// улучшение для комнаты

	}
}