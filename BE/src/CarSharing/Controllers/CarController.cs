using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Presentation.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CarController : ControllerBase
    {
        private readonly ICarManager _carManager;

        public CarController(ICarManager carManager)
        {
            _carManager = carManager;
        }

        [HttpGet]
        public IEnumerable<AllCarsDTO> Get()
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
    }
}
