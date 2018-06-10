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
		/// Изменение состояния ачивок
		/// </summary>
		public Action OnAchieveChanged;
		
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
		public void SetupAvievementsActions(ViewTransportGame vtg, Ships ships)
		{
			LoadAchievementsDescriptions();
			CreateAndFillAchievementValues();
			Dictionary<string, KeyValuePair<object, MemberInfo>> _achivementEvents = new Dictionary<string, KeyValuePair<object, MemberInfo>>();
			Dictionary<string, KeyValuePair<object, MemberInfo>> _achivementMethods = new Dictionary<string, KeyValuePair<object, MemberInfo>>();
			GetAchievementMethods(_achivementEvents, _achivementMethods, vtg);
			GetAchievementMethods(_achivementEvents, _achivementMethods, ships);
			foreach (var ach in _Achievements) {
				//ach.OnAchieved -= Achieved;
				//ach.OnAchieved += Achieved;
				if (_achivementEvents.ContainsKey(ach.Achieve.Code)) {
					var p = _achivementEvents[ach.Achieve.Code];
					ach.StoreWaitParams(p.Key, p.Value);
				}
				if (_achivementMethods.ContainsKey(ach.Achieve.Code)) {
					var p = _achivementMethods[ach.Achieve.Code];
					ach.StoreOutParams(p.Key, p.Value);
				}
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
		public void Achieved(GameAchievementValue achievement)
		{
			// для начала проверим по присланной ачивке есть ли у кого previous такая же и активируем её
			OnAchieveChanged?.Invoke();
		}

		/// <summary>
		/// Получить из класса методы и заполнить словарь ссылками на них
		/// </summary>
		/// <param name="achivementEvents"></param>
		/// <param name="obj"></param>
		private void GetAchievementMethods(
			Dictionary<string, KeyValuePair<object, MemberInfo>> achivementEvents,
			Dictionary<string, KeyValuePair<object, MemberInfo>> achivementMethods,
			Object obj)
		{
			var t = obj.GetType();
			var membersAll = t.GetMembers();
			var membersAch = new List<MemberInfo>();
			var searchType = typeof(AchievementInfoAttribute).FullName;
			foreach (var member in membersAll) {
				var attributes = member.GetCustomAttributesData()
					.Where(a => a.AttributeType.FullName == searchType);
				foreach (var attr in attributes) {
					foreach (var namedArg in attr.NamedArguments) {
						var name = namedArg.MemberName;
						if (name == "Name") {
							if (member.MemberType == MemberTypes.Field
								|| member.MemberType == MemberTypes.Property) {
								if (achivementEvents.ContainsKey(namedArg.TypedValue.Value as string))
									throw new InvalidOperationException("Уже объявлено и заполнено " + name
										+ " для ачивки " + (namedArg.TypedValue.Value as string));
								achivementEvents.Add(namedArg.TypedValue.Value as string, new KeyValuePair<object, MemberInfo>(obj, member));
							}
							if (member.MemberType == MemberTypes.Method) {
								if (achivementMethods.ContainsKey(namedArg.TypedValue.Value as string))
									throw new InvalidOperationException("Уже объявлено и заполнено " + name
										+ " для ачивки " + (namedArg.TypedValue.Value as string));
								achivementMethods.Add(namedArg.TypedValue.Value as string, new KeyValuePair<object, MemberInfo>(obj, member));
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
				if (ach.Achieve.Code == achCode) return ach;
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
			_AchievementsDescription.Add(ach);

			ach = new AchieveDescription()
			{
				Code = GameAchievementsConstants.StartRace,
				PreviousAchievements = GameAchievementsConstants.SelectPlanet,
				Title = "Запустите выполнение заказа",
				Description = "Выберите планету с необходимыми ресурсами для запуска рейса",
				DescriptionReward = "Награда - дополнительный бесплатный корабль",
				Group = "Tutorial",
				Count = 0,
			};
			_AchievementsDescription.Add(ach);
		}

		public GameAchievementValue GetTutorialAchieves()
		{
			foreach (var ach in _Achievements) {
				if (ach.Achieve.Group != "Tutorial") continue;
				if (ach.IsActive)
					return ach;
			}
			return null;
		}
	}
}