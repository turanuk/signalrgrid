var EmployeeViewModel = function (signalRContext) {
  var self = this;
  self.employees = ko.observableArray();
  self.loading = ko.observable(true);
  self.displayMode = function (employee) {
    if (employee.Locked()) {
      return 'lock-template';
    } else {
      return employee.Edit() ? 'edit-template' : 'read-template';
    }
  }
  self.connected = ko.observable('');

  self.add = function () {
    var payload = { Name: "New", Email: "New", Salary: 1 };
    $.ajax({
      url: '/odata/Employees',
      type: 'POST',
      data: JSON.stringify(payload),
      contentType: 'application/json',
      dataType: 'json'
    });
  }

  self.remove = function (employee) {
    $.ajax({
      url: '/odata/Employees(' + employee.Id + ')',
      type: 'DELETE',
      contentType: 'application/json',
      dataType: 'json'
    });
  }

  self.edit = function (employee) {
    employee.Edit(true);
    signalRContext.server.lock(employee.Id);
  }

  self.done = function (employee) {
    employee.Edit(false);
    signalRContext.server.unlock(employee.Id);
  }

  self.watchModel = function (model, callback) {
    for (var key in model) {
      if (model.hasOwnProperty(key) && ko.isObservable(model[key]) && key !== 'Locked' && key != 'Edit') {
        self.subscribeToProperty(model, key, function (key, val) {
          callback(model, key, val);
        });
      }
    }
  }

  self.subscribeToProperty = function (model, key, callback) {
    model[key].subscribe(function (val) {
      callback(key, val);
    });
  }

  self.modelChanged = function (model, key, val) {
    var payload = {};
    payload[key] = val;
    $.ajax({
      url: '/odata/Employees(' + model.Id + ')',
      type: 'PATCH',
      data: JSON.stringify(payload),
      contentType: 'application/json',
      dataType: 'json'
    });
  }

  $.getJSON('odata/Employees', function (data) {
    self.employees(ko.utils.arrayMap(data.value, function (employee) {
      var obsEmployee = {
        Id: employee.Id,
        Name: ko.observable(employee.Name),
        Email: ko.observable(employee.Email),
        Salary: ko.observable(employee.Salary),
        Edit: ko.observable(false),
        Locked: ko.observable(employee.Locked)
      }
      self.watchModel(obsEmployee, self.modelChanged);
      return obsEmployee;
    }));
    self.loading(false);
  });
}

$(function () {
  var employeeSignalR = $.connection.employee;
  var viewModel = new EmployeeViewModel(employeeSignalR);

  var findEmployee = function (id) {
    return ko.utils.arrayFirst(viewModel.employees(), function (item) {
      if (item.Id == id) {
        return item;
      }
    });
  }

  employeeSignalR.client.addEmployee = function (employee) {
    var obsEmployee = {
      Id: employee.Id,
      Name: ko.observable(employee.Name),
      Email: ko.observable(employee.Email),
      Salary: ko.observable(employee.Salary),
      Edit: ko.observable(false),
      Locked: ko.observable(employee.Locked)
    }
    viewModel.employees.push(obsEmployee);
    viewModel.watchModel(obsEmployee, viewModel.modelChanged);
  }

  employeeSignalR.client.removeEmployee = function (id) {
    viewModel.employees.remove(function (item) { return item.Id == id });
  }

  employeeSignalR.client.updatedEmployee = function (id, key, value) {
    var employee = findEmployee(id);
    employee[key](value);
  }

  employeeSignalR.client.lockEmployee = function (id) {
    var employee = findEmployee(id);
    employee.Locked(true);
  }

  employeeSignalR.client.unlockEmployee = function (id) {
    var employee = findEmployee(id);
    employee.Locked(false);
  }

  employeeSignalR.client.newConnection = function (id) {
    viewModel.connected(id + ' connected.');
  }

  ko.applyBindings(viewModel);
  $.connection.hub.start();
});
