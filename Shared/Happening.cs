namespace Shared
{
    public class Happening
    {
        public Guid id { get; set; }
        public string Name { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? NumberOfVolunteersNeededPerShift { get; set; }
        public List<Shift> Shifts { get; set; } = new();
        
    }
}
