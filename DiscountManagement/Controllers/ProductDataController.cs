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
    public class ProductDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all Products in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Products in the database
        /// </returns>
        /// <example>
        /// GET: api/ProductData/ListProducts
        /// </example>
        [HttpGet]
        [ResponseType(typeof(ProductDto))]
        public IHttpActionResult ListProducts()
        {
            List<Product> Products = db.Products.ToList();
            List<ProductDto> ProductDtos = new List<ProductDto>();

            Products.ForEach(p => ProductDtos.Add(new ProductDto()
            {
                ProductID = p.ProductID,
                ProductName = p.ProductName,
                Price = p.Price,
                URL = p.URL
            }));

            return Ok(ProductDtos);
        }

        /// <summary>
        /// Returns all Products in the system 
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Products in the database that are sold by a particular store
        /// </returns>
        /// <param name="id">Store Primary Key</param>
        /// <example>
        /// GET: api/ProductData/ListProductsForStore/2
        /// </example>
        [HttpGet]
        [ResponseType(typeof(ProductDto))]
        public IHttpActionResult ListProductsForStore(int id)
        {
            List<Product> Products = db.Products.Where(
                p => p.Stores.Any(
                    s => s.StoreID == id)
                ).ToList();
            List<ProductDto> ProductDtos = new List<ProductDto>();

            Products.ForEach(p => ProductDtos.Add(new ProductDto()
            {
                ProductID = p.ProductID,
                ProductName = p.ProductName,
                Price = p.Price,
                URL = p.URL
            }));

            return Ok(ProductDtos);
        }

        /// <summary>
        /// Returns Products in the system not sold by a particular store
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Products in the database not sold by a particular store
        /// </returns>
        /// <param name="id">Store Primary Key</param>
        /// <example>
        /// GET: api/ProductData/ListProductsNotForStore/4
        /// </example>
        [HttpGet]
        [ResponseType(typeof(ProductDto))]
        public IHttpActionResult ListProductsNotForStore(int id)
        {
            List<Product> Products = db.Products.Where(
                 p => !p.Stores.Any(
                      s => s.StoreID == id)
             ).ToList();
            List<ProductDto> ProductDtos = new List<ProductDto>();

            Products.ForEach(p => ProductDtos.Add(new ProductDto()
            {
                ProductID = p.ProductID,
                ProductName = p.ProductName,
                Price = p.Price,
                URL = p.URL
            }));

            return Ok(ProductDtos);
        }

        /// <summary>
        /// Returns all Products in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: A product in the system matching up to the Store ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the Product</param>
        /// <example>
        /// GET: api/ProductData/FindProduct/3
        /// </example>
        [ResponseType(typeof(ProductDto))]
        [HttpGet]
        public IHttpActionResult FindProduct(int id)
        {
            Product Product = db.Products.Find(id);
            ProductDto ProductDto = new ProductDto()
            {
                ProductID = Product.ProductID,
                ProductName = Product.ProductName,
                Price = Product.Price,
                URL = Product.URL
            };
            if (Product == null)
            {
                return NotFound();
            }

            return Ok(ProductDto);
        }

        /// <summary>
        /// Updates a particular Product in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Producy ID primary key</param>
        /// <param name="product">JSON FORM DATA of an Product</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/ProductData/UpdateProduct/5
        /// FORM DATA: Product JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateProduct(int id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.ProductID)
            {
                return BadRequest();
            }

            db.Entry(product).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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
        /// Adds a Product to the system
        /// </summary>
        /// <param name="product">JSON FORM DATA of a Product</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Product ID, Product Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/ProductData/AddProduct
        /// FORM DATA: Product JSON Object
        /// </example>
        [ResponseType(typeof(Product))]
        [HttpPost]
        public IHttpActionResult AddProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Products.Add(product);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = product.ProductID }, product);
        }

        /// <summary>
        /// Deletes a Product from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the Product</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/ProductData/DeleteProduct/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Product))]
        [HttpPost]
        public IHttpActionResult DeleteProduct(int id)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            db.Products.Remove(product);
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

        private bool ProductExists(int id)
        {
            return db.Products.Count(e => e.ProductID == id) > 0;
        }
    }
}