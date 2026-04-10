namespace Products.Server.Data
{
    /// <summary>
    /// Entity Framework Core database context for the Products application.
    /// Provides access to the application's entity sets.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="AppDbContext"/> class.
    /// </remarks>
    /// <param name="options">The options used by a <see cref="DbContext"/>.</param>
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {

        /// <summary>
        /// Gets the set of <see cref="Product"/> entities stored in the database.
        /// </summary>
        public DbSet<Product> Products => Set<Product>();
    }
}