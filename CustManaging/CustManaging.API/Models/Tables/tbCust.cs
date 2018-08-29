using System.ComponentModel.DataAnnotations;

namespace CustManaging.API.Models.Tables
{
    public class tbCust
    {
        [Key]
        public int CustId { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public virtual Location Location { get; set; }
        public string LegalEntityId { get; set; }
    }

    public class Location
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }
}