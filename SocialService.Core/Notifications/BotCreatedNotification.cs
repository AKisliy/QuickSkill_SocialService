using MediatR;
using SocialService.Core.Models;

namespace SocialService.Core.Notifications
{
    public record BotCreatedNotification(List<User> bots): INotification;
}