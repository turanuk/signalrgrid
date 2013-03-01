using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using SignalRGridDemo.Models;
using System.Web.Http.OData;

namespace SignalRGridDemo.Controllers {
  public class EmployeesController : EntitySetController<Employee, int> {
    private SignalRGridDemoContext db = new SignalRGridDemoContext();

    [Queryable]
    public override IQueryable<Employee> Get() {
      return db.Employees;
    }

    protected override Employee GetEntityByKey(int key) {
      return db.Employees.Find(key);
    }

    protected override Employee PatchEntity(int key, Delta<Employee> patch) {
      Employee employeeToPatch = db.Employees.Find(key);
      patch.Patch(employeeToPatch);
      db.Entry(employeeToPatch).State = EntityState.Modified;
      db.SaveChanges();
      return employeeToPatch;
    }

    protected override void Dispose(bool disposing) {
      db.Dispose();
      base.Dispose(disposing);
    }
  }
}