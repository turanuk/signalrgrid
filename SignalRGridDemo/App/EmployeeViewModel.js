var EmployeeViewModel = function () {
  var self = this;
  self.employees = ko.observableArray();
  self.loading = ko.observable(true);

  $.getJSON('api/employee', function (data) {
    self.employees(data);
    self.loading(false);
  });
}

$(function () {
  ko.applyBindings(new EmployeeViewModel());
});