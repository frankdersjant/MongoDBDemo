using Marvin.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using WebAppClient.Helpers;
using WebAppClient.Models;
using WebAppClient.ViewModels;

namespace WebAppClient.Controllers
{
    public class HomeController : Controller
    {

        public async Task<IActionResult> Index()
        {
            HttpClient client = MVCClientHttpClient.GetClient();
            HttpResponseMessage bikesResponse = await client.GetAsync("api/bike/");

            BikesVM AllBikesVM = new BikesVM();

            if (bikesResponse.IsSuccessStatusCode)
            {
                string Content = await bikesResponse.Content.ReadAsStringAsync();
                AllBikesVM.lstBikes = JsonConvert.DeserializeObject<IEnumerable<Bike>>(Content);
            }
            else
            {
                return Content("An error occurred.");
            }

            return View(AllBikesVM);
        }

        public async Task<IActionResult> CountThem()
        {
            long noOfBikes;
            HttpClient client = MVCClientHttpClient.GetClient();
            HttpResponseMessage bikesResponse = await client.GetAsync("/api/bike/count/");

            if (bikesResponse.IsSuccessStatusCode)
            {
                string Content = await bikesResponse.Content.ReadAsStringAsync();
                noOfBikes = JsonConvert.DeserializeObject<long>(Content);

            }
            else
            {
                return Content("An error occurred.");
            }
            return Content(noOfBikes.ToString());
        }

        public async Task<IActionResult> Get(string id)
        {
            HttpClient client = MVCClientHttpClient.GetClient();

            HttpResponseMessage bikesResponse = await client.GetAsync("/api/bike/getbikebyid/" + id);

            if (bikesResponse.IsSuccessStatusCode)
            {
                string Content = await bikesResponse.Content.ReadAsStringAsync();
                var foundBike = JsonConvert.DeserializeObject<Bike>(Content);
            }
            else
            {
                return Content("Bike not found.");
            }
            return null;
        }


        [HttpGet]
        public IActionResult Create()
        {
            BikeVM bikeVM = new BikeVM();
            return View(bikeVM);
        }

        [HttpPost]
        public async Task<IActionResult> Create(BikeVM bikeVM)
        {
            HttpClient client = MVCClientHttpClient.GetClient();
            var SerializedItemToCreate = JsonConvert.SerializeObject(bikeVM);

            HttpResponseMessage bikesResponse = await client.PostAsync("/api/bike/create/",
                                                new StringContent(SerializedItemToCreate,
                                                System.Text.Encoding.Unicode,
                                                "application/json"));

            if (bikesResponse.IsSuccessStatusCode)
            {
                return this.RedirectToAction("Index");
            }
            return this.RedirectToAction("Error");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string Id)
        {
            HttpClient client = MVCClientHttpClient.GetClient();
            HttpResponseMessage bikesResponse = await client.GetAsync("/api/bike/getbikebyid/" + Id);

            if (bikesResponse.IsSuccessStatusCode)
            {

                BikeVM bikeVM = new BikeVM();

                string Content = await bikesResponse.Content.ReadAsStringAsync();
                Bike foundBike = JsonConvert.DeserializeObject<Bike>(Content);
                bikeVM.Id = foundBike.Id;
                bikeVM.Brand = foundBike.Brand;

                return View(bikeVM);
            }
            return Content("An error occurred.");

        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, BikeVM bikeVM)
        {
            JsonPatchDocument<Bike> patchDoc = new JsonPatchDocument<Bike>();
            patchDoc.Replace(e => e.Brand, bikeVM.Brand);

            //serialize patch
            var serializedPatch = JsonConvert.SerializeObject(patchDoc);

            HttpClient client = MVCClientHttpClient.GetClient();

            HttpResponseMessage bikesResponse = await client.PatchAsync("/api/bike/update/"+ id, 
                                                new StringContent(serializedPatch, System.Text.Encoding.Unicode,
                                                "application/json"));


            if (bikesResponse.IsSuccessStatusCode)
            {
                return this.RedirectToAction("Index");
            }

            return Content("An error occurred.");
        }

        public async Task<IActionResult> Delete(string Id)
        {
            HttpClient client = MVCClientHttpClient.GetClient();
            HttpResponseMessage bikesResponse = await client.DeleteAsync("/api/bike/deletebike/" + Id);

            if (bikesResponse.IsSuccessStatusCode)
            {
                return this.RedirectToAction("Index");
            }
            return Content("An error occurred.");
        }
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
