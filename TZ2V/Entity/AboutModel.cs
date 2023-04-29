using TZ2V.Data;

namespace TZ2V.Entity
{
    /// <summary>
    /// Сущность, хранящая данные про модель машины, определённой марки
    /// </summary>
    public class AboutModel
    {     
        public string? Code { get; set; }
        public List<Complectations>? CompleteSets { get; set; } = new List<Complectations>();
        public string? UrlCompleteSets { get; set; }
        public string? DataRange { get; set; }
        public string? CodeModel { get; set; }
        public int BrandModelId { get; set; }
    }
}
