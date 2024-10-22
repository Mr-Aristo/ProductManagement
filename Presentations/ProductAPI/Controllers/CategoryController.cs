using Application.Abstractions;
using Domain.Entities;
using Infrastructure.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductAPI.DTOs;
using Serilog;

namespace ProductAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitofwork;       
        public CategoryController(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;           
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAllCategory()
        {
            var result = await _unitofwork.Category.GetAll().ToListAsync();
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ProductDTO>> GetCategoryById(Guid id)
        {
            var category =  await _unitofwork.Category.GetByIdAsync(id);

            if (category == null)
            {
                Log.Error("Category not found");
                return NotFound();
            }

            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDTO category)
        {
            if (category is null)
            {
                Log.Error("Catagory is null! While creating,category cannot be null");
                return BadRequest("Catagory is null! While creating,category cannot be null");
            }

            var result = await _unitofwork.Category.AddAsync(new()
            {
                Name = category.Name
            });

            if (!result)
            {
                return BadRequest("Error occurred while adding the category.");
            }

            await _unitofwork.SaveAsync();

            return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> UpdateCategory(Guid id, [FromBody] CategoryDTO categoryDto)
        {
            if (categoryDto is null)
            {
                Log.Error("Category is null");
                return BadRequest("Category is null");
            }
            var existedCategory = await _unitofwork.Category.GetByIdAsync(id);

            if(existedCategory is null)
            {
                Log.Error("Category not found");
                return NotFound();
            }

            existedCategory.Name = categoryDto.Name;

            _unitofwork.Category.Update(existedCategory);
            await _unitofwork.SaveAsync();
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteCategory(Guid id)
        {
            var existingCategory = await _unitofwork.Category.GetByIdAsync(id);
            if (existingCategory == null)
            {
                Log.Error("Category not found");
                return NotFound();
            }

            _unitofwork.Category.Remove(existingCategory);
            await _unitofwork.SaveAsync(); 
            return NoContent();
        }

    }    
}
