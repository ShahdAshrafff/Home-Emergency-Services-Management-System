using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeServices.Models
{
    public class ServiceOffer
    {
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Offer Amount")]
        public decimal Amount { get; set; } // السعر اللي البروفيدر بيعرضه

        [Display(Name = "Provider Note")]
        public string? Note { get; set; } // ملاحظة من البروفيدر (مثلاً: هخلص في ساعتين)

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool IsAccepted { get; set; } = false; // هل العميل قبل العرض ده؟

        // الربط مع الطلب
        [Required]
        public int RequestId { get; set; }
        [ForeignKey("RequestId")]
        public virtual Request Request { get; set; }

        // الربط مع البروفيدر اللي قدم العرض
        [Required]
        public string ProviderId { get; set; }
        [ForeignKey("ProviderId")]
        public virtual ApplicationUser Provider { get; set; }
    }
}