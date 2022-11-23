using System.Net;
using ecommerce;
using Xunit;

namespace Test
{
    [Collection("Sequential")]
    public class AdminTest
    {
        // Test: ListAllProduct
        [Fact]
        public void ListAllProducts_GettingExpectedProducts()
        {
            // Arrange
            Database.ClearDatabase();
            var admin = new Admin();
            admin.AddProduct("Product 1", "Category 1", 100);
            admin.AddProduct("Product 2", "Category 2", 200);
            var expected = Database.Products;

            // Act
            var result = admin.ListAllProducts();

            // Assert
            Assert.Equal(expected.Count, result.Count);
        }

        // Test: AddProduct
        [Fact]
        public void AddProduct_ValuesAreInvalid()
        {
            // Arrange
            Database.ClearDatabase();
            var admin = new Admin();

            // Act
            var result = admin.AddProduct(string.Empty, string.Empty, 0);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Product name and category cannot be null or empty", result.Message);
        }

        [Fact]
        public void AddProduct_ProductAlreadyExists()
        {
            // Arrange
            Database.ClearDatabase();
            var admin = new Admin();

            admin.AddProduct("Test Product", "Category 1", 10);

            // Act
            var result = admin.AddProduct("Test Product", "Category 1", 10);

            // Assert
            Assert.Equal(HttpStatusCode.Conflict, result.StatusCode);
            Assert.Equal("Product is already in the enlisted in store", result.Message);
        }

        [Fact]
        public void AddProduct_ValuesAreValid()
        {
            // Arrange
            Database.ClearDatabase();
            var admin = new Admin();

            // Act
            var result = admin.AddProduct("Valid Product", "Category 1", 100);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Product is added successfully", result.Message);
        }

        [Fact]
        public void ModifyProduct_Valid()
        {
            // Arrange
            Database.ClearDatabase();
            var admin = new Admin();
            admin.AddProduct("Test", "Category 1", 10);
            var product = new Product
            {
                Id = 1,
                Name = "Product 1",
                Category = "Category 1",
                Price = 10
            };

            // Act
            var result = admin.ModifyProduct(product);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Product has been modified successfully", result.Message);
        }

        [Fact]
        public void ModifyProduct_InvalidProductId()
        {
            // Arrange
            Database.ClearDatabase();
            var admin = new Admin();
            var product = new Product
            {
                Id = -1,
                Name = "Product 1",
                Category = "Category 1",
            };

            // Act
            var result = admin.ModifyProduct(product);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            Assert.Equal("Product with the given id does not exist", result.Message);
        }

        [Fact]
        public void ModifyProduct_InvalidProduct()
        {
            // Arrange
            Database.ClearDatabase();
            var admin = new Admin();
            admin.AddProduct("Test", "Category 1", 10);
            var product = new Product
            {
                Id = 1,
                Name = string.Empty,
                Category = string.Empty,
                Price = 10
            };

            // Act
            var result = admin.ModifyProduct(product);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Product name and category cannot be null or empty", result.Message);
        }

        [Fact]
        public void ModifyProduct_ProductAlreadyExists()
        {
            // Arrange
            Database.ClearDatabase();
            var admin = new Admin();
            admin.AddProduct("Test Product", "Category 1", 10);
            admin.AddProduct("Test Product 2", "Category 1", 10);
            var product = new Product
            {
                Id = 2,
                Name = "Test Product",
                Category = "Category 1",
                Price = 10
            };

            // Act
            var result = admin.ModifyProduct(product);

            // Assert
            Assert.Equal(HttpStatusCode.Conflict, result.StatusCode);
            Assert.Equal("Product is already in the enlisted in store", result.Message);
        }

        [Fact]
        public void DeleteProduct_InvalidProductId()
        {
            // Arrange
            Database.ClearDatabase();
            var admin = new Admin();

            // Act
            var result = admin.DeleteProduct(-1);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            Assert.Equal("Product with the given id does not exist", result.Message);
        }

        [Fact]
        public void DeleteProduct_ValidProductId()
        {
            // Arrange
            Database.ClearDatabase();
            var admin = new Admin();

            admin.AddProduct("Test Product", "Category 1", 10);

            // Act
            var result = admin.DeleteProduct(1);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Product has been deleted successfully", result.Message);
        }
    }
}
