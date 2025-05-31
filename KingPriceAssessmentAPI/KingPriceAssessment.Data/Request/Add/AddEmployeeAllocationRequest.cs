using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingPriceAssessment.Data.Request.Add
{
    public class AddEmployeeAllocationRequest
    {
        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public int RoleId { get; set; }

        [Required]
        public int DepartmentId { get; set; }
    }
}
