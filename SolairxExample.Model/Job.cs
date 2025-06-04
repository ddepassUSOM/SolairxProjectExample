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
    public partial class Job
    {
        public Job()
        {
            WebClientIntJobs = new HashSet<WebClientIntJob>();
        }

        [Key]
        [Column("Job_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int JobId { get; set; }
        [Required]
        [Column("Job_Name")]
        [StringLength(200)]
        public string? JobName { get; set; }
        [Column("Job_Description")]
        public string? JobDescription { get; set; }
        [Column("Date_Modified", TypeName = "datetime")]
        public DateTime DateModified { get; set; }

        [InverseProperty(nameof(WebClientIntJob.Job))]
        public virtual ICollection<WebClientIntJob> WebClientIntJobs { get; set; }
    }
}
