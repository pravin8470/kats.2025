using Core.Entities;
using Core.UseCases;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProductRepository _repo) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts()
        { 
            var products=await _repo.GetProductsAsync();
            if (products == null||!products.Any())
            {
                return NotFound("No Products Found");
            }
            return Ok(products);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _repo.GetProductByIdAsync(id);  
            if (product == null)
            {
                return NotFound("No Product Found");
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            if (product == null)
            {
                return BadRequest("Product Cannot be null1!");
                
            }
           _repo.AddProduct(product);
          if ( !await _repo.SaveChangesAsync())
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Saving Product to the Database. ");
            }
            return CreatedAtAction(nameof(GetProduct),new {id=product.Id},product);
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Product>> UpdateProduct(int id,Product product)
        {
            if (id != product.Id)
            {
                return BadRequest("Product Id Mismatch.");

            }
           _repo.UpdateProduct(product);
            try
            {
                await _repo.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_repo.ProductExists(id))
                {
                    return NotFound("Product Not Found.");
                }
                throw;
            }
                return NoContent();
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            var product = await _repo.GetProductByIdAsync(id);  
            if (product==null)
            {
                return NotFound("Product Not Found.");

            }
          _repo.DeleteProduct(product);
            await _repo.SaveChangesAsync();
            return NoContent();
          
           
        }
    }
}
