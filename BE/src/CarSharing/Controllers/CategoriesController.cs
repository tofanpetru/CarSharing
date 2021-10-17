using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoriesManager _categoriesManager;

        public CategoriesController(ICategoriesManager categoriesManager)
        {
            _categoriesManager = categoriesManager;
        }

        [HttpGet("")]
        public IEnumerable<CarCategoryDTO> GetAll()
        {
            try
            {
                return _categoriesManager.GetAllCategories();
            }
            catch
            {
                return null;
            }
        }
    }
}
