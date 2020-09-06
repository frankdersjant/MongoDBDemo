using Marvin.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using WebAppMVCClient.Helpers;
using WebAppMVCClient.ViewModels;

namespace WebAppMVCClient.Controllers
{
    public class HomeController : Controller
    {

        public HomeController()
        {

        }

        public async Task<IActionResult> Index()
        {
            HttpClient client = MVCClientHttpClient.GetClient();
            HttpResponseMessage bikesResponse = await client.GetAsync("api/bike/");

            var vm = new BikesVM();

            if (bikesResponse.IsSuccessStatusCode)
            {
                string Content = await bikesResponse.Content.ReadAsStringAsync();
                var lstBikes = JsonConvert.DeserializeObject<IEnumerable<Bike>>(Content);
                vm.lstBikes = lstBikes;
            }
            else
            {
                return Content("An error occurred.");
            }


            //return View();
            return Content("Done");
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

        public async Task<IActionResult> Create(Bike bike)
        {
            //Bike b_ike = new Bike();
            bike.Brand = "Bimota";

            HttpClient client = MVCClientHttpClient.GetClient();

            var SerializedItemToCreate = JsonConvert.SerializeObject(bike);

            HttpResponseMessage bikesResponse = await client.PostAsync("/api/bike/create/",
                                                new StringContent(SerializedItemToCreate,
                                                System.Text.Encoding.Unicode,
                                                "application/json"));

            if (bikesResponse.IsSuccessStatusCode)
            {
                return this.RedirectToAction("Index");
            }
            return Content("An error occurred.");
        }

        public async Task<IActionResult> Edit(string Id)
        {
            HttpClient client = MVCClientHttpClient.GetClient();

            Id = "5e4e6ef521f13f5c68937cf2";

            HttpResponseMessage bikesResponse = await client.GetAsync("/api/bike/getbikebyid/" + Id);
            BikeVM bikevm = new BikeVM();

            if (bikesResponse.IsSuccessStatusCode)
            {
                string Content = await bikesResponse.Content.ReadAsStringAsync();
                var foundBike = JsonConvert.DeserializeObject<Bike>(Content);
                return View(foundBike);

            }
            return Content("An error occurred.");
        }


        public async Task<IActionResult> Edit(string Id, Bike bike)
        {
            HttpClient client = MVCClientHttpClient.GetClient();

            Id = "5e4e6ef521f13f5c68937cf2";

            JsonPatchDocument<Bike> patchDoc = new JsonPatchDocument<Bike>();
            patchDoc.Replace(e => e.Brand, bike.Brand);

            //serialze patch
            var serilaizedPatch = JsonConvert.SerializeObject(patchDoc);

            HttpResponseMessage bikesResponse = await client.PatchAsync("/api/bike/update/" + Id, 
                                                new StringContent(serilaizedPatch, System.Text.Encoding.Unicode,
                                                "application/json"));

            if (bikesResponse.IsSuccessStatusCode)
            {
                return this.RedirectToAction("Index");
                
                //or
                //this.redicttoaction("Details", "Home" , new { id = bike.id});
            }

            return Content("An error occurred.");
        }

            public async Task<IActionResult> Delete(string Id)
        {
            HttpClient client = MVCClientHttpClient.GetClient();
            HttpResponseMessage bikesResponse = await client.GetAsync("/api/bike/deletebike/" + Id);

            if (bikesResponse.IsSuccessStatusCode)
            {
                return this.RedirectToAction("Index");
            }
            return Content("An error occurred.");
        }

        public IActionResult Error()
        {
            return Content("Error");
        }
    }
}



