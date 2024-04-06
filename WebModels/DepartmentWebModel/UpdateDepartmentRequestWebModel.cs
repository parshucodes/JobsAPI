using System.ComponentModel.DataAnnotations;

namespace JobsAPI.WebModels.DepartmentWebModel
{
    public class UpdateDepartmentRequestWebModel
    {
        [Required]
        [MinLength(2, ErrorMessage = "Title Should contain atleaset 2 Characters")]
        public string? Title { get; set; }
    }
}
