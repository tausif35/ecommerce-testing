using System.Net;

namespace ecommerce
{
    public class Cart
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        private List<Product> _products = new List<Product>();
        public int NumberOfProducts
        {
            get { return _products.Count; }
        }
        public decimal TotalPrice
        {
            get { return _products.Sum(product => product.Price); }
        }


        public List<Product> ViewProductsInCart()
        {
            return _products;
        }

        public Response AddToCart(Product product)
        {
            if (product == null)
            {
                return new Response
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Invalid cart entry"
                };
            }
            _products.Add(product);
            return new Response
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Product has been added to the cart"
            };
        }

        public Response RemoveFromCart(int productId)
        {
            var product = _products.FirstOrDefault(p => p.Id == productId);

            if (product == null)
                return new Response
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Product not found in cart"
                };

            _products.Remove(product);
            return new Response
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Product has been removed from the cart"
            };
        }

        public Response ClearCart()
        {
            _products.Clear();
            return new Response
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Cart has been emptied"
            };
        }
    }
}
