using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRGridDemo.Models {
  public class Employee {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public float Salary { get; set; }
    public bool Locked { get; set; }
  }
}