using Microsoft.AspNetCore.Mvc;
using rmsbe.SysModels;

namespace rmsbe.Controllers
{
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        protected ApiResponse<T> CollectionSuccessResponse<T>(int count, List<T> data)
        {
            return new ApiResponse<T>
            {
                Total = count, StatusCode = NotFound().StatusCode, Messages = null,
                Data = data
            };
        }
        
        protected ApiResponse<T> SingleSuccessResponse<T>(List<T> data)
        {
            return new ApiResponse<T>
            {
                Total = 1, StatusCode = NotFound().StatusCode, Messages = null,
                Data = data
            };
        }
        protected ApiResponse<T> DeletionSuccessResponse<T>(int count, string message)
        {
            return new ApiResponse<T>
            {
                Total = count, StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { message }, 
                Data = null
            };
        }
        
        protected ApiResponse<T> NoStudyResponse<T>()
        {
            return new ApiResponse<T>
            {
                Total = 0, StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No study was found with the id provided." }, Data = null
            };
        }
        
        protected ApiResponse<T> StudyDoesNotExistResponse<T>()
        {
            return new ApiResponse<T>
            {
                Total = 0, StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No study was found with the id provided." }, Data = null
            };
        }
        
        protected ApiResponse<T> NoObjectResponse<T>()
        {
            return new ApiResponse<T>
            {
                Total = 0, StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No data object was found with the id provided." }, Data = null
            };
        }
        
        protected ApiResponse<T> NoDtpResponse<T>()
        {
            return new ApiResponse<T>
            {
                Total = 0, StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No DTP was found with the id provided." }, Data = null
            };
        }
        
        protected ApiResponse<T> NoDupResponse<T>()
        {
            return new ApiResponse<T>
            {
                Total = 0, StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No DUP was found with the id provided." }, Data = null
            };
        }
        
        protected ApiResponse<T> NoAttributesResponse<T>(string message)
        {
            return new ApiResponse<T>
            {
                Total = 0, StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { message }, Data = null
            };
        }
        
        protected ApiResponse<T> NoAttributesFoundResponse<T>(string message)
        {
            return new ApiResponse<T>
            {
                Total = 0, StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { message }, Data = null
            };
        }
        
        protected ApiResponse<T> MissingAttributeResponse<T>(string message)
        {
            return new ApiResponse<T>
            {
                Total = 0, StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { message }, Data = null
            };
        }
        
        protected ApiResponse<T> ErrorInActionResponse<T>(string message)
        {
            return new ApiResponse<T>
            {
                Total = 0, StatusCode = BadRequest().StatusCode,
                Messages = new List<string>() { message }, Data = null
            };
        }
        
        protected ApiResponse<T> NoLupResponse<T>(string type_name)
        {
            return new ApiResponse<T>
            {
                Total = 0, StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No lookup values found using type name '" + type_name + "." },
                Data = null
            };
        }
        
        protected ApiResponse<T> NoLupDecode<T>(string type_name, string code_name)
        {
            return new ApiResponse<T>
            {
                Total = 0, StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { @"No matching values for code " + code_name + " " +
                                                "found in look up type '" + type_name + "." },
                Data = null
            };
        }
        
        protected ApiResponse<T> NoLupCode<T>(string type_name, string decode_name)
        {
            return new ApiResponse<T>
            {
                Total = 0, StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { @"No matching values for decode " + decode_name + " " +
                                                "found in look up type '" + type_name + "."  },
                Data = null
            };
        }
    }
}