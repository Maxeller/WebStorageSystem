// Transfer Index
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
                    if (row.State == 1) return "Not Yet Transfered"
                    return luxon.DateTime.fromISO(data, { zone: "utc" }).toLocal().toFormat("dd.LL.yyyy TT") // Formats data from UTC to local time
                }
            },
            {
                data: "MainTransfer.State",
                searchable: true,
                orderable: true,
                render: function (data, type, row) {
                    switch (data) {
                        case (1): return "Prepared";
                        case (2): return "Transfered";
                        default: "Unknown";
                    }
                }
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
            initComplete: function () {
                // Creation of search bars for searchable columns
                $("#dtTransfer thead tr").after("<tr>");
                var counter = 0;
                $("#dtTransfer thead th").each(function () {
                    var title = $("#dtTransfer thead th").eq($(this).index()).text();
                    if (myColumns[counter].searchable && title != "Transfer State") {
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

                    if (elem.is("#searchCheckbox")) {
                        elem.on("click", function () {
                            column.search(this.checked).draw();
                        });
                    } else if (elem.is("#searchDate")) {
                        elem.on("change", function () {
                            column.search(luxon.DateTime.fromISO(this.value).toUTC().toString()).draw(); // convert date from local time to UTC
                        });
                    } else {
                        elem.on("keyup clear", function () {
                            column.search(this.value).draw();
                        });
                    }
                });


                this.api()
                    .columns(2)
                    .every(function () {
                        var column = this;

                        // Create select element and listener
                        var select = $('<select><option value="">Select all</option></select>')
                            .appendTo($(`#dtTransfer thead tr:nth-child(2) th:nth-child(3)`))
                            .on('change', function () {
                                column
                                    .search($(this).val(), { exact: true })
                                    .draw();
                            });

                        // Add list of options
                        column
                            .data()
                            .unique()
                            .sort()
                            .each(function (d, j) {
                                select.append(
                                    '<option value="' + d + '">' + d + '</option>'
                                );
                            });
                    });
            },
            columns: myColumns
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
                orderable: true
            },
            {
                data: "Unit.Product.Name",
                searchable: true,
                orderable: true
            },
            {
                data: "Bundle.BundledUnits",
                searchable: true,
                orderable: false,
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

            },
            {
                data: "DefaultLocation.Name",
                searchable: true,
                orderable: true,
            },
            {
                data: "HasDefect",
                searchable: true,
                orderable: true,
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
                type: "POST"
            },
            layout: {
                topEnd: null
            },
            columns: myColumns
        });

        // Creation of search bars for searchable columns
        $("#dtCreateTransfer thead tr").after("<tr>");
        var counter = 0;
        $("#dtCreateTransfer thead th").each(function () {
            var title = $("#dtCreateTransfer thead th").eq($(this).index()).text();
            if (myColumns[counter].searchable) {
                if (myColumns[counter].data.includes("IsDeleted") || myColumns[counter].data.includes("HasDefect")) {
                    $("#dtCreateTransfer thead tr:last").append(`<th><div class="form-check"><input class="form-check-input" type="checkbox"></div></th>`);
                } else if (myColumns[counter].data.includes("Date")) {
                    $("#dtCreateTransfer thead tr:last").append(`<th><input type="datetime-local" id="searchDate" placeholder="Search ${title}" /></th>`);
                } else {
                    $("#dtCreateTransfer thead tr:last").append(`<th><input type="search" placeholder="Search ${title}" /></th>`);
                }
            } else {
                $("#dtCreateTransfer thead tr:last").append(`<th></th>`);
            }
            counter++;
        });
        $("#dtCreateTransfer thead th:last").after("</tr>");

        // Creation of trigger for search event
        table.columns().every(function (index) {
            var column = this;
            var elem = $(`#dtCreateTransfer thead tr:last th:eq(${index}) input`);

            if (elem.is("#searchCheckbox")) {
                elem.on("click", function () {
                    column.search(this.checked).draw();
                });
            } else if (elem.is("#searchDate")) {
                elem.on("change", function () {
                    column.search(luxon.DateTime.fromISO(this.value).toUTC().toString()).draw(); // convert date from local time to UTC
                });
            } else {
                elem.on("keyup clear", function () {
                    column.search(this.value).draw();
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

