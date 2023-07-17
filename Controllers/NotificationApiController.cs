using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using rmsbe.BasicAuth;
using rmsbe.Contracts.Notifications;
using rmsbe.Hub;

namespace rmsbe.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = "Bearer"), BasicAuthorization]
[Route("notification-controller")]
public class NotificationApiController : ControllerBase
{
    private IHubContext<NotificationHub, INotificationHub> _hub;

    public NotificationApiController(IHubContext<NotificationHub, INotificationHub> hub)
    {
        _hub = hub;
    }

    [HttpPost]
    [Route("push")]
    public IActionResult SendPushNotification([FromBody] Notification notification)
    {
        _hub.Clients.All.SendPushNotification(notification);
        return Ok();
    }
}