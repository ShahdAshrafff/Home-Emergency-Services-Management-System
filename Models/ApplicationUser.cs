using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeServices.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "Full name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Full name must be between 3 and 100 characters")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters")]
        [Display(Name = "Address")]
        public string? Address { get; set; }

        [Display(Name = "Account Created At")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // --- التعديلات الجديدة للمحفظة ---

        [Display(Name = "Wallet Balance")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal WalletBalance { get; set; } = 1000.00m; // الرصيد المبدئي 1000 دولار

        // ---------------------------------

        // Relationships
        public virtual ICollection<Request> CustomerRequests { get; set; }
        public virtual ICollection<Request> ProviderTasks { get; set; }

        // علاقة مع جدول العروض (البيدنج) اللي هنكريته
        public virtual ICollection<ServiceOffer> Offers { get; set; }
    }
}