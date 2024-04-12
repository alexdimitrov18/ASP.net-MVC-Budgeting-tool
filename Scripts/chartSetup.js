$(document).ready(function () {
    var canvas = document.getElementById('expensesChart');
    if (!canvas) {
        console.error('Canvas element not found!');
        return;
    }

    var ctx = canvas.getContext('2d');
    var labels = JSON.parse(canvas.getAttribute('data-labels') || '[]');
    var data = JSON.parse(canvas.getAttribute('data-data') || '[]');

    if (!labels.length || !data.length) {
        console.error('No data or labels available for the chart.');
        return;
    }

    var chart = new Chart(ctx, {
        type: 'pie',
        data: {
            labels: labels,
            datasets: [{
                data: data,
                backgroundColor: [
                    '#FF6384', '#36A2EB', '#FFCE56', '#4BC0C0', '#9966FF', '#FF9F40'
                ],
                hoverBackgroundColor: [
                    '#FF6384', '#36A2EB', '#FFCE56', '#4BC0C0', '#9966FF', '#FF9F40'
                ]
            }]
        },
        options: {
            responsive: true
        }
    });
});
