using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
	/// <summary>
	/// Основа для классов-моделей
	/// </summary>
	public class Model
	{
		public void Init()
		{

		}

		/// <summary>
		/// Маленькая часть работы мат модели
		/// </summary>
		/// <remarks>В основном для опроса состояния и передачи/формирования нового состояния</remarks>
		public virtual void Tick()
		{

		}

		/// <summary>
		/// Остановить модель
		/// </summary>
		public virtual void Stop()
		{

		}
	}
}
