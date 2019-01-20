using System;
using System.Runtime.InteropServices;
using Engine.Data;

namespace Submarines
{
	internal class Ship
	{
		// TODO двигатель поместить в отдельный класс который будет контролировать текущую скорость двигателя
		// TODO продумать как будут сохраняться айтемы - айтемы можно будет апгрейдить и покупать с разными характеристиками
		// но будут и такие же как и в SpaceConstruction - изменяющиеся только количественно

		// https://nptel.ac.in/courses/108103009/3
		// dV = Fengine-Ftotalresistance/(sigma*Mass)*dt
		// F[Nm] sigma - koefficient of inertia

		// дельта т взять из констант движка. добавить механизм проверки чтоб период был точно таким же
		// иначе придётся поместить корабль в другой поток (как минимум двигатель)

		public int Mass { get; private set; }
		private float _engineKoefficient = 0;
		public int EnginePercent { get; private set; }
		public int EnginePercentMax { get; private set; }
		/// <summary>
		/// Допустимая обратная скорость (обычно меньше чем скорость вперед)
		/// </summary>
		public int EnginePercentMin { get; private set; }
		public Vector CurrentVector { get; private set; }
		public Vector CurrentDeltaVector { get; private set; }
		private DateTime _currentTime;

		public Ship()
		{
			EnginePercentMax = 150;
			EnginePercentMin = -50;
			_currentTime = DateTime.Now;
			Mass = 1000000;
			AddSpeed(5);
		}

		public void AddSpeed(int delta)
		{
			EnginePercent += delta;
			if (EnginePercent > EnginePercentMax)
				EnginePercent = EnginePercentMax;
			if (EnginePercent < EnginePercentMin)
				EnginePercent = EnginePercentMin;
			_engineKoefficient = EnginePercent / 100f;
		}

		public void StopEngine() => EnginePercent = 0;

		public void CalculateMovement()
		{
			// попробовать использовать это, задача 5
			//http://www.teoretmeh.ru/dinamika1.htm
			//http://phys-portal.ru/examples/dinamika_prz.htm
			// момент инерции шара - тоже может пригодиться http://ru.solverbook.com/spravochnik/fizika/moment-inercii-shara/
			// книга - https://phys.bspu.by/static/um/phys/meh/1mehanika/pos/glava05/5_2.pdf
			// да и вообще по ссылке ниже там много задач разных
			// http://easyfizika.ru/zadachi/dinamika/telo-massoj-100-kg-dvizhetsya-po-gorizontalnoj-poverhnosti-pod-dejstviem-sily/

			//тут
			// попробовать вычислить. потом подставить нужные константы и вычислить. и провверить обратно - какая скорость получится при заданных параметрах
			// http://easyfizika.ru/zadachi/dinamika/kakuyu-srednyuyu-moshhnost-i-silu-tyagi-dolzhen-razvivat-elektrovoz-chtoby-sostav-massoj/

			// mv2+mvkgt-2Nt=0;

			var enginePower = 10000000;// мощность двигателя
			var kT = 0.001f;// коэффициент трения
			var g = 9.8f;// ускорение свободного падения
			float dt = (DateTime.Now - _currentTime).Milliseconds / 1000f;

			var n = enginePower * _engineKoefficient;

			float a = Mass;
			float b = Mass * kT * g * dt;
			float c = 2 * n * dt;

			float d = b * b + 4f * a * c;
			float x1 = (-b + (float) Math.Sqrt(d)) / (2f * a);

			var vcur = CurrentVector.Z;
			CurrentVector = new Vector(0, 0, vcur + x1);


			//float m = 10 * 10 * 10 * 10 * 10 * 10;
			////var a1 = (216000f * 120 / m) - 120 * 0.005 * 9.8;
			//float b1 = m * 0.005f * 9.8f * 120;
			//float d1 = b1 + 8 * m * 120 * 2160000f;

			//float k1 = (-b1 + (float)Math.Sqrt(d1)) / (2 * m);
			//float k2 = (-b1 - (float)Math.Sqrt(d1)) / (2 * m);

			// импульс тела p=m*v.  уравнение движения тела p-p0=F*dt => F=m*(v-v0)/dt 

			//var fEngine = Vector.Forward() * EnginePercent;
			//var fResistance = Vector.Back() * (0.01f * (1 + CurrentVector.Z / 160));
			//float dt = (DateTime.Now - _currentTime).Milliseconds / 1000f;

			//CurrentDeltaVector = dt * (fEngine + fResistance) / (2 / 5f * Mass);

			//CurrentVector += CurrentDeltaVector;


			//var f = 100;//EnginePercent / enginePower;// сила двигателя
			//var a = f * dt / Mass;// ускорение
			//CurrentVector += new Vector(0, 0, a);

			//var v = CurrentVector.Z;// текущая скорость
			//var f = EnginePercent == 0 
			//	? 0 
			//	: 1f * enginePower / EnginePercent;// сила двигателя
			//var a = f / Mass;// ускорение

			//v = v * kT + a * dt;
			//CurrentVector = new Vector(0, 0, v);

			_currentTime = DateTime.Now;
		}
	}
}