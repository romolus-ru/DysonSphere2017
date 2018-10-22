namespace SpaceConstruction.Game.Resources
{
	/// <summary>
	/// Описание ресурса
	/// </summary>
	public class ResourceInfo
	{
		public ResourcesGroupEnum ResourceGroup;
		public ResourcesEnum ResourceType;
		public string Texture;
		public string Name;
		public string Description;
		/// <summary>
		/// Коэффициент объема - сколько занимает груз по отношению к 1 кубическому метру (для объема)
		/// </summary>
		public float VolumeCoefficient;
		/// <summary>
		/// Коэфициент плотности - сколько занимает груз по отношению к 1 килограмму (для веса)
		/// </summary>
		public float DencityCoefficient;
	}
}