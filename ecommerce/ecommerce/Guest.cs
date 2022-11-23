using System.Net;

namespace ecommerce
{
    public class Guest
    {
        public List<Product> ViewProducts()
        {
            return Database.Products;
        }

        public Response Register(
            string name,
            string email,
            string password,
            string address,
            string phoneNo
        )
        {
            if (
                string.IsNullOrEmpty(name)
                || string.IsNullOrEmpty(email)
                || string.IsNullOrEmpty(password)
                || string.IsNullOrEmpty(address)
                || string.IsNullOrEmpty(phoneNo)
            )
                return new Response
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Invalid customer details"
                };

            var emailSyntax = new System.Text.RegularExpressions.Regex(
                @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"
            );

            if (!emailSyntax.IsMatch(email))
                return new Response
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Email address is not valid"
                };

            if (Database.Customers.Any(c => c.Email == email))
                return new Response
                {
                    StatusCode = HttpStatusCode.Conflict,
                    Message = "Email address already exists"
                };

            var validPass = new System.Text.RegularExpressions.Regex(
                @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$"
            );

            if (!validPass.IsMatch(password))
                return new Response
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Password must be between 8 and 15 characters and contain at least one uppercase letter, one lowercase letter and one number"
                };

            Database.Customers.Add(
                new Customer
                {
                    Id = Database.Customers.Count + 1,
                    Name = name,
                    Email = email,
                    Password = password,
                    Address = address,
                    PhoneNo = phoneNo
                }
            );
            return new Response
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Customer registered successfully"
            };
        }
    }
}
