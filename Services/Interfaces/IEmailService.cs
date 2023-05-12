using rmsbe.Contracts.Email.Request;
using rmsbe.Contracts.Email.Response;

namespace rmsbe.Services.Interfaces;

public interface IEmailService
{
    EmailServiceResponse Send(EmailRequestBody emailRequestBody);
}