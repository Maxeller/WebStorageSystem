// --- TRANSFER ---
/*
$(document).ready(function () {
    if ($("#dtTransfer").length !== 0) {
        // Column definition
        var myColumns = [
            {
                className: 'dt-control',
                searchable: false,
                orderable: false,
                data: null,
                defaultContent: ''
            },
            {
                data: "TransferNumber",
                searchable: true,
                orderable: true
            },
            {
                data: "TransferTime",
                searchable: true,
                orderable: true,
                render: function (data, type, row) {
                    return luxon.DateTime.fromISO(data, { zone: "utc" }).toLocal().toFormat("dd.LL.yyyy TT") // Formats data from UTC to local time
                }
            },
            {
                data: "State",
                searchable: false,
                orderable: true
            },
            {
                data: "User.UserName",
                searchable: true,
                orderable: true
            },
            {
                data: "SubTransfers",
                searchable: true,
                orderable: false
            },
            {
                data: "Action",
                searchable: false,
                orderable: false,
                render: function (data, type, row) {
                    return `<a href="${row.Action.Details}" class="text-primary">Details</a>`;
                }
            }
        ];

        function format(d) {
            // `d` is the original data object for the row
            return (
                "TEST"
            );
        }

        // DataTable initialization 
        var table = $("#dtTransfer").DataTable({
            paging: false,
            processing: true,
            serverSide: true,
            ajax: {
                url: "Transfer/LoadTable",
                type: "POST"
            },
            layout: {
                topEnd: null
            },
            columns: myColumns
        });

        // Add event listener for opening and closing details
        table.on('click', 'td.dt-control', function (e) {
            let tr = e.target.closest('tr');
            let row = table.row(tr);

            if (row.child.isShown()) {
                // This row is already open - close it
                row.child.hide();
            }
            else {
                // Open this row
                row.child(format(row.data())).show();
            }
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
                    column.search(luxon.DateTime.fromISO(this.value).toUTC().toString()).draw(); // convert date from local time to UTC
                } else {                                 // If search is triggered from textbox
                    column.search(this.value).draw();    // send value from textbox
                }
            });
        });
    }
});
*/

$(document).ready(function () {
    if ($("#dtTransfer").length !== 0) {
        // Column definition
        var myColumns = [
            {
                data: "MainTransfer.TransferNumber",
                searchable: true,
                orderable: true
            },
            {
                data: "MainTransfer.TransferTime",
                searchable: true,
                orderable: true,
                render: function (data, type, row) {
                    return luxon.DateTime.fromISO(data, { zone: "utc" }).toLocal().toFormat("dd.LL.yyyy TT") // Formats data from UTC to local time
                }
            },
            {
                data: "MainTransfer.State",
                searchable: true,
                orderable: true
            },
            {
                data: "MainTransfer.User.UserName",
                searchable: true,
                orderable: true,
                render: function (data, type, row) {
                    const words = data.split("@");
                    return words[0];
                }
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
                data: "UnitBundleView.InventoryNumber",
                searchable: true,
                orderable: true,
                render: function (data, type, row) {
                    if (row.BundleId == null) {
                        return row.Unit.InventoryNumber;
                    } else {
                        return row.Bundle.InventoryNumber;
                    }
                }
            },
            {
                data: "Action",
                searchable: false,
                orderable: false,
                render: function (data, type, row) {
                    return `<a href="${row.Action.Details}" class="text-primary">Details</a>`;
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
            layout: {
                topEnd: null
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
            } else {
                $("#dtTransfer thead tr:last").append(`<th></th>`);
            }
            counter++;
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
                    column.search(luxon.DateTime.fromISO(this.value).toUTC().toString()).draw(); // convert date from local time to UTC
                } else {                                 // If search is triggered from textbox
                    column.search(this.value).draw();    // send value from textbox
                }
            });
        });
    }
});