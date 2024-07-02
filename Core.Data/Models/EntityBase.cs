using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Data.Models
{
    public class EntityBase
    {
        public Int64 Id { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public bool Cancelled { get; set; }
    }
}
