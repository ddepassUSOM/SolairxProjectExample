using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc;

namespace SolairxExample.Model
{
    [Table("Project")]
    public partial class Project
    {
        [Key]
        public int ProjectId { get; set; }
        [Required]
        [StringLength(100)]
        public string? ProjectName { get; set; }
        [StringLength(200)]
        public string? ProjectShortDesc { get; set; }
        public string? ProjectLongDesc { get; set; }
        public int? ProjectManager { get; set; }
        
        [Column("Modified_Date", TypeName = "datetime")]
        public DateTime? ModifiedDate { get; set; }
        [Column("Create_Date", TypeName = "datetime")]
        public DateTime CreateDate { get; set; }

        [ForeignKey(nameof(ProjectManager))]
        [InverseProperty(nameof(Employee.Projects))]
        [ValidateNever]
        //public virtual Employee ProjectManagerNavigation { get; set; }
        public virtual Employee Employee { get; set; }
        [ValidateNever]
        public List<ProjectImage> ProjectImages { get; set; }
    }
}
