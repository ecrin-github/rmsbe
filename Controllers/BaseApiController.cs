using Microsoft.AspNetCore.Mvc;
using rmsbe.SysModels;

namespace rmsbe.Controllers
{
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        protected ApiResponse<T> ListSuccessResponse<T>(int count, List<T> data)
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
        
        protected EmptyApiResponse DeletionSuccessResponse(int count, string attribute_type,
                  string parent_id, string id)
        {
            var message = parent_id == "" ? $"{attribute_type} {id} removed."
                : $"{attribute_type} {parent_id} :: {id} removed.";
            return new EmptyApiResponse
            {
                Total = count, StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { message }
            };
        }
        
        protected EmptyApiResponse NoParentResponse(string parent_type, string id_type, string id)
        {
            string message = $"Parent {parent_type} with {id_type} {id} was not found";
            return new EmptyApiResponse
            {
                Total = 0, StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { message }
            };
        }

        protected EmptyApiResponse NoAttributesResponse(string attribute_types)
        {
            var message = $"No {attribute_types} were found";
            return new EmptyApiResponse
            {
                Total = 0, StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { message }
            };
        }
        
        protected EmptyApiResponse NoEntityResponse(string entity_type ,string id)
        {
            var message = $"No {entity_type} with id {id} was found";
            return new EmptyApiResponse
            {
                Total = 0, StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { message }
            };
        }
        
        protected EmptyApiResponse NoParentAttResponse(string attribute_type,
                           string parent_type, string parent_id, string id)
        {
            var message = $"No {attribute_type} with id {id} was found for {parent_type} {id}";
            return new EmptyApiResponse
            {
                Total = 0, StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { message }
            };
        }
        
        protected EmptyApiResponse ErrorResponse(string error_context, 
                             string entity_type, string parent_type, string parent_id, string id)
        { 
            string message = "";
            switch (error_context)
            {
                case "c":
                {
                    message = parent_id == "" ? $"Error occured during creation of {entity_type} {id}"
                        : $"Error occured during creation of {entity_type} for {parent_type} {parent_id}";
                    break;
                }
                case "r":
                {
                    message = parent_id == "" ? $"Error occured during fetch of {entity_type} {id}"
                        : $"Error occured during fetch of {entity_type} for {parent_type} {parent_id}";
                    break;
                }
                case "u":
                {
                    message = parent_id == "" ? $"Error occured during update of {entity_type} {id}"
                        : $"Error occured during update of {entity_type} {parent_id} :: {id}";
                    break;
                }
                case "d":
                {
                    message = parent_id == "" ? $"Error occured during deletion of {entity_type} {id}"
                    : $"Error occured during deletion of {entity_type} {parent_id} :: {id}";
                    break;
                }
            }
            return new EmptyApiResponse
            {
                Total = 0, StatusCode = BadRequest().StatusCode,
                Messages = new List<string>() { message }
            };
        }
        
        protected EmptyApiResponse NoLupResponse<T>(string type_name)
        {
            var message = $"No lookup values found using type name '{type_name}'.";
            return new EmptyApiResponse
            {
                Total = 0, StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { message }
            };
        }
        
        protected EmptyApiResponse NoLupDecode<T>(string type_name, string code_name)
        {
            var message = $"No matching values for code {code_name} found in look up type '{type_name}.";
            return new EmptyApiResponse
            {
                Total = 0, StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { message }
               
            };
        }
        
        protected EmptyApiResponse NoLupCode<T>(string type_name, string decode_name)
        {
            var message = $"No matching values for decode {decode_name} found in look up type '{type_name}.";;
            return new EmptyApiResponse
            {
                Total = 0, StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { message }
            };
        }
        
        
        /*
        protected ApiResponse<T> NoAttributesFoundResponse<T>(string message)
        {
            return new ApiResponse<T>
            {
                Total = 0, StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { message }, Data = null
            };
        }
        */
        
        /*
        protected ApiResponse<T> MissingAttributeResponse<T>(string message)
        {
            return new ApiResponse<T>
            {
                Total = 0, StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { message }, Data = null
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
        */
    }
}