using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Data.Models.EmployeeTypeViewModel
{
    public class EmployeeTypeCRUDViewModel : EntityBase
    {        
        [Display(Name = "Name"), Required]
        public string Name { get; set; }
        public string Description { get; set; }

        public static implicit operator EmployeeTypeCRUDViewModel(EmployeeType _EmployeeType)
        {
            return new EmployeeTypeCRUDViewModel
            {
                Id = _EmployeeType.Id,
                Name = _EmployeeType.Name,
                Description = _EmployeeType.Description,
                CreatedDate = _EmployeeType.CreatedDate,
                ModifiedDate = _EmployeeType.ModifiedDate,
                CreatedBy = _EmployeeType.CreatedBy,
                ModifiedBy = _EmployeeType.ModifiedBy,
                Cancelled = _EmployeeType.Cancelled,
            };
        }

        public static implicit operator EmployeeType(EmployeeTypeCRUDViewModel vm)
        {
            return new EmployeeType
            {
                Id = vm.Id,
                Name = vm.Name,
                Description = vm.Description,
                CreatedDate = vm.CreatedDate,
                ModifiedDate = vm.ModifiedDate,
                CreatedBy = vm.CreatedBy,
                ModifiedBy = vm.ModifiedBy,
                Cancelled = vm.Cancelled,
            };
        }
    }
}
