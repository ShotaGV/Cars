using Cars.Data;
using Cars.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace Cars.Controllers
{

    [ApiController]
    [Route("api/cars")]
    public class CarController : ControllerBase
    {
        private readonly DataContext _dbContext;
  
        public CarController(DataContext dbcontext)
        {
            _dbContext = dbcontext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CarBrand>> GetCars()
        {
            var carBrands = _dbContext.CarBrands
                .Include(brand => brand.Models)
                .ToList();

            return carBrands;
        }
        [HttpGet("ByBrand/{brandName}")]
        public ActionResult<IEnumerable<CarModel>> GetByBrand(string brandName)
        {
            var carModels = _dbContext.CarModels
                .Include(m => m.CarBrand)
                .Where(m => m.CarBrand.Name == brandName)
                .ToList();

            if (carModels == null || carModels.Count == 0)
            {
                return NotFound($"No CarModels found for the specified CarBrand ({brandName})");
            }

            return carModels;
        }
        [HttpPost]
        public ActionResult<CarBrand> CreateCar(CarBrand carBrand)
        {
            _dbContext.CarBrands.Add(carBrand);
            _dbContext.SaveChanges();
            foreach (var model in carBrand.Models)
            {
                model.CarBrandId = carBrand.Id;
                _dbContext.CarModels.Add(model);
            }

            _dbContext.SaveChanges();

            return CreatedAtAction("GetCars", new { id = carBrand.Id }, carBrand);
        }
        [HttpPut("{id}")]
        public IActionResult EditCar(int id, CarBrand updatedCarBrand)
        { 

            var existingCarBrand = _dbContext.CarBrands.Find(id);

            if (existingCarBrand == null)
            {
                return NotFound("Brand Not Found");
            }

            existingCarBrand.Name = updatedCarBrand.Name;

            foreach (var updatedCarModel in updatedCarBrand.Models)
            {
                var existingCarModel = _dbContext.CarModels.Find(id);
                if (existingCarModel == null)
                {
                    return NotFound("Model Not Found");
                }
                existingCarModel.Name = updatedCarModel.Name;
            }
            _dbContext.SaveChanges();

            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var carBrand = _dbContext.CarBrands.Find(id);

            if (carBrand == null)
            {
                return NotFound("CarBrand not found");
            }
            _dbContext.CarBrands.Remove(carBrand);
            _dbContext.SaveChanges();

            return NoContent(); 
        }
        [HttpDelete("all")]
        public IActionResult DeleteAll()
        {
            var carBrands = _dbContext.CarBrands.ToList();


            _dbContext.CarBrands.RemoveRange(carBrands);
            _dbContext.SaveChanges();

            return NoContent();
        }
        [HttpDelete("allModels")]
        public IActionResult DeleteAllModels()
        {
            var carModels = _dbContext.CarModels.ToList();


            _dbContext.CarModels.RemoveRange(carModels);
            _dbContext.SaveChanges();

            return NoContent();
        }
    }
}
