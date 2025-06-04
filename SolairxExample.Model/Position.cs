using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SolairxExample.Model
{
    [Table("Positions")]
    public class Position
    {
        [Key]
        public int PositionId { get; set; }
        [StringLength(100)]
        public string PositionName { get; set; }
        public string Description { get; set; }
        [Column("Create_Date", TypeName = "datetime")]
        public DateTime? CreateDate { get; set; }

        [InverseProperty(nameof(Employee.Position))]
        [ValidateNever]
        public virtual ICollection<Employee> Employees { get; set; }
    }
}
