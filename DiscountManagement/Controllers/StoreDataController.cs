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

        /// <summary>
        /// Returns all stores in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all stores in the database, 
        /// </returns>
        /// <example>
        /// GET: api/StoreData/ListStores
        /// </example>
        [HttpGet]
        [ResponseType(typeof(StoreDto))]
        public IHttpActionResult ListStores()
        {
            List<Store> Stores = db.Stores.ToList();
            List<StoreDto> StoreDtos = new List<StoreDto>();

            Stores.ForEach(s => StoreDtos.Add(new StoreDto()
            {
                StoreID = s.StoreID,
                StoreName = s.StoreName,
                DiscountCode = s.DiscountCode,
                DiscountPercentage = s.DiscountPercentage
            }));
            return Ok(StoreDtos);
        }
        /// <summary>
        /// Gathers information about stores related to a particular product
        /// </summary>
        /// <param name="id">Product ID</param>
        /// <returns>
        /// CONTENT: all stores in the database
        /// </returns>
        /// <example>
        /// GET: api/StoreData/ListStoresForProduct/3
        /// </example>
        [HttpGet]
        [ResponseType(typeof(StoreDto))]
        public IHttpActionResult ListStoresForProduct(int id)
        {
            //all stores that have products which match with our ID
            List<Store> Stores = db.Stores.Where(
                s => s.Products.Any(
                    p => p.ProductID == id
                    )).ToList();
            List<StoreDto> StoreDtos = new List<StoreDto>();

            Stores.ForEach(s => StoreDtos.Add(new StoreDto()
            {
                StoreID = s.StoreID,
                StoreName = s.StoreName,
                DiscountCode = s.DiscountCode,
                DiscountPercentage = s.DiscountPercentage
            }));
            return Ok(StoreDtos);
        }
        /// <summary>
        /// Associates a particular product with a particular store
        /// </summary>
        /// <param name="storeid">The store ID primary key</param>
        /// <param name="productid">The product ID primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST api/StoreData/AssociateStoreWithProduct/8/7
        /// </example>
        [HttpPost]
        [Route("api/StoreData/AssociateStoreWithProduct/{storeid}/{productid}")]
        public IHttpActionResult AssociateStoreWithProduct(int storeid, int productid)
        {
            Store SelectedStore = db.Stores.Include(s => s.Products).Where(s => s.StoreID == storeid).FirstOrDefault();
            Product SelectedProduct = db.Products.Find(productid);

            if (SelectedStore == null || SelectedProduct == null)
            {
                return NotFound();
            }

            Debug.WriteLine("input store id is: " + storeid);
            Debug.WriteLine("selected store name is: " + SelectedStore.StoreName);
            Debug.WriteLine("input product id is: " + productid);
            Debug.WriteLine("selected product name is: " + SelectedProduct.ProductName);


            SelectedStore.Products.Add(SelectedProduct);
            db.SaveChanges();

            return Ok();
        }
        /// <summary>
        /// Removes an association between a particular product and a particular store
        /// </summary>
        /// <param name="storeid">The store ID primary ke</param>
        /// <param name="productid">The product ID primary key</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST api/StoreData/UnAssociateStoreWithProduct/2/3
        /// </example>
        [HttpPost]
        [Route("api/StoreData/UnAssociateStoreWithProduct/{storeid}/{productid}")]
        public IHttpActionResult UnAssociateStoreWithProduct(int storeid, int productid)
        {
            Store SelectedStore = db.Stores.Include(s => s.Products).Where(s => s.StoreID == storeid).FirstOrDefault();
            Product SelectedProduct = db.Products.Find(productid);

            if (SelectedStore == null || SelectedProduct == null)
            {
                return NotFound();
            }

            Debug.WriteLine("input store id is: " + storeid);
            Debug.WriteLine("selected store name is: " + SelectedStore.StoreName);
            Debug.WriteLine("input product id is: " + productid);
            Debug.WriteLine("selected product name is: " + SelectedProduct.ProductName);


            SelectedStore.Products.Remove(SelectedProduct);
            db.SaveChanges();

            return Ok();
        }
        /// <summary>
        /// Returns all stores in the system.
        /// </summary>
        /// <param name="id">The primary key of the store</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An store in the system matching up to the store ID primary key
        /// </returns>
        /// <example>
        /// GET: api/StoreData/FindStore/5
        /// </example>  
        [ResponseType(typeof(StoreDto))]
        [HttpGet]
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


        /// <summary>
        /// Updates a particular store in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Store ID primary key</param>
        /// <param name="store">JSON FORM DATA of an store</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/StoreData/UpdateStore/5
        /// FORM DATA: Store JSON Object
        /// </example>
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

        /// <summary>
        /// Adds a store to the system
        /// </summary>
        /// <param name="store">JSON FORM DATA of a store</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Animal ID, Animal Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/StoreData/AddStore
        /// FORM DATA: Store JSON Object
        /// </example>
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

        /// <summary>
        /// Deletes a store from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the store</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/StoreData/DeleteStore/5
        /// FORM DATA: (empty)
        /// </example>
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