using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarBrandsController : ControllerBase
    {
        private readonly ICarBrandManager _carBrandsManager;

        public CarBrandsController(ICarBrandManager carBrandsManager)
        {
            _carBrandsManager = carBrandsManager;
        }

        [HttpGet("")]
        public IEnumerable<CarBrandDTO> GetAll()
        {
            try
            {
                return _carBrandsManager.GetAllCarBrands();
            }
            catch
            {
                return null;
            }
        }
    }
}
