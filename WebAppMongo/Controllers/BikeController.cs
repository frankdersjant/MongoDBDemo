using Services.BikeServices;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.JsonPatch;

namespace WebAppMongo.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class BikeController : ControllerBase
    {
        private readonly IBikeService _bikeService;

        public BikeController(IBikeService bikeService)
        {
            this._bikeService = bikeService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Bike>> Get()
        {
            return Ok(_bikeService.GetAllBikes() as List<Bike>);
        }

        [Route("/api/bike/count/")]
        public ActionResult<long> Count()
        {
            return Ok(_bikeService.CountThemAll());
        }

        [Route("/api/bike/GetBikeById/{id}")]
        public ActionResult<Bike> GetBikeById(string id)
        {
            Bike bike = _bikeService.GetSingle(id);

            if (bike == null)
            {
                return NotFound();
            }

            return bike;
        }

        [HttpPost]
        [Route("/api/bike/create/", Name = "Create")]
        public ActionResult<Bike> Create(Bike bike)
        {

            if (bike == null)
            {
                return BadRequest();
            }
            _bikeService.AddBike(bike);

            return Ok(bike);
        }

        [HttpPatch]
        [Route("/api/bike/update/{id}", Name = "Update")]
        public IActionResult Update(string id, [FromBody]JsonPatchDocument<Bike> patchDoc)
        {
            Bike foundbike = _bikeService.GetSingle(id);
            patchDoc.ApplyTo(foundbike);
            _bikeService.UpdateBike(foundbike);

            return Ok();
        }



        [Route("/api/bike/DeleteBike/{id}")]
        public IActionResult DeleteBike(string id)
        {
            Bike bike = _bikeService.GetSingle(id);

            if (bike == null)
            {
                return NotFound();
            }

            _bikeService.RemoveBike(bike);

            return Ok();
        }
    }
}