using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
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
        public IEnumerable<AllCarsDTO> GetAllCars()
        {
            try
            {
                return _carManager.GetAll();
            }
            catch
            {
                return null;
            }
        }

        [HttpGet("GetHomePageCars")]
        public IEnumerable<HomePageCarsDTO> GetHomePageCars()
        {
            try
            {
                return _carManager.GetLastThreeAvalableCars();
            }
            catch
            {
                return null;
            }
        }

        [HttpGet("Car/Details/{id}")]
        public CarDetailsDTO Details(int id)
        {
            try
            {
                return _carManager.Get(id);
            }
            catch
            {

                return null;
            }
        }
    }
}
