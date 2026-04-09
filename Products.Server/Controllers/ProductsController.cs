namespace Products.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        [HttpGet("CheckHealth")]
        public IActionResult CheckHealth() => Ok("Healthy");
    }
}
