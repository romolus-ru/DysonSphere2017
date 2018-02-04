// TODO вывод окна перенести в GUIHelper
// TODO сделать модальное окно ввода логина, автономное - что бы только 2 метода туда передавалось - ok cancel
// TODO должна быть возможность посылать в систему команды
// TODO вынести некоторые элементы в визуализацию - вывод оттекстуренного прямоугольника, вывод горизонтальной полоски - что бы уголки определялись через текстуру 
// и вывод при отсутствии текстуры заменялся на линии
// TODO разделить диалоговое модальное окно и сделать специализированное - окно ввода строки (заодно будет как пример, и на его основе сделать окно ввода логина)
// возможно разделить типы событий на отдельные классы даже, что бы они работали отдельно
// TODO от модального окна возможно лучше избавиться. ViewInput должен получить сигнал о том что он обрабатывает ввод и он подключается к нужным событиям
// если нажаты некоторые кнопки (enter, tab) или кликнуто вне области ввода - то отключается (и визуально это надо тоже показывать)
// TODO поэкспериментировать с gl.CallLists - может быть получится сделать из текстуры список и выводить его как шрифт
// TODO переделать настройки - счас "настройки" специализированы хранить только тип и код класса
// а надо сделать чтоб они хранили настройки в виде строк, и могли заполнять класс Settings нужными настройками
// TODO найти возможность подключаться к дебаггеру и просматривать сообщения от него. в том числе и записывать их.
// TODO работа с шейдерами
// TODO обработка кнопок клавиатуры
// TODO разделить объект по работе с БД на части. например авторизация, логирование, обработка игр
// TODO 2 режима работы с клавиатурой - обрабатывать каждое нажатие отдельно (через отдельный класс в котором хранится код нажатой кнопки) 
// TODO если комбинации кнопок должны обрабатываться отдельно то значит надо всё таки делать по другому
// или получать список нажатых кнопок с заданной периодичностью. но всё это должно быть в функционале класса input
// TODO кроме модального объекта есть ещё и перемещаемый плюс могут быть несколько вложенных.
// TODO для загрузки сервера сделать отдельный загрузчик с выводом прогресса. сделать стандартный загрузчик
// TODO сделать консоль с основными командами. хотя по идее всё должно управляться через интерфейс, так что можт и не нужно
// TODO основной цикл сервера перенести в отдельный поток в классе, который управляет этим потоком
// TODO попробовать перенести ViewDragable которая ведает перемещением в InputHelper. 
// как минимум оно должно реагировать на события отпускания кнопки и если перемещение было маленьким - запускать клик
// TODO элементы управления можно хранить в базе и сделать для них редактор, на основе движка

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
