using TZ2V.Data;

namespace TZ2V.Entity
{
    /// <summary>
    /// Сущность, которая хранит под-детали определённой группы запчастей
    /// </summary>
    public class UnderTile
    {
        public string NameTile { get; set; }
        public string DataTileUrl { get; set; }
        public Tile DataOnScheme { get; set; } = new Tile();
    }
}
