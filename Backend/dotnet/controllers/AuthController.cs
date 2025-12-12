using Microsoft.AspNetCore.Mvc;
using dotnet.services;
using dotnet.models;

namespace dotnet.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly LoginServices _loginService;

        public AuthController(LoginServices loginService)
        {
            _loginService = loginService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup(employeeModel emp)
        {
            var result = await _loginService.Signup(emp);
            if (result == "Success")
            {
                return Ok(new { message = "User registered successfully" });
            }
            return Conflict(new { message = result });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest login)
        {
            var user = await _loginService.LoginAsync(login.Username, login.Password);
            if (user == null)
                return Unauthorized(new { message = "Invalid username or password" });

            return Ok(new
            {
                message = "Login successful",
                user.Id,
                user.Name,
                user.IdNo,
                Otp = user.Otp
            });
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpModel model)
        {
            var result = await _loginService.VerifyOtpAsync(model.Name, model.Otp);

            if (!result)
                return BadRequest(new { message = "Invalid or expired OTP" });

            return Ok(new { message = "OTP verified successfully" });
        }
    }
}
