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
    public class StoreController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static StoreController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44350/api/storedata/");
        }

        // GET: Store/List
        public ActionResult List()
        {
            //objective: communicative with our store data api to retrieve a list of stores
            // curl https://localhost:44350/api/storedata/liststores

            string url = "liststores";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<StoreDto> stores = response.Content.ReadAsAsync<IEnumerable<StoreDto>>().Result;
            //Debug.WriteLine("Number of stores received: ");
            //Debug.WriteLine(stores.Count());

            return View(stores);
        }

        // GET: Store/Details/5
        public ActionResult Details(int id)
        {
            //objective: communicative with our store data api to retrieve one store
            // curl https://localhost:44350/api/storedata/findstore/{id}

            string url = "findstore/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            StoreDto selectedStore = response.Content.ReadAsAsync<StoreDto>().Result;
            Debug.WriteLine("Store received: ");
            Debug.WriteLine(selectedStore.StoreName);
            

            return View(selectedStore);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Store/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Store/Create
        [HttpPost]
        public ActionResult Create(Store store)
        {
            Debug.WriteLine("the json payload is");
            //Debug.WriteLine(store.StoreName);
            //objective: add a new store into our system using the API
            //curl -H "Content-Type:application/json" -d @store.json https://localhost:44350/api/storedata/addstore

            string url = "addstore";

            string jsonpayload = jss.Serialize(store);

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
            
        }

        // GET: Store/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Store/Edit/5
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

        // GET: Store/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Store/Delete/5
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
