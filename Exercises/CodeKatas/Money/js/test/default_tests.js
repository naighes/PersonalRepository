var Dollar = function(amount) {
    this.amount = amount;
    this.times = function(multiplier) {
        return new Dollar(this.amount * multiplier);
    };

    this.equals = function(other) {
        return other.amount === this.amount;
    };
};

describe("smoke test", function() {
    it("contains spec with an expectation", function() {
        expect(true).toBe(true);
    });
});

describe("money example", function() {
    it("multiply", function()  {
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
    });
});
