// --- LOCATION TYPE ---
$(document).ready(function() {
    if ($("#dtLocationType").length !== 0) {
        // Column definition
        var myColumns = [
            {
                data: "Name",
                searchable: true,
                orderable: true,
                responsivePriority: 1
            },
            {
                data: "Description",
                searchable: true,
                orderable: true,
                responsivePriority: 3
            },
            {
                data: "CreatedDate",
                searchable: true,
                orderable: true,
                render: function(data, type, row) {
                    return luxon.DateTime.fromISO(data, { zone: "utc" }).toLocal()
                        .toFormat("dd.LL.yyyy TT"); // Formats data from UTC to local time
                }
            },
            {
                data: "ModifiedDate",
                searchable: true,
                orderable: true,
                render: function(data, type, row) {
                    return luxon.DateTime.fromISO(data, { zone: "utc" }).toLocal()
                        .toFormat("dd.LL.yyyy TT"); // Formats data from UTC to local time
                }
            },
            {
                data: "IsDeleted",
                searchable: true,
                orderable: true,
                render: function(data, type, row) {
                    var s = "";
                    if (row.IsDeleted === true) s = "checked";
                    return `<div class="form-check"><input class="form-check-input" type="checkbox" disabled ${s
                        }></div>`;
                }
            },
            {
                data: "Action",
                searchable: false,
                orderable: false,
                responsivePriority: 2,
                render: function(data, type, row) {
                    var s = `<a href="${row.Action.Edit}" class="text-primary">Edit</a> `;
                    s = s + `<a href="${row.Action.Details}" class="text-primary">Details</a> `;
                    if (!row.IsDeleted) {
                        s = s +
                            `<a href="#" class="text-primary" data-toggle="modal" data-target="#deleteRestoreModal" data-url="${
                            row.Action.Delete}" data-name="${row.Name}">Delete</a>`;
                    } else {
                        s = s +
                            `<a href="#" class="text-primary" data-toggle="modal" data-target="#deleteRestoreModal" data-url="${
                            row.Action.Restore}" data-name="${row.Name}">Restore</a>`;
                    }
                    return s;
                }
            }
        ];

        // DataTable initialization 
        var table = $("#dtLocationType").DataTable({
            paging: false,
            processing: true,
            serverSide: true,
            ajax: {
                url: "LocationType/LoadTable",
                type: "POST",
                data: {
                }
            },
            layout: {
                topEnd: null
            },
            responsive: true,
            columns: myColumns,
            initComplete: function() {
                $("#dtLocationType thead tr").after("<tr>");
                this.api().columns().every(function() {
                    var column = this;
                    var index = column.index();
                    var searchable = myColumns[index].searchable;
                    var isVisible = column.responsiveHidden(); // returns true if visible
                    var data = myColumns[index].data;
                    var title = column.header().textContent;

                    if (searchable && isVisible) {
                        if (data.includes("IsDeleted")) {
                            $("#dtLocationType thead tr:last")
                                .append(`<th><div class="form-check"><input class="form-check-input" type="checkbox" id="searchCheckbox"></div></th>`);
                            $("#dtLocationType thead tr:last th:last input")
                                .on("click", function () {
                                        column.search(this.checked).draw();
                                    });
                        } else if (data.includes("Date")) {
                            $("#dtLocationType thead tr:last")
                                .append(`<th><input type="datetime-local" id="searchDate" placeholder="Search ${title}" /></th>`);
                            $("#dtLocationType thead tr:last th:last input")
                                .on("change", function () {
                                    column.search(luxon.DateTime.fromISO(this.value).toUTC().toString()).draw(); // convert date from local time to UTC
                                });
                        } else {
                            $("#dtLocationType thead tr:last")
                                .append(`<th><input type="search" placeholder="Search ${title}" /></th>`);
                            $("#dtLocationType thead tr:last th:last input")
                                .on("keyup clear", function () {
                                    if (column.search() !== this.value) {
                                        column.search(this.value).draw();
                                    }
                                });
                        }
                    }

                    if (!searchable && isVisible) {
                        $("#dtLocationType thead tr:last").append(`<th></th>`);
                    }
                });
            }
        });
        table.columns(4).search("false").draw();
    }
});

