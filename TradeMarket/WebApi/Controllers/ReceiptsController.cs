using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiptsController : ControllerBase
    {
        readonly IReceiptService receiptService;

        public ReceiptsController(IReceiptService receiptService)
        {
            this.receiptService = receiptService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReceiptModel>>> Get()
        {
            return new ObjectResult(await receiptService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReceiptModel>> GetByID(int id)
        {
            return await receiptService.GetByIdAsync(id);
        }

        [HttpGet("{id}/details")]
        public async Task<ActionResult<IEnumerable<ReceiptDetailModel>>> GetReceiptDetailsById(int id)
        {
            return new ObjectResult(await receiptService.GetReceiptDetailsAsync(id));
        }

        [HttpGet("{id}/sum")]
        public async Task<ActionResult<decimal>> GetSumById(int id)
        {
            return await receiptService.ToPayAsync(id);
        }

        [HttpGet("period")]
        public async Task<ActionResult<IEnumerable<ReceiptModel>>> GetReceiptByPeriod([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            return new ObjectResult(await receiptService.GetReceiptsByPeriodAsync(startDate, endDate));
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody] ReceiptModel value)
        {
            try
            {
                await receiptService.AddAsync(value);

                return Ok(value);
            }
            catch (MarketException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int Id, [FromBody] ReceiptModel value)
        {
            try
            {
                await receiptService.UpdateAsync(value);

                return Ok(value);
            }
            catch (MarketException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/products/add/{productId}/{quantity}")]
        public async Task<ActionResult> AddProduct(int Id, int productId, int quantity)
        {
            try
            {
                await receiptService.AddProductAsync(productId, Id, quantity);

                return Ok();
            }
            catch (MarketException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/products/remove/{productId}/{quantity}")]
        public async Task<ActionResult> RemoveProduct(int Id, int productId, int quantity)
        {
            try
            {
                await receiptService.RemoveProductAsync(productId, Id, quantity);

                return Ok();
            }
            catch (MarketException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/checkout")]
        public async Task<ActionResult> CheckOut(int Id)
        {
            try
            {
                await receiptService.CheckOutAsync(Id);

                return Ok();
            }
            catch (MarketException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            await receiptService.DeleteAsync(id);

            return Ok();
        }
    }
}
