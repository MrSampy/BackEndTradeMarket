using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        readonly IProductService productService;

        public ProductsController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductModel>> GetById(int id)
        {
            try
            {
                var product = await productService.GetByIdAsync(id);

                return product;
            }
            catch(MarketException ex)
            {
                return new NotFoundObjectResult(ex.Message);
            }
            
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetWithFilter([FromQuery] FilterSearchModel filter)
        {
            return new ObjectResult(await productService.GetByFilterAsync(filter));
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody] ProductModel value)
        {
            try
            {
                await productService.AddAsync(value);

                return Ok(value);
            }
            catch(MarketException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int Id, [FromBody] ProductModel value)
        {
            try
            {
                await productService.UpdateAsync(value);

                return Ok(value);
            }
            catch (MarketException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await productService.DeleteAsync(id);

            return Ok();
        }

        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<ProductCategoryModel>>> GetCategories()
        {
            return new ObjectResult(await productService.GetAllProductCategoriesAsync());
        }

        [HttpPost("categories")]
        public async Task<ActionResult> AddCategory([FromBody] ProductCategoryModel value)
        {
            try
            {
                await productService.AddCategoryAsync(value);

                return Ok(value);
            }
            catch (MarketException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("categories/{id}")]
        public async Task<ActionResult> UpdateCategory(int Id, [FromBody] ProductCategoryModel value)
        {
            try
            {
                await productService.UpdateCategoryAsync(value);

                return Ok(value);
            }
            catch (MarketException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("categories/{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            await productService.RemoveCategoryAsync(id);

            return Ok();
        }
    }
}
