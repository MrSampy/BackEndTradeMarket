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
    public class CustomersController : ControllerBase
    {
        readonly ICustomerService customerService;

        public CustomersController(ICustomerService customerService)
        {
            this.customerService = customerService;
        }

        // GET: api/customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerModel>>> Get()
        {
            return new ObjectResult(await customerService.GetAllAsync());
        }

        //GET: api/customers/1
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerModel>> GetById(int id)
        {
            var customer = await customerService.GetByIdAsync(id);

            if (customer is null)
                return NotFound(customer);

            return customer;
        }

        //GET: api/customers/products/1
        [HttpGet("products/{id}")]
        public async Task<ActionResult<CustomerModel>> GetByProductId(int id)
        {
            return new ObjectResult(await customerService.GetCustomersByProductIdAsync(id));

        }

        // POST: api/customers
        [HttpPost]
        public async Task<ActionResult> Add([FromBody] CustomerModel value)
        {
            try
            {
                await customerService.AddAsync(value);

                return Ok(value);
            }
            catch (MarketException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/customers/1
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] CustomerModel value)
        {
            value.Id = id;

            try
            {
                await customerService.UpdateAsync(value);

                return Ok(value);
            }
            catch (MarketException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/customers/1
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await customerService.DeleteAsync(id);

            return Ok();
        }
    }
}
