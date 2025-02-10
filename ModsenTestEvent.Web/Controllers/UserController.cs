namespace ModsenTestEvent.Web.Controllers;

[Route("api/UsersAuthorization")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserRepository _repository; 

    public UserController(IUserRepository repository)
    {
        _repository = repository;
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(LoginRequestDto loginRequestDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var loginResponse = await _repository.LoginAsync(loginRequestDto,cancellationToken);
        
        return Ok(loginResponse);
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(RegistrationRequestDto registrationRequestDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var user = await _repository.RegisterAsync(registrationRequestDto, cancellationToken);
        
        return Ok(user);
    }
    
    [HttpPost("refresh-token")] 
    public async Task<IActionResult> RefreshTokenAsync(RefreshTokenRequestDto request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var response = await _repository.RefreshTokenAsync(request.RefreshToken, cancellationToken);

        return Ok(response);
    }
}