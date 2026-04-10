using System.ComponentModel.DataAnnotations;

namespace Products.Models
{
    /// <summary>
    /// Represents a product with a name, colour and price.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Gets or sets the unique identifier for the product.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the colour of the product.
        /// </summary>
        [Required] 
        public required string Colour { get; set; }

        /// <summary>
        /// Gets or sets the product name.
        /// </summary>
        [Required] 
        public required string ProductName { get; set; }

        /// <summary>
        /// Gets or sets the product price.
        /// </summary>
        /// <remarks>
        /// The price is stored as an integer value in the application's currency units.
        /// </remarks>
        [Required] 
        public int ProductPrice { get; set; }
    }
}
