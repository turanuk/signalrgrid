namespace SignalRGridDemo.Migrations {
  using SignalRGridDemo.Models;
  using System;
  using System.Data.Entity;
  using System.Data.Entity.Migrations;
  using System.Linq;

  internal sealed class Configuration : DbMigrationsConfiguration<SignalRGridDemo.Models.SignalRGridDemoContext> {
    public Configuration() {
      AutomaticMigrationsEnabled = false;
    }

    protected override void Seed(SignalRGridDemo.Models.SignalRGridDemoContext context) {
      context.Employees.AddOrUpdate(
         e => e.Name,
         new Employee { Name = "Jim Wang", Email = "jim.wang@microsoft.com", Salary = 1 },
         new Employee { Name = "Kiran Challa", Email = "kiranchalla@microsoft.com", Salary = 1 },
         new Employee { Name = "Steve Sanderson", Email = "stevesanderson@microsoft.com", Salary = 1 }
       );
    }
  }
}
