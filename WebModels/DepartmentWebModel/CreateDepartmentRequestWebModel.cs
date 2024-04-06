using System.ComponentModel.DataAnnotations;

namespace JobsAPI.WebModels.DepartmentWebModel
{
    public class CreateDepartmentRequestWebModel
    {
        [Required]
        [MinLength(2, ErrorMessage = "Title Must Contain Atleast 2")]
        public string? Title { get; set; }
    }
}
