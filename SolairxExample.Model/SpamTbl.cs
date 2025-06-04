using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolairxExample.Model
{

    [Table("SpamTbls")]
    public partial class SpamTbl
    {
        [Key]
        [Column("Spam_Id")]
        public int SpamId { get; set; }
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
    }
}
