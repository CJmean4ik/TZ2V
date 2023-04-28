using TZ2V.Data;

namespace TZ2V.Entity
{
    /// <summary>
    /// Сущность которая хранит изображения и список информации про под-деталь
    /// </summary>
    public class Tile
    {
        public byte[] ImageScheme { get; set; }
        public List<TreeInfo> Info { get; set; } = new List<TreeInfo>();
    }
}
