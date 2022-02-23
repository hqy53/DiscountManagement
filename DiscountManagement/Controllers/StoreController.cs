using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using DiscountManagement.Models;
using DiscountManagement.Models.ViewModels;
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
            client.BaseAddress = new Uri("https://localhost:44350/api/");
        }

        // GET: Store/List
        public ActionResult List()
        {
            //objective: communicative with our store data api to retrieve a list of stores
            // curl https://localhost:44350/api/storedata/liststores

            string url = "storedata/liststores";
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
            DetailsStore ViewModel = new DetailsStore();

            //objective: communicative with our store data api to retrieve one store
            // curl https://localhost:44350/api/storedata/findstore/{id}

            string url = "storedata/findstore/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            StoreDto SelectedStore = response.Content.ReadAsAsync<StoreDto>().Result;
            Debug.WriteLine("Store received: ");
            Debug.WriteLine(SelectedStore.StoreName);

            ViewModel.SelectedStore = SelectedStore;

            //show associated products with this store
            url = "productdata/listproductsforstore/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<ProductDto> SellingProducts = response.Content.ReadAsAsync<IEnumerable<ProductDto>>().Result;

            ViewModel.SellingProducts = SellingProducts;

            url = "productdata/listproductsnotforstore/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<ProductDto> AvailableProducts = response.Content.ReadAsAsync<IEnumerable<ProductDto>>().Result;

            ViewModel.AvailableProducts = AvailableProducts;

            return View(ViewModel);
        }
        //POST: Store/Associate/{storeid}
        [HttpPost]
        public ActionResult Associate(int id, int ProductID)
        {
            Debug.WriteLine("Attempting to associate store :" + id + " with product " + ProductID);

            //call our api to associate store with product
            string url = "storedata/associatestorewithproduct/" + id + "/" + ProductID;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }
        //Get: Store/UnAssociate/{id}?ProductID={productID}
        [HttpGet]
        public ActionResult UnAssociate(int id, int ProductID)
        {
            Debug.WriteLine("Attempting to unassociate store :" + id + " with product: " + ProductID);

            //call our api to associate store with product
            string url = "storedata/unassociatestorewithproduct/" + id + "/" + ProductID;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
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

            string url = "storedata/addstore";

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
            UpdateStore ViewModel = new UpdateStore();
            //the existing store information
            string url = "storedata/findstore/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            StoreDto SelectedStore = response.Content.ReadAsAsync<StoreDto>().Result;
            ViewModel.SelectedStore = SelectedStore;

            return View(ViewModel);
        }

        // POST: Store/Update/5
        [HttpPost]
        public ActionResult Update(int id, Store store)
        {
            string url = "storedata/updatestore/" + id;
            string jsonpayload = jss.Serialize(store);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Store/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "storedata/findstore/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            StoreDto selectedstore = response.Content.ReadAsAsync<StoreDto>().Result;
            return View(selectedstore);
        }

        // POST: Store/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "storedata/deletestore/" + id;
            HttpContent content = new StringContent("");
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
    }
}
