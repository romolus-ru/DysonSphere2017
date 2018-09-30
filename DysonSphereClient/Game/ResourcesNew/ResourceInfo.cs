namespace DysonSphereClient.Game.ResourcesNew
{
	/// <summary>
	/// Описание ресурса
	/// </summary>
	public class ResourceInfo
	{
		public ResourcesGroupEnum Group;
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
