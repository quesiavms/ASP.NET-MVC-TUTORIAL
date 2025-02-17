using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.Graph.Models;

namespace MVCTutorial.Models
{
    [Table("Employee")]
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeID { get; set; }
        public string Name { get; set; }
        public Nullable<int> DepartmentID { get; set; }
        public string Address {  get; set; }
        public bool isDeleted { get; set; }

        public virtual Sites Sites { get; set; }

        public virtual Department Department { get; set; }
    }
}
