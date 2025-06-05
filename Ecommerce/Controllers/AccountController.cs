using AutoMapper;
using Ecommerce.Core.DTOs;
using Ecommerce.Core.Interfaces;
using Ecommerce.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterUserDTO registerUserDTO)
        {
            var result = await unitOfWork.Auth.RegisterAsync(registerUserDTO);
            if(result != "done")
            {
                return BadRequest((new ResponseAPI(400, result)));
            }
            return Ok((new ResponseAPI(200, result)));
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginUserDTO loginUserDTO)
        {
            string result = await unitOfWork.Auth.LoginAsync(loginUserDTO);
            if(result.StartsWith("Please"))
            {
                return BadRequest((new ResponseAPI(400, result)));
            }

            Response.Cookies.Append("token", result, new CookieOptions
            {
                Secure = true,
                HttpOnly = true,
                Domain = "localhost",
                Expires = DateTime.Now.AddDays(1),
                IsEssential = true,
                SameSite = SameSiteMode.Strict
            });
            return Ok((new ResponseAPI(200)));
        }
        [HttpPost("Active-account")]
        public async Task<IActionResult> active(ActiveAccountDTO accountDTO)
        {
            var result = await unitOfWork.Auth.ActiveAccount(accountDTO);
            return result ? Ok(new ResponseAPI(200)) : BadRequest(new ResponseAPI(400));
        }

        [HttpGet("send-email-forget-password")]
        public async Task<IActionResult> forgetPassword(string email)
        {
            var result = await unitOfWork.Auth.SendEmailForForgetPassword(email);
            return result ? Ok(new ResponseAPI(200)) : BadRequest(new ResponseAPI( 400));
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> resetPassword(ResetPasswordDTO resetPasswordDTO)
        {
            var result = await unitOfWork.Auth.ResetPassword(resetPasswordDTO);
            return result == "Password Change Successfully"  ? Ok(new ResponseAPI(200, result)) : BadRequest(new ResponseAPI( 400, result));
        }


    }
}
