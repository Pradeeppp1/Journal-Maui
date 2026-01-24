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

window.generatePDF = async (htmlContent) => {
  const element = document.createElement("div");
  element.innerHTML = htmlContent;
  element.style.padding = "40px";
  element.style.fontFamily = "Inter, sans-serif";

  const opt = {
    margin: 10,
    filename: "journal.pdf",
    image: { type: "jpeg", quality: 0.98 },
    html2canvas: { scale: 2 },
    jsPDF: { unit: "mm", format: "a4", orientation: "portrait" },
  };

  const pdfBase64 = await html2pdf()
    .from(element)
    .set(opt)
    .outputPdf("datauristring");
  return pdfBase64.split(",")[1]; // Return only the base64 part
};
