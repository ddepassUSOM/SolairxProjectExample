using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SolairxExample.Model
{
    [Table("WebClientIntJobs")]
    public partial class WebClientIntJob
    {
        [Key]
        [Column("WC_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WcId { get; set; }
        [Key]
        [Column("Job_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int JobId { get; set; }

        [ForeignKey(nameof(JobId))]
        [InverseProperty("WebClientIntJobs")]
        public virtual Job? Job { get; set; }
        [ForeignKey(nameof(WcId))]
        [InverseProperty(nameof(WebClient.WebClientIntJobs))]
        public virtual WebClient Wc { get; set; }
    }
}
