namespace MVCTutorial.Models
{
    public class EmployeeViewModel
    {
        public int EmployeeID { get; set; }
        public string Name { get; set; }
        public Nullable<int> DepartmentID { get; set; }
        public string Address { get; set; }

        public string DepartmentName {  get; set; }
    }
}
