using System.ComponentModel.DataAnnotations;

namespace Core.Data.Models.AccountViewModels
{
    public class ExternalLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
