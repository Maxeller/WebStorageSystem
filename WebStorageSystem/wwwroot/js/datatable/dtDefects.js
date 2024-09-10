// --- TRANSFER ---
$(document).ready(function () {
    if ($("#dtDefect").length !== 0) {
        // Column definition
        var myColumns = [
            {
                data: "DefectNumber",
                searchable: true,
                orderable: true,
                responsivePriority: 1,
                render: $.fn.dataTable.render.text()
            },
            {
                data: "Unit.InventoryNumber",
                searchable: true,
                orderable: true,
                responsivePriority: 4,
                render: $.fn.dataTable.render.text()
            },
            {
                data: "Unit.Product.Manufacturer.Name",
                searchable: true,
                orderable: true,
                responsivePriority: 10,
                render: $.fn.dataTable.render.text()
            },
            {
                data: "Unit.Product.Name",
                searchable: true,
                orderable: true,
                responsivePriority: 11,
                render: $.fn.dataTable.render.text()
            },
            {
                data: "Unit.Product.ProductType.Name",
                searchable: true,
                orderable: true,
                responsivePriority: 12,
                render: $.fn.dataTable.render.text()
            },
            {
                data: "Unit.Location.Name",
                searchable: true,
                orderable: true,
                responsivePriority: 20,
                render: $.fn.dataTable.render.text()
            },
            {
                data: "State",
                searchable: true,
                orderable: true,
                responsivePriority: 3,
                render: function (data, type, row) {
                    switch (data) {
                        case (1): return "Broken";
                        case (2): return "In repair";
                        case (3): return "Repaired";
                        default: return "Unknown";
                    }
                }
            },
            {
                data: "Description",
                searchable: true,
                orderable: true,
                render: $.fn.dataTable.render.text()
            },
            {
                data: "ReportedByUser.UserName",
                searchable: true,
                orderable: true,
                responsivePriority: 30,
                render: $.fn.dataTable.render.text()
            },
            {
                data: "CausedByUser.UserName",
                searchable: true,
                orderable: true,
                responsivePriority: 31,
                render: $.fn.dataTable.render.text()
            },
            {
                data: "CreatedDate",
                searchable: true,
                orderable: true,
                render: function (data, type, row) {
                    return luxon.DateTime.fromISO(data, { zone: "utc" }).toLocal().toFormat("dd.LL.yyyy TT") // Formats data from UTC to local time
                }
            },
            {
                data: "ModifiedDate",
                searchable: true,
                orderable: true,
                render: function (data, type, row) {
                    return luxon.DateTime.fromISO(data, { zone: "utc" }).toLocal().toFormat("dd.LL.yyyy TT") // Formats data from UTC to local time
                }
            },
            {
                data: "Action",
                searchable: false,
                orderable: false,
                responsivePriority: 2,
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
        var table = $("#dtDefect").DataTable({
            paging: false,
            processing: true,
            serverSide: true,
            ajax: {
                url: "Defect/LoadTable",
                type: "POST",
                headers: {
                    "RequestVerificationToken": document.getElementById("RequestVerificationToken").value
                }
            },
            layout: {
                topEnd: null
            },
            responsive: true,
            columns: myColumns,
            initComplete: function () {
                $("#dtDefect thead tr").after("<tr>");
                this.api().columns().every(function () {
                    var column = this;
                    var index = column.index();
                    var searchable = myColumns[index].searchable;
                    var isVisible = column.responsiveHidden(); // returns true if visible
                    var data = myColumns[index].data;
                    var title = column.header().textContent;

                    if (searchable && isVisible && index !== 6) {
                        if (data.includes("IsDeleted")) {
                            $("#dtDefect thead tr:last")
                                .append(`<th><div class="form-check"><input class="form-check-input" type="checkbox" id="searchCheckbox"></div></th>`);
                            $("#dtDefect thead tr:last th:last input")
                                .on("click", function () {
                                    column.search(this.checked).draw();
                                });
                        } else if (data.includes("Date")) {
                            $("#dtDefect thead tr:last")
                                .append(`<th><input type="datetime-local" id="searchDate" placeholder="Search ${title}" /></th>`);
                            $("#dtDefect thead tr:last th:last input")
                                .on("change", function () {
                                    column.search(luxon.DateTime.fromISO(this.value).toUTC().toString()).draw(); // convert date from local time to UTC
                                });
                        } else {
                            $("#dtDefect thead tr:last")
                                .append(`<th><input type="search" placeholder="Search ${title}" /></th>`);
                            $("#dtDefect thead tr:last th:last input")
                                .on("keyup change clear", function () {
                                    if (column.search() !== this.value) {
                                        column.search(this.value).draw();
                                    }
                                });
                        }
                    }

                    if (index === 6) {
                        // Create select element and listener
                        var select = $('<select><option value="">Show all</option></select>')
                            .appendTo($(`#dtDefect thead tr:last`))
                            .on('change', function () {
                                column
                                    .search($(this).val(), { exact: true })
                                    .draw();
                            });

                        var states = ["Broken", "In repair", "Repaired"];

                        // Add list of options
                        column
                            .data()
                            .unique()
                            .sort()
                            .each(function (d, j) {
                                select.append(
                                    '<option value="' + d + '">' + states[d] + '</option>'
                                );
                            });
                    }
                });
            }
        });
    }
});