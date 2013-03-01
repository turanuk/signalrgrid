using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using SignalRGridDemo.Models;
using System.Data;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace SignalRGridDemo.Hubs {
  [HubName("employee")]
  public class EmployeeHub : Hub {
    private static ConcurrentDictionary<string, List<int>> _mapping = new ConcurrentDictionary<string, List<int>>();
    private SignalRGridDemoContext db = new SignalRGridDemoContext();

    public override Task OnConnected() {
      _mapping.TryAdd(Context.ConnectionId, new List<int>());
      Clients.All.newConnection(Context.ConnectionId);
      return base.OnConnected();
    }

    public void Lock(int id) {
      var employeeToPatch = db.Employees.Find(id);
      employeeToPatch.Locked = true;
      db.Entry(employeeToPatch).State = EntityState.Modified;
      db.SaveChanges();
      Clients.Others.lockEmployee(id);
      _mapping[Context.ConnectionId].Add(id);
    }

    public void Unlock(int id) {
      var employeeToPatch = db.Employees.Find(id);
      employeeToPatch.Locked = false;
      db.Entry(employeeToPatch).State = EntityState.Modified;
      db.SaveChanges();
      Clients.Others.unlockEmployee(id);
      _mapping[Context.ConnectionId].Remove(id);
    }

    public override Task OnDisconnected() {
      foreach (var id in _mapping[Context.ConnectionId]) {
        Unlock(id);
      }
      var list = new List<int>();
      _mapping.TryRemove(Context.ConnectionId, out list);
      return base.OnDisconnected();
    }
  }
}