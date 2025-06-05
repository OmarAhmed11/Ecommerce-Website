using Ecommerce.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Interfaces
{
    public interface IAuth
    {
        Task<string> RegisterAsync(RegisterUserDTO registerUserDTO);

        Task<string> LoginAsync(LoginUserDTO loginUserDTO);

        Task<bool> SendEmailForForgetPassword(string email);

        Task<string> ResetPassword(ResetPasswordDTO resetPasswordDTO);

        Task<bool> ActiveAccount(ActiveAccountDTO accountDTO);

    }
}
