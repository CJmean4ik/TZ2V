using TZ2V.Data;

namespace TZ2V.Entity
{
    /// <summary>
    /// Сущность которая представляет собою название груп под-деталей и их информацию
    /// </summary>
    public class TreeInfo
    {
        public string TreeCode { get; set; }
        public string TreeName { get; set; }
        public List<Tree> Trees { get; set; } = new List<Tree>();
        public override string ToString()
        {
            return
                $"TreeCode: {TreeCode}\n" +
                $"Tree: {TreeName}\n";
        }
    }
}
