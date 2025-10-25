using BlackCoffee.API.Models;

namespace BlackCoffee.API.Services.Interfaces;

public interface IJwtService
{
    string GenerateToken(Usuario user);
}
