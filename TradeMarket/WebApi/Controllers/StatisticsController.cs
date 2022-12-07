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
    public class StatisticsController : ControllerBase
    {
        readonly IStatisticService statisticService;

        public StatisticsController(IStatisticService statisticService)
        {
            this.statisticService = statisticService;
        }

        [HttpGet("popularProducts")]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetMostPopularProducts([FromQuery] int productCount)
        {
            return new ObjectResult(await statisticService.GetMostPopularProductsAsync(productCount));
        }

        [HttpGet("customer/{id}/{productCount}")]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetCustomersMostPopularProducts(int id, int productCount)
        {
            return new ObjectResult(await statisticService.GetCustomersMostPopularProductsAsync(id,productCount));
        }

        [HttpGet("activity/{customerCount}")]
        public async Task<ActionResult<IEnumerable<CustomerActivityModel>>> GetMostValuableCustomers(int customerCount, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            return new ObjectResult(await statisticService.GetMostValuableCustomersAsync(customerCount, startDate, endDate));
        }

        [HttpGet("income/{categoryId}")]
        public async Task<ActionResult<decimal>> GetIncomeOfCategoryInPeriod(int categoryId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            return await statisticService.GetIncomeOfCategoryInPeriod(categoryId, startDate, endDate);
        }
    }
}
