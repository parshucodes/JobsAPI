namespace JobsAPI.Models
{
    public partial class Department
    {
        public Department()
        {
            Jobs = new HashSet<Job>();
        }
        public int DepartmentId { get; set; }
        public string? Title { get; set; }
        public virtual ICollection<Job> Jobs { get; set; }
    }
}
