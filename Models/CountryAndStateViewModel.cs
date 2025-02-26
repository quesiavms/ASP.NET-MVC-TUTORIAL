namespace MVCTutorial.Models
{
    public class CountryAndStateViewModel
    {
        public int CountryID { get; set; }
        public int StateID { get; set; }

        public List<Country> CountryList { get; set; }

    }
}
