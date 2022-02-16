using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using DiscountManagement.Models;
using System.Web.Script.Serialization;


namespace DiscountManagement.Controllers
{
    public class ProductController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static ProductController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44350/api/productdata/");
        }
        // GET: Product/List
        public ActionResult List()
        {
            //objective: communicate with our product data api to retrieve a list of products
            //curl https://localhost:44350/api/productdata/listproducts

            string url = "listproducts";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is: ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<ProductDto> products = response.Content.ReadAsAsync<IEnumerable<ProductDto>>().Result;
            
            return View(products);
        }

        // GET: Product/Details/5
        public ActionResult Details(int id)
        {
            //objective: communicate with our product data api to retrieve one product
            //curl https://localhost:44350/api/productdata/findproduct/{id}

            string url = "findproduct/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is: ");
            Debug.WriteLine(response.StatusCode);

            ProductDto selectedProduct = response.Content.ReadAsAsync<ProductDto>().Result;
            Debug.WriteLine("Product received: ");
            Debug.WriteLine(selectedProduct.ProductName);

            return View(selectedProduct);
        }
        public ActionResult Error()
        {
            return View();
        }
        // GET: Product/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        public ActionResult Create(Product product)
        {
            Debug.WriteLine("The json payload is: ");
            //Debug.WriteLine(product.ProductName);
            //objective: add a new animal into our system using the API
            //curl -H "Content-Type:application/json" -d @product.json https://localhost:44350/api/productdata/addproduct
            string url = "addproduct";

            string jsonpayload = jss.Serialize(product);

            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }

            return RedirectToAction("List");
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Product/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Product/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Product/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
