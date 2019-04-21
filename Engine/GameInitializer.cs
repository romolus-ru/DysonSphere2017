﻿// траектория
// обработка и визуализация траектории
// обработка столкновений со стенами

// добавить обработку перезагрузки - в engineState добавить flag или событие которое остановит таймер и запустит инициализацию по новой, создав всё снова

// сделать файл карты и сделать обработку столкновений с картой (с учётом что будет ещё несколько разных обработчиков столкновений)
// itemmap автономный класс. возможно стоит ввести ещё 1 уровень - RawData - как submarine - содержит только названия и т.п. (он же используется для сохранения)
// следующий уровень это уже готовый любой объект где воссоздаются данные - корабль, карта и т.п.
// третий уровень это который эмулировался сейчас - предметы для магазина - там как раз нужны будут специфические параметры в виде количества и т.п.

// контроллер - SubmamrineAutoController - загружает из файла вероятности своих действий (одна из разновидностей ИИ будет).
// там определяются вероятности нападения, защиты, дальность атаки, агрессивность, условия посылания сигнала о помощи

// столкновения проверять так - вычислить столкновение в начале и конце пути.
// если длина пути больше чем длина корпуса подлодки - вычислить и столкновения по вектору движения

// сделать простой построитель графиков - принимает число, сохраняет время и по нему строит график. сохраняет определенное количество значений остальные удаляет

// ввести в enginestate 2 переменные - deltaTime и IsDebugMode - что бы можно было останавливать и/или включать некоторые дополнительные части кода
// по идее ввести бы ещё директиву debug - но они вроде объявляются только в каждом конкретном файле


// сохранять команды у подлодок - что бы можно было восстановить действие каждой подлодки и посмотреть бой

// на данный момент об игре известно немного. 
// в начале подводная лодка просто достигнет указанной точки (туториал перемещения, управления инерцией, сбор грузов и стрельба)
// после прибытия на базу там можно переделать груз в патроны, получить немного денег и отправиться в начальное путешествие
// в начале будут миссии в которых будет в деталях описано что нужно сделать
// по мере продвижения по этим вступительным заданиям будет открываться полный интерфейс игры
// в конце перед игроком будет поставлена глобальная задача - получить мегаподлодку, победить врагов сюжетных и получить влияние во всех основных городах планеты
// соответственно основные города выставляют условия что надо получить влияние в подчиненных городах и заплатить много денег
// каждый город поменьше выставляет 2-5 условий сотрудничества с игроком - перевоз столького количества груза, зачистка территорий от врагов и т.п.
// так же в некоторых второстепенных городах будут протекать сюжетные миссии - перевезти груз, доставить товар и т.п.
// в основном товары делятся минимум на 4 группы
// - которые только доставлять между городами
// - которые можно переделать в полезные товары или для улучшения чего-нибудь (например патроны или усилитель скоростельности, ускорение перезарядки и т.п.)
// - уникальные квестовые товары (просто для перевозки)
// - оружие/ части оружия (части оружия хорошо стакаются, оружие может быть как просто товаром так и конкретной моделью оружие без точных параметров)
// оружие можно купить и поставить на него улучшения (просто изменить некоторые характеристики и заточить)
// патроны можно создать разные, в том числе пока не исключаю случайного создания улучшенных патронов (минимум 4 уровня качества (белое, синее зеленое фиолетовое)
// (и самое лучшее качество патронов накрафтить будет сложно, процент маленький, но можно будет купить за валюту
// патрон/ракета торпеда. просто будет основной урон и характеристики, и при следующем уровне просто будет добавка к характеристикам
// (самые хорошие по качеству боеприпасы будут производиться редко (ну, в лучшем случае каждый 20й боеприпас будет отличного качества,
// но их можно будет купить за отдельную валюту)
// некоторые боеперипасы требуют для своего использования вспомогательные части. без него они будут неуправляемые, не усиленные и т.п.
// (т.е. дополнительные характеристики могут добавляться при выстреле)
// выстрел собирается непосредственно перед самим выстрелом. т.е. собирается патрон, обтекатель, гильза и т.п.
// если некоторых частей не хватает то используются стандартные
// для ракет и торпед и так будет пауза перед запуском (в том числе для снаряжения). для мелких оружий будет время на замену ленты, отдельный параметр

// на корабле всё пространство является складом. но устанавливая дополнительные отсеки можно увеличить ёмкость корабля (как склад на базе)

// строительство базы завязано на сюжет и последующие покупки расходных материалов для постройки базы
// некоторые модули для постройки на базе можно будет построить только 1 или несколько штук. другие можно хоть сколько строить
// например отсек предварительной обработки материалов - повышает вероятность изготовления более качественных патронов
// и можно поставить 5 таких отсеков, и в сумме они будут повышать вероятность получения более и более качественных патронов
// почти всё оставшееся место надо будет под склад отвести
// часть миссий будет перевезти товар к себе на склад или наоборот - со склада в какой-нибудь город
// на начальных этапах будет только перевозка на своих двоих. далее будут доступны порталы (после покупки или открытия карты местности)
// они перенесут подлодку близко к указанной точке, даже к городу
// будут специализированные склады на которые можно складировать специализированные грузы - так они будут занимать мешьше места чем просто на складе
// соответственно будет дополнительная характеристика - занимаемое место на специализированном складе

// определить основные характеристики вещей. часть из них будет задаваться сразу, часть будет задаваться отдельными таблицами и объединяться при запуске
// например все предметы будут иметь код. но изменения характеристик будут храниться как ссылки и будут заполняться отдельно
// например запись о месте в специализированном хранилище - будет ссылка. заполнится только у тех товаров которые это будут уметь

// (под вопросом) на большую подлодку можно будет взять тягач - это специальная подводная лодка которая помогает увеличить скорость передвижения
// возможно такие лодки можно будет взять на любую подлодку - как отдельный боеприпас, что бы можно было успеть уплыть от врагов
// так же можно будет создать или купить боеприпас-подлодку для защиты от попаданий - они будут брать часть урона на себя
// (это не учитывая генератор щита, увеличение брони и увеличение живучести (хп) корпуса)
// генератор щита просто уменьшает входящий урон, постоянно расходую ресурсы при включении
// так же можно сделать креонное оружие - маленькие боеприпасы охлаждают воду перед вражескими снарядами, уменьшая их урон.
// так же может быть будет снижать скорость вражеских лодок
// броня уменьшает входящий урон (уменьшая свой ресурс) и остальное идёт уже в корпус
// корпус легче ремонтировать (но расходуются расходники). броню возможно надо ремонтировать отдельно или вообще только в порту



// сделать консоль - просто текстовый режим с курсором и возможностью отлавливать на какой символ нажали, по идее желательно приблизительно функционал Console
// этот режим потом пригодится для имитации консольных режимов и создания сервисных программ работающих в консольном режиме с поддержкой множества цветов
// и как консоль внутриигровая тоже нужен будет

// Отладка по UDP - отдельный проект, возможно текстовый (там можно экран настроить, будет не хуже winforms) запускает UDP сервер
// игра, если включена такая отладка, посылает по UDP данные. а в окне отладки их можно фильтровать и настраивать и даже сохранять

// SequenceGenerator - имя, описание, код, код согласованности (чтоб можно было заменить, фактически это главный параметр), seed, отступ от начала, длина последовательности (параметры для random) и тип последовательности (какой генератор использовался)
// RandomSequence - в этом классе описаны основные характеристики
// так же SequenceValues - сгенерированная детерминированная последовательность. в записи игрока будет храниться код последовательности и текущий итератор в последовательности
// так же нужны инструменты которые сгенерируют последовательность с заданными параметрами и сохранят её в памяти - например 
// случайная последовательность из 10 чисел в диапазоне из 100 чисел в которой все 10 чисел распределены равномерно. или в которых бОльшие числа встречаются реже
// так же нужны тестовые последовательности, для тестов, что бы точно проверять что генератор работает как часы, и если не как часы то значит надо перегенерировать последовательности
// каждый элемент последовательности должен появляться в последовательности заданное количество раз. 
// например последовательность 123 должна быть примерно такая 121321 (линейное распределение, бОльшее значение попадается меньшее количество раз). при этом могут быть пропуски значений, 
// важно что бы суммарное значение в автодиапазонах было распределено как надо (например 1-100, в последовательности всего 10 чисел, автораспределение по десяткам. значит 4-40-48-5-8-95 будет примерно правильной последовательностью
// ИЛИ в последовательности из N чисел K из них будут больше заданного значения
// это значит что при формировании награды отдельный предмет будет выдан игроку если вероятность превысит заданный порог.
// остальное будет выдано деньгами или добрано из менее ценных случайных призов до общей стоимости награды

