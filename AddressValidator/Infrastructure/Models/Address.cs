namespace AddressValidator.Infrastructure.Models
{
    public class Address
    {
        public string StreetNumber { get; set; }
        public string Subremise { get; set; }
        public string Route { get; set; }
        public string Locality { get; set; }
        public string AdministrativeAreaLevel1 { get; set; }
        public string AdministrativeAreaLevel2 { get; set; }
    }
}