using Application.Interfaces;
using Domain.Entities;
using Domain.Entities.Pagination;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Presentation.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api")]
    public class CarController : ControllerBase
    {
        private readonly ICarManager _carManager;

        public CarController(ICarManager carManager)
        {
            _carManager = carManager;
        }

        [HttpGet("GetAllCars")]
        public IActionResult GetAllCars([FromQuery] CarParameters carParameters)
        {
            var cars = _carManager.GetPagedCars(carParameters);

            var metadata = new
            {
                cars.TotalCount,
                cars.PageSize,
                cars.CurrentPage,
                cars.TotalPages,
                cars.HasNext,
                cars.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(cars);
        }

        [HttpGet("GetHomePageCars")]
        public IEnumerable<HomePageCarsDTO> GetHomePageCars()
        {
            return _carManager.GetLastThreeAvalableCars();
        }

        [HttpGet("Car/specifications")]
        public IEnumerable<CarsSpecificationsDTO> GetCarSpecifications()
        {
            return _carManager.GetCarsSpecifications();
        }

        [HttpGet("Car/Details/{id}")]
        public CarDetailsDTO Details(int id)
        {
            return _carManager.Get(id);
        }
    }
}
