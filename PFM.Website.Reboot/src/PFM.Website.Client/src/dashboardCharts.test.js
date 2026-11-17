import test from "node:test";
import assert from "node:assert/strict";
import { categoryData, lastTwelveMonths, monthLabel, monthlyValues } from "./dashboardCharts.js";

test("monthly chart data is chronological and fills missing months", () => {
  const months = lastTwelveMonths(new Date("2026-01-15T12:00:00Z"));
  assert.equal(months.length, 12);
  assert.equal(months[0], "202502");
  assert.equal(months.at(-1), "202601");
  assert.equal(monthLabel(months[0]), "2025-02");
  assert.deepEqual(monthlyValues(["202511", "202512", "202601"], { 202511: { actual: 10 }, 202601: { actual: 5 } }, "actual"), [10, 0, 5]);
  assert.deepEqual(monthlyValues(["202511", "202512"], { 202512: 9 }), [0, 9]);
});

test("category charts use visible expense type names and colours", () => {
  const result = categoryData({ Food: 25, Hidden: 40, Unknown: 30 }, [
    { name: "Food", graphColor: "ff0022", showOnDashboard: true },
    { name: "Hidden", graphColor: "bad", showOnDashboard: false },
    { name: "Empty", graphColor: "bad", showOnDashboard: true },
  ]);
  assert.deepEqual(result, [{ name: "Food", value: 25, color: "#ff0022" }]);
  assert.equal(categoryData({ Food: 1 }, [{ name: "Food", graphColor: "invalid", showOnDashboard: true }])[0].color, "#527da1");
});
