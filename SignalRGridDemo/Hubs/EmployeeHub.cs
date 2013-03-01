using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace SignalRGridDemo.Hubs {
  [HubName("employee")]
  public class EmployeeHub : Hub {
    public void Hello() {
      Clients.All.hello();
    }
  }
}