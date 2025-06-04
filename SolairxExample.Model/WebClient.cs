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
    [Table("WebClients")]
    public partial class WebClient
    {
        public WebClient()
        {
            WebClientIntJobs = new HashSet<WebClientIntJob>();
        }

        [Key]
        [Column("WC_id")]
        public int WcId { get; set; }
        [Required]
        [Column("First_Name")]
        [StringLength(100)]
        public string FirstName { get; set; }
        [Required]
        [Column("Last_Name")]
        [StringLength(100)]
        public string LastName { get; set; }
        [Required]
        [StringLength(100)]
        public string Email { get; set; }
        [Required]
        [StringLength(50)]
        public string Phone { get; set; }
        public bool Residential { get; set; }
        public bool Commercial { get; set; }
        public string Message { get; set; }
        [Column("Date_Modified", TypeName = "datetime")]
        public DateTime DateModified { get; set; }

        [InverseProperty(nameof(WebClientIntJob.Wc))]
        public virtual ICollection<WebClientIntJob> WebClientIntJobs { get; set; }
    }
}
