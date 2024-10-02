import React, { useState, useEffect } from "react";
import axios from "axios";
import { Chart as ChartJS, CategoryScale, LinearScale, PointElement, BarElement, Title, Tooltip, Legend } from 'chart.js';
import { Bar } from 'react-chartjs-2';

ChartJS.register(
  CategoryScale,
  LinearScale,
  PointElement,
  BarElement,
  Title,
  Tooltip,
  Legend
);

const BooksChart = () => {
  const [borrowedData, setBorrowedData] = useState([]);
  const [returnedData, setReturnedData] = useState([]);
  const [chartHeight, setChartHeight] = useState('400px'); // Starea pentru înălțimea graficului

  useEffect(() => {
    const fetchBorrowed = async () => {
      const response = await axios.get('https://localhost:7115/api/v1/Books/GetCurrentLoansInLastSevenDays');
      setBorrowedData(response.data);
    };

    const fetchReturned = async () => {
      const response = await axios.get('https://localhost:7115/api/v1/Books/GetReturnsInLastSevenDays');
      setReturnedData(response.data);
    };

    fetchBorrowed();
    fetchReturned();
  }, []);

  useEffect(() => {
    const handleResize = () => {
      if (window.innerWidth < 1140) {
        setChartHeight('300px');
      } else if (window.innerWidth < 720) {
        setChartHeight('250px');
      } else {
        setChartHeight('400px');
      }
    };

    window.addEventListener('resize', handleResize);
    return () => window.removeEventListener('resize', handleResize);
  }, []);

  // Funcție pentru generarea etichetelor zilelor
  const getLast7DaysLabels = () => {
    let labels = [];
    for (let i = 6; i >= 0; i--) {
      const date = new Date();
      date.setDate(date.getDate() - i);
      labels.push(date.toLocaleDateString('en-US', { weekday: 'short' })); // Format: 'Mon', 'Tue', ...
    }
    return labels;
  };

  const data = {
    labels: getLast7DaysLabels(),
    datasets: [
      {
        label: 'Borrowed',
        data: borrowedData,
        borderColor: 'rgb(53, 162, 235)',
        backgroundColor: 'rgba(255, 0, 0, 0.75)',
      },
      {
        label: 'Returned',
        data: returnedData,
        borderColor: 'rgb(122,229,130)',
        backgroundColor: 'rgba(0,100,0, 0.75)',
      },
    ],
  };

  const options = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: {
      legend: {
        position: 'top',
      },
      title: {
        display: true,
        text: 'Check-out Statistics',
      },
    },
  };

  return (
    <div style={{ height: chartHeight }}>
      <Bar options={options} data={data} />
    </div>
  );
};

export default BooksChart;
