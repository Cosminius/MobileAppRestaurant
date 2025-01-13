using AppRestaurant.Data;
using AppRestaurant.Models;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

public class AuthenticationService
{
    private readonly RestaurantDbContext _dbContext;

    public AuthenticationService(RestaurantDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public bool Authenticate(string username, string password)
    {
        
        var user = _dbContext.Users.FirstOrDefault(u => u.Username == username);

        if (user == null)
            return false;

        
        return VerifyPassword(password, user.PasswordHash);
    }

    
    private bool VerifyPassword(string password, string storedHash)
    {
        var hash = HashPassword(password);
        return hash == storedHash;
    }

    
    private string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashBytes);
        }
    }
}
