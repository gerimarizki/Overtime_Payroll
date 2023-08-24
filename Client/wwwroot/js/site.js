// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

@{
    //ViewData["Title"] = "Home Page";
    Layout = "_LayoutAdmin";
}


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
            type: "pie",
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