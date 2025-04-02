using Ditransa.Application.Common.Mappings;
using Ditransa.Domain.Entities;

namespace Ditransa.Application.DTOs
{
    public class RecoveryPasswordDto
    {
        public string Email { get; set; } = string.Empty;
    }
}