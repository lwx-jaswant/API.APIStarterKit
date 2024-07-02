using System.ComponentModel.DataAnnotations;

namespace Core.Data.Models.EmailConfigViewModel
{
    public class EmailConfigCreateUpdateVM : EntityBase
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Hostname { get; set; }
        [Required]
        public int Port { get; set; }
        [Display(Name = "SSL Enabled")]
        public string SSLEnabled { get; set; }
        [Display(Name = "Sender Full Name")]
        public string SenderFullName { get; set; }
        public string IsDefault { get; set; }
        public string IsDefaultDisplay { get; set; }

        public static implicit operator EmailConfig(EmailConfigCreateUpdateVM vm)
        {
            return new EmailConfig
            {
                Id = vm.Id,
                Email = vm.Email,
                Password = vm.Password,
                Hostname = vm.Hostname,
                Port = vm.Port,
                SSLEnabled = Convert.ToBoolean(vm.SSLEnabled),
                SenderFullName = vm.SenderFullName,
                IsDefault = Convert.ToBoolean(vm.IsDefault),
                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
                CreatedBy = vm.CreatedBy,
                ModifiedBy = vm.ModifiedBy,
                Cancelled = vm.Cancelled,
            };
        }
    }
}

