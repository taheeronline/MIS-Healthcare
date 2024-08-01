using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using MIS_Healthcare.API.Data.DTOs.User;
using MIS_Healthcare.API.Data.Models;
using MIS_Healthcare.API.Middleware;
using MIS_Healthcare.API.Repository.Interface;

namespace MIS_Healthcare.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly iUserRepo _userRepo;
        private readonly iTokenService _tokenService;

        public UsersController(iUserRepo userRepo, iTokenService tokenService)
        {
            _userRepo = userRepo;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserToRegister request)
        {
            var existingUser = await _userRepo.GetUserByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return Conflict("User already exists");
            }

            var passwordHash = HashPassword(request.Password);
            var user = new User
            {
                UserName = request.UserName,
                PasswordHash = passwordHash,
                Email = request.Email,
                UserType = "User" // Set the appropriate user type
            };

            await _userRepo.AddUserAsync(user);
            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserToLogin request)
        {
            var user = await _userRepo.GetUserByEmailAsync(request.Email);
            if (user == null || !VerifyPassword(request.Password, user.PasswordHash))
            {
                return Unauthorized("Invalid credentials");
            }

            var token = _tokenService.GenerateToken(user);
            return Ok(new { Token = token });
        }

        private string HashPassword(string password)
        {
            // Use a secure hash algorithm (e.g., PBKDF2) to hash passwords
            // This is a simple example and should be replaced with a secure implementation
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: new byte[16], // Use a unique salt for each password
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            // Verify the password using the same hash algorithm
            var hash = HashPassword(password);
            return hash == storedHash;
        }
    }
}
