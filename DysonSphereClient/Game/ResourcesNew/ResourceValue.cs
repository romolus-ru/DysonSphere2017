namespace DysonSphereClient.Game.ResourcesNew
{
	public class ResourceValue
	{
		public ResourcesGroupEnum ResGroup { get; } = ResourcesGroupEnum.Error;
		public ResourcesEnum ResType { get; } = ResourcesEnum.Error;
		public float Weight { get; private set; }
		public float Volume { get; private set; }
		private int _value;
		public int Value {
			get {
				return _value;
			}

			set {
				_value = value;
				if (ResInfo != null) {
					Weight = ResInfo.DencityCoefficient * value;
					Volume = ResInfo.VolumeCoefficient * value;
				}
			}
		}
		/// <summary>
		/// Описание ресурса в том числе и графическое
		/// </summary>
		public ResourceInfo ResInfo;

		public ResourceValue(ResourceInfo resourceInfo)
		{
			ResInfo = resourceInfo;
			if (ResInfo != null) {
				ResGroup = ResInfo.ResourceGroup;
				ResType = ResInfo.ResourceType;
			}
			_value = 0;
		}

		public ResourceValue GetCopy()
		{
			var r = new ResourceValue(ResInfo);
			r.Value = Value;
			return r;
		}
	}
}