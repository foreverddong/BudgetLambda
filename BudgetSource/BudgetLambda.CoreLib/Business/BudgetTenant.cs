using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetLambda.CoreLib.Business
{
    public class BudgetTenant
    {
        [Key]
        public Guid TenantID { get; set; } = Guid.NewGuid();
        public string TenantName { get; set;}

        public string Prefix 
        {
            get => TenantID.ToString()[0..8];
        }
    }
}
