using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MVCTutorial.Models
{
    [Table("Department")]
    public class Department
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DepartmentID { get; set; }
        public string DepartmentName {  get; set; }

        //public virtual ICollection<Employee> Employees { get; set; }
    }
}
