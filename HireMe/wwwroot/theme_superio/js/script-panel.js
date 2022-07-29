(function ($) {

    "use strict";

    if (window.jQuery) {
    var script2 = document.createElement('script');
    script2.src = "https://grandjob.eu/theme_superio/js/knob.min.js";
    document.getElementsByTagName('body')[0].appendChild(script2);

    var script3 = document.createElement('script');
        script3.src = "https://grandjob.eu/theme_superio/js/chart.min.js";
    document.getElementsByTagName('body')[0].appendChild(script3);
    }

    var companies = $("#SelectCompanies");
    if (companies.length) {
        companies.prop("multiple", ($(this).checked) ? "multiple" : "");
        companies.on('change', function (e) {

            var selectedId = $(this).find("option:selected").val();

            Chart.defaults.global.defaultFontFamily = "Sofia Pro";
            Chart.defaults.global.defaultFontColor = '#888';
            Chart.defaults.global.defaultFontSize = '14';

            var labelsData = [];
            var valuesData = [];

            //function GetData(valId) {
            $.ajax({
                url: '/ChartApi/GetChartDataById/',
                type: "GET",
                dataType: "json",
                data: { selectItemId: selectedId },
                success: function (response) {

                    Object.keys(response).forEach(function (key) {
                        labelsData.push(key);
                    });

                    Object.values(response).forEach(function (value) {
                        valuesData.push(value);
                    });

                    var ctx = document.getElementById('chart').getContext('2d');

                    var chart = new Chart(ctx, {

                        type: 'line',
                        // The data for our dataset
                        data: {
                            labels: labelsData,
                            // Information about the dataset
                            datasets: [{
                                label: "Views",
                                backgroundColor: 'transparent',
                                borderColor: '#1967D2',
                                borderWidth: "1",
                                data: valuesData,
                                pointRadius: 3,
                                pointHoverRadius: 3,
                                pointHitRadius: 10,
                                pointBackgroundColor: "#1967D2",
                                pointHoverBackgroundColor: "#1967D2",
                                pointBorderWidth: "2",
                            }]
                        },

                        // Configuration options
                        options: {

                            layout: {
                                padding: 10,
                            },

                            legend: {
                                display: false
                            },
                            title: {
                                display: false
                            },

                            scales: {
                                yAxes: [{
                                    scaleLabel: {
                                        display: false
                                    },
                                    gridLines: {
                                        borderDash: [6, 10],
                                        color: "#d8d8d8",
                                        lineWidth: 1,
                                    },
                                }],
                                xAxes: [{
                                    scaleLabel: {
                                        display: false
                                    },
                                    gridLines: {
                                        display: false
                                    },
                                }],
                            },

                            tooltips: {
                                backgroundColor: '#333',
                                titleFontSize: 13,
                                titleFontColor: '#fff',
                                bodyFontColor: '#fff',
                                bodyFontSize: 13,
                                displayColors: false,
                                xPadding: 10,
                                yPadding: 10,
                                intersect: false
                            }
                        },
                    });
                },
                error: function (e) {

                }
            });
            //};     
        });
    };

    var jobs = $("#SelectJobs");
    if (jobs.length) {
        jobs.prop("multiple", ($(this).checked) ? "multiple" : "");
        jobs.on('change', function (e) {

            var selectedId = $(this).find("option:selected").val();

            Chart.defaults.global.defaultFontFamily = "Sofia Pro";
            Chart.defaults.global.defaultFontColor = '#888';
            Chart.defaults.global.defaultFontSize = '14';

            var labelsData = [];
            var valuesData = [];

            //function GetData(valId) {
            $.ajax({
                url: '/ChartApi/GetChartDataByIdJob/',
                type: "GET",
                dataType: "json",
                data: { selectItemId: selectedId },
                success: function (response) {
                    console.log(response);

                    Object.keys(response).forEach(function (key) {
                        labelsData.push(key);
                    });

                    Object.values(response).forEach(function (value) {
                        valuesData.push(value);
                    });

                    var ctx = document.getElementById('chart-job').getContext('2d');

                    var chart = new Chart(ctx, {

                        type: 'line',
                        // The data for our dataset
                        data: {
                            labels: labelsData,
                            // Information about the dataset
                            datasets: [{
                                label: "Views",
                                backgroundColor: 'transparent',
                                borderColor: '#1967D2',
                                borderWidth: "1",
                                data: valuesData,
                                pointRadius: 3,
                                pointHoverRadius: 3,
                                pointHitRadius: 10,
                                pointBackgroundColor: "#1967D2",
                                pointHoverBackgroundColor: "#1967D2",
                                pointBorderWidth: "2",
                            }]
                        },

                        // Configuration options
                        options: {

                            layout: {
                                padding: 10,
                            },

                            legend: {
                                display: false
                            },
                            title: {
                                display: false
                            },

                            scales: {
                                yAxes: [{
                                    scaleLabel: {
                                        display: false
                                    },
                                    gridLines: {
                                        borderDash: [6, 10],
                                        color: "#d8d8d8",
                                        lineWidth: 1,
                                    },
                                }],
                                xAxes: [{
                                    scaleLabel: {
                                        display: false
                                    },
                                    gridLines: {
                                        display: false
                                    },
                                }],
                            },

                            tooltips: {
                                backgroundColor: '#333',
                                titleFontSize: 13,
                                titleFontColor: '#fff',
                                bodyFontColor: '#fff',
                                bodyFontSize: 13,
                                displayColors: false,
                                xPadding: 10,
                                yPadding: 10,
                                intersect: false
                            }
                        },
                    });
                },
                error: function (e) {
                    console.log(e);
                }
            });
            //};     
        });
    };
})(window.jQuery);



