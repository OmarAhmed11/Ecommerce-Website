using Ecommerce.Core.DTOs;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Interfaces;
using Ecommerce.Core.Services;
using Ecommerce.Core.Shared;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Repositories
{
    public class AuthRepository : IAuth
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly IGenerateToken _generateToken;
        private readonly SignInManager<AppUser> _signInManager;

        public AuthRepository(UserManager<AppUser> userManager, IEmailService emailService, SignInManager<AppUser> signInManager, IGenerateToken generateToken)
        {
            _userManager = userManager;
            _emailService = emailService;
            _signInManager = signInManager;
            _generateToken = generateToken;
        }
        public async Task<string> RegisterAsync(RegisterUserDTO registerUserDTO)
        {
            if (registerUserDTO == null)
            {
                return null;
            }
            if (await _userManager.FindByNameAsync(registerUserDTO.UserName) is not null)
            {
                return "UserName Already Exist";
            }
            if (await _userManager.FindByEmailAsync(registerUserDTO.Email) is not null)
            {
                return "Email Already Exist";
            }
            AppUser user = new AppUser
            {
                UserName = registerUserDTO.UserName,
                Email = registerUserDTO.Email,
                DisplayName = registerUserDTO.DisplayName,
            };
            var result = await _userManager.CreateAsync(user, registerUserDTO.Password);
            if (result.Succeeded is not true)
            {
                return result.Errors.ToList()[0].Description;
            }
            string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            await SendEmail(user.Email, token, "active", "ActivateEmail", "Please Active your Email click on button to active");
            return "done";
        }
        public async Task SendEmail(string email, string code, string component, string subject, string message)
        {
            var resut = new EmailDTO(
                email,
                "omarahmed19989796@gmail.com",
                subject,
                EmailBody.send(email, code, component, message)
                );

            await _emailService.SendEmail(resut);
        }
        public async Task<string> LoginAsync(LoginUserDTO loginUserDTO)
        {
            if (loginUserDTO == null)
            {
                return null;
            }
            var findUser = await _userManager.FindByEmailAsync(loginUserDTO.Email);
            if (!findUser.EmailConfirmed)
            {
                string token = await _userManager.GenerateEmailConfirmationTokenAsync(findUser);
                await SendEmail(findUser.Email, token, "active", "ActivateEmail", "Please Active your Email click on button to active");
                return "Please Confirm your Email First, Activation Email was send to your Email";
            }
            var result = await _signInManager.CheckPasswordSignInAsync(findUser, loginUserDTO.Password, true);
            if (result.Succeeded)
            {
                return _generateToken.GetAndCreateToken(findUser);
            }

            return "Please Check your Email, Something went wrong";
        }
        public async Task<bool> SendEmailForForgetPassword(string email)
        {
            var findUser = await _userManager.FindByEmailAsync(email);
            if (findUser is null) 
            { 
                return false;
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(findUser);
            await SendEmail(findUser.Email, token, "reset-password", "Reset Password", " click on button to Reset your Password");

            return true;
        }
        public async Task<string> ResetPassword(ResetPasswordDTO resetPasswordDTO)
        {
            var findUser = await _userManager.FindByEmailAsync(resetPasswordDTO.Email);
            if (findUser is null)
            {
                return null;
            }
            var result = await _userManager.ResetPasswordAsync(findUser, resetPasswordDTO.Token, resetPasswordDTO.Password);
            if (result.Succeeded) 
            {
                return "Password Change Successfully";
            }
            return result.Errors.ToList()[0].Description;
        }
        public async Task<bool> ActiveAccount(ActiveAccountDTO activeAccountDTO)
        {
            var findUser = await _userManager.FindByEmailAsync(activeAccountDTO.Email);
            if(findUser is null)
            {
                return false;
            }
            var result = await _userManager.ConfirmEmailAsync(findUser, activeAccountDTO.Token);
            if (result.Succeeded)
            {
                return true;
            }
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(findUser);
            await SendEmail(findUser.Email, token, "active", "ActivateEmail", "Please Active your Email click on button to active");
            return false;

        }
    }
}
