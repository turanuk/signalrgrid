var EmployeeViewModel = function () {
  var self = this;
  self.employees = ko.observableArray();

  $.getJSON('api/employee', function (data) {
    self.employees(data);
  });
}

$(function () {
  ko.applyBindings(new EmployeeViewModel());
});