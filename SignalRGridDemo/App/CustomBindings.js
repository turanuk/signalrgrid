ko.bindingHandlers.displayMoney = {
  init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
    $(element).html('$' + valueAccessor()().toFixed(2));
  },
  update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
    $(element).html('$' + valueAccessor()().toFixed(2));
  }
};