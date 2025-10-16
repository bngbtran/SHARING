using Microsoft.AspNetCore.SignalR;

namespace VoNguyenHoangKim.RazorPages.Hubs
{
    public class NewsHub : Hub
    {
        public async Task SendNewsUpdate(string action, string newsId)
        {
            await Clients.All.SendAsync("ReceiveNewsUpdate", action, newsId);
        }
    }
}