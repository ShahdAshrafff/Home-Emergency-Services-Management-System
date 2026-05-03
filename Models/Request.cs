using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeServices.Models
{
    public class Request
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please describe the problem")]
        [MinLength(10, ErrorMessage = "Description must be at least 10 characters long")]
        [Display(Name = "Request Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please select your preferred schedule")]
        [Display(Name = "Preferred Schedule")]
        public DateTime PreferredSchedule { get; set; }

        [Display(Name = "Request Status")]
        public string Status { get; set; } = "Pending";

        [Range(1, 5)]
        [Display(Name = "Rating")]
        public int? Rating { get; set; }

        [Display(Name = "Feedback Comment")]
        public string? FeedbackComment { get; set; }

        [Display(Name = "Final Price")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? FinalPrice { get; set; }

        // --- التعديل هنا ---
        // أضفنا علامة الاستفهام لتجنب مشكلة "The Offers field is required"
        public virtual ICollection<ServiceOffer>? Offers { get; set; }
        // --------------------

        [Required]
        [Display(Name = "Customer")]
        public string CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public virtual ApplicationUser Customer { get; set; }

        [Required]
        [Display(Name = "Service Category")]
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        [Display(Name = "Service Provider")]
        public string? ProviderId { get; set; }

        [ForeignKey("ProviderId")]
        public virtual ApplicationUser? Provider { get; set; }

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}