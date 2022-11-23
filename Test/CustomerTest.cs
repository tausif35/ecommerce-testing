using System.Net;
using Moq;
using ecommerce;
using Xunit;

namespace Test
{
    [Collection("Sequential")]
    public class CustomerTest
    {
        [Fact]
        public void AddToCart_ProductNotFound()
        {
            // Arrange
            Admin admin = new Admin();
            admin.AddProduct("product 1", "Category 1", 10);
            admin.AddProduct("product 2", "Category 2", 100);
            var customer = new Customer();
            // Act
            var result = customer.AddToCart(3, 100);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            Assert.Equal("Product not found", result.Message);
        }

        [Fact]
        public void AddToCart_QuantityIsInvalid()
        {
            // Arrange
            Admin admin = new Admin();
            admin.AddProduct("product 1", "Category 1", 10);
            admin.AddProduct("product 2", "Category 2", 100);
            var customer = new Customer();

            // Act
            var result = customer.AddToCart(1, 0);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Quantity must be greater than 0", result.Message);
        }

        [Fact]
        public void RemoveFromCart_ProductNotFound()
        {
            // Arrange
            Admin admin = new Admin();
            admin.AddProduct("product 1", "Category 1", 10);
            admin.AddProduct("product 2", "Category 2", 100);
            var customer = new Customer();
            customer.AddToCart(1, 10);
            // Act
            var result = customer.RemoveFromCart(4, 1);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            Assert.Equal("Product not found in cart", result.Message);
        }

        [Fact]
        public void RemoveFromCart_QuantityIsInvalid()
        {
            // Arrange
            Admin admin = new Admin();
            admin.AddProduct("product 1", "Category 1", 10);
            admin.AddProduct("product 2", "Category 2", 100);
            var customer = new Customer();
            customer.AddToCart(1, 10);

            // Act
            var result = customer.RemoveFromCart(1, 0);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Quantity must be greater than 0", result.Message);
        }

        [Fact]
        public void RemoveFromCart_QuantityIsGreaterThanCartQuantity()
        {
            // Arrange
            Admin admin = new Admin();
            admin.AddProduct("product 1", "Category 1", 10);
            admin.AddProduct("product 2", "Category 2", 100);
            var customer = new Customer();
            customer.AddToCart(1, 10);

            // Act
            var result = customer.RemoveFromCart(1, 11);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Quantity greater than products in cart", result.Message);
        }

        [Fact]
        public void RemoveFromCart_Valid()
        {
            // Arrange
            Admin admin = new Admin();
            admin.AddProduct("product 1", "Category 1", 10);
            admin.AddProduct("product 2", "Category 2", 100);
            var customer = new Customer();
            customer.AddToCart(1, 10);

            // Act
            var result = customer.RemoveFromCart(1, 5);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Product has been removed from the cart", result.Message);
        }

        [Fact]
        public void ClearCart_Valid()
        {
            // Arrange
            Admin admin = new Admin();
            admin.AddProduct("product 1", "Category 1", 10);
            admin.AddProduct("product 2", "Category 2", 100);
            var customer = new Customer();

            // Act
            var result = customer.ClearCart();

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Cart has been cleared", result.Message);
        }

        [Fact]
        public void Checkout_CartIsEmpty()
        {
            // Arrange
            Admin admin = new Admin();
            admin.AddProduct("product 1", "Category 1", 10);
            admin.AddProduct("product 2", "Category 2", 100);
            var customer = new Customer();

            // Act
            var result = customer.Checkout();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Cart is empty", result.Message);
        }

        [Fact]
        public void Checkout_Valid()
        {
            // Arrange
            Admin admin = new Admin();
            admin.AddProduct("product 1", "Category 1", 10);
            admin.AddProduct("product 2", "Category 2", 100);
            var customer = new Customer();
            customer.Id = 1;
            customer.AddToCart(1, 10);

            // Act
            var result = customer.Checkout();

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Checkout successful", result.Message);
        }

        [Fact]
        public void MakePurchase_Valid()
        {
            // Arrange
            Database.ClearDatabase();
            Admin admin = new Admin();
            admin.AddProduct("product 1", "Category 1", 10);
            admin.AddProduct("product 2", "Category 2", 100);
            var customer = new Customer();
            customer.Id = 1;
            customer.AddToCart(1, 10);
            customer.Checkout();
            // Act
            var result = customer.MakePurchase(1, "123", "456");

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Purchase successful", result.Message);
        }
    }
}
