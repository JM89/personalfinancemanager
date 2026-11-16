export function lastTwelveMonths(now = new Date()) {
  return Array.from({ length: 12 }, (_, index) => {
    const month = new Date(Date.UTC(now.getUTCFullYear(), now.getUTCMonth() - 11 + index, 1));
    return `${month.getUTCFullYear()}${String(month.getUTCMonth() + 1).padStart(2, "0")}`;
  });
}

export function monthLabel(month) {
  return `${month.slice(0, 4)}-${month.slice(4)}`;
}

export function monthlyValues(months, aggregates, field) {
  return months.map((month) => field ? aggregates[month]?.[field] ?? 0 : aggregates[month] ?? 0);
}

export function categoryData(aggregates, expenseTypes) {
  return expenseTypes
    .filter((type) => type.showOnDashboard && Object.hasOwn(aggregates, type.name))
    .map((type) => ({
      name: type.name,
      value: aggregates[type.name],
      color: /^[0-9a-f]{6}$/i.test(type.graphColor) ? `#${type.graphColor}` : "#527da1",
    }));
}
