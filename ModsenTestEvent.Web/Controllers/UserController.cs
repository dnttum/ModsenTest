namespace ModsenTestEvent.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILoginUseCase _loginUseCase;
        private readonly IRegisterUseCase<RegistrationRequestDto, UserDto> _registerUseCase;
        private readonly IRefreshTokenUseCase _refreshTokenUseCase;

        public UserController(
            ILoginUseCase loginUseCase,
            IRegisterUseCase<RegistrationRequestDto, UserDto> registerUseCase,
            IRefreshTokenUseCase refreshTokenUseCase)
        {
            _loginUseCase = loginUseCase;
            _registerUseCase = registerUseCase;
            _refreshTokenUseCase = refreshTokenUseCase;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequestDto loginRequestDto, CancellationToken cancellationToken)
        {
            var user = await _loginUseCase.ExecuteAsync(loginRequestDto, cancellationToken);
            
            return Ok(user);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegistrationRequestDto registrationRequestDto, CancellationToken cancellationToken)
        {
            var user = await _registerUseCase.ExecuteAsync(registrationRequestDto, cancellationToken);
            
            return Ok(user);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] string refreshToken, CancellationToken cancellationToken)
        {
            var token = await _refreshTokenUseCase.ExecuteAsync(refreshToken, cancellationToken);
            
            return Ok(token);
        }
    }
}