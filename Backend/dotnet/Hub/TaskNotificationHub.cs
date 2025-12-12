using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace dotnet.Hubs
{
    public class TaskNotificationHub : Hub
    {
         public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }
        public async Task SendTaskAssignedNotification(string assignedTo, string taskName)
        {
            await Clients.User(assignedTo).SendAsync("ReceiveTaskNotification", taskName);
        }
    }
}
