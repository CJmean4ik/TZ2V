namespace TZ2V.Data
{  
    /// <summary>
    /// Сущность, представляющую собой информацию про группу под-деталей
    /// </summary>
    public class Tree
    {
        public string Code { get; set; }
        public int Count { get; set; }
        public string Data { get; set; }
        public string Info { get; set; }
        public int TileId { get; set; }
        public int TreeInfoId { get; set; }

    }
}



