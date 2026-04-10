namespace Products.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("CheckHealth")]
        public IActionResult CheckHealth() => Ok("Healthy");

        [HttpGet("GetProducts")]
        [Authorize]
        public IActionResult GetProducts([FromQuery] string? colour)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(colour))
            {
                query = query.Where(p => string.Equals(p.Colour, colour, StringComparison.OrdinalIgnoreCase));
            }

            var products = query.ToList();
            return Ok(products);
        }

        [HttpPost("AddProduct")]
        [Authorize]
        public IActionResult AddProduct([FromBody] Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
            return Ok(product);
        }
    }
}
