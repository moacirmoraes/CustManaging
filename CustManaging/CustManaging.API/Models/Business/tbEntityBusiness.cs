using CustManaging.API.Models.Tables;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;
using System.Configuration;

namespace CustManaging.API.Models.Business
{
    public class tbEntityBusiness
    {
        public void InsertEntity()
        {
            string jsonPath = ConfigurationManager.AppSettings["EntityJsonPath"].ToString();

            DBCustManaging db = new DBCustManaging();

            var tbentity = new List<tbEntity>();

            using (var reader = new StreamReader(jsonPath))
            {
                JavaScriptSerializer parser = new JavaScriptSerializer();
                tbentity = parser.Deserialize<List<tbEntity>>(reader.ReadToEnd());
            }

            foreach (var entity in tbentity)
            {
                var geoLocation = new tbCustBusiness().getGeoLocation(entity.Address);

                entity.Lat = geoLocation.results[0].geometry.location.Lat;
                entity.Lng = geoLocation.results[0].geometry.location.Lng;
                entity.Distance = 0;

                db.tbEntity.Add(entity);
                db.SaveChanges();
            }
        }

        public string getClosestEntity(double custLat, double custLng)
        {
            DBCustManaging db = new DBCustManaging();

            var entityList = db.tbEntity;

            foreach(var entity in entityList)
            {
                entity.Distance = compareDistance(custLat, custLng,entity.Lat,entity.Lng);
            }

            List<tbEntity> entities = entityList.ToList();

            entities = entities.OrderByDescending(e => e.Distance).ToList();

            return entities.Select(e => e.Id).Last();
        }

        public double compareDistance(double custLat, double custLng, double entLat, double entLng)
        {

            var R = 6371;

            var dLat = (entLat - custLat) * Math.PI / 180;
            var dLon = (entLng - custLng) * Math.PI / 180;

            custLat = custLat * Math.PI / 180;
            entLat = entLat * Math.PI / 180;

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Sin(dLon / 2) * Math.Sin(dLon / 2) * Math.Cos(custLat) * Math.Cos(entLat);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;

        }
    }
}