import React from "react";
import { Chart as ChartJS, ArcElement, BarController, BarElement, CategoryScale, Legend, LinearScale, LineController, LineElement, PointElement, Tooltip } from "chart.js";
import { Bar, Chart, Pie } from "react-chartjs-2";
import { categoryData, lastTwelveMonths, monthLabel, monthlyValues } from "./dashboardCharts.js";

ChartJS.register(ArcElement, BarController, BarElement, CategoryScale, Legend, LinearScale, LineController, LineElement, PointElement, Tooltip);

const colors = { expenses: "#2a83c5", expected: "#8998a8", incomes: "#37945e", savings: "#d7a53f" };
const options = { responsive: true, maintainAspectRatio: false, animation: false, plugins: { legend: { position: "bottom" } }, scales: { y: { beginAtZero: true } } };

function NoData({ aggregates, children }) {
  return Object.keys(aggregates).length ? children : <p>No summary data for this account.</p>;
}

export function ExpenseTrendChart({ aggregates }) {
  const months = lastTwelveMonths();
  const data = {
    labels: months.map(monthLabel),
    datasets: [
      { label: "Actual", data: monthlyValues(months, aggregates, "actual"), borderColor: colors.expenses, backgroundColor: colors.expenses, tension: 0.2 },
      { type: "bar", label: "Expected", data: monthlyValues(months, aggregates, "expected"), borderColor: colors.expected, backgroundColor: colors.expected },
    ],
  };
  return <NoData aggregates={aggregates}><div className="chart-container"><Chart type="line" aria-label="Expenses over time chart" role="img" data={data} options={options} /></div></NoData>;
}

export function MovementChart({ aggregates }) {
  const months = lastTwelveMonths();
  const data = {
    labels: months.map(monthLabel),
    datasets: [
      { label: "Incomes", data: monthlyValues(months, aggregates, "incomesAmount"), backgroundColor: colors.incomes },
      { label: "Expenses", data: monthlyValues(months, aggregates, "expensesAmount"), backgroundColor: colors.expenses },
      { label: "Savings", data: monthlyValues(months, aggregates, "savingsAmount"), backgroundColor: colors.savings },
    ],
  };
  return <NoData aggregates={aggregates}><div className="chart-container"><Bar aria-label="Movement over time chart" role="img" data={data} options={options} /></div></NoData>;
}

export function CategoryChart({ aggregates, expenseTypes, onSelect }) {
  const categories = categoryData(aggregates, expenseTypes);
  if (!categories.length || categories.every((item) => item.value === 0)) return <p>No expense category data for this account.</p>;
  const data = {
    labels: categories.map((item) => item.name),
    datasets: [{ data: categories.map((item) => item.value), backgroundColor: categories.map((item) => item.color) }],
  };
  return <><div className="chart-container"><Pie aria-label="Expenses by category chart" role="img" data={data} options={{
    responsive: true,
    maintainAspectRatio: false,
    animation: false,
    plugins: { legend: { display: false } },
    onClick: (_event, elements) => { if (elements.length) onSelect(categories[elements[0].index].name); },
  }} /></div>
    <ul className="chart-legend">{categories.map((item) => <li key={item.name}>
      <button className="text-button" onClick={() => onSelect(item.name)}><span className="chart-swatch" style={{ backgroundColor: item.color }} />{item.name}: {item.value.toFixed(2)}</button>
    </li>)}</ul></>;
}

export function CategoryTrendChart({ aggregates, category, expenseTypes }) {
  const months = lastTwelveMonths();
  const color = expenseTypes.find((type) => type.name === category)?.graphColor;
  const data = { labels: months.map(monthLabel), datasets: [{ label: category, data: monthlyValues(months, aggregates), backgroundColor: /^[0-9a-f]{6}$/i.test(color) ? `#${color}` : colors.expenses }] };
  return <NoData aggregates={aggregates}><div className="chart-container"><Bar aria-label={`${category} expenses over time chart`} role="img" data={data} options={options} /></div></NoData>;
}
