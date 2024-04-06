namespace JobsAPI.Models
{
    public class Location
    {
        public Location()
        {
            Jobs = new HashSet<Job>();
        }
        public int LocationId { get; set; }
        public string? Title { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public int? Zip { get; set; }
        public virtual ICollection<Job> Jobs { get; set; }
    }
}
