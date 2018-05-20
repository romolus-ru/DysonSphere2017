using Engine.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Engine;

namespace DysonSphereClient.Game
{
	public class GameAchievementValue
	{
		public AchieveDescription Achieve;
		public long Id;
		public bool IsTutorialAchievement;// тип ачивки - обычная или туториальная
		public int RequiredSequenceLevel;// енум для условия вывода ачивки
		public int SequenceId;// вспомогательный код для енума
		public int Step;// вспомогательный код для енума
		//или лучше сделать общий класс и уже каждый тип енума условия и т.п. сделать дополнительно - они будут определять видна ли ачивка или нет
		public string Code;
		/// <summary>
		/// Есть условия для активации ачивки и подключения установлены
		/// </summary>
		public bool IsActive;
		/// <summary>
		/// Ачивка достигнута
		/// </summary>
		public bool IsAchieved;
		public float ValueMax;
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
		/// Объект для которого делается эта ачивка
		/// </summary>
		public object LiveObject;
		/// <summary>
		/// Член класса связанный с ачивкой
		/// </summary>
		public MemberInfo LiveMember;

		/// <summary>
		/// Сохраняем объекты для будущего подключения/отключения
		/// </summary>
		public void StoreParams(object liveObject, MemberInfo liveMember)
		{
			if (IsAchieved) return;
			LiveObject = liveObject;
			LiveMember = liveMember;
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
			OnAchieved += achievements.OnAchieved;
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
			LiveMember = null;
			LiveObject = null;
		}

		/// <summary>
		/// Устанавливаем присланному обработчику (Action) метод для обработки ачивок
		/// </summary>
		private void LinkToObject()
		{
			if (LiveMember.MemberType != MemberTypes.Field) return;
			var f = LiveMember as FieldInfo;
			Action action = f.GetValue(LiveObject) as Action;
			action += OnAchieveActionStarted;
			f.SetValue(LiveObject, action);
		}

		private void UnlinkFromObject()
		{
			if (LiveMember.MemberType != MemberTypes.Field) return;
			var f = LiveMember as FieldInfo;
			Action action = f.GetValue(LiveObject) as Action;
			action -= OnAchieveActionStarted;
			f.SetValue(LiveObject, action);
		}

		private void OnAchieveActionStarted()
		{
			AchievementGranted();
		}

		private void AchievementGranted()
		{
			IsAchieved = true;
			OnAchieved?.Invoke(this);
		}
	}
}