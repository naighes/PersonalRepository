var Money = function(amount) {
    this.amount = amount;
}

var Dollar = function(amount) {
    this.amount = amount;
};

Dollar.prototype = new Money();

var Franc = function(amount) {
    this.amount = amount;
};

Franc.prototype = new Money();

Money.prototype.currency = function() {
    return "";
};

Dollar.prototype.currency = function() {
    return "USD";
};

Franc.prototype.currency = function() {
    return "CHF";
};

Money.prototype.dollar = function(amount) {
    return new Dollar(amount);
}

Money.prototype.franc = function(amount) {
    return new Franc(amount);
}

Money.prototype.times = function(multiplier) {
    return new Money(this.amount * multiplier);
};

Dollar.prototype.times = function(multiplier) {
    return new Dollar(this.amount * multiplier);
};

Franc.prototype.times = function(multiplier) {
    return new Franc(this.amount * multiplier);
};

Money.prototype.equals = function(other) {
    return other.amount === this.amount && this.currency() === other.currency();
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
