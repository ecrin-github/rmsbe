using rmsbe.Contracts.Notifications;

namespace rmsbe.Hub;

public interface INotificationHub
{
    public Task SendPushNotification(Notification notification);
}