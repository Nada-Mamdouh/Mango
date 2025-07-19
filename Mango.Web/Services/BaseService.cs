using Mango.Web.Models;
using Mango.Web.Services.IServices;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using static Mango.Web.Utility.SD;

namespace Mango.Web.Services
{
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenProvider _tokenProvider;
        public BaseService(IHttpClientFactory httpClientFactory, ITokenProvider tokenProvider)
        {
            _httpClientFactory = httpClientFactory;
            _tokenProvider = tokenProvider;
        }
        public async Task<ResponseDTO> SendAsync(RequestDto request, bool withTokenEnabled)
        {
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("MangoApi");
                HttpRequestMessage message = new();
                //message.Headers.Add("Content-Type", "application/json"); This throws an exception - content type will be automatically set tho
                message.RequestUri = new Uri(request.Url);
                //Token

                if (request.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(request.Data), Encoding.UTF8, "application/json");
                }
                switch (request.ApiType)
                {
                    case ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }
                if (withTokenEnabled)
                {
                    var token = _tokenProvider.GetToken();
                    message.Headers.Add("Authorization", $"Bearer {token}");
                }
                HttpResponseMessage? response = await client.SendAsync(message);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        return new ResponseDTO() { IsSuccess = false, Message = "Not found!" };
                        break;
                    case HttpStatusCode.Forbidden:
                        return new ResponseDTO() { IsSuccess = false, Message = "Denied" };
                        break;
                    case HttpStatusCode.Unauthorized:
                        return new ResponseDTO() { IsSuccess = false, Message = "Unauthorized" };
                        break;
                    case HttpStatusCode.InternalServerError:
                        return new ResponseDTO() { IsSuccess = false, Message = "Internal server error" };
                        break;
                    default:
                        var responseContent = await response.Content.ReadAsStringAsync();
                        ResponseDTO responseDTO = JsonConvert.DeserializeObject<ResponseDTO>(responseContent);
                        return responseDTO;
                }
            }
            catch (Exception ex)
            {
                return new ResponseDTO()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }



        }
    }
}
