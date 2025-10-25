using Microsoft.AspNetCore.Identity;
using GStore.API.DTOs;
using GStore.API.Services.Interfaces;
using GStore.API.Models;

namespace GStore.API.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly UserManager<Usuario> _userManager;
    private readonly SignInManager<Usuario> _signInManager;
    private readonly IJwtService _jwtService;
    private readonly IFileService _fileService;

    public AuthService(
        UserManager<Usuario> userManager,
        SignInManager<Usuario> signInManager,
        IJwtService jwtService,
        IFileService fileService
    )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtService = jwtService;
        _fileService = fileService;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
    {
        var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
        if (existingUser != null)
        {
            return null;
        }

        // Salvar a foto se existir
        string fotoPath = null;
        if (registerDto.Foto != null)
        {
            fotoPath = await _fileService.SaveFileAsync(registerDto.Foto, "img/usuarios");
        }

        var user = new Usuario
        {
            UserName = registerDto.Email,
            Email = registerDto.Email,
            Nome = registerDto.Nome,
            DataNascimento = registerDto.DataNascimento,
            Foto = fotoPath // Armazena "img/usuarios/guid.ext"
        };

        var result = await _userManager.CreateAsync(user, registerDto.Senha);
        if (!result.Succeeded)
        {
            if (fotoPath != null)
                await _fileService.DeleteFileAsync(fotoPath);
            return null;
        }

        var token = _jwtService.GenerateToken(user);
        var userDto = new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            Nome = user.Nome,
            DataNascimento = user.DataNascimento,
            Foto = fotoPath != null ? _fileService.GetFileUrl(fotoPath) : null
        };

        return new AuthResponseDto
        {
            Token = token,
            Expiration = DateTime.UtcNow.AddMinutes(60),
            User = userDto
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user == null)
        {
            return null;
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Senha, false);
        if (!result.Succeeded)
        {
            return null;
        }

        var token = _jwtService.GenerateToken(user);
        var userDto = new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            Nome = user.Nome,
            DataNascimento = user.DataNascimento,
            Foto = !string.IsNullOrEmpty(user.Foto) ? _fileService.GetFileUrl(user.Foto) : null
        };

        return new AuthResponseDto
        {
            Token = token,
            Expiration = DateTime.UtcNow.AddMinutes(60),
            User = userDto
        };
    }

    public async Task<UserDto> GetUserByIdAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return null;
        }

        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            Nome = user.Nome,
            DataNascimento = user.DataNascimento,
            Foto = !string.IsNullOrEmpty(user.Foto) ? _fileService.GetFileUrl(user.Foto) : null
        };
    }
}