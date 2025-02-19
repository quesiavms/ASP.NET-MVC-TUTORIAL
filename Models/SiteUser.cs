using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.Graph.Models;

namespace MVCTutorial.Models
{
    [Table("SiteUser")]
    public class SiteUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string EmailId { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }

        public Nullable<int> RoleID { get; set; }

        [ForeignKey("RoleID")]
        public virtual UserRole UserRole { get; set; }
    }
}
