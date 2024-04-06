using System.ComponentModel.DataAnnotations;

namespace JobsAPI.WebModels.LocationWebModel
{
    public class CreateLocationRequestWebModel
    {
        [Required]
        [MinLength(2, ErrorMessage = "Title must contain at least 2 characters")]
        public string? Title { get; set; }
        [Required]
        [MinLength(2, ErrorMessage = "City must contain at least 2 characters")]
        public string? City { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "State must contain at least 2 characters")]
        public string? State { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Country must contain at least 2 characters")]
        public string? Country { get; set; }
        [Required]
        [RegularExpression("([0-9]+)")]
        public int? Zip { get; set; }
    }
}
