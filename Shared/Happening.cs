namespace Shared
{
    public class Happening
    {
        public Guid id { get; set; }
        public string Name { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string BeginDate { get; set; }
        public string EndDate { get; set; }
        public List<Happening> ChildEvents { get; set; }
        public List<Shift> Shifts { get; set; }
        public List<Volunteer> AssignedVolunteers { get; set; }
    }
}
