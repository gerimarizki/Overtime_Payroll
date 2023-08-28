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
    url: "https://localhost:7128/api/payrolls"
}).done((result) => {
    let temp = "";
    $("#tbodySW").html(temp);
    $.each(result.results, (indeks, val) => {
        temp += `
                    <div class="pokemon-card">
                        <h4 class="pokemon-name">${val.name}</h4>
                        <button onclick="detailPokemon('${val.url}')" data-bs-toggle="modal" data-bs-target="#exampleModal" class="btn btn-primary">Detail</button>
                    </div>
                `;
    })
    $("#pokemonContainer").html(temp);
});