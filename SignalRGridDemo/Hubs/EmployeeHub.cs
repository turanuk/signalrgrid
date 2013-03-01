using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using SignalRGridDemo.Models;
using System.Data;

namespace SignalRGridDemo.Hubs {
  [HubName("employee")]
  public class EmployeeHub : Hub {
    private SignalRGridDemoContext db = new SignalRGridDemoContext();

    public void Lock(int id) {
      var employeeToPatch = db.Employees.Find(id);
      employeeToPatch.Locked = true;
      db.Entry(employeeToPatch).State = EntityState.Modified;
      db.SaveChanges();
      Clients.Others.lockEmployee(id);
    }

    public void Unlock(int id) {
      var employeeToPatch = db.Employees.Find(id);
      employeeToPatch.Locked = false;
      db.Entry(employeeToPatch).State = EntityState.Modified;
      db.SaveChanges();
      Clients.Others.unlockEmployee(id);
    }
  }
}