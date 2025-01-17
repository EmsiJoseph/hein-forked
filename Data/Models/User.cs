using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Hein.Data.Models
{
    public class User : IdentityUser
    {
        public string? Phone { get; set; }

        [MaxLength(50)] public string Role { get; set; } = "Customer";

        [MaxLength(3)] public string? Age { get; set; }

        [MaxLength(100)] public string? FullName { get; set; }

        [MaxLength(50)] public string? FirstName { get; set; }

        [MaxLength(50)] public string? LastName { get; set; }

        [MaxLength(50)] public string? Gender { get; set; }

        public DateOnly? Dob { get; set; }

        [Column(TypeName = "text")] public string? StylePreference { get; set; }
        [Column(TypeName = "text")] public string? ShoppingPreference { get; set; }
        [Column(TypeName = "text")] public string? FashionStylePreference { get; set; }

        [Column(TypeName = "decimal(10,7)")] public decimal? Latitude { get; set; }

        [Column(TypeName = "decimal(10,7)")] public decimal? Longitude { get; set; }

        public bool EmailSubscription { get; set; } = false;
        public bool IsOnboarded { get; set; } = false;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}