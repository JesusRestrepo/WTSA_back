using Ditransa.Application.DTOs.Clientes;
using Ditransa.Application.Features.Clientes.Commands;
using Ditransa.Application.Features.Clientes.Queries.GetAccesoClientes;
using Ditransa.Application.Features.Clientes.Queries.GetCanalesClientes;
using Ditransa.Application.Features.Clientes.Queries.GetClientesSap;
using Ditransa.Application.Features.Clientes.Queries.GetCodigoClientes;
using Ditransa.Application.Features.Clientes.Queries.GetEstadisticasPQRSClientes;
using Ditransa.Application.Features.Clientes.Queries.GetEstadoCarteraClientes;
using Ditransa.Application.Features.Clientes.Queries.GetEstadosPQRSClientes;
using Ditransa.Application.Features.Clientes.Queries.GetMensajeClientes;
using Ditransa.Application.Features.Clientes.Queries.GetMenuClientes;
using Ditransa.Application.Features.Clientes.Queries.GetPendFacturarClientes;
using Ditransa.Application.Features.Clientes.Queries.GetPQRSClientes;
using Ditransa.Application.Features.Clientes.Queries.GetTipoDescripcionPqrsClientes;
using Ditransa.Application.Features.Clientes.Queries.GetTtoDatosClientes;
using Ditransa.Application.Features.Clientes.Queries.GetUsuarioClientes;
using Ditransa.Application.Interfaces.Clientes;
using Ditransa.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph.DeviceManagement.ManagedDevices.Item.Retire;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Ditransa.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class ClientesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IClientesService _clientesService;
        public ClientesController(IMediator mediator, IClientesService clientesService)
        {
            _clientesService = clientesService;
            _mediator = mediator;
        }

        //[HttpGet(Name = "cartera")]
        //public async Task<List<ClientesCarteraDto>> ConsultaCartera(string nit)
        //{
        //    return await _clientesService.ConsultarCartera(nit);
        //}

        [AllowAnonymous]
        [HttpGet(Name = "cartera")]
        public async Task<List<ClientesCarteraDto>> ConsultaCartera([FromQuery] string? nit)
        {
            var query = new getEstadoCarteraQuery(nit);
            var result = (await _mediator.Send(query)).Data;
            return result;
        }

        [AllowAnonymous]
        [HttpGet(Name = "eta")]
        public async Task<object> ConsultarEta(string nit)
        {
            return await _clientesService.ConsultarEta(nit);
        }

        [AllowAnonymous]
        [HttpGet(Name = "datosManifiesto")]
        public async Task<object> ConsultarDatosManifiesto(string manifiesto)
        {
            return await _clientesService.ConsultarDatosManifiesto(manifiesto);
        }

        [HttpGet(Name = "micarga")]
        public async Task<List<ClientesMiCargaDto>> ConsultarMiCarga(string nit, string startDate, string endDate)
        {
            return await _clientesService.ConsultarMiCarga(nit, startDate, endDate);
        }

        //[HttpGet(Name = "pendientefacturar")]
        //public async Task<List<ClientesPendFacturarDto>> ConsultarPendiente(string nit)
        //{
        //    return await _clientesService.ConsultarPendFacturar(nit);
        //}

        [AllowAnonymous]
        [HttpGet(Name = "pendientefacturar")]
        public async Task<List<ClientesPendFacturarDto>> ConsultarPendiente([FromQuery] string? nit)
        {
            var query = new getPendFacturarClientesQuery(nit);
            var result = (await _mediator.Send(query)).Data;
            return result;
        }

        [HttpGet(Name = "powerbi")]
        public async Task<ClientesPowerBiDto> ConsultarReporte(Guid workspaceId, Guid reportId)
        {
            return await _clientesService.ConsultarPowerBi(workspaceId, reportId);
        }

        [AllowAnonymous]
        [HttpGet(Name = "navegacion")]
        public async Task<List<NavegacionClientesDto>> GetNavegacion([FromQuery] GetNavegacionClientesQuery query)
        {
            var result = (await _mediator.Send(query)).Data;
            return new List<NavegacionClientesDto> { result };
        }

        [AllowAnonymous]
        [HttpGet(Name = "tratamientodatos")]
        public async Task<TtoDatosClientesDto> GetTtoDatos([FromQuery] GetTtoDatosClientesQuery query)
        {
            var result = (await _mediator.Send(query)).Data;
            return result;
        }

        [HttpGet(Name = "usuarios")]
        public async Task<List<UsuariosClientesDto>> GetUsuarios([FromQuery] GetUsuariosClienteQuery query)
        {
            var result = (await _mediator.Send(query)).Data;
            return result;
        }

        [AllowAnonymous]
        [HttpGet(Name = "usuarioEmail")]
        public async Task<UsuariosClientesDto> GetUsuarioEmail([FromQuery] string email)
        {
            var query = new GetUsuarioEmailClienteQuery(email);
            var result = (await _mediator.Send(query)).Data;
            return result;
        }


        [HttpGet(Name = "redContactos")]
        public async Task<List<UsuariosClientesDto>> GetRedContactos([FromQuery] string nit)
        {
            var query = new GetUsuarioCientesNitQuery(nit);
            var result = (await _mediator.Send(query)).Data;
            return result;
        }

        [HttpGet(Name = "AccesosAnno")]
        public async Task<List<AccesoRepsCientesDto>> GetAccesosAno([FromQuery] int year, string tipoUsuario)
        {
            var query = new GetAccesoAnoClientesQuery(year, tipoUsuario);
            var result = (await _mediator.Send(query)).Data;
            return result;
        }

        [HttpGet(Name = "getAccesos")]
        public async Task<List<AccesosClientesConteoDto>> GetAccesos([FromQuery] DateTime fechaInicio, DateTime fechaFin, string tipoUsuario)
        {
            var query = new GetAccesoClientesQuery(fechaInicio, fechaFin, tipoUsuario);
            var result = (await _mediator.Send(query)).Data;
            return result;
        }

        [HttpGet(Name = "getAccesosFecha")]
        public async Task<List<AccesosFechaClientes>> GetAccesosFecha([FromQuery] DateTime date, string tipoUsuario)
        {
            var query = new GetAccesoFEchaClientesQuery(date, tipoUsuario);
            var result = (await _mediator.Send(query)).Data;
            return result;
        }

        [HttpPost]
        public async Task<ActionResult<Result<bool>>> SaveUsuario([FromBody] SaveUsuarioClienteCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.Data) return await Result<bool>.SuccessAsync(result.Data, result.Messages.FirstOrDefault());

            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [AllowAnonymous]
        [HttpPut]
        public async Task<ActionResult<Result<bool>>> Password([FromBody] UpdateUsuarioClienteCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.Exception != null) return BadRequest(Result<bool>.FailureAsync(result.Exception));

            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginClienteCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.Data.ErrorMessage != null)
                return BadRequest(Result<bool>.FailureAsync(result.Data.ErrorMessage));

            if (string.IsNullOrWhiteSpace(result.Data.token))
            {
                return Unauthorized();
            }

            var response = new TokensDto
            {
                token = result.Data.token,
                refreshToken = result.Data.refreshToken
            };

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPut]
        public async Task<IActionResult> UpdateTtdoDatos([FromBody] UpdateTtoDatosClientesQuery command)
        {
            var result = await _mediator.Send(command);
            if (result.Exception != null) return BadRequest(Result<bool>.FailureAsync(result.Exception));

            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> UpdateToken([FromBody] UpdateTokenCommand command)
        {
            var result = await _mediator.Send(command);
            if (string.IsNullOrWhiteSpace(result.Data.token))
            {
                return Unauthorized();
            }
            return Ok(result.Data);
        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<Result<bool>>> SavePqrs([FromBody] SavePQRSClienteCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.Data) return await Result<bool>.SuccessAsync(result.Data, result.Messages.FirstOrDefault());

            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpPut]
        public async Task<ActionResult<Result<bool>>> UpdatePqrs([FromBody] UpdatePQRSClientesCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.Exception != null) return BadRequest(Result<bool>.FailureAsync(result.Exception));

            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpGet(Name = "tipoPqrs")]
        public async Task<List<TipoDescripcionPQRSClientesDto>> GetTipoPqrs([FromQuery] GetTipoDescripcionPQRSClientesQUery query)
        {
            var result = await _mediator.Send(query);

            if (result.Data == null)
            {
                return new List<TipoDescripcionPQRSClientesDto>();
            }

            return result.Data;
        }

        [HttpGet(Name = "Canales")]
        public async Task<List<CanalesClientesDto>> GetCanales([FromQuery] GetCanalesClientesQuery query)
        {
            var result = await _mediator.Send(query);

            if (result.Data == null)
            {
                return new List<CanalesClientesDto>();
            }

            return result.Data;
        }

        [HttpGet(Name = "estados")]
        public async Task<List<EstadosPQRSClientesDto>> GetEstadosPQRS([FromQuery] GetEstadosPQRSClientesQuery query)
        {
            var result = (await _mediator.Send(query)).Data;
            return result;
        }

        [AllowAnonymous]
        [HttpGet(Name = "clientesSAP")]
        public async Task<List<Clientes_SapClientesDto>> GetClientesSAP([FromQuery] string? nit)
        {
            var query = new GetClientesSapQuery(nit);
            var result = (await _mediator.Send(query)).Data;
            return result;
        }

        [AllowAnonymous]
        [HttpGet(Name = "pqrs")]
        public async Task<List<PQRSDto>> GetPQRS([FromQuery] string? nit)
        {
            var query = new GetPQRSClientesQuery(nit);
            var result = (await _mediator.Send(query)).Data;
            return result;
        }

        [AllowAnonymous]
        [HttpGet(Name = "estadisticasPQRS")]
        public async Task<EstadisticasPQRSDto> GetEstaditciasPQRS([FromQuery] GetEstadisticasPQRSQuery query)
        {
            var result = (await _mediator.Send(query)).Data;
            return result;
        }

        [HttpPut]
        public async Task<ActionResult<Result<bool>>> UpdateDescripcionPqrs([FromBody] UpdateDescripcionPQRSClientesCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.Exception != null) return BadRequest(Result<bool>.FailureAsync(result.Exception));

            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<Result<bool>>> SaveDescripcionPqrs([FromBody] SaveDescripcionPQRSClientesCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.Data) return await Result<bool>.SuccessAsync(result.Data, result.Messages.FirstOrDefault());

            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpGet(Name = "pqrsConsecutivo")]
        public async Task<PQRSDto> GetPQRSConsecutivo([FromQuery] int consecutivo)
        {
            var query = new GetPQRSClienteByIdQuery(consecutivo);
            var result = (await _mediator.Send(query)).Data;
            return result;
        }

        [HttpGet(Name = "pqrsCerradas")]
        public async Task<List<PQRSDto>> GetPQRSCerradas([FromQuery] GetPQRSCerradaQuery query)
        {
            var result = (await _mediator.Send(query)).Data;
            return result;
        }

        [HttpGet(Name = "Menus")]
        public async Task<List<MenusDto>> GetMenus([FromQuery] GetMenuClientesQuery query)
        {
            var result = (await _mediator.Send(query)).Data;
            return result;
        }

        [AllowAnonymous]
        [HttpGet(Name = "Submenus")]
        public async Task<List<SubmenusDto>> GetSubmenus([FromQuery] string MenuId)
        {
            var query = new GetSubmenuClientesQuery(MenuId);
            var result = (await _mediator.Send(query)).Data;
            return result;
        }

        [HttpPost]
        public async Task<ActionResult<Result<bool>>> SolicitudUsuario([FromBody] SolicitudUsuarioCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.Data) return await Result<bool>.SuccessAsync(result.Data, result.Messages.FirstOrDefault());

            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<Result<bool>>> SolicitudCliente([FromBody] SolicitudClienteCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.Data) return await Result<bool>.SuccessAsync(result.Data, result.Messages.FirstOrDefault());

            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<Result<bool>>> EnviarCodigo([FromBody] SendWhatsappClientesCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.Data) return await Result<bool>.SuccessAsync(result.Data, result.Messages.FirstOrDefault());

            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [AllowAnonymous]
        [HttpGet(Name = "CodigoTwilio")]
        public async Task<RespuestaCodigoTwilio> GetCodigo([FromQuery] string celular, string codigo)
        {
            var query = new GetCodigoClientesQuery(celular, codigo);
            var result = (await _mediator.Send(query)).Data;
            return result;
        }

        [AllowAnonymous]
        [HttpGet(Name = "MensajeAPP")]
        public async Task<MensajeClientesDto> GetMensaje([FromQuery] GetMensajesCLientesQuery query)
        {
            var result = (await _mediator.Send(query)).Data;
            return result;
        }

        [AllowAnonymous]
        [HttpPut]
        public async Task<ActionResult<Result<bool>>> UpdateMensaje([FromBody] UpdateMensajeAppClientesCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.Exception != null) return BadRequest(Result<bool>.FailureAsync(result.Exception));

            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] EditUserStatusClientes command)
        {
            var result = await _mediator.Send(command);
            if (result.Exception != null) return BadRequest(Result<bool>.FailureAsync(result.Exception));

            return result.Succeeded ? Ok(result) : BadRequest(result);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<RecaptchaResponseDto> VerificarCaptcha([FromBody] ValidarCaptchaClientesCommand command)
        {
            var result = (await _mediator.Send(command)).Data;
            return result;
        }
    }
}