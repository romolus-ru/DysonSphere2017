//создать, визуализировать, должна быть достигнута ачивка, сохранить состояние локально, передавать на сервер
using DysonSphereClient.Game.Achievements;
using Engine.Data;
using Engine.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DysonSphereClient.Game
{
	/// <summary>
	/// Ачивки
	/// </summary>
	public class GameAchievements
	{
		/// <summary>
		/// Данные об ачивках, состоянии и т.п. - абстрактные данные, унифицированные, хранятся в файле или БД
		/// </summary>
		private List<AchieveDescription> _AchievementsDescription = new List<AchieveDescription>();
		/// <summary>
		/// Ачивки с текущим состоянием - хранят своё значение и ссылку на описание
		/// </summary>
		private List<GameAchievementValue> _Achievements = new List<GameAchievementValue>();
		/// <summary>
		/// Активные - которые подключены
		/// </summary>
		private Dictionary<string, GameAchievementValue> _ActiveAchievements = new Dictionary<string, GameAchievementValue>();
		//тут. получаем Ships и надо подключиться к событию отправки корабля
		//точнее последовательности событий которые надо сделать для ачивки
		//нечто наподобие туториала - будет выведена ачивка (неубираемая) и там будет написано что надо сделать что бы её получить
		public void SetupAvievementsActions(ViewTransportGame vtg)
		{
			LoadAchievementsDescriptions();
			CreateAndFillAchievementValues();
			Dictionary<string, MemberInfo> _achivementEvents = new Dictionary<string, MemberInfo>();
			GetAchievementMethods(_achivementEvents, vtg);
			foreach (var ach in _Achievements) {
				if (!_achivementEvents.ContainsKey(ach.Achieve.Title)) continue;
				ach.OnAchieved -= OnAchieved;
				ach.OnAchieved += OnAchieved;
				ach.StoreParams(vtg, _achivementEvents[ach.Achieve.Title]);
			}
			foreach (var ach in _Achievements) {
				if (ach.IsAchieved) continue;
				ach.Setup(this);
			}
			//ach1.Setup(vtg);
			//_ActiveAchievements.Add(ach1.Name, ach1);
		}

		/// <summary>
		/// Запускаем проверку ачивок на запуск и установку
		/// </summary>
		/// <param name="achievement"></param>
		public void OnAchieved(GameAchievementValue achievement)
		{
			// для начала проверим по присланной ачивке есть ли у кого previous такая же и активируем её
		}

		/// <summary>
		/// Получить из класса методы и заполнить словарь ссылками на них
		/// </summary>
		/// <param name="_achivementEvents"></param>
		/// <param name="obj"></param>
		private void GetAchievementMethods(Dictionary<string, MemberInfo> _achivementEvents, Object obj)
		{
			var t = obj.GetType();
			var methodsAll = t.GetMembers();
			var methodsAch = new List<MemberInfo>();
			var searchType = typeof(AchievementInfoAttribute).FullName;
			foreach (var method in methodsAll) {
				//if (method.ReflectedType.Name!= "Action") continue;
				var attributes = method.GetCustomAttributesData();
				foreach (var attr in attributes) {
					var isAchieveAttribute = attr.AttributeType.FullName == searchType;
					if (isAchieveAttribute) {
						var names = attr.NamedArguments;
						foreach (var item in names) {
							var name = item.MemberName;
							if (name == "Name") {
								if (_achivementEvents.ContainsKey(name))
									throw new InvalidOperationException("Уже объявлено и заполнено " + name + " для ачивки");
								_achivementEvents.Add(item.TypedValue.Value as string, method);
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// По коду ачивки получить текущее состояние ачивки
		/// </summary>
		/// <param name="achCode"></param>
		/// <returns></returns>
		public GameAchievementValue GetAchievement(string achCode)
		{
			foreach (var ach in _Achievements) {
				if (ach.Code == achCode) return ach;
			}
			return null;
		}

		/// <summary>
		/// Получаем текущие данные об ачивках, текущие значения (из базы или с сервера, из другой таблицы)
		/// </summary>
		private void CreateAndFillAchievementValues()
		{
			_Achievements.Clear();
			var values = new List<AchieveValues>();
			var av1 = new AchieveValues();
			foreach (var description in _AchievementsDescription) {
				GameAchievementValue ach = CreateAchieveValueClass(description);
				_Achievements.Add(ach);
			}
		}

		/// <summary>
		/// Создать класс ачивки (фабрика)
		/// </summary>
		/// <param name="description"></param>
		/// <returns></returns>
		private GameAchievementValue CreateAchieveValueClass(AchieveDescription description)
		{
			GameAchievementValue value = null;
			switch (description.Type) {
				case AchievementClassTypeEnum.EventCounter:
					value = new GameAchievementValue();
					break;
				default:
					value = new GameAchievementValue();
					break;
			}
			value.Achieve = description;
			return value;
		}

		private void LoadAchievementsDescriptions()
		{
			AchieveDescription ach;
			ach = new AchieveDescription()
			{
				Code = GameAchievementsConstants.SelectPlanet,
				Title = "Выберите планету",
				Description = "Выберите планету",
				Group = "Tutorial",
				Count = 0,
			};
			ach = new AchieveDescription()
			{
				Code = GameAchievementsConstants.StartRace,
				PreviousAchievements=GameAchievementsConstants.SelectPlanet,
				Title = "Запустите выполнение заказа",
				Description = "Выберите планету с необходимыми ресурсами для запуска рейса",
				DescriptionReward = "Награда - дополнительный бесплатный корабль",
				Group = "Tutorial",
				Count = 0,
			};
			_AchievementsDescription.Add(ach);
		}
	}
}