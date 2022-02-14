using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using DiscountManagement.Models;
using System.Diagnostics;

namespace DiscountManagement.Controllers
{
    public class StoreDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/StoreData/ListStores
        [HttpGet]
        public IEnumerable<StoreDto> ListStores()
        {
            List<Store> Stores = db.Stores.ToList();
            List<StoreDto> StoreDtos = new List<StoreDto>();

            Stores.ForEach(a => StoreDtos.Add(new StoreDto()
            {
                StoreID = a.StoreID,
                StoreName = a.StoreName,
                DiscountCode = a.DiscountCode,
                DiscountPercentage = a.DiscountPercentage
            }));
            return StoreDtos;
        }

        // GET: api/StoreData/FindStore/5
        [HttpGet]
        [ResponseType(typeof(Store))]
        public IHttpActionResult FindStore(int id)
        {
            Store Store = db.Stores.Find(id);
            StoreDto StoreDto = new StoreDto()
            {
                StoreID = Store.StoreID,
                StoreName = Store.StoreName,
                DiscountCode = Store.DiscountCode,
                DiscountPercentage = Store.DiscountPercentage
            };
            if (Store == null)
            {
                return NotFound();
            }

            return Ok(StoreDto);
        }

        // POST: api/StoreData/UpdateStore/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateStore(int id, Store store)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != store.StoreID)
            {
                return BadRequest();
            }

            db.Entry(store).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StoreExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/StoreData/AddStore
        [ResponseType(typeof(Store))]
        [HttpPost]
        public IHttpActionResult AddStore(Store store)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Stores.Add(store);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = store.StoreID }, store);
        }

        // POST: api/StoreData/DeleteStore/5
        [ResponseType(typeof(Store))]
        [HttpPost]
        public IHttpActionResult DeleteStore(int id)
        {
            Store store = db.Stores.Find(id);
            if (store == null)
            {
                return NotFound();
            }

            db.Stores.Remove(store);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StoreExists(int id)
        {
            return db.Stores.Count(e => e.StoreID == id) > 0;
        }
    }
}