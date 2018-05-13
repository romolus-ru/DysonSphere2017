﻿// для вывода на экран Orders должен содержать дополнительные данные, уже объединенные - сколько ресурсов надо перевезти

// сделать 4 таблицы. одна будет хранить информацию о самой игре(название, описание и т.п.) 
// вторая таблица будет хранить ИД, ИДИгры, КодДоступа, Раздел, Данные раздела в виде джисона (все данные сразу, одной строкой
// для описания и настройки заказов это сейчас делается
// третья таблица - иерархия жанров и их описаний
// четвертая - игра-жанры

// свойства кораблей должны считаться - в корабли наверно можно будет вставлять улучшения
// отсюда следует что надо сделать 2 окна - просмотр кораблей/ просмотр деталей/ установка (перенос)деталей и второе - просмотр ачивок/достижений
// оба скролируемые
// сделать скролируемый компонент в котором будет бесконечная прокрутка и выбранный элемент должен центрироваться

// ввести ещё 2 валюты - одна для особо ценных покупок/улучшений и вторая счётчик покупок (float - некоторые покупки теряют свою ценность с уровнем и т.п.)
// эта валюта генерится раз в час, можно будет купить генератор этой валюты, но её будет мало - проще купить

// для планет сделать отдельный объект который хранит визуализационные данные
// для зданий надо сделать фабрику, которая по умолчанию создаёт здания, на основе прототипов. и необходимо что бы эти здания потом могли меняться и менять обрабатываемую информацию
// например менять скорость погрузки/разгрузки, цену, и т.п.

// сделать глобальную подсказку. что бы она выводилась выше всех
// и чтоб можно было сделать дополнительную группу элементов, которые выводятся выше всех остальных элементов
// сделать механизм для запуска действий (Checkers - посмотреть можт подойдёт, возможно надо будет доделать). 
// например при скролировании там нужен обработчик который работает не всё время, а только если было скролирование

// иногда почему то не летают - было на расстоянии 1 линии заказ/ресурс и не полетел
// сделать обнуление - при нажатии кнопки RecreatePoints обнулить все данные у модели и вьюхи
// отдельный элемент должен быть для кораблей - каждый корабль на начальном этапе это кнопка покупки. потом это кнопка возврата на базу
// если корабль на базе то появляется кнопка покупки улучшения

// нужна система ачивок. возможно, лучше её реализовать тк - будет общий элемент ачивки, он же общается с базой и т.п.
// второй элемент уже для каждой игры отдельно - создаётся куча обработчиков и они при срабатывании отправляют всё основному объекту
// и при запуске так же отправляет правила формирования (сложения, вычисления показателей ачивок
// ачивки могут быть пользовательские и серверные

// у картинок из атласа на сервере должно быть поле "source" - источник каждой отдельной картинки. если он пустой значит эту текстуру трогать нельзя

// для ачивок обязательно предусмотреть взаимозависимости - например некоторые должны появляться только при достижении определенного уровня у других учивок
// отдельно предусмотреть хранение данных для вывода графиков. ачивки хранят свои значения, графики хранят свои все промежуточные значения
// при перезапуске карты бОльшая часть данных удаляется. остаются только сжатые интересные статистические данные, 
// например когда были получены учивки, когда был достигнут предел в деньгах и т.п.

// сделать список заказов на стройки и присвоить их всем зданиям
// отдельно идут 3 мегастройки которые будут требовать много ресурсов
// событие загрузки/разгрузки корабля уже есть - надо списывать ресурсы и загружать их на корабль. в случае завершения заказа отправлять корабль на базу

// TODO на карте часть планет производят ресурсы, часть потребляют - выдают задание привезти определенное количество ресурсов, за это выдают деньги
// за деньги можно покупать ещё космических кораблей
// вверху несколько кнопок - купить ещё корабль, отозвать все корабли на ремонтную базу, купить модуль автоматизации

// каждый следующий корабль стоит дороже, но увеличивает у всех кораблей грузоподъемность (или надо как-либо увязать с количеством перевезенных ресурсов или выполненных контрактов)
// (или корабль можно купить не дорого, но он будет медленным и мало перевозить - а что бы нормальным кораблем был - надо вложиться. первый корабль изначально со всеми почти улучшениями)
// но пока наверно лучше просто покупку подороже сделать

// поставить 3 мега здания квестовых которым надо много ресурсов. если все они "построены" - сессия завершается

// сделать распределение точек - получаем точки безье и равномерно их располагаем - сейчас там между точками разное расстояние
// так же они должны располагаться очень близко друг к другу - что бы было плавное движение

// TODO переделать settings - программа для редактирования должна уметь редактировать и общие, и клиентские и серверные настройки
// состояние State должно сохраняться отдельно, это не часть настроек (во всяком случае пока)

// TODO у событий всё таки остаются пустые словари. попробовать нажать эти кнопки, можт что сломалось
// TODO сделать перемещение карты, распределение товаров, вывод информации о точке. точка должна быть не ScreenPoint а игровой точкой

// TODO добавить к GUIHelper функциональность привязки элементов к краям экрана, центрированию по области и т.п.
// например указываем область, и операцию привязки к точке. параметрами идут ViewComponents которые надо равномерно разместить у выбранной области
// объект который хранит область или сразу область, операция привязки к краю(центру), 
// опция разрешения изменения размеров (просто разместить поближе к краю или распределить равномерно), ViewComponents как параметры которые надо разместить

// TODO переделать StateClient в просто State которому передаётся при загрузке константа - часть имени. и сервер будет свои настройки там хранить, клиент в своём файле
// наверно пусть там будут все нужные на данный момент настройки - потом всё равно переделывать settings и они туда перенесутся. а state останется и будет хранить состояние системы

// сделать в базе нужную таблицу. вторую таблицу тоже - там хранить результат проверен ли адрес или нет и некоторые другие свойства пользователя

// TODO текущая миниигра - карта статическая, хранится у клиента (набор из 20-30 точек) характеристики товаров (что где производится/требуется)
// дороги между точками тоже заранее сгенерированы
// в "городах" выводится что нужно доставить чтоб получить награду
// у игрока 1 грузовик. движение грузовика по траектории занимает время
// возможность купить несколько дополнительных грузовиков. потом можно купить модуль что бы грузовик сам выбирал что купить и куда отвезти
// основное - сделать запуск игры - инициализатор должен создать нужные модели и виды и объединить их. связаться с сервером и получить данные о текущем прогрессе игрока оттуда

// TODO добавить к Settings загрузку данных из БД. по идее может быть одинаковый для клиента и сервера
// возможно это будет переделка и замена _settings которые счас есть
// TODO при закрытии и клиент и сервер должны отправлять сигнал о завершении работы. 
// Дополнительно на сервере должно быть отслеживание соединения, и если соединение неактивно то закрывать его. 
// у клиента должна быть проверка после долгого ничего не делания что сервер не закрыл соединение

// TODO подрихтовать оставшееся в визуальном плане. 
// TODO сделать окно регистрации. в базу всё это записывать
// TODO у пользователя должна быть кнопка синхронизировать данные с сервером - что бы содержимое таблиц переправилось с сервера на клиент (или в настройках или отладочный режим

// TODO настройки переделать - они должны быть трёх типов - общие, клиент и сервер. строка в базе должна быть типа тип записи, строка имя записи строка значение записи
// TODO форма ввода логина должна реагировать на нажатия на поля ввода и отменяться при нажатии вне формы
// TODO узнать что с ентити и длиной строк -  надо как то проверять длину строк по разрешенности при записи в БД

// TODO поэкспериментировать с gl.CallLists - может быть получится сделать из текстуры список и выводить его как шрифт
// TODO переделать настройки - счас "настройки" специализированы хранить только тип и код класса
// а надо сделать чтоб они хранили настройки в виде строк, и могли заполнять класс Settings нужными настройками
// TODO найти возможность подключаться к дебаггеру и просматривать сообщения от него. в том числе и записывать их.
// TODO работа с шейдерами
// TODO разделить объект по работе с БД на части. например авторизация, логирование, обработка игр

// TODO сделать консоль с основными командами. хотя по идее всё должно управляться через интерфейс, так что можт и не нужно
// TODO попробовать перенести ViewDragable которая ведает перемещением в InputHelper. 
// как минимум оно должно реагировать на события отпускания кнопки и если перемещение было маленьким - запускать клик

// TODO элементы управления можно хранить в базе и сделать для них редактор, на основе движка
// TODO возможно проще будет сразу разделить работы с БД на серверную с БД и на клиентскую на файлах. А серверную ещё разделить минимум на 3 части - логин, общее управление (логи, чат и т.п.) и игровое управление
// TODO перенести перемещение в InputHelper - что бы можно было включать этот режим у любого элемента
// TODO возможно диспетчер событий будет в самый раз - можно будет подписываться централизованно на любое событие из любого места и отправлять так же
// пока кажется что сложность прямого использования Action и пробрасывание их будет расти

// TODO рекомендуют хранить соль (длиною как хэш) вместе с паролем - присланный пароль XOR с солью и хэшируем - если совпало с тем что есть - окей
// у каждого своя соль

// TODO сделать дополнительный клиент. Проект конкретной игры надо вести в отдельном проекте - там же будет и прямой их запуск для отладки

using Engine.Models;
using Engine.Utils;
using Engine.Visualization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
	/// <summary>
	/// Инициализирует игру. вид и модель
	/// </summary>
	public class GameInitializer
	{
		private const string LogTag = "GameInitializer";
		/// <summary>
		/// Инициализируем игру
		/// </summary>
		/// <param name="modelMain">Основной объект модели</param>
		/// <param name="viewMain">Основной объект вида</param>
		/// <param name="provider">Основной объект визуализации</param>
		/// <param name="logs">Логи</param>
		/// <param name="input">Устройство пользовательского ввода</param>
		public void InitGame(ModelMain modelMain, ViewManager viewMain, VisualizationProvider provider, LogSystem logs, Input input)
		{
			logs.AddLog(LogTag, "запускаем инициализацию игры");
			var model = CreateModel();
			var view = CreateView(provider);
			InitDelegates(model, view);
			InitResourcesThread(provider);
			// TODO добавить модель и вид в нужные места
			modelMain.AddModel(model);
			viewMain.AddView(view);
			logs.AddLog(LogTag, "инициализацию игры завершена");
		}

		/// <summary>
		/// Запуск потока для загрузки ресурсов. Переводим менеджера в режим ожидания и вывода загружаемых данных
		/// </summary>
		private void InitResourcesThread(VisualizationProvider provider)
		{
			// пока напрямую вызываем
			InitResources(provider);
		}

		/// <summary>
		/// Инициализируем ресурсы. запускается в отдельном потоке
		/// </summary>
		protected void InitResources(VisualizationProvider provider)
		{

		}
		/// <summary>
		/// Соединяем нужные делегаты модели и вида
		/// </summary>
		/// <param name="model"></param>
		/// <param name="view"></param>
		protected void InitDelegates(Model model, ViewComponent view)
		{
			
		}

		/// <summary>
		/// Создаём нужную модель
		/// </summary>
		/// <returns></returns>
		protected virtual Model CreateModel()
		{
			return null;
		}

		/// <summary>
		/// Создаём нужный вид
		/// </summary>
		/// <param name="provider"></param>
		/// <returns></returns>
		protected virtual ViewComponent CreateView(VisualizationProvider provider)
		{
			return null;
		}

	}
}
