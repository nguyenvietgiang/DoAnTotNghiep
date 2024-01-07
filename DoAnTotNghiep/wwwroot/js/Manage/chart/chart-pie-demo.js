var ctx = document.getElementById("myPieChart");

// Kiểm tra xem canvas có biểu đồ cũ hay không
var existingChart = Chart.getChart(ctx);

// Nếu có biểu đồ cũ, hủy nó trước khi tạo biểu đồ mới
if (existingChart) {
    existingChart.destroy();
}

fetch('/api/Statistics/account-roles')
    .then(response => response.json())
    .then(data => {
        // Lọc chỉ lấy thông tin của EmployerFree, EmployerPaid, CandidateFree, CandidatePaid
        var filteredData = data.filter(item => [2, 3, 4, 5].includes(item.role));

        var labels = filteredData.map(item => getRoleLabel(item.role));
        var values = filteredData.map(item => item.count);

        var backgroundColors = ['#1cc88a', '#36b9cc', 'yellow', 'green']; // Cập nhật màu sắc ở đây

        var hoverBackgroundColors = backgroundColors.map(color => {
            // Có thể tùy chỉnh màu hover tương ứng nếu cần
            return color;
        });

        var myPieChart = new Chart(ctx, {
            type: 'doughnut',
            data: {
                labels: labels,
                datasets: [{
                    data: values,
                    backgroundColor: backgroundColors,
                    hoverBackgroundColor: hoverBackgroundColors,
                    hoverBorderColor: "rgba(234, 236, 244, 1)",
                }],
            },
            options: {
                maintainAspectRatio: false,
                tooltips: {
                    backgroundColor: "rgb(255,255,255)",
                    bodyFontColor: "#858796",
                    borderColor: '#dddfeb',
                    borderWidth: 1,
                    xPadding: 15,
                    yPadding: 15,
                    displayColors: false,
                    caretPadding: 10,
                },
                legend: {
                    display: false
                },
                cutoutPercentage: 80,
            },
        });
    })
    .catch(error => console.error('Error:', error));

// Hàm chuyển đổi giá trị enum thành label tương ứng
function getRoleLabel(role) {
    switch (role) {
        case 2:
            return 'DN cơ bản';
        case 3:
            return 'DN trả phí';
        case 4:
            return 'UV cơ bản';
        case 5:
            return 'UV trả phí';
        default:
            return '';
    }
}
