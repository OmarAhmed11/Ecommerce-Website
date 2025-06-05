using Ecommerce.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.Services
{
    public interface IEmailService
    {
        Task SendEmail(EmailDTO emailDTO);

    }
}
