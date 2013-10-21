using Microsoft.Owin;
using Owin;

namespace SignalRGridDemo
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}