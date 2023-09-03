// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(() => {
    $.ajax({
        url: "https://localhost:7128/api/employees"
    }).done((result) => {
        let female = 0;
        let male = 0;

        result.data.forEach(employee => {
            if (employee.gender === 0) { female++ }
            if (employee.gender === 1) { male++ }
        });

        var xValues = ["Female", "Male"];
        var yValues = [female, male];
        var barColors = [
            "#b91d47",
            "#00aba9",
        ];
        new Chart("myChart", {
            type: "doughnut",  // Changed to a doughnut chart
            data: {
                labels: xValues,
                datasets: [{
                    backgroundColor: barColors,
                    data: yValues
                }]
            },
            options: {
                title: {
                    display: true,
                    text: "Gender of Employees"
                }
            }
        });
    });
});


$(document).ready(function () {
    $.ajax({
        url: "https://localhost:7128/api/employees",
        //headers:
        //{
        //    "Authorization": "Bearer " + Token
        //}
    }).done(function (result) {
        var totalEmployee = result.data.length;
        $("#total-employee").text(totalEmployee);
    }).fail(function (error) {
        $("#total-employee").text("Failed to fetch data");
        console.log(error)
    });
});

$(document).ready(function () {
    $.ajax({
        url: "https://localhost:7128/api/overtimes",
    }).done(function (result) {
        var totalOvertime = result.data.length;
        $("#total-overtime").text(totalOvertime);
    }).fail(function (error) {
        $("#total-overtime").text("Failed to fetch data");
        console.log(error)
    });
});

$.ajax({
    url:"https://localhost:7128/api/overtimes/CountStatus"
}).done((result) => {
    $("#countAccepted").html(`${result.data.countAccepted}`)
    $("#countRejected").html(`${result.data.countRejected}`)
    $("#countWaiting").html(`${result.data.countWaiting}`)
})

function updateDateTime() {
    var now = new Date();
    var options = { year: 'numeric', month: 'short', day: 'numeric', hour: '2-digit', minute: '2-digit', second: '2-digit', hour12: false };
    var formattedDateTime = now.toLocaleString('en-US', options);

    document.getElementById('currentDateTime').textContent = formattedDateTime;
}





//@* ----------------------------------------
//    AJAX UNTUK REGISTER
//----------------------------------------*@
function register() {
    var registerData = {
        firstName: $("#firstName").val(),
        lastName: $("#lastName").val(),
        birthDate: $("#birthDate").val(),
        gender: parseInt($("#gender").val()),
        hiringDate: $("#hiringDate").val(),
        email: $("#email").val(),
        phoneNumber: $("#phoneNumber").val(),
        managerGuid: $("#managerGuid").val(),
        password: $("#password").val(),
        confirmPassword: $("#confirmPassword").val(),
        salary: parseInt($("#salary").val())
    };

    $.ajax({
        url: "https://localhost:7128/api/accounts/Register",
        type: "POST",
        headers: {
            'Content-Type': 'application/json'
        },
        data: JSON.stringify(registerData),
        success: function (result) {
            Swal.fire({
                title: 'Success',
                text: 'Data has been registered',
                icon: 'success'
            }).then(function () {
                location.reload();
            });
        },
        error: function (error) {
            Swal.fire({
                title: 'Oops',
                text: 'Failed to register. Please try again.',
                icon: 'error'
            });
            console.log(error);
        }
    });
}



    $(document).ready(() => {
        $.ajax({
            url: "https://localhost:7128/api/overtimes"
        }).done((result) => {
            const overtimeData = result.data.filter(overtime => overtime.EmployeeGuid === "@User.FindFirstValue(ClaimTypes.NameIdentifier)");

            // Buat grafik batang
            const overtimeRemainingValues = overtimeData.map(overtime => overtime.OvertimeRemaining);
            const overtimeLabels = overtimeData.map(overtime => overtime.OvertimeId.toString());

            new Chart("myBar", {
                type: "bar",
                data: {
                    labels: overtimeLabels,
                    datasets: [{
                        label: "Overtime Remaining",
                        data: overtimeRemainingValues,
                        backgroundColor: "rgba(75, 192, 192, 0.2)",
                        borderColor: "rgba(75, 192, 192, 1)",
                        borderWidth: 1
                    }]
                },
                options: {
                    scales: {
                        y: {
                            beginAtZero: true
                        }
                    },
                    plugins: {
                        title: {
                            display: true,
                            text: "Grafik Overtime Remaining"
                        }
                    }
                }
            });
        });
    });




// Memanggil fungsi updateDateTime() setiap detik
setInterval(updateDateTime, 1000);

// Memanggil fungsi untuk pertama kali
updateDateTime();