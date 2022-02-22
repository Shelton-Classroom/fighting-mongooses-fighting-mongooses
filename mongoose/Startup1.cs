using Microsoft.Owin;
using Owin;
[assembly: OwinStartup(typeof(SignalRChat.Startup1))]
namespace SignalRChat
{
    public class Startup1
    {
        public void Configuration(IAppBuilder app)
        {
            // Any connection or hub wire up and configuration should go here
            app.MapSignalR();
        }
    }
}