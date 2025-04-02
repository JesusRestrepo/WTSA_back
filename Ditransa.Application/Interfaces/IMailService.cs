using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ditransa.Application.DTOs;

namespace Ditransa.Application.Interfaces
{
    public interface IMailService
    {
        Task SendEmail(string subject, string[] toRecipients, string body, bool isHtml, FileDto[] attachments = null);
    }
}
