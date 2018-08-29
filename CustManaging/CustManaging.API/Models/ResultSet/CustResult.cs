using CustManaging.API.Models.Tables;

namespace CustManaging.API.Models.ResultSet
{
    public class CustResult
    {
        public int CustId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public Location Location { get; set; }
    }
}