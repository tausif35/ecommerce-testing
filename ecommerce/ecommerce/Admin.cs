using System.Net;

namespace ecommerce
{
    public class Admin : User
    {
        public List<Product> ListAllProducts()
        {
            return Database.Products;
        }

        public Response AddProduct(string name, string category, decimal price)
        {
            if (
                string.IsNullOrEmpty(name)
                || string.IsNullOrEmpty(category)
            )
                return new Response
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Product name and category cannot be null or empty"
                };

            if (
                Database.Products.Any(
                    p => p.Name == name && p.Category == category
                )
            )
                return new Response
                {
                    StatusCode = HttpStatusCode.Conflict,
                    Message = "Product is already in the enlisted in store"
                };

            Database.Products.Add(
                new Product
                {
                    Id = Database.Products.Count + 1,
                    Name = name,
                    Category = category,
                    Price = price
                }
            );
            return new Response
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Product is added successfully"
            };
        }

        public Response DeleteProduct(int productId)
        {
            var product = Database.Products.FirstOrDefault(p => p.Id == productId);
            if (product == null)
                return new Response
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Product with the given id does not exist"
                };

            Database.Products.Remove(product);
            return new Response
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Product has been deleted successfully"
            };
        }

        public Response ModifyProduct(Product product)
        {
            var index = Database.Products.FindIndex(p => p.Id == product.Id);
            if (index == -1)
                return new Response
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Product with the given id does not exist"
                };

            if (
                string.IsNullOrEmpty(product.Name)
                || string.IsNullOrEmpty(product.Category)
            )
                return new Response
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Product name and category cannot be null or empty"
                };

            if (
                Database.Products.Any(
                    p =>
                        p.Name == product.Name
                        && p.Category == product.Category
                        && p.Id != product.Id
                )
            )
                return new Response
                {
                    StatusCode = HttpStatusCode.Conflict,
                    Message = "Product is already in the enlisted in store"
                };

            Database.Products[index] = product;
            return new Response
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Product has been modified successfully"
            };
        }
    }
}