// --- LOCATION ---
$(document).ready(function() {
    if ($("#dtLocation").length !== 0) {
        // Column definition
        var myColumns = [
            {
                data: "Name",
                searchable: true,
                orderable: true,
                responsivePriority: 1
            },
            {
                data: "Description",
                searchable: true,
                orderable: true,
                responsivePriority: 5
            },
            {
                data: "Address",
                searchable: true,
                orderable: true,
                responsivePriority: 4
            },
            {
                data: "LocationType.Name",
                searchable: true,
                orderable: true,
                responsivePriority: 3
            },
            {
                data: "CreatedDate",
                searchable: true,
                orderable: true,
                render: function(data, type, row) {
                    return luxon.DateTime.fromISO(data, { zone: "utc" }).toLocal().toFormat("dd.LL.yyyy TT"); // Formats data from UTC to local time
                }
            },
            {
                data: "ModifiedDate",
                searchable: true,
                orderable: true,
                render: function(data, type, row) {
                    return luxon.DateTime.fromISO(data, { zone: "utc" }).toLocal().toFormat("dd.LL.yyyy TT"); // Formats data from UTC to local time
                }
            },
            {
                data: "IsDeleted",
                searchable: true,
                orderable: true,
                render: function(data, type, row) {
                    var s = "";
                    if (row.IsDeleted === true) s = "checked";
                    return `<div class="form-check"><input class="form-check-input" type="checkbox" disabled ${s}></div>`;
                }
            },
            {
                data: "Action",
                searchable: false,
                orderable: false,
                responsivePriority: 2,
                render: function(data, type, row) {
                    var s = `<a href="${row.Action.Edit}" class="text-primary">Edit</a> `;
                    s = s + `<a href="${row.Action.Details}" class="text-primary">Details</a> `;
                    if (!row.IsDeleted) {
                        s = s + `<a href="#" class="text-primary" data-toggle="modal" data-target="#deleteRestoreModal" data-url="${row.Action.Delete}" data-name="${row.Name}">Delete</a>`;
                    } else {
                        s = s + `<a href="#" class="text-primary" data-toggle="modal" data-target="#deleteRestoreModal" data-url="${row.Action.Restore}" data-name="${row.Name}">Restore</a>`;
                    }
                    return s;
                }
            }
        ];

        // DataTable initialization 
        var table = $("#dtLocation").DataTable({
            paging: false,
            processing: true,
            serverSide: true,
            ajax: {
                url: "Location/LoadTable",
                type: "POST"
            },
            layout: {
                topEnd: null
            },
            responsive: true,
            columns: myColumns,
            initComplete: function () {
                $("#dtLocation thead tr").after("<tr>");
                this.api().columns().every(function () {
                    var column = this;
                    var index = column.index();
                    var searchable = myColumns[index].searchable;
                    var isVisible = column.responsiveHidden(); // returns true if visible
                    var data = myColumns[index].data;
                    var title = column.header().textContent;

                    if (searchable && isVisible) {
                        if (data.includes("IsDeleted")) {
                            $("#dtLocation thead tr:last")
                                .append(`<th><div class="form-check"><input class="form-check-input" type="checkbox" id="searchCheckbox"></div></th>`);
                            $("#dtLocation thead tr:last th:last input")
                                .on("click", function () {
                                    column.search(this.checked).draw();
                                });
                        } else if (data.includes("Date")) {
                            $("#dtLocation thead tr:last")
                                .append(`<th><input type="datetime-local" id="searchDate" placeholder="Search ${title}" /></th>`);
                            $("#dtLocation thead tr:last th:last input")
                                .on("change", function () {
                                    column.search(luxon.DateTime.fromISO(this.value).toUTC().toString()).draw(); // convert date from local time to UTC
                                });
                        } else {
                            $("#dtLocation thead tr:last")
                                .append(`<th><input type="search" placeholder="Search ${title}" /></th>`);
                            $("#dtLocation thead tr:last th:last input")
                                .on("keyup change clear", function () {
                                    if (column.search() !== this.value) {
                                        column.search(this.value).draw();
                                    }
                                });
                        }
                    }

                    if (!searchable && isVisible) {
                        $("#dtLocation thead tr:last").append(`<th></th>`);
                    }
                });
            }
        });
        table.columns(6).search("false").draw();
    }
});