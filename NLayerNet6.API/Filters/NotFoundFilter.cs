using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NLayerNet6.Core.Dtos;
using NLayerNet6.Core.Models;
using NLayerNet6.Core.Services;

namespace NLayerNet6.API.Filters
{
    public class NotFoundFilter<T> : IAsyncActionFilter where T : BaseEntity
    {
        private readonly IService<T> _service;

        public NotFoundFilter(IService<T> service)
        {
            _service = service;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var value = context.ActionArguments.Values.FirstOrDefault();

            if (value == null)
            {
                await next.Invoke();
                return;
            }

            var id = (int)value;

            var entity = await _service.AnyAsync(x => x.Id == id);

            if (entity)
            {
                await next.Invoke();
                return;
            }

            context.Result = new NotFoundObjectResult(ResponseDto<NoContentDto>.Fail(404, $"{typeof(T).Name}({id}) not found"));
        }
    }
}
