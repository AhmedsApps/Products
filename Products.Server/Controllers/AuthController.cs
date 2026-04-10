namespace Products.Server.Controllers
{
    /// <summary>
    /// Controller that handles authentication operations such as login and JWT generation.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="config">Application configuration from which JWT settings are read.</param>
        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// Validates credentials and returns a JWT token when successful.
        /// </summary>
        /// <param name="request">The login request containing username and password.</param>
        /// <returns>
        /// An <see cref="IActionResult"/> containing the generated token on success (HTTP 200) or
        /// Unauthorized (HTTP 401) when credentials are invalid.
        /// </returns>
        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginRequestModel request)
        {
            // For demonstration, we are using hardcoded credentials.
            // In a real application, you should validate against a database.
            if (request.Username == "admin" && request.Password == "password")
            {
                var token = GenerateJwtToken(request.Username);
                return Ok(new { Token = token });
            }
            return Unauthorized();
        }

        /// <summary>
        /// Generates a JSON Web Token (JWT) for the specified username.
        /// </summary>
        /// <param name="username">The username to include in the token claims.</param>
        /// <returns>The serialized JWT as a string.</returns>
        private string GenerateJwtToken(string username)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? string.Empty));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: new[] { new Claim(ClaimTypes.Name, username) },
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
