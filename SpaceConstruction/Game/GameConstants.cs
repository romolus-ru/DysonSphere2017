using System;

namespace SpaceConstruction.Game
{
	/// <summary>
	/// Константы используются для определения изменяемых величин и для вывода их в информации об улучшениях
	/// </summary>
	internal static class GameConstants
	{
		#region константы для исследований
		internal const int OpenTopOrdersLevel = 2;
		internal const int ShipVolume = 80;
		internal const int ShipWeight = 80;
		internal const int ShipVolume2 = 200;
		internal const int ShipWeight2 = 200;
		internal const int AddShips1 = 4;
		internal const int AddShips2 = 3;
		internal const int AddShips3 = 2;
		internal const int AddOrders1 = 3;
		internal const int AddOrders2 = 2;
		internal const int AddOrders3 = 1;
		#endregion

		internal const int AutopilotVolumeDecrease = -35;
		internal const int AutopilotWeightDecrease = -35;

		internal const string MeasurePercent = " %";
		internal const string MeasureUnits = " ед";
		internal const string MeasureMSec = " мсек";

		internal const int CargoVolumeMaxExtra = 2300;
		internal const int CargoVolumeMaxGood = 800;
		internal const int CargoVolumeMaxNormal = 70;

		internal const int CargoWeightMaxExtra = 2300;
		internal const int CargoWeightMaxGood2 = 850;
		internal const int CargoWeightMaxGood = 800;
		internal const int CargoWeightMaxNormal = 70;

		internal const int EngineSpeedExtra = 350;
		internal const int EngineSpeedGood = 70;
		internal const int EngineSpeedNormal = 35;

		internal const int TakeOffExtra = 1900;
		internal const int TakeOffGood = 1200;
		internal const int TakeOffNormal = 800;

		internal const int LoadingExtra = 1900;
		internal const int LoadingGood = 1200;
		internal const int LoadingNormal = 800;

		internal static TimeSpan FinalOrderTimer = new TimeSpan(0, 0, minutes: 2, seconds: 0);

		internal const int OrderLevel1Reward = 7;
	}
}
