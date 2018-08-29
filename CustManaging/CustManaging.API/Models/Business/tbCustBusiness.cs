using CustManaging.API.Models.ResultSet;
using CustManaging.API.Models.Tables;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Script.Serialization;


namespace CustManaging.API.Models.Business
{
    public class tbCustBusiness
    {
        public string GoogleUrl = ConfigurationManager.AppSettings["GoogleURLLocalizar"].ToString();

        public tbCust InsertCust(tbCust cust)
        {
            DBCustManaging db = new DBCustManaging();

            var bus = new tbEntityBusiness();

            if (db.tbEntity.FirstOrDefault() == null)
            {
                bus.InsertEntity();
            }

            var geoLocation = this.getGeoLocation(cust.Address);

            cust.Location = new Tables.Location()
            {
                Lat = geoLocation.results[0].geometry.location.Lat,
                Lng = geoLocation.results[0].geometry.location.Lng
            };

            cust.LegalEntityId = bus.getClosestEntity(cust.Location.Lat, cust.Location.Lng);

            db.tbCust.Add(cust);
            db.SaveChanges();

            return cust;
        }
        
        public LocationResult getGeoLocation(string address)
        {
            string Url = GoogleUrl + address;

            try
            {
                var request = WebRequest.Create(Url);

                request.Proxy.Credentials = CredentialCache.DefaultCredentials;
                request.UseDefaultCredentials = true;
                ((HttpWebRequest)request).UserAgent = ".NET Framework";

                var response = request.GetResponse();

                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    JavaScriptSerializer parser = new JavaScriptSerializer();
                    return parser.Deserialize<LocationResult>(reader.ReadToEnd());
                }
            }
            catch(WebException ex)
            {
                return null;
            }

        }

    }
}