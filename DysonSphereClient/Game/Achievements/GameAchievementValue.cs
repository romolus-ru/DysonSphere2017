using Engine.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Engine;
using System.Diagnostics;

namespace DysonSphereClient.Game
{
	[DebuggerDisplay("{Achieve.Code}")]
	public class GameAchievementValue
	{
		public AchieveDescription Achieve;
		/// <summary>
		/// Есть условия для активации ачивки и подключения установлены
		/// </summary>
		public bool IsActive;
		/// <summary>
		/// Ачивка достигнута
		/// </summary>
		public bool IsAchieved;
		public float Value;
		/// <summary>
		/// Временные значения, например для определения нескольких разных событий - типа выполнили 5 разных квестов
		/// </summary>
		public string Values;

		/// <summary>
		/// Сообщаем что ачивка достигнута
		/// </summary>
		public Action<GameAchievementValue> OnAchieved;
		/// <summary>
		/// Объект от которого получается событие для ачивки
		/// </summary>
		public object WaitObject;
		/// <summary>
		/// Член класса (Action) который присылает нужное событие
		/// </summary>
		public MemberInfo WaitMember;
		/// <summary>
		/// Объект которому передаётся значение установлена ачивка или нет
		/// </summary>
		public object OutObject;
		/// <summary>
		/// Член класса (метод) который получит информацию об ачивке
		/// </summary>
		public MemberInfo OutMember;

		/// <summary>
		/// Сохраняем объекты для будущего подключения/отключения
		/// </summary>
		public void StoreWaitParams(object waitObject, MemberInfo waitMember)
		{
			if (IsAchieved) return;
			WaitObject = waitObject;
			WaitMember = waitMember;
		}

		/// <summary>
		/// Сохраняем объекты для будущего подключения/отключения
		/// </summary>
		public void StoreOutParams(object outObject, MemberInfo outMember)
		{
			if (IsAchieved) return;
			OutObject = outObject;
			OutMember = outMember;
		}


		/// <summary>
		/// Установить нужные праметры, подключиться к событиям и т.п.
		/// </summary>
		/// <param name="achievements"></param>
		public void Setup(GameAchievements achievements)
		{
			if (IsAchieved) return;
			IsActive = CanActivateAchievement(achievements);
			if (!IsActive) return;
			OnAchieved += achievements.Achieved;
			LinkToObject();
		}

		/// <summary>
		/// Проверяем, можно ли активировать ачивку
		/// </summary>
		/// <param name="achievements"></param>
		/// <returns></returns>
		private bool CanActivateAchievement(GameAchievements achievements)
		{
			// в данном случае проверяем на основе отсутствия и/или открытости предыдущей ачивки
			var prev = Achieve.PreviousAchievements;
			if (string.IsNullOrEmpty(prev)) return true;
			var prevs = prev.Split(Constants.BaseStringSeparator);
			var prevAchieved = true;// все предыдущие ачивки получены
			foreach (var achCode in prevs) {
				var ach = achievements.GetAchievement(achCode);
				if (ach == null)
					throw new Exception("В ачивках нету ачивки с кодом " + achCode);
				if (!ach.IsAchieved) {
					prevAchieved = false;
					break;
				}
			}
			return prevAchieved;
		}

		/// <summary>
		/// Убрать все соединения
		/// </summary>
		public void Clear()
		{
			UnlinkFromObject();
			WaitMember = null;
			WaitObject = null;
			OutMember = null;
			OutObject = null;
		}

		/// <summary>
		/// Устанавливаем присланному обработчику (Action) метод для обработки ачивок
		/// </summary>
		private void LinkToObject()
		{
			if (WaitMember == null || WaitMember.MemberType != MemberTypes.Field) return;
			var f = WaitMember as FieldInfo;
			Action action = f.GetValue(WaitObject) as Action;
			action += OnAchieveActionStarted;
			f.SetValue(WaitObject, action);
		}

		private void UnlinkFromObject()
		{
			if (WaitMember == null) return;
			if (WaitMember.MemberType != MemberTypes.Field) return;
			var f = WaitMember as FieldInfo;
			Action action = f.GetValue(WaitObject) as Action;
			action -= OnAchieveActionStarted;
			f.SetValue(WaitObject, action);
		}

		private void OnAchieveActionStarted()
		{
			AchievementGranted();
		}

		private void AchievementGranted()
		{
			IsAchieved = true;
			IsActive = false;
			OnAchieved?.Invoke(this);
			if (OutMember != null && OutMember.MemberType == MemberTypes.Method) {
				var method = OutMember as MethodInfo;
				method.Invoke(OutObject, new object[] { IsAchieved });
			}
			Clear();
		}
	}
}