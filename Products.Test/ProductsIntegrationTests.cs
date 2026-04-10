using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Products.Test
{
    public class ProductsIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        public ProductsIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetProducts_ReturnsUnauthorized()
        {
            // Act
            var response = await _client.GetAsync("https://localhost:7266/api/Products/GetProducts");
            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetProducts_ReturnsAuthorizedResults() {
            // Arrange

            var username = "admin";
            var creds = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes("TestKey-TestKey-TestKey-TestKey-TestKey-TestKey----")),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
               issuer: "ProductsApi",
               audience: "ProductsAU",
               claims: [new Claim(ClaimTypes.Name, username)],
               expires: DateTime.Now.AddMinutes(30),
               signingCredentials: creds);
            
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenString);
            // Act
            var response = await _client.GetAsync("https://localhost:7266/api/Products/GetProducts");
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
    }
  
}
