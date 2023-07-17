using Microsoft.AspNetCore.SignalR;
using rmsbe.Contracts.Notifications;

namespace rmsbe.Hub;

public class NotificationHub : Hub<INotificationHub>
{
    public async Task SendPushNotification(Notification notification)
    {
        await Clients.All.SendPushNotification(notification);
    }
}