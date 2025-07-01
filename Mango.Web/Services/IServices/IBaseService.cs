using Mango.Web.Models;

namespace Mango.Web.Services.IServices
{
    public interface IBaseService
    {
        Task<ResponseDTO?> SendAsync(RequestDto request);
    }
}
