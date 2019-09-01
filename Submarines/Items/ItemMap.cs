using System.Collections.Generic;
using System.Drawing;

namespace Submarines.Items
{
    /// <summary>
    /// Содержит характеристики карты и параметры в общем виде
    /// </summary>
    internal class ItemMap
    {
        /// <summary>
        /// точка спавна - координата, имя и настройки
        /// </summary>
        internal class ItemMapSpawnPoint
        {
            public int Id { get; set; }// уникальный номер для каждой карты
            public string Name { get; set; }
            public Vector Point { get; set; }
            /// <summary>
            /// Векторное представление знака спавна
            /// </summary>
            public string SignGeometryName { get; set; }
            /// <summary>
            /// Текстурное представление знака спавна на карте
            /// </summary>
            public string SingMapTexture { get; set; }
            /// <summary>
            /// Изображение точки на карте. если отсутствует - использовать или закрашенную область геометрии или уменьшенную текстуру для карты
            /// </summary>
            public string SignRadarTexture { get; set; }
            /// <summary>
            /// Тип спавна
            /// </summary>
            public SpawnType SpawnType { get; set; }
            /// <summary>
            /// Геометрия области для взаимодействия
            /// </summary>
            public string AreaGeometryName { get; set; }
            public Color ChangeColor { get; set; }// цвет если надо переопределить
            /// <summary>
            /// Дополнительная информация для скриптов
            /// </summary>
            public string CodeInfo { get; set; }
            /// <summary>
            /// Описание точки спавна
            /// </summary>
            public string Description { get; set; }
        }
        public string MapCode { get; set; }
        public string MapName { get; set; }
        public string MapDescription { get; set; }
        public string MapGeometryName { get; set; }
        public List<ItemMapSpawnPoint> MapSpawns { get; set; }

        public ItemMapSpawnPoint AddNewSpawn(int x, int y) {
            if (MapSpawns == null)
                MapSpawns = new List<ItemMapSpawnPoint>();

            var maxNum = 0;
            foreach (var mapSpawn in MapSpawns) {
                if (maxNum < mapSpawn.Id)
                    maxNum = mapSpawn.Id;
            }

            var newMapSpawn = new ItemMapSpawnPoint();
            newMapSpawn.Id = maxNum + 1;
            newMapSpawn.Name = MapName + "Spawn" + newMapSpawn.Id;
            newMapSpawn.Point = new Vector(x, y, 0);
            MapSpawns.Add(newMapSpawn);
            return newMapSpawn;
        }


    }
}