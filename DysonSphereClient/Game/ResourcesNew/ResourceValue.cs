namespace DysonSphereClient.Game.ResourcesNew
{
	public class ResourceValue
	{
		public ResourcesGroupEnum ResGroup { get { return ResInfo != null ? ResInfo.Group : ResourcesGroupEnum.Error; } }
		public float Weight { get { return ResInfo != null ? ResInfo.DencityCoefficient * Value : 0; } }
		public float Volume { get { return ResInfo != null ? ResInfo.VolumeCoefficient * Value : 0; } }

		public int Value;
		/// <summary>
		/// Описание ресурса в том числе и графическое
		/// </summary>
		public ResourceInfo ResInfo;
		public ResourceValue GetCopy()
		{
			var r = new ResourceValue();
			r.ResInfo = ResInfo;
			r.Value = Value;
			return r;
		}
	}
}