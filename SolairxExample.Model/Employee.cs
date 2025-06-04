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
    [Table("Employees")]
    public partial class Employee
    {
        public Employee()
        {
            Projects = new HashSet<Project>();
        }

        [Key]
        public int EmplyeeId { get; set; }
        [Column("First_Name")]
        [StringLength(100)]
        public string? FirstName { get; set; }
        [Column("Last_Name")]
        [StringLength(100)]
        public string? LastName { get; set; }
        [StringLength(100)]
        public string? Phone { get; set; }
        [StringLength(100)]
        public string? Email { get; set; }
        public int? PositionId { get; set; }
        [Column("Create_Date", TypeName = "datetime")]
        public DateTime CreateDate { get; set; }
        [Column("Modified_Date", TypeName = "datetime")]
        public DateTime? ModifiedDate { get; set; }

        [ForeignKey(nameof(PositionId))]
        [InverseProperty(nameof(Position.Employees))]
        [ValidateNever]
        public virtual Position Position { get; set; }
        [InverseProperty(nameof(Project.Employee))]
        public virtual ICollection<Project> Projects { get; set; }
    }



}
