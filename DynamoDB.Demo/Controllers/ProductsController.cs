using Amazon.DynamoDBv2.DataModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DynamoDB.Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IDynamoDBContext _context;

        public ProductsController(IDynamoDBContext context)
        {
            _context = context;
            
        }
        [HttpGet("{id}/{barcode}")]
        public async Task<IActionResult> Get(string id, string barcode)
        {
            var product = await _context.LoadAsync<Product>(id, barcode);
            if (product == null) { return NotFound(); }
            return Ok(product);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var product = await _context.ScanAsync<Product>(default).GetRemainingAsync();
            return Ok(product);
        }
        [HttpPost]
        public async Task<IActionResult> Create(Product request)
        {
            var product = await _context.LoadAsync<Product>(request.Id, request.Barcode);
            if (product != null) { return BadRequest($"Product with Id {request.Id} and Barcode {request.Barcode} already exists"); };
            await _context.SaveAsync(request);
            return NoContent();
        }
    }
}
