Number.prototype.isDivisibleBy = function(num) {
    return this % num == 0;
}

var Game = function(end) {
    var rules = [
        {
            check: function(i) { return i.isDivisibleBy(3) && i.isDivisibleBy(5); },
            print: function(i) { return "FizzBuzz"; }
        },
        {
            check: function(i) { return i.isDivisibleBy(3); },
            print: function(i) { return "Fizz"; }
        },
        {
            check: function(i) { return i.isDivisibleBy(5); },
            print: function(i) { return "Buzz"; }
        },
        {
            check: function(i) { return true; },
            print: function(i) { return i.toString(); }
        }
    ];

    this.run = function() {
        var result = [];

        for (var i = 1; i <= end; i++) {
            for (var j = 0; j < rules.length; j++) {
                if (rules[j].check(i)) {
                    result.push(rules[j].print(i));
                    break;
                }
            }
        } 

        return result.join(" ");
    }
}

describe("smoke test", function() {
    it("contains spec with an expectation", function() {
        expect(true).toBe(true);
    });
});

describe("game tests", function() {
    it("count until one", function() {
        var result = new Game(1).run();
        expect(result).toBe("1");
    });

    it("count until two", function() {
        var result = new Game(2).run();
        expect(result).toBe("1 2");
    });

    it("count until three", function() {
        var result = new Game(3).run();
        expect(result).toBe("1 2 Fizz");
    });

    it("count until five", function() {
        var result = new Game(5).run();
        expect(result).toBe("1 2 Fizz 4 Buzz");
    });
});
