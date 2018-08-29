using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using CustManaging.API.Models;
using CustManaging.API.Models.Business;
using CustManaging.API.Models.ResultSet;
using CustManaging.API.Models.Tables;

namespace CustManaging.API.Controllers
{
    public class CustController : ApiController
    {
        private DBCustManaging db = new DBCustManaging();
        private tbCustBusiness bus = new tbCustBusiness();

        // GET: ApiUlr/Cust
        public IQueryable<tbCust> GettbCust()
        {
            return db.tbCust;
        }

        // GET: ApiUlr/Cust/5
        [ResponseType(typeof(tbCust))]
        public IHttpActionResult GettbCust(int id)
        {
            tbCust tbCust = db.tbCust.Find(id);
            if (tbCust == null)
            {
                return Json("Id não encontrado. Por favor, digite um id válido");
            }

            return Ok(tbCust);
        }

        // PUT: ApiUlr/Cust/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PuttbCust(int id, tbCust tbCust)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbCust.CustId)
            {
                return Json("Ids informados não conferem. Por favor, digite um id válido");
            }

            db.Entry(tbCust).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tbCustExists(id))
                {
                    return Json("Id não encontrado. Por favor, digite um id válido");
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: ApiUlr/Cust
        [ResponseType(typeof(tbCust))]
        public IHttpActionResult PosttbCust(tbCust tbCust)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                tbCust = bus.InsertCust(tbCust);

                var custResult = new CustResult();
                custResult.CustId = tbCust.CustId;
                custResult.Name = tbCust.Name;
                custResult.Email = tbCust.Email;
                custResult.Address = tbCust.Address;
                custResult.Location = tbCust.Location;
                
                return CreatedAtRoute("DefaultApi", new { id = custResult.CustId }, custResult);
            }
            catch(Exception ex)
            {
                return Json("Por favor reveja os parâmetros digitados e envie novamente a solicitação");
            }            
        }

        // DELETE: ApiUlr/Cust/5
        [ResponseType(typeof(tbCust))]
        public IHttpActionResult DeletetbCust(int id)
        {
            tbCust tbCust = db.tbCust.Find(id);
            if (tbCust == null)
            {
                return Json("Id não encontrado. Por favor, digite um id válido");
            }

            db.tbCust.Remove(tbCust);
            db.SaveChanges();

            return Ok("Cliente removido com sucesso");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tbCustExists(int id)
        {
            return db.tbCust.Count(e => e.CustId == id) > 0;
        }
    }
}