using TZ2V.Entity;

namespace TZ2V.Data
{
    /// <summary>
    /// Под-сущность хранящая информацию про модель машины, определённой марки
    /// </summary>
    public class BrandModel
    {
        public int Id { get; set; }
        public string? NameModel { get; set; }
        public List<AboutModel>? AboutModels { get; set; } = new List<AboutModel>();
    }
}
