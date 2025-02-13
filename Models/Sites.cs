using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MVCTutorial.Models
{
    [Table("Site")]
    public class Sites
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int SiteID { get; set; }
        public int EmployeeID { get; set; }
        public string SiteName { get; set; }
    }
}
