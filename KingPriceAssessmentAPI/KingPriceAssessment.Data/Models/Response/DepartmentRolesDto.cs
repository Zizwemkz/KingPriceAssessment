using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingPriceAssessment.Data.Models.Response
{
    public class DepartmentRolesDto
    {
        public string DepartmentName { get; set; }
        public List<string> Roles { get; set; }
    }
}
