using System.ComponentModel.DataAnnotations;

namespace MVCTutorial.Models
{
    public class EmployeeViewModel
    {
        public int EmployeeID { get; set; }

        [Required(ErrorMessage ="Enter Name")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Enter Department")]
        public Nullable<int> DepartmentID { get; set; }
        
        [Required(ErrorMessage = "Enter Address")]
        public string Address { get; set; }

        public bool isDeleted { get; set; }

        //Custom Attributes
        public string DepartmentName {  get; set; }
        public string SiteName { get; set; }
    }
}
