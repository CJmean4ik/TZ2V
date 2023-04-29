using TZ2V.Data;

namespace TZ2V.Entity
{
    /// <summary>
    /// Сущность которая представляет группу запчастей определённой комплектации
    /// <br/> Всего их 4:
    /// <br/>Принадлежности/Двигатель/Топливная система
    /// <br/>Трансмиссия/Подвеска
    /// <br/>Кузов
    /// <br/>Электрика
    /// </summary>
    public class GroupGears
    {
        public string NameGroup { get; set; }
        public string UrlOnGears { get; set; }
        public int UnderTileId { get; set; }
        public int ComplectationsID { get; set; }
        public List<UnderTile> UnderTiles { get; set; } = new List<UnderTile>();

    }
}
