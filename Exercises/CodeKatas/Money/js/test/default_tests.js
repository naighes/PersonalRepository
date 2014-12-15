var Money = function(amount, currency) {
    this.amount = amount;
    this.currency = currency;
}

var Dollar = function(amount) {
    Money.call(this, amount, "USD");
};

Dollar.prototype = new Money();

var Franc = function(amount) {
    Money.call(this, amount, "CHF");
};

Franc.prototype = new Money();

Money.prototype.times = function(multiplier) {
    return new Money(this.amount * multiplier, this.currency);
};

Money.prototype.equals = function(other) {
    return other.amount === this.amount && this.currency === other.currency;
};

describe("smoke test", function() {
    it("contains spec with an expectation", function() {
        expect(true).toBe(true);
    });
});

describe("money example", function() {
    it("multiply dollars", function()  {
        var five = new Dollar(5);
        var product = five.times(2);
        expect(product.amount).toBe(10);
        product = five.times(3);
        expect(product.amount).toBe(15);
    });

    it("equality", function() {
        var a = new Dollar(5);
        var b = new Dollar(5);
        expect(a.equals(b)).toBe(true);
        var c = new Dollar(6);
        expect(a.equals(c)).toBe(false);
        var d = new Franc(6);
        expect(c.equals(d)).toBe(false);
    });

    it("multiply francs", function()  {
        var five = new Franc(5);
        var product = five.times(2);
        expect(product.amount).toBe(10);
        product = five.times(3);
        expect(product.amount).toBe(15);
    });
});
