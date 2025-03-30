
using Dto;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController(AuthService authService) : ControllerBase
{

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto user)
    {
        var result = await authService.Login(user);
        return Ok(result);
    }


    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto user)
    {
        var result = await authService.Register(user);
        return CreatedAtAction(nameof(Register), result);
    }

    [HttpPost("refresh")]
    public IActionResult Refresh([FromBody] RefreshDto user)
    {
        var result = authService.Refresh(user);
        return Ok(result);
    }
}