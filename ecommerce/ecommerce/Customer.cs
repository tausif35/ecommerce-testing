using System.Net;

namespace ecommerce
{
    public class Customer : User
    {
        public string Address { get; set; }
        public string PhoneNo { get; set; }
        private Cart _cart = new Cart();

        public Response AddToCart(int productId, int quantity)
        {
            var product = Database.Products.FirstOrDefault(p => p.Id == productId);

            if (product == null)
                return new Response
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Product not found"
                };

            if (quantity <= 0)
                return new Response
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Quantity must be greater than 0"
                };

            for (int i = 0; i < quantity; i++)
            {
                _cart.AddToCart(product);
            }

            return new Response
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Products have been added to the cart"
            };
        }

        public Response RemoveFromCart(int productId, int quantity)
        {
            var products = _cart.ViewProductsInCart().FindAll(p => p.Id == productId);

            if (products.Count == 0)
                return new Response
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Product not found in cart"
                };

            if (quantity <= 0)
                return new Response
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Quantity must be greater than 0"
                };

            if (quantity > products.Count)
                return new Response
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Quantity greater than products in cart"
                };

            for (int i = 0; i < quantity; i++)
            {
                _cart.RemoveFromCart(productId);
            }

            return new Response
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Product has been removed from the cart"
            };
        }

        public Response ClearCart()
        {
            _cart.ClearCart();

            return new Response
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Cart has been cleared"
            };
        }

        public Response Checkout()
        {
            if (_cart.NumberOfProducts == 0)
                return new Response
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Cart is empty"
                };


            _cart.Id = Database.Carts.Count + 1;
            _cart.CustomerId = Id;

            Database.Carts.Add(_cart);

            _cart = new Cart();

            return new Response { StatusCode = HttpStatusCode.OK, Message = "Checkout successful" };
        }

        public Response MakePurchase(int cartId, string cardNumber, string cardPin)
        {
            var cart = Database.Carts.FirstOrDefault(c => c.Id == cartId && c.CustomerId == Id);

            if (cart == null)
                return new Response
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Cart not found"
                };
            
            if (cardNumber == null || cardPin == null)
                return new Response
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Card number or pin cannot be null"
                };

            return new Response { StatusCode = HttpStatusCode.OK, Message = "Purchase successful" };
        }
    }
}
