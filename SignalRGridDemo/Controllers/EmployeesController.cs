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
using SignalRGridDemo.Hubs;
using SignalrGrid.WebApi;

namespace SignalRGridDemo.Controllers {
  public class EmployeesController : EntitySetControllerWithHub<EmployeeHub> {
    private SignalRGridDemoContext db = new SignalRGridDemoContext();

    [Queryable]
    public override IQueryable<Employee> Get() {
      return db.Employees;
    }

    protected override Employee GetEntityByKey(int key) {
      return db.Employees.Find(key);
    }

    protected override int GetKey(Employee entity) {
      return entity.Id;
    }

    protected override Employee PatchEntity(int key, Delta<Employee> patch) {
      Employee employeeToPatch = db.Employees.Find(key);
      patch.Patch(employeeToPatch);
      db.Entry(employeeToPatch).State = EntityState.Modified;
      db.SaveChanges();

      var changedProperty = patch.GetChangedPropertyNames().ToList()[0];
      object changedPropertyValue;
      patch.TryGetPropertyValue(changedProperty, out changedPropertyValue);

      Hub.Clients.All.updatedEmployee(employeeToPatch.Id, changedProperty, changedPropertyValue);

      return employeeToPatch;
    }

    protected override Employee CreateEntity(Employee entity) {
      var newEmployee = db.Employees.Add(entity);
      db.SaveChanges();

      Hub.Clients.All.addEmployee(newEmployee);
      return newEmployee;
    }

    public override void Delete(int key) {
      var employeeToDelete = db.Employees.Find(key);
      db.Employees.Remove(employeeToDelete);
      db.SaveChanges();

      Hub.Clients.All.removeEmployee(key);
    }

    protected override void Dispose(bool disposing) {
      db.Dispose();
      base.Dispose(disposing);
    }
  }
}