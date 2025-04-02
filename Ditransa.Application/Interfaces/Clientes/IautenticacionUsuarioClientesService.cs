using Ditransa.Application.DTOs.Clientes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ditransa.Application.Interfaces.Clientes
{
    public interface IautenticacionUsuarioClientesService
    {
        Task<UsuarioClientes> Register(UsuarioClientes clientes);
        Task<ResultLoginClientesDto> Login(string email, string password);
        Task<RefreshToken> RefreshToken(string token, string refreshToken);
    }
}
