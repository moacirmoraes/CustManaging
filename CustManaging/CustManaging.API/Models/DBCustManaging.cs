using CustManaging.API.Models.Tables;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CustManaging.API.Models
{
    public class DBCustManaging : DbContext
    {
        public DBCustManaging() : base("name=DBcustManaging")
        {

        }

        public DbSet<tbCust> tbCust { get; set; }
        public DbSet<tbEntity> tbEntity { get; set; }
    }
}