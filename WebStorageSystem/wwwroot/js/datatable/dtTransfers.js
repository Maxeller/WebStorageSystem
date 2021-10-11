// --- TRANSFER ---
$(document).ready(function () {
    if ($("#dtTransfer").length !== 0) {
        // Column definition
        var myColumns = [
            {
                data: "TransferNumber",
                searchable: true,
                orderable: true
            },
            {
                data: "OriginLocation.Name",
                searchable: true,
                orderable: true
            },
            {
                data: "DestinationLocation.Name",
                searchable: true,
                orderable: true
            },
            {
                data: "State",
                searchable: true,
                orderable: true
            },
            {
                data: "TransferTime",
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
                data: "Units",
                searchable: true,
                orderable: true
            },
            {
                data: "User.UserName",
                searchable: true,
                orderable: true
            },/*
            {
                data: "CreatedDate",
                searchable: true,
                orderable: true,
                visible : false,
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
                visible: false,
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
                visible: false,
                render: function (data, type, row) {
                    var s = "";
                    if (row.IsDeleted === true) s = "checked";
                    return `<div class="form-check"><input class="form-check-input" type="checkbox" disabled ${s}></div>`;
                }
            },*/
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
        var table = $("#dtTransfer").DataTable({
            paging: false,
            processing: true,
            serverSide: true,
            ajax: {
                url: "Transfer/LoadTable",
                type: "POST"
            },
            columns: myColumns
        });
        
        // Creation of search bars for searchable columns
        $("#dtTransfer thead tr").after("<tr>");
        var counter = 0;
        $("#dtTransfer thead th").each(function () {
            var title = $("#dtTransfer thead th").eq($(this).index()).text();
            if (myColumns[counter].searchable) {
                if (myColumns[counter].data.includes("IsDeleted")) {
                    $("#dtTransfer thead tr:last").append(`<th><div class="form-check"><input class="form-check-input" type="checkbox"></div></th>`);
                } else if (myColumns[counter].data.includes("Date")) {
                    $("#dtTransfer thead tr:last").append(`<th><input type="datetime-local" id="searchDate" placeholder="Search ${title}" /></th>`);
                } else {
                    $("#dtTransfer thead tr:last").append(`<th><input type="search" placeholder="Search ${title}" /></th>`);
                }
                counter++;
            }
        });
        $("#dtTransfer thead th:last").after("</tr>");

        // Creation of trigger for search event
        table.columns().every(function (index) {
            var column = this;
            var elem = $(`#dtTransfer thead tr:last th:eq(${index}) input`);
            elem.on("keyup change", function () {
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