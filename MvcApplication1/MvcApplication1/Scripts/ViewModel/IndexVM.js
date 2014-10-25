var urlPath = window.location.pathname;
 
$(function () {
    ko.applyBindings(indexVM);
    indexVM.loadOrderSummaries(0);
});
 
var indexVM = {
    OrderSummaries: ko.observableArray([]),
 
    loadOrderSummaries: function (type) {
        var self = this;
        //Ajax Call Get All Articles
        $.ajax({
            type: "GET",
        url: 'Home/FillIndex?type=' + type,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var count = Object.keys(data).length;
            var totalOrdered = 0, totalShipped = 0;
            data.forEach(function (d) {
                var milli = d.PeriodDate.replace(/\/Date\((-?\d+)\)\//, '$1');
                var d2 = new Date(parseInt(milli));
                d.PeriodDate = d2;
                totalOrdered += d.TotalOrderedAmount;
                totalShipped += d.TotalShippedAmount;
            });
            $('.totOrdered').html('$' + totalOrdered.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,'));
            var avgOrdered = totalOrdered / count;
            $('.avgOrdered').html('$'+avgOrdered.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,'));
            $('.totShipped').html('$' + totalShipped.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,'));
            var avgShipped = totalShipped / count;
            $('.avgShipped').html('$' + avgShipped.toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,'));
            self.OrderSummaries(data); //Put the response in ObservableArray
            initLineGraph(data);
            switch (type) {
                case 0:
                    $('#title').html('Order Summary, Week Over Week');
                    break;
                case 1:
                    $('#title').html('Order Summary, Month Over Month');
                    break;
                case 2:
                    $('#title').html('Order Summary, Year Over Year');
                    break;
                default:
                    break;
            }
        },
        error: function (err) {
            alert(err.status + " : " + err.statusText);
        }
    });
 
}
};
 
function OrderSummaries(OrderSummaries) {
    this.Id = ko.observable(OrderSummaries.Id);
    var milli = OrderSummaries.PeriodDate.replace(/\/Date\((-?\d+)\)\//, '$1');
    var d = new Date(parseInt(milli));
    this.PeriodDate = ko.observable(d);
    this.PeriodLabel = ko.observable(OrderSummaries.PeriodLabel);
    this.Ordered = ko.observable(OrderSummaries.Ordered);
    this.Shipped = ko.observable(OrderSummaries.Shipped);
    this.Remainder = ko.observable(OrderSummaries.Remainder);
    this.TotalOrderedAmount = ko.observable(OrderSummaries.TotalOrderedAmount);
    this.TotalShippedAmount = ko.observable(OrderSummaries.TotalShippedAmount);
}