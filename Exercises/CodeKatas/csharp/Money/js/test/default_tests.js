var Money = function(amount, currency) {
    this.amount = amount;
    this.currency = currency;
};

var Dollar = function(amount) {
    Money.call(this, amount, "USD");
};
Dollar.prototype = new Money();

var Franc = function(amount) {
    Money.call(this, amount, "CHF");
};
Franc.prototype = new Money();

Money.prototype.multiply = function(times) {
    return new Money(this.amount * times, this.currency);
};

Money.prototype.equals = function(other) {
    return this.amount === other.amount && this.currency == other.currency;
};

describe("smoke test", function() {
    it("contains spec with an expectation", function() {
        expect(true).toBe(true);
    });
});

describe("money example", function() {
    it("multiply dollars by scalar", function() {
        var five = new Dollar(5);
        var ten = five.multiply(2);
        expect(ten.equals(new Dollar(5 * 2))).toBe(true);
        var fifteen = five.multiply(3);
        expect(fifteen.equals(new Dollar(5 * 3))).toBe(true);
    });

    it("multiply francs by scalar", function() {
        var five = new Franc(5);
        var ten = five.multiply(2);
        expect(ten.equals(new Franc(5 * 2))).toBe(true);
    });

    it("equality", function() {
        var a = new Dollar(5);
        var b = new Dollar(5);
        expect(a.equals(b)).toBe(true);
        var c = new Dollar(6);
        expect(c.equals(a)).toBe(false);
        var d = new Franc(6);
        expect(c.equals(d)).toBe(false);
        expect(d.equals(c)).toBe(false);
    });

    it("dollar currency", function() {
        var dollar = new Dollar(5);
        expect(dollar.currency).toBe("USD");
    });

    it("franc currency", function() {
        var franc = new Franc(5);
        expect(franc.currency).toBe("CHF");
    });
});
