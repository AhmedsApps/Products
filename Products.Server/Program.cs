var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("ProductsDb"));

// Configure JWT Auth
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

// Configure Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Register a Swagger document so the UI can load /swagger/v1/swagger.json
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Products API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();


// Configure Middleware
// Expose Swagger UI at the app root so it becomes the home page.
// Place Swagger middleware before static file middleware so it can respond to '/'.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    // Serve the Swagger UI at application root
    c.RoutePrefix = string.Empty;
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Products API V1");
});

app.UseCors("AllowAll");

app.UseDefaultFiles();
app.MapStaticAssets();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Products.AddRange(
        new Product { ProductName = "Product 1", Colour = "Red", ProductPrice = 100 },
        new Product { ProductName = "Product 2", Colour = "Green", ProductPrice = 150 },
        new Product { ProductName = "Product 3", Colour = "Blue", ProductPrice = 200 },
        new Product { ProductName = "Product 4", Colour = "White", ProductPrice = 250 },
        new Product { ProductName = "Product 5", Colour = "White", ProductPrice = 300 },
        new Product { ProductName = "Product 6", Colour = "Blue", ProductPrice = 350 }
    );
    dbContext.SaveChanges();
}

app.MapFallbackToFile("/index.html");

app.Run();
