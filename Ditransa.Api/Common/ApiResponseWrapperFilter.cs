using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Ditransa.Api.Common
{
    public class ApiResponseWrapperFilter : IAsyncResultFilter
    {
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            // Si la respuesta ya está envuelta, no la volvemos a envolver
            if (context.Result is ObjectResult objectResult &&
                objectResult.Value is not ApiResponse<object>)
            {
                var wrapped = new ApiResponse<object>(
                    data: objectResult.Value,
                    message: "Operación exitosa",
                    success: true
                );

                context.Result = new ObjectResult(wrapped)
                {
                    StatusCode = objectResult.StatusCode
                };
            }

            await next();
        }
    }
}
