window.renderPieChart = (elementId, labels, series, title) => {
  const options = {
    series: series,
    chart: {
      type: "donut",
      height: 350,
      animations: {
        enabled: true,
        easing: "easeinout",
        speed: 800,
      },
    },
    labels: labels,
    title: {
      text: title,
      align: "center",
      style: {
        fontSize: "18px",
        fontWeight: "600",
        color: "#2d3748",
      },
    },
    colors: ["#4fd1c5", "#63b3ed", "#f6ad55", "#fc8181", "#b794f4"],
    legend: {
      position: "bottom",
    },
    responsive: [
      {
        breakpoint: 480,
        options: {
          chart: {
            width: 200,
          },
          legend: {
            position: "bottom",
          },
        },
      },
    ],
  };

  const chart = new ApexCharts(document.getElementById(elementId), options);
  chart.render();
};

window.renderBarChart = (elementId, labels, series, title) => {
  const options = {
    series: [
      {
        name: "Count",
        data: series,
      },
    ],
    chart: {
      type: "bar",
      height: 350,
      toolbar: {
        show: false,
      },
    },
    plotOptions: {
      bar: {
        borderRadius: 4,
        horizontal: true,
        distributed: true,
      },
    },
    dataLabels: {
      enabled: false,
    },
    colors: ["#4fd1c5", "#63b3ed", "#f6ad55", "#fc8181", "#b794f4"],
    xaxis: {
      categories: labels,
    },
    title: {
      text: title,
      align: "center",
      style: {
        fontSize: "18px",
        fontWeight: "600",
        color: "#2d3748",
      },
    },
  };

  const chart = new ApexCharts(document.getElementById(elementId), options);
  chart.render();
};

window.renderLineChart = (elementId, dates, values, title) => {
  const options = {
    series: [
      {
        name: "Words",
        data: values,
      },
    ],
    chart: {
      height: 350,
      type: "line",
      zoom: {
        enabled: false,
      },
    },
    dataLabels: {
      enabled: false,
    },
    stroke: {
      curve: "smooth",
      width: 3,
    },
    title: {
      text: title,
      align: "left",
      style: {
        fontSize: "18px",
        fontWeight: "600",
        color: "#2d3748",
      },
    },
    grid: {
      row: {
        colors: ["#f3f3f3", "transparent"],
        opacity: 0.5,
      },
    },
    xaxis: {
      categories: dates,
      type: "datetime",
    },
    colors: ["#4fd1c5"],
  };

  const chart = new ApexCharts(document.getElementById(elementId), options);
  chart.render();
};

window.renderAreaChart = (elementId, dates, values, title) => {
  const options = {
    series: [{
      name: 'Words',
      data: values
    }],
    chart: {
      type: 'area',
      height: 350,
      zoom: {
        enabled: false
      },
      toolbar: {
        show: false
      }
    },
    dataLabels: {
      enabled: false
    },
    stroke: {
      curve: 'smooth'
    },
    title: {
      text: title,
      align: 'left',
      style: {
        fontSize: '18px',
        fontWeight: '600'
      }
    },
    xaxis: {
      type: 'datetime',
      categories: dates
    },
    yaxis: {
      opposite: true
    },
    legend: {
      horizontalAlign: 'left'
    },
    colors: ['#63b3ed'],
    fill: {
      type: 'gradient',
      gradient: {
        shadeIntensity: 1,
        opacityFrom: 0.7,
        opacityTo: 0.9,
        stops: [0, 90, 100]
      }
    }
  };

  const chart = new ApexCharts(document.getElementById(elementId), options);
  chart.render();
};

window.renderColumnChart = (elementId, labels, series, title, color = '#4fd1c5') => {
  const options = {
    series: [{
      name: 'Count',
      data: series
    }],
    chart: {
      type: 'bar',
      height: 350,
      toolbar: {
        show: false
      }
    },
    plotOptions: {
      bar: {
        borderRadius: 4,
        columnWidth: '60%',
      }
    },
    dataLabels: {
      enabled: false
    },
    xaxis: {
      categories: labels,
    },
    title: {
      text: title,
      align: 'center',
      style: {
        fontSize: '18px',
        fontWeight: '600'
      }
    },
    colors: [color]
  };

  const chart = new ApexCharts(document.getElementById(elementId), options);
  chart.render();
};

window.renderHeatMap = (elementId, data, title) => {
  const options = {
    series: data,
    chart: {
      height: 350,
      type: 'heatmap',
      toolbar: { show: false }
    },
    dataLabels: { enabled: false },
    colors: ["#008FFB"],
    title: {
      text: title,
      align: 'center',
      style: { fontSize: '18px', fontWeight: '600' }
    },
    plotOptions: {
      heatmap: {
        shadeIntensity: 0.5,
        radius: 0,
        useFillColorAsStroke: true,
        colorScale: {
          ranges: [
            { from: 0, to: 0, color: '#f1f5f9', name: 'low' },
            { from: 1, to: 1, color: '#63b3ed', name: 'medium' },
            { from: 2, to: 5, color: '#2563eb', name: 'high' }
          ]
        }
      }
    }
  };

  const chart = new ApexCharts(document.getElementById(elementId), options);
  chart.render();
};

window.renderRadarChart = (elementId, labels, series, title) => {
  const options = {
    series: [{
      name: 'Mood Intensity',
      data: series,
    }],
    chart: {
      height: 350,
      type: 'radar',
      toolbar: { show: false }
    },
    title: {
      text: title,
      align: 'center',
      style: { fontSize: '18px', fontWeight: '600' }
    },
    xaxis: {
      categories: labels
    },
    colors: ['#2563eb'],
    markers: {
      size: 4,
      colors: ['#fff'],
      strokeColor: '#2563eb',
      strokeWidth: 2,
    }
  };

  const chart = new ApexCharts(document.getElementById(elementId), options);
  chart.render();
};

window.generatePDF = async (htmlContent) => {
  try {
    // For MAUI, we'll use a simpler approach - just return the HTML
    // The C# side can handle the actual PDF generation
    return btoa(unescape(encodeURIComponent(htmlContent)));
  } catch (error) {
    console.error("PDF generation error:", error);
    throw error;
  }
};
