﻿// --- LOCATION TYPE ---
$(document).ready(function () {
    if ($("#dtLocationType").length !== 0) {
        // Column definition
        var myColumns = [
            {
                data: "Name",
                searchable: true,
                orderable: true
            },
            {
                data: "Description",
                searchable: true,
                orderable: true
            },
            {
                data: "CreatedDate",
                searchable: true,
                orderable: true,
                render: function (data, type, row) {
                    if (data)
                        return moment(data).local().format("DD.MM.YYYY HH:mm:ss"); // Formats data from UTC to local time
                    else
                        return null;
                }
            },
            {
                data: "ModifiedDate",
                searchable: true,
                orderable: true,
                render: function (data, type, row) {
                    if (data)
                        return moment(data).local().format("DD.MM.YYYY HH:mm:ss"); // Formats data from UTC to local time
                    else
                        return null;
                }
            },
            {
                data: "IsDeleted",
                searchable: true,
                orderable: true,
                render: function (data, type, row) {
                    var s = "";
                    if (row.IsDeleted === true) s = "checked";
                    return `<div class="form-check"><input class="form-check-input" type="checkbox" disabled ${s}></div>`;
                }
            },
            {
                data: "Action",
                searchable: false,
                orderable: false,
                render: function (data, type, row) {
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
        var table = $("#dtLocationType").DataTable({
            paging: false,
            processing: true,
            serverSide: true,
            ajax: {
                url: "LocationType/LoadTable",
                type: "POST",
                data: {
                    /*
                    additionalData:
                    {
                        userTimeZone: Intl.DateTimeFormat().resolvedOptions().timeZone
                    }
                    */
                }
            },
            columns: myColumns
        });

        // Creation of search bars for searchable columns
        $("#dtLocationType thead tr").after("<tr>");
        var counter = 0;
        $("#dtLocationType thead th").each(function () {
            var title = $("#dtLocationType thead th").eq($(this).index()).text();
            if (myColumns[counter].searchable) {
                if (myColumns[counter].data.includes("IsDeleted")) {
                    $("#dtLocationType thead tr:last").append(`<th><div class="form-check"><input class="form-check-input" type="checkbox"></div></th>`);
                } else if (myColumns[counter].data.includes("Date")) {
                    $("#dtLocationType thead tr:last").append(`<th><input type="datetime-local" id="searchDate" placeholder="Search ${title}" /></th>`);
                } else {
                    $("#dtLocationType thead tr:last").append(`<th><input type="search" placeholder="Search ${title}" /></th>`);
                }
                counter++;
            }
        });
        $("#dtLocationType thead th:last").after("</tr>");

        // Creation of trigger for search event
        table.columns().every(function (index) {
            var column = this;
            var elem = $(`#dtLocationType thead tr:last th:eq(${index}) input`);
            elem.on("keyup change", function () { // TODO: Add datetime picker for date searching?
                if (elem.hasClass("form-check-input")) { // If search is triggered from checkbox
                    column.search(this.checked).draw();  // send value of checkbox
                } if (elem.is("#searchDate")) {          // If search is triggered on "Date" column
                    column.search(moment.utc(moment(this.value).utc()).format()).draw(); // convert date from local time to UTC
                } else {                                 // If search is triggered from textbox
                    column.search(this.value).draw();    // send value from textbox
                }
            });
        });
    }
});

// --- LOCATION ---
$(document).ready(function () {
    if ($("#dtLocation").length !== 0) {
        // Column definition
        var myColumns = [
            {
                data: "Name",
                searchable: true,
                orderable: true
            },
            {
                data: "Description",
                searchable: true,
                orderable: true
            },
            {
                data: "Address",
                searchable: true,
                orderable: true
            },
            {
                data: "LocationType.Name",
                searchable: true,
                orderable: true
            },
            {
                data: "CreatedDate",
                searchable: true,
                orderable: true,
                render: function (data, type, row) {
                    if (data)
                        return moment(data).local().format("DD.MM.YYYY HH:mm:ss"); // Formats data from UTC to local time
                    else
                        return null;
                }
            },
            {
                data: "ModifiedDate",
                searchable: true,
                orderable: true,
                render: function (data, type, row) {
                    if (data)
                        return moment(data).local().format("DD.MM.YYYY HH:mm:ss"); // Formats data from UTC to local time
                    else
                        return null;
                }
            },
            {
                data: "IsDeleted",
                searchable: true,
                orderable: true,
                render: function (data, type, row) {
                    var s = "";
                    if (row.IsDeleted === true) s = "checked";
                    return `<div class="form-check"><input class="form-check-input" type="checkbox" disabled ${s}></div>`;
                }
            },
            {
                data: "Action",
                searchable: false,
                orderable: false,
                render: function (data, type, row) {
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
                type: "POST",
                data: {
                    /*
                    additionalData:
                    {
                        userTimeZone: Intl.DateTimeFormat().resolvedOptions().timeZone
                    }
                    */
                }
            },
            columns: myColumns
        });

        // Creation of search bars for searchable columns
        $("#dtLocation thead tr").after("<tr>");
        var counter = 0;
        $("#dtLocation thead th").each(function () {
            var title = $("#dtLocation thead th").eq($(this).index()).text();
            if (myColumns[counter].searchable) {
                if (myColumns[counter].data.includes("IsDeleted")) {
                    $("#dtLocation thead tr:last").append(`<th><div class="form-check"><input class="form-check-input" type="checkbox"></div></th>`);
                } else if (myColumns[counter].data.includes("Date")) {
                    $("#dtLocation thead tr:last").append(`<th><input type="datetime-local" id="searchDate" placeholder="Search ${title}" /></th>`);
                } else {
                    $("#dtLocation thead tr:last").append(`<th><input type="search" placeholder="Search ${title}" /></th>`);
                }
                counter++;
            }
        });
        $("#dtLocation thead th:last").after("</tr>");

        // Creation of trigger for search event
        // Creation of trigger for search event
        table.columns().every(function (index) {
            var column = this;
            var elem = $(`#dtLocation thead tr:last th:eq(${index}) input`);
            elem.on("keyup change", function () { // TODO: Add datetime picker for date searching?
                if (elem.hasClass("form-check-input")) { // If search is triggered from checkbox
                    column.search(this.checked).draw();  // send value of checkbox
                } if (elem.is("#searchDate")) {          // If search is triggered on "Date" column
                    column.search(moment.utc(moment(this.value).utc()).format()).draw(); // convert date from local time to UTC
                } else {                                 // If search is triggered from textbox
                    column.search(this.value).draw();    // send value from textbox
                }
            });
        });
    }
});