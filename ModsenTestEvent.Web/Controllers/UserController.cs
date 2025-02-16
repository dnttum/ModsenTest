namespace ModsenTestEvent.Web.Controllers;

[Route("api/users")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(LoginRequestDto loginRequestDto, CancellationToken cancellationToken)
    {
        var user = await _userService.LoginAsync(loginRequestDto, cancellationToken);
        
        return Ok(user);
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(RegistrationRequestDto registrationRequestDto, CancellationToken cancellationToken)
    {
        var user = await _userService.RegisterAsync(registrationRequestDto, cancellationToken);
        
        return Ok(user);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshTokenAsync(RefreshTokenRequestDto request, CancellationToken cancellationToken)
    {
        var token = await _userService.RefreshTokenAsync(request.RefreshToken, cancellationToken);
        
        return Ok(token);
    }
}