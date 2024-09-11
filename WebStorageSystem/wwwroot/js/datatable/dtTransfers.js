// Transfer Index
$(document).ready(function() {
    if ($("#dtTransfer").length !== 0) {
        // Column definition
        var myColumns = [
            {
                data: "MainTransfer.TransferNumber",
                searchable: true,
                orderable: true,
                responsivePriority: 1,
                render: $.fn.dataTable.render.text()
            },
            {
                data: "MainTransfer.TransferTime",
                searchable: true,
                orderable: true,
                responsivePriority: 11,
                render: function(data, type, row) {
                    if (row.MainTransfer.State == 1) return "Not Yet Transfered";
                    return luxon.DateTime.fromISO(data, { zone: "utc" }).toLocal().toFormat("dd.LL.yyyy TT") // Formats data from UTC to local time
                }
            },
            {
                data: "MainTransfer.State",
                searchable: true,
                orderable: true,
                responsivePriority: 10,
                render: function(data, type, row) {
                    switch (data) {
                    case (1):
                        return "Prepared";
                    case (2):
                            return "Transferred";
                    default:
                        return "Unknown";
                    }
                }
            },
            {
                data: "MainTransfer.User.UserName",
                searchable: true,
                orderable: true,
                responsivePriority: 20,
                render: function(data, type, row) {
                    const words = data.split("@");
                    return words[0];
                }
            },
            {
                data: "OriginLocation.Name",
                searchable: true,
                orderable: true,
                responsivePriority: 3,
                render: $.fn.dataTable.render.text()
            },
            {
                data: "DestinationLocation.Name",
                searchable: true,
                orderable: true,
                responsivePriority: 4,
                render: $.fn.dataTable.render.text()
            },
            {
                data: "UnitBundleView.InventoryNumber",
                searchable: true,
                orderable: false,
                responsivePriority: 5,
                defaultContent: "",
                render: function(data, type, row) {
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
                responsivePriority: 2,
                render: function(data, type, row) {
                    var s = "";
                    if (row.Action.Transfer != null)
                        s = `<a href="${row.Action.Transfer}" class="text-primary">Transfer</a> `;
                    s = s + `<a href="${row.Action.Details}" class="text-primary">Details</a>`;
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
                type: "POST",
                headers: {
                    "RequestVerificationToken": document.getElementById("RequestVerificationToken").value
                }
            },
            layout: {
                topEnd: null
            },
            responsive: {
                details: {
                    display: $.fn.dataTable.Responsive.display.childRow
                }
            },
            columns: myColumns,
            order: [[0, "asc"]],
            initComplete: function() {
                $("#dtTransfer thead tr").after("<tr>");
                this.api().columns().every(function() {
                    var column = this;
                    var index = column.index();
                    var searchable = myColumns[index].searchable;
                    var isVisible = column.responsiveHidden(); // returns true if visible
                    var data = myColumns[index].data;
                    var title = column.header().textContent;

                    if (searchable && isVisible && index !== 2) {
                        if (data.includes("IsDeleted")) {
                            $("#dtTransfer thead tr:last")
                                .append(`<th><div class="form-check"><input class="form-check-input" type="checkbox" id="searchCheckbox"></div></th>`);
                            $("#dtTransfer thead tr:last th:last input")
                                .on("click", function() {
                                        column.search(this.checked).draw();
                                });
                        } else if (data.includes("Date")) {
                            $("#dtTransfer thead tr:last")
                                .append(`<th><input type="datetime-local" id="searchDate" placeholder="Search ${title}" /></th>`);
                            $("#dtTransfer thead tr:last th:last input")
                                .on("change", function () {
                                    column.search(luxon.DateTime.fromISO(this.value).toUTC().toString()).draw(); // convert date from local time to UTC
                                });
                        } else {
                            $("#dtTransfer thead tr:last")
                                .append(`<th><input type="search" placeholder="Search ${title}" /></th>`);
                            $("#dtTransfer thead tr:last th:last input")
                                .on("keyup change clear", function() {
                                    if (column.search() !== this.value) {
                                        column.search(this.value).draw();
                                    }
                                });
                        }
                    }

                    if (index === 2) {
                        // Create select element and listener
                        var select = $('<select><option value="">Select all</option></select>')
                            .appendTo($(`#dtTransfer thead tr:last`))
                            .on('change', function() {
                                column.search($(this).val(), { exact: true }).draw();
                            });

                        var states = ["Prepared", "Transferred"];

                        // Add list of options
                        column.data().unique().sort().each(function(d, j) {
                                select.append('<option value="' + d + '">' + states[d-1] + '</option>');
                            });
                    }
                });
            }
        });
    }
});

// Transfer Details
$(document).ready(function () {
    if ($("#dtTransferDetails").length !== 0) {
        // Column definition
        var myColumns = [
            {
                data: "UnitBundleView.InventoryNumber",
                searchable: true,
                orderable: false,
                defaultContent: "",
                responsivePriority: 1,
                render: function (data, type, row) {
                    if (row.BundleId == null) {
                        return row.Unit.InventoryNumber;
                    } else {
                        return row.Bundle.InventoryNumber;
                    }
                }
            },
            {
                data: "OriginLocation.Name",
                searchable: true,
                orderable: true,
                responsivePriority: 2,
                render: $.fn.dataTable.render.text()
            }
        ];

        // DataTable initialization 
        var table = $("#dtTransferDetails").DataTable({
            paging: false,
            processing: true,
            serverSide: true,
            ajax: {
                url: "/Transfer/LoadTableDetails",
                type: "POST",
                headers: {
                    "RequestVerificationToken": document.getElementById("RequestVerificationToken").value
                },
                data: {
                    "AdditionalData": {
                        "MainTransferId": $('#MainTransferId').val()
                    }
                }
            },
            layout: {
                topEnd: null
            },
            responsive: {
                details: {
                    display: $.fn.dataTable.Responsive.display.childRow
                }
            },
            columns: myColumns,
            initComplete: function () {
                $("#dtTransferDetails thead tr").after("<tr>");
                this.api().columns().every(function () {
                    var column = this;
                    var index = column.index();
                    var searchable = myColumns[index].searchable;
                    var isVisible = column.responsiveHidden(); // returns true if visible
                    var data = myColumns[index].data;
                    var title = column.header().textContent;

                    if (searchable && isVisible) {
                        $("#dtTransferDetails thead tr:last")
                            .append(`<th><input type="search" placeholder="Search ${title}" /></th>`);
                        $("#dtTransferDetails thead tr:last th:last input")
                            .on("keyup change clear", function () {
                                if (column.search() !== this.value) {
                                    column.search(this.value).draw();
                                }
                            });
                    }
                });
            }
        });
    }
});


// Transfer Create
$(document).ready(function () {
    if ($("#dtCreateTransfer").length !== 0) {
        // Column definition
        var myColumns = [
            {
                data: "InventoryNumber",
                searchable: true,
                orderable: true,
                responsivePriority: 1,
                render: $.fn.dataTable.render.text()
            },
            {
                data: "Unit.Product.Name",
                searchable: true,
                orderable: true,
                responsivePriority: 2,
                render: $.fn.dataTable.render.text()
            },
            {
                data: "Bundle.BundledUnits",
                searchable: true,
                orderable: false,
                responsivePriority: 3,
                render: function (data, type, row) {
                    var s = "";
                    for (const i in data) {
                        s = s + `${data[i].InventoryNumber} (${data[i].Product.ProductType.Name} - ${data[i].Product.Name})  <br />`;
                    }
                    return s;
                }
            },
            {
                data: "Location.Name",
                searchable: true,
                orderable: true,
                responsivePriority: 4,
                render: $.fn.dataTable.render.text()

            },
            {
                data: "DefaultLocation.Name",
                searchable: true,
                orderable: true,
                responsivePriority: 20,
                render: $.fn.dataTable.render.text()
            },
            {
                data: "HasDefect",
                searchable: true,
                orderable: true,
                responsivePriority: 10,
                render: function (data, type, row) {
                    var s = "";
                    if (row.HasDefect === true) s = "checked";
                    return `<div class="form-check"><input class="form-check-input" type="checkbox" disabled ${s}></div>`;
                }
            },
        ];

        // DataTable initialization 
        var table = $("#dtCreateTransfer").DataTable({
            paging: false,
            processing: true,
            serverSide: true,
            ajax: {
                url: "/Transfer/LoadUnitBundleView",
                type: "POST",
                headers: {
                    "RequestVerificationToken": document.getElementById("RequestVerificationToken").value
                }
            },
            layout: {
                topEnd: null
            },
            responsive: {
                details: {
                    display: $.fn.dataTable.Responsive.display.childRowImmediate
                }
            },
            columns: myColumns,
            searchCols: [
                null, null, null, null, null, { search: "false" }
            ],
            order: [[0, "asc"]],
            initComplete: function() {
                $("#dtCreateTransfer thead tr").after("<tr>");
                this.api().columns().every(function() {
                    var column = this;
                    var index = column.index();
                    var searchable = myColumns[index].searchable;
                    var isVisible = column.responsiveHidden(); // returns true if visible
                    var data = myColumns[index].data;
                    var title = column.header().textContent;

                    if (searchable && isVisible) {
                        if (data.includes("HasDefect")) {
                            $("#dtCreateTransfer thead tr:last")
                                .append(`<th><div class="form-check"><input class="form-check-input" type="checkbox" id="searchCheckbox"></div></th>`);
                            $("#dtCreateTransfer thead tr:last th:last input")
                                .on("click", function() {
                                        column.search(this.checked).draw();
                                    });
                        } else if (data.includes("Date")) {
                            $("#dtCreateTransfer thead tr:last")
                                .append(`<th><input type="datetime-local" id="searchDate" placeholder="Search ${title}" /></th>`);
                            $("#dtCreateTransfer thead tr:last th:last input")
                                .on("change", function() {
                                        column.search(luxon.DateTime.fromISO(this.value).toUTC().toString()).draw(); // convert date from local time to UTC
                                    });
                        } else {
                            $("#dtCreateTransfer thead tr:last")
                                .append(`<th><input type="search" placeholder="Search ${title}" /></th>`);
                            $("#dtCreateTransfer thead tr:last th:last input")
                                .on("keyup change clear", function() {
                                        if (column.search() !== this.value) {
                                            column.search(this.value).draw();
                                        }
                                    });
                        }
                    }
                });
            }
        });

        $('#dtCreateTransfer tbody').on('click', 'tr', function () {
            $(this).toggleClass('selected');
        });

        $('input[name="btnCreateTransfer"]').click(function () {
            var selectedRows = table.rows('.selected').data();
            var result = [];
            for (let index = 0; index < selectedRows.length; ++index) {
                const element = selectedRows[index];
                result.push(element);
            }
            $('#selectedRows').val(JSON.stringify(result));
        });
    }
});