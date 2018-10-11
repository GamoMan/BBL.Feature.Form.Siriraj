require(["jasmineEnv"], function (jasmineEnv) {
  var setupTests = function () {
    "use strict";

    describe("Given a BualuangExclusive model", function () {
      var component = new Sitecore.Definitions.Models.BualuangExclusive();

      describe("when I create a BualuangExclusive model", function () {
        it("it should have a 'isVisible' property that determines if the BualuangExclusive component is visible or not", function () {
          expect(component.get("isVisible")).toBeDefined();
        });

        it("it should set 'isVisible' to true by default", function () {
          expect(BualuangExclusive.get("isVisible")).toBe(true);
        });

        it("it should have a 'toggle' function that either shows or hides the BualuangExclusive component depending on the 'isVisible' property", function () {
          expect(component.toggle).toBeDefined();
        });
      });
    });
  };

  runTests(jasmineEnv, setupTests);
});