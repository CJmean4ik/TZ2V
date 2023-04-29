using TZ2V.Data;

namespace TZ2V.Entity
{
    /// <summary>
    /// Сущность, которая представляет набор комплектации модели машины, определённой марки 
    /// </summary>
    public class Complectations
    {
        public string Code { get; set; }
        public string? Engine1 { get; set; }
        public string? Body { get; set; }
        public string? Grade { get; set; }
        public string? ATMMTM { get; set; }
        public string? GearShiftType { get; set; }
        public string? CAB { get; set; }
        public string? TransmissionType { get; set; }
        public string? LoadingCapacity { get; set; }
        public string? RearTire { get; set; }
        public string? FuelInduction { get; set; }
        public string? Destitanion { get; set; }
        public string? BuildingCondition { get; set; }
        public string? NoOfDoors { get; set; }
        public string? Product { get; set; }

        public string? GroupGearsUrl { get; set; }
        public List<GroupGears> GroupGears { get; set; } = new List<GroupGears>();

        public override string ToString()
        {
            string line = $"Engine: {Engine1}\n" +
                 $"Body: {Body}\n" +
                 $"Grade: {Grade}\n" +
                 $"ATM-MTM: {ATMMTM}\n" +
                 $"GearShiftType: {GearShiftType}\n" +
                 $"CAB: {CAB}\n" +
                 $"TransmissionType: {TransmissionType}\n" +
                 $"LoadingCapasity: {LoadingCapacity}\n" +
                 $"RearTire: {RearTire}\n" +
                 $"FuelInduction: {FuelInduction}\n" +
                 $"Destitanion: {Destitanion}\n" +
                 $"BuildingCondition: {BuildingCondition}\n" +
                 $"NoOfDoors: {NoOfDoors}\n" +
                 $"Product: {Product}\n" +
                 $"\n";
            return line;
        }
    }
}
