using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Application.Contracts.Models.EmailService
{
    public interface IEmailService
    {
        public Task<bool> SendMailAsync(Email email);
    }
}
