function CalculateDeal() {
    const unitPrice = parseFloat($("#deal-unit-price").val());
    var amount = parseFloat($("#deal-amount").val());
    var deliveryCost = parseFloat($("#delivery-cost").val());
    $("#total-cost").val((amount * unitPrice) + deliveryCost);
    $("#show-currency").text($("#selected-currency").find(":selected").text());
}
