using System.Runtime.InteropServices;
using Application.Abstractions;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductAPI.DTOs;
using Serilog;
namespace ProductAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
  
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;           
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductDTO productDTO)
        {
            if (productDTO is null)
            {
                Log.Error("ProductDTO is null. While creating, product cannot be null.");
                return BadRequest("ProductDTO cannot be null.");
            }

            var category = await _unitOfWork.Category.GetByIdAsync(productDTO.CategoryID);

            if (category == null)
            {
                Log.Error($"Category with ID {productDTO.CategoryID} not found.");
                return BadRequest($"Category with ID {productDTO.CategoryID} not found.");
            }                     

            var result = await _unitOfWork.Product.AddAsync(new()
            {
                Name = productDTO.Name,
                Price = productDTO.Price,
                CategoryID = productDTO.CategoryID,
                Category = category

            });

            if (!result)
            {
                Log.Error("Error occurred while adding product.");
                return BadRequest("Error occurred while adding product.");
            }

            await _unitOfWork.SaveAsync();

            return CreatedAtAction(nameof(GetProductById), new { id = productDTO.Id }, productDTO);
        }


        [HttpPut("{id:guid}")]
        public async Task<ActionResult> UpdateProduct(Guid id, [FromBody] ProductDTO productDTO)
        {
            if (productDTO is null)
            {
                Log.Error("ProductDTO is null. Cannot update product.");
                return BadRequest("Product cannot be null");
            }

            var existingProduct = await _unitOfWork.Product.GetByIdAsync(id);
            if (existingProduct is null)
            {
                Log.Error("Product could not be found");
                return NotFound();
            }

            // Güncellemeleri mevcut ürüne uygula
            existingProduct.Name = productDTO.Name;
            existingProduct.Price = productDTO.Price;
            existingProduct.CategoryID = productDTO.CategoryID;

            // Ürünü güncelle
            _unitOfWork.Product.Update(existingProduct);

            await _unitOfWork.SaveAsync();

            return NoContent();
        }


        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteProduct(Guid id)
        {
            var existingProduct = await _unitOfWork.Product.GetByIdAsync(id);
            if (existingProduct is null)
            {
                Log.Error("Product could not found");
                return NotFound();
            }

            _unitOfWork.Product.Remove(existingProduct);
            await _unitOfWork.SaveAsync();
            return NoContent();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllProducts()
        {
            try
            {
                var products = await _unitOfWork.Product.GetAll().ToListAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                Log.Error("An error occuor while getting products - " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Product>> GetProductById(Guid id)
        {
            var product = await _unitOfWork.Product.GetByIdAsync(id);
            if (product is null)
            {
                Log.Error("Product could not find in database!");
                return NotFound();
            }

            return Ok(product);
        }

        [HttpGet("getall-with-category")]
        public async Task<IActionResult> GetAllProductWithCategory()
        {
            // Ürünleri ve ilişkili kategorileri veritabanından al
            var productsWithCategories = await _unitOfWork.Product
                .GetAll() // Burada ürünlerin listesi alınır
                .Include(p => p.Category) // Ürünlerin ilişkili kategorilerini de dahil et
                .ToListAsync();

           
            if (productsWithCategories == null || !productsWithCategories.Any())
            {
                return NotFound("No products found.");
            }

            
            var result = productsWithCategories.Select(product => new
            {
                product.Id,
                product.Name,
                product.Price,
                Category = product.Category != null ? new
                {
                    product.Category.Id,
                    product.Category.Name
                } : null 
            });

            return Ok(result);
        }

    }
}
