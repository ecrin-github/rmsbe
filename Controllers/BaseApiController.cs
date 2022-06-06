using Microsoft.AspNetCore.Mvc;
using rmsbe.SysModels;

namespace rmsbe.Controllers
{
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        protected ApiResponse<T> NoStudyResponse<T>()
        {
            return new ApiResponse<T>
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No study was found with the id provided." },
                Data = null
            };
        }
        
        protected ApiResponse<T> NoObjectResponse<T>()
        {
            return new ApiResponse<T>
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data object was found with the id provided." },
                Data = null
            };
        }
        
        protected ApiResponse<T> ErrorInActionResponse<T>(string message)
        {
            return new ApiResponse<T>
            {
                Total = 0,
                StatusCode = BadRequest().StatusCode,
                Messages = new List<string>() { message },
                Data = null
            };
        }
    }
}