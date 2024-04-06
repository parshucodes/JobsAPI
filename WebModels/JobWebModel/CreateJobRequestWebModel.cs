using System.ComponentModel.DataAnnotations;

namespace JobsAPI.WebModels.JobWebModel
{
    public class CreateJobRequestWebModel
    {
        [Required]
        [MinLength(2, ErrorMessage = "Title must contain at least 2 characters")]
        public string? Title { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Description must contain at least 2 characters")]
        public string? Description { get; set; }

        [Required]
        [Range(1, 100, ErrorMessage = "Please enter a valid Location ID")]
        public int? LocationId { get; set; }

        [Required]
        [Range(1, 100, ErrorMessage = "Please enter a valid Department ID")]
        public int? DepartmentId { get; set; }

        [Required]
        public DateTime? ClosingDate { get; set; }
    }
}