// !! если вероятность другая, касающаяся нескольких предметов, то генерируемая последовательность должна обеспечить появление нужного диапазона с заданной вероятностью

// модели разделить на 2 вида - которые надо постоянно вызывать каждый тик и которые реагируют на внешние события 
// - тогда часть функционала View можно будет вынести в модель.

// сделать скролируемый компонент в котором будет бесконечная прокрутка и выбранный элемент должен центрироваться

// у картинок из атласа на сервере должно быть поле "source" - источник каждой отдельной картинки. если он пустой значит эту текстуру трогать нельзя

// TODO переделать settings - программа для редактирования должна уметь редактировать и общие, и клиентские и серверные настройки

// TODO добавить к GUIHelper функциональность привязки элементов к краям экрана, центрированию по области и т.п.
// например указываем область, и операцию привязки к точке. параметрами идут ViewComponents которые надо равномерно разместить у выбранной области

// TODO добавить к Settings загрузку данных из БД. по идее может быть одинаковый для клиента и сервера
// возможно это будет переделка и замена _settings которые счас есть

// TODO поэкспериментировать с gl.CallLists - может быть получится сделать из текстуры список и выводить его как шрифт
// TODO переделать настройки - счас "настройки" специализированы хранить только тип и код класса
// а надо сделать чтоб они хранили настройки в виде строк, и могли заполнять класс Settings нужными настройками
// TODO работа с шейдерами
// TODO разделить объект по работе с БД на части. например авторизация, логирование, обработка игр

// TODO сделать консоль с основными командами. хотя по идее всё должно управляться через интерфейс, так что можт и не нужно
// TODO попробовать перенести ViewDraggable которая ведает перемещением в InputHelper. 
// как минимум оно должно реагировать на события отпускания кнопки и если перемещение было маленьким - запускать клик

// TODO элементы управления можно хранить в базе и сделать для них редактор, на основе движка
// TODO перенести перемещение в InputHelper - что бы можно было включать этот режим у любого элемента
// TODO возможно диспетчер событий будет в самый раз - можно будет подписываться централизованно на любое событие из любого места и отправлять так же
// пока кажется что сложность прямого использования Action и пробрасывание их будет расти

// TODO рекомендуют хранить соль (длиною как хэш) вместе с паролем - присланный пароль XOR с солью и хэшируем - если совпало с тем что есть - окей
// у каждого своя соль

// TODO подумать что делать с CollectClass и его идентификатором. сейчас он int, при передаче по сети используется ushort. можно сделать переходную таблицу - оставить передаваемые классы в список рассылки и держать там не все возможные классы а только нужные
// или хватит 65 000 классов на ближайшее время

// TODO возможно стоит внедрить Jint - управление через джаваскрипт. некоторые методы можно туда вынести, особенно если они могут/должны настраиваться
// TODO изучить Steamwork.Net - апи для работы со стимом
// TODO программное формирование текстур. перерисовка их в процессе работы программы
// TODO OpenCL и cloo - вычисления через видеокарту. возможно могут пригодиться

// шрифт вынести в отдельный класс. будет минимум 2 варианта - обычный шрифт с GDI и WGL и текстурный шрифт - из-за проблем с вычислением длины текста.

// TODO определить 3й слой, 3D - первый это основной системный, второй это текущий, интерфейсный

// переименовать VisualizationProvider в Graphics

using Engine.Models;
using Engine.Utils;
using Engine.Visualization;

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
		protected virtual void InitResources(VisualizationProvider provider){}
		/// <summary>
		/// Соединяем нужные делегаты модели и вида
		/// </summary>
		/// <param name="model"></param>
		/// <param name="view"></param>
		protected virtual void InitDelegates(Model model, ViewComponent view){	}

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
