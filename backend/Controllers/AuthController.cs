using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _userRepo;
    private readonly IPasswordHasher<User> _passwordHasher;

    public AuthController(IUserRepository userRepo, IPasswordHasher<User> passwordHasher)
    {
        _userRepo = userRepo;
        _passwordHasher = passwordHasher;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest req)
    {
        if (await _userRepo.EmailExists(req.Email))
            return BadRequest("Email already in use");

        if (req.Password != req.ConfirmPassword)
            return BadRequest("Passwords do not match");

        var user = new User
        {
            FullName = req.FullName,
            Email = req.Email
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, req.Password);
        await _userRepo.CreateAsync(user);

        return Ok("User registered successfully");
    }
}
