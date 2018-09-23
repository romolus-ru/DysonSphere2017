using Engine.Utils;

namespace Engine
{
	/// <summary>
	/// Оперирует Jint
	/// </summary>
	/// <remarks>Регистрируем метод и потом по имени его вызываем</remarks>
	public class JintController
	{
		private Jint.Engine _JintEngine;
		public JintController()
		{
			_JintEngine = new Jint.Engine();
		}

		public void LoadFunction(string fileName)
		{
			var txt=FileUtils.LoadStringFromFile(fileName);
			_JintEngine.Execute(txt);
		}

		public string RunFunction<T>(string functionName, T param)
		{
			return _JintEngine.Invoke(functionName, param).ToString();
		}
	}
}