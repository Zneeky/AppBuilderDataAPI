using System.ComponentModel.DataAnnotations;

namespace AppBuilderDataAPI.Data.Models
{
    public class Membership
    {
        [Key]
        public int MembershipId { get; set; } // Primary key

        [Required(ErrorMessage = "Membership type is required")]
        [StringLength(50, ErrorMessage = "Membership type can't be longer than 50 characters")]
        public string Type { get; set; } = null!;

        [Required(ErrorMessage = "Price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Duration in days is required")]
        [Range(1, 365, ErrorMessage = "Duration must be between 1 and 365 days")]
        public int DurationInDays { get; set; }

        [Required(ErrorMessage = "Photo URL is required")]
        [StringLength(500, ErrorMessage = "Photo URL can't be longer than 500 characters")]
        public string PhotoUrl { get; set; } = null!; // Link to membership photo
    }
}
