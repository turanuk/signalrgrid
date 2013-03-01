using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using SignalRGridDemo.Models;
using System;
using System.Web.Http.OData;

namespace SignalrGrid.WebApi {
  public class EntitySetControllerWithHub<THub> : EntitySetController<Employee, int>
  where THub : IHub {
    Lazy<IHubContext> hub = new Lazy<IHubContext>(
      () => GlobalHost.ConnectionManager.GetHubContext<THub>()
    );

    protected IHubContext Hub {
      get { return hub.Value; }
    }
  }
}