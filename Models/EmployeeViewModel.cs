namespace MVCTutorial.Models
{
    public class EmployeeViewModel
    {
        public int EmployeeID { get; set; }
        public string Name { get; set; }
        public Nullable<int> DepartmentID { get; set; }
        public string Address { get; set; }

        //Custom Attributes
        public string DepartmentName {  get; set; }
        public bool Remember {  get; set; }
    }
}
