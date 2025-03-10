import React from "react";
import { PieChart, Pie, Cell, Tooltip, ResponsiveContainer, Legend } from "recharts";

const COLORS = ["#8884d8", "#82ca9d", "#ffc658", "#ff8042", "#ff6384", "#36a2eb"];

const LanguagePieChart = ({ languages }) => {
  if (!languages || Object.keys(languages).length === 0) {
    return <p>Nenhum dado disponibilizado!</p>;
  }

  const totalLines = Object.values(languages).reduce((sum, value) => sum + value, 0);

  const threshold = totalLines * 0.05;

  const groupedData = [];
  let otherSum = 0;

  Object.entries(languages).forEach(([name, value]) => {
    if (value >= threshold) {
      groupedData.push({ name, value });
    } else {
      otherSum += value;
    }
  });

  if (otherSum > 0) {
    groupedData.push({ name: "Outros", value: otherSum });
  }

  return (
    <ResponsiveContainer width="100%" height={400}>
      <PieChart>
        <Pie
          data={groupedData}
          dataKey="value"
          nameKey="name"
          cx="50%"
          cy="50%"
          outerRadius={100}
          label={({ name, percent }) => `${name} ${(percent * 100).toFixed(0)}%`}
        >
          {groupedData.map((entry, index) => (
            <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
          ))}
        </Pie>
        <Tooltip formatter={(value, name) => [`${value} linhas de código`, `Linguagem: ${name}`]} />
        <Legend />
      </PieChart>
    </ResponsiveContainer>
  );
};

export default LanguagePieChart;
