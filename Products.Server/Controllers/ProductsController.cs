namespace Products.Server.Controllers
{
    /// <summary>
    /// API controller for managing products.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductsController"/> class.
        /// </summary>
        /// <param name="context">The application's <see cref="AppDbContext"/>.</param>
        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Simple health check endpoint.
        /// </summary>
        /// <returns>An HTTP 200 response with the text "Healthy".</returns>
        [HttpGet("CheckHealth")]
        public IActionResult CheckHealth() => Ok("Healthy");

        /// <summary>
        /// Retrieves products, optionally filtered by colour.
        /// </summary>
        /// <param name="colour">Optional query parameter to filter products by their colour (case-insensitive).</param>
        /// <returns>An <see cref="IActionResult"/> containing the list of matching products.</returns>
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

        /// <summary>
        /// Adds a new product to the database.
        /// </summary>
        /// <param name="product">The product to add.</param>
        /// <returns>The added product wrapped in an <see cref="IActionResult"/>.</returns>
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
