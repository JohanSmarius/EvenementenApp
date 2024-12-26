namespace Shared
{
    public class Shift
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string BeginTime { get; set; }
        public string EndTime { get; set; }

        public int NumberOfVolunteersNeeded { get; set; }

        public List<Volunteer> AssignedVolunteers { get; set; }
    }
}
