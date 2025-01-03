namespace Shared
{
    public class Shift
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }

        public int NumberOfVolunteersNeeded { get; set; }

        public List<Volunteer> AssignedVolunteers { get; set; } = new List<Volunteer>();
    }
}
