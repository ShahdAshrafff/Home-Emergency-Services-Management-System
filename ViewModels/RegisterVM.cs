  using System.ComponentModel.DataAnnotations;

namespace HomeServices.ViewModels
    {
        public class RegisterVM
        {
        [Required(ErrorMessage = "Full name is required.")]
        [RegularExpression(@"^\s*[a-zA-Z]{2,}(?:\s+[a-zA-Z]{2,})+\s*$",
     ErrorMessage = "Please enter at least a first and last name.")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email address is required.")]
            [EmailAddress(ErrorMessage = "Please enter a valid email address (e.g., name@example.com).")]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Phone number is required.")]
            [RegularExpression(@"^01[0125][0-9]{8}$", ErrorMessage = "Invalid Egyptian phone number.")]
            public string PhoneNumber { get; set; }

            [Required(ErrorMessage = "Address is required.")]
            [StringLength(200, MinimumLength = 10, ErrorMessage = "Address must be at least 10 characters long.")]
            public string Address { get; set; }

            [Required(ErrorMessage = "Password is required.")]
            [DataType(DataType.Password)]
            [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,20}$",
                ErrorMessage = "Password must be 8-20 characters with at least one uppercase, one lowercase, one number, and one special character.")]
            public string Password { get; set; }

            [Required(ErrorMessage = "Please confirm your password.")]
            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "Passwords do not match.")]
            [Display(Name = "Confirm Password")]
            public string ConfirmPassword { get; set; }

            [Required(ErrorMessage = "Please select if you are a Customer or a Service Provider.")]
            public string UserRole { get; set; }
        }
    }

