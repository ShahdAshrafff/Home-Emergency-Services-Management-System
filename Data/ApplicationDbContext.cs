using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HomeServices.Models;

namespace HomeServices.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Database Tables
        public DbSet<Category> Categories { get; set; }
        public DbSet<Request> Requests { get; set; }

        // --- إضافة جدول العروض الجديد ---
        public DbSet<ServiceOffer> ServiceOffers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // 1. Configure the relationship between Request and Customer (User)
            builder.Entity<Request>()
                .HasOne(r => r.Customer)
                .WithMany(u => u.CustomerRequests)
                .HasForeignKey(r => r.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // 2. Configure the relationship between Request and Provider (User)
            builder.Entity<Request>()
                .HasOne(r => r.Provider)
                .WithMany(u => u.ProviderTasks)
                .HasForeignKey(r => r.ProviderId)
                .OnDelete(DeleteBehavior.Restrict);

            // --- 3. التعديلات الجديدة لربط العروض (Bidding System) ---

            // ربط العرض بالطلب: الطلب الواحد ممكن يجيله كذا عرض
            builder.Entity<ServiceOffer>()
                .HasOne(so => so.Request)
                .WithMany(r => r.Offers)
                .HasForeignKey(so => so.RequestId)
                .OnDelete(DeleteBehavior.Cascade); // لو الطلب اتحذف، عروضه تتحذف

            // ربط العرض بالبروفيدر: البروفيدر يقدر يقدم كذا عرض على كذا طلب
            builder.Entity<ServiceOffer>()
                .HasOne(so => so.Provider)
                .WithMany(u => u.Offers)
                .HasForeignKey(so => so.ProviderId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}