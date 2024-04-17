using Microsoft.AspNetCore.Identity;


namespace MyApiNetCore6.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { set; get; } = null!;
        public string LastName { set;get;} = null!;

    }
}
