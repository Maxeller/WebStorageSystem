// --- MANUFACTURER ---
$(document).ready(function () {
    if ($("#dtManufacturer").length !== 0) {
        // Column definition
        var myColumns = [
            {
                data: "Name",
                searchable: true,
                orderable: true,
                responsivePriority: 1,
                render: $.fn.dataTable.render.text()
            },
            {
                data: "Description",
                searchable: true,
                orderable: true,
                responsivePriority: 100,
                render: $.fn.dataTable.render.text()
            },
            {
                data: "CreatedDate",
                searchable: true,
                orderable: true,
                render: function (data, type, row) {
                    return luxon.DateTime.fromISO(data, { zone: "utc" }).toLocal().toFormat("dd.LL.yyyy TT"); // Formats data from UTC to local time
                }
            },
            {
                data: "ModifiedDate",
                searchable: true,
                orderable: true,
                responsivePriority: 9000,
                render: function (data, type, row) {
                    return luxon.DateTime.fromISO(data, { zone: "utc" }).toLocal().toFormat("dd.LL.yyyy TT"); // Formats data from UTC to local time
                }
            },
            {
                data: "IsDeleted",
                searchable: true,
                orderable: true,
                responsivePriority: 200,
                render: function (data, type, row) {
                    var s = "";
                    if (row.IsDeleted === true) s = "checked";
                    return `<div class="form-check"><input class="form-check-input w-100" type="checkbox" disabled ${s}></div>`;
                }
            },
            {
                data: "Action",
                searchable: false,
                orderable: false,
                responsivePriority: 10,
                render: function (data, type, row) {
                    var s = `<a href="${row.Action.Edit}" class="text-primary">Edit</a> <br />`;
                    s = s + `<a href="${row.Action.Details}" class="text-primary">Details</a> <br />`;
                    if (!row.IsDeleted) {
                        s = s +
                            `<a href="#" class="text-primary" data-toggle="modal" data-target="#deleteRestoreModal" data-url="${row.Action.Delete}" data-name="${row.Name}">Delete</a>`;
                    } else {
                        s = s +
                            `<a href="#" class="text-primary" data-toggle="modal" data-target="#deleteRestoreModal" data-url="${row.Action.Restore}" data-name="${row.Name}">Restore</a>`;
                    }
                    return s;
                }
            }
        ];

        // DataTable initialization 
        var table = $("#dtManufacturer").DataTable({
            paging: false,
            processing: true,
            serverSide: true,
            ajax: {
                url: "Manufacturer/LoadTable",
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
            searchCols: [
                null, null, null, null, { search: "false" }, null
            ],
            order: [[0, "asc"]],
            initComplete: function () {
                $("#dtManufacturer thead tr").after("<tr>");
                this.api().columns().every(function () {
                    var column = this;
                    var index = column.index();
                    var searchable = myColumns[index].searchable;
                    var isVisible = column.responsiveHidden(); // returns true if visible
                    var data = myColumns[index].data;
                    var title = column.header().textContent;

                    if (searchable && isVisible) {
                        if (data.includes("IsDeleted")) {
                            $("#dtManufacturer thead tr:last")
                                .append(`<th><div class="form-check"><input class="form-check-input w-100" type="checkbox" id="searchCheckbox"></div></th>`);
                            $("#dtManufacturer thead tr:last th:last input")
                                .on("click", function () {
                                    column.search(this.checked).draw();
                                });
                        } else if (data.includes("Date")) {
                            $("#dtManufacturer thead tr:last")
                                .append(`<th><input type="datetime-local" id="searchDate" placeholder="Search ${title}" /></th>`);
                            $("#dtManufacturer thead tr:last th:last input")
                                .on("change", function () {
                                    column.search(luxon.DateTime.fromISO(this.value).toUTC().toString()).draw(); // convert date from local time to UTC
                                });
                        } else {
                            $("#dtManufacturer thead tr:last")
                                .append(`<th><input class="search w-100" placeholder="Search ${title}" /></th>`);
                            $("#dtManufacturer thead tr:last th:last input")
                                .on("keyup change clear", function () {
                                    if (column.search() !== this.value) {
                                        column.search(this.value).draw();
                                    }
                                });
                        }
                    }

                    if (!searchable && isVisible) {
                        $("#dtManufacturer thead tr:last").append(`<th></th>`);
                    }
                });
            }
        });
    }
});

// --- PRODUCT TYPE ---
$(document).ready(function () {
    if ($("#dtProductType").length !== 0) {
        // Column definition
        var myColumns = [
            {
                data: "Name",
                searchable: true,
                orderable: true,
                responsivePriority: 1,
                render: $.fn.dataTable.render.text()
            },
            {
                data: "Description",
                searchable: true,
                orderable: true,
                responsivePriority: 100,
                render: $.fn.dataTable.render.text()
            },
            {
                data: "CreatedDate",
                searchable: true,
                orderable: true,
                render: function (data, type, row) {
                    return luxon.DateTime.fromISO(data, { zone: "utc" }).toLocal().toFormat("dd.LL.yyyy TT"); // Formats data from UTC to local time
                }
            },
            {
                data: "ModifiedDate",
                searchable: true,
                orderable: true,
                responsivePriority: 9000,
                render: function (data, type, row) {
                    return luxon.DateTime.fromISO(data, { zone: "utc" }).toLocal().toFormat("dd.LL.yyyy TT"); // Formats data from UTC to local time
                }
            },
            {
                data: "IsDeleted",
                searchable: true,
                orderable: true,
                responsivePriority: 200,
                render: function (data, type, row) {
                    var s = "";
                    if (row.IsDeleted === true) s = "checked";
                    return `<div class="form-check"><input class="form-check-input w-100" type="checkbox" disabled ${s}></div>`;
                }
            },
            {
                data: "Action",
                searchable: false,
                orderable: false,
                responsivePriority: 10,
                render: function (data, type, row) {
                    var s = `<a href="${row.Action.Edit}" class="text-primary">Edit</a> <br />`;
                    s = s + `<a href="${row.Action.Details}" class="text-primary">Details</a> <br />`;
                    if (!row.IsDeleted) {
                        s = s +
                            `<a href="#" class="text-primary" data-toggle="modal" data-target="#deleteRestoreModal" data-url="${row.Action.Delete}" data-name="${row.Name}">Delete</a>`;
                    } else {
                        s = s +
                            `<a href="#" class="text-primary" data-toggle="modal" data-target="#deleteRestoreModal" data-url="${row.Action.Restore}" data-name="${row.Name}">Restore</a>`;
                    }
                    return s;
                }
            }
        ];

        // DataTable initialization 
        var table = $("#dtProductType").DataTable({
            paging: false,
            processing: true,
            serverSide: true,
            ajax: {
                url: "ProductType/LoadTable",
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
            searchCols: [
                null, null, null, null, { search: "false" }, null
            ],
            order: [[0, "asc"]],
            initComplete: function () {
                $("#dtProductType thead tr").after("<tr>");
                this.api().columns().every(function () {
                    var column = this;
                    var index = column.index();
                    var searchable = myColumns[index].searchable;
                    var isVisible = column.responsiveHidden(); // returns true if visible
                    var data = myColumns[index].data;
                    var title = column.header().textContent;

                    if (searchable && isVisible) {
                        if (data.includes("IsDeleted")) {
                            $("#dtProductType thead tr:last")
                                .append(`<th><div class="form-check"><input class="form-check-input w-100" type="checkbox" id="searchCheckbox"></div></th>`);
                            $("#dtProductType thead tr:last th:last input")
                                .on("click", function () {
                                    column.search(this.checked).draw();
                                });
                        } else if (data.includes("Date")) {
                            $("#dtProductType thead tr:last")
                                .append(`<th><input type="datetime-local" id="searchDate" placeholder="Search ${title}" /></th>`);
                            $("#dtProductType thead tr:last th:last input")
                                .on("change", function () {
                                    column.search(luxon.DateTime.fromISO(this.value).toUTC().toString()).draw(); // convert date from local time to UTC
                                });
                        } else {
                            $("#dtProductType thead tr:last")
                                .append(`<th><input class="search w-100" placeholder="Search ${title}" /></th>`);
                            $("#dtProductType thead tr:last th:last input")
                                .on("keyup change clear", function () {
                                    if (column.search() !== this.value) {
                                        column.search(this.value).draw();
                                    }
                                });
                        }
                    }

                    if (!searchable && isVisible) {
                        $("#dtProductType thead tr:last").append(`<th></th>`);
                    }
                });
            }
        });
    }
});

// --- VENDOR ---
$(document).ready(function () {
    if ($("#dtVendor").length !== 0) {
        // Column definition
        var myColumns = [
            {
                data: "Name",
                searchable: true,
                orderable: true,
                responsivePriority: 1,
                render: $.fn.dataTable.render.text()
            },
            {
                data: "Address",
                searchable: true,
                orderable: true,
                responsivePriority: 23,
                render: $.fn.dataTable.render.text()
            },
            {
                data: "Phone",
                searchable: true,
                orderable: true,
                responsivePriority: 20,
                render: $.fn.dataTable.render.text()
            },
            {
                data: "Email",
                searchable: true,
                orderable: true,
                responsivePriority: 21,
                render: $.fn.dataTable.render.text()
            },
            {
                data: "Website",
                searchable: true,
                orderable: true,
                responsivePriority: 22,
                render: $.fn.dataTable.render.text()
            },
            {
                data: "CreatedDate",
                searchable: true,
                orderable: true,
                render: function (data, type, row) {
                    return luxon.DateTime.fromISO(data, { zone: "utc" }).toLocal().toFormat("dd.LL.yyyy TT"); // Formats data from UTC to local time
                }
            },
            {
                data: "ModifiedDate",
                searchable: true,
                orderable: true,
                responsivePriority: 9000,
                render: function (data, type, row) {
                    return luxon.DateTime.fromISO(data, { zone: "utc" }).toLocal().toFormat("dd.LL.yyyy TT"); // Formats data from UTC to local time
                }
            },
            {
                data: "IsDeleted",
                searchable: true,
                orderable: true,
                responsivePriority: 200,
                render: function (data, type, row) {
                    var s = "";
                    if (row.IsDeleted === true) s = "checked";
                    return `<div class="form-check"><input class="form-check-input w-100" type="checkbox" disabled ${s}></div>`;
                }
            },
            {
                data: "Action",
                searchable: false,
                orderable: false,
                responsivePriority: 10,
                render: function (data, type, row) {
                    var s = `<a href="${row.Action.Edit}" class="text-primary">Edit</a> <br />`;
                    s = s + `<a href="${row.Action.Details}" class="text-primary">Details</a> <br />`;
                    if (!row.IsDeleted) {
                        s = s +
                            `<a href="#" class="text-primary" data-toggle="modal" data-target="#deleteRestoreModal" data-url="${row.Action.Delete}" data-name="${row.Name}">Delete</a>`;
                    } else {
                        s = s +
                            `<a href="#" class="text-primary" data-toggle="modal" data-target="#deleteRestoreModal" data-url="${row.Action.Restore}" data-name="${row.Name}">Restore</a>`;
                    }
                    return s;
                }
            }
        ];

        // DataTable initialization 
        var table = $("#dtVendor").DataTable({
            paging: false,
            processing: true,
            serverSide: true,
            ajax: {
                url: "Vendor/LoadTable",
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
            searchCols: [
                null, null, null, null, null, null, null, { search: "false" }, null
            ],
            order: [[0, "asc"]],
            initComplete: function () {
                $("#dtVendor thead tr").after("<tr>");
                this.api().columns().every(function () {
                    var column = this;
                    var index = column.index();
                    var searchable = myColumns[index].searchable;
                    var isVisible = column.responsiveHidden(); // returns true if visible
                    var data = myColumns[index].data;
                    var title = column.header().textContent;

                    if (searchable && isVisible) {
                        if (data.includes("IsDeleted")) {
                            $("#dtVendor thead tr:last")
                                .append(`<th><div class="form-check"><input class="form-check-input w-100" type="checkbox" id="searchCheckbox"></div></th>`);
                            $("#dtVendor thead tr:last th:last input")
                                .on("click", function () {
                                    column.search(this.checked).draw();
                                });
                        } else if (data.includes("Date")) {
                            $("#dtVendor thead tr:last")
                                .append(`<th><input type="datetime-local" id="searchDate" placeholder="Search ${title}" /></th>`);
                            $("#dtVendor thead tr:last th:last input")
                                .on("change", function () {
                                    column.search(luxon.DateTime.fromISO(this.value).toUTC().toString()).draw(); // convert date from local time to UTC
                                });
                        } else {
                            $("#dtVendor thead tr:last")
                                .append(`<th><input class="search w-100" placeholder="Search ${title}" /></th>`);
                            $("#dtVendor thead tr:last th:last input")
                                .on("keyup change clear", function () {
                                    if (column.search() !== this.value) {
                                        column.search(this.value).draw();
                                    }
                                });
                        }
                    }

                    if (!searchable && isVisible) {
                        $("#dtVendor thead tr:last").append(`<th></th>`);
                    }
                });
            }
        });
    }
});

// --- PRODUCT ---
$(document).ready(function () {
    if ($("#dtProduct").length !== 0) {
        // Column definition
        var myColumns = [
            {
                data: "Name",
                searchable: true,
                orderable: true,
                responsivePriority: 1,
                render: $.fn.dataTable.render.text()
            },
            {
                data: "ProductNumber",
                searchable: true,
                orderable: true,
                responsivePriority: 20,
                render: $.fn.dataTable.render.text()
            },
            {
                data: "Description",
                searchable: true,
                orderable: true,
                responsivePriority: 30,
                render: $.fn.dataTable.render.text()
            },
            {
                data: "Webpage",
                searchable: true,
                orderable: true,
                responsivePriority: 31,
                render: $.fn.dataTable.render.text()
            },
            {
                data: "ProductType.Name",
                searchable: true,
                orderable: true,
                responsivePriority: 21,
                render: $.fn.dataTable.render.text()
            },
            {
                data: "Manufacturer.Name",
                searchable: true,
                orderable: true,
                responsivePriority: 22,
                render: $.fn.dataTable.render.text()
            },
            {
                data: "CreatedDate",
                searchable: true,
                orderable: true,
                render: function (data, type, row) {
                    return luxon.DateTime.fromISO(data, { zone: "utc" }).toLocal().toFormat("dd.LL.yyyy TT"); // Formats data from UTC to local time
                }
            },
            {
                data: "ModifiedDate",
                searchable: true,
                orderable: true,
                responsivePriority: 9000,
                render: function (data, type, row) {
                    return luxon.DateTime.fromISO(data, { zone: "utc" }).toLocal().toFormat("dd.LL.yyyy TT"); // Formats data from UTC to local time
                }
            },
            {
                data: "IsDeleted",
                searchable: true,
                orderable: true,
                responsivePriority: 200,
                render: function (data, type, row) {
                    var s = "";
                    if (row.IsDeleted === true) s = "checked";
                    return `<div class="form-check"><input class="form-check-input w-100" type="checkbox" disabled ${s}></div>`;
                }
            },
            {
                data: "Action",
                searchable: false,
                orderable: false,
                responsivePriority: 10,
                render: function (data, type, row) {
                    var s = `<a href="${row.Action.Edit}" class="text-primary">Edit</a> <br />`;
                    s = s + `<a href="${row.Action.Details}" class="text-primary">Details</a> <br />`;
                    if (!row.IsDeleted) {
                        s = s +
                            `<a href="#" class="text-primary" data-toggle="modal" data-target="#deleteRestoreModal" data-url="${row.Action.Delete}" data-name="${row.Name}">Delete</a>`;
                    } else {
                        s = s +
                            `<a href="#" class="text-primary" data-toggle="modal" data-target="#deleteRestoreModal" data-url="${row.Action.Restore}" data-name="${row.Name}">Restore</a>`;
                    }
                    return s;
                }
            }
        ];

        // DataTable initialization 
        var table = $("#dtProduct").DataTable({
            paging: false,
            processing: true,
            serverSide: true,
            ajax: {
                url: "Product/LoadTable",
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
            searchCols: [
                null, null, null, null, null, null, null, null, { search: "false" }, null
            ],
            order: [[0, "asc"]],
            initComplete: function () {
                $("#dtProduct thead tr").after("<tr>");
                this.api().columns().every(function () {
                    var column = this;
                    var index = column.index();
                    var searchable = myColumns[index].searchable;
                    var isVisible = column.responsiveHidden(); // returns true if visible
                    var data = myColumns[index].data;
                    var title = column.header().textContent;

                    if (searchable && isVisible) {
                        if (data.includes("IsDeleted")) {
                            $("#dtProduct thead tr:last")
                                .append(`<th><div class="form-check"><input class="form-check-input w-100" type="checkbox" id="searchCheckbox"></div></th>`);
                            $("#dtProduct thead tr:last th:last input")
                                .on("click", function () {
                                    column.search(this.checked).draw();
                                });
                        } else if (data.includes("Date")) {
                            $("#dtProduct thead tr:last")
                                .append(`<th><input type="datetime-local" id="searchDate" placeholder="Search ${title}" /></th>`);
                            $("#dtProduct thead tr:last th:last input")
                                .on("change", function () {
                                    column.search(luxon.DateTime.fromISO(this.value).toUTC().toString()).draw(); // convert date from local time to UTC
                                });
                        } else {
                            $("#dtProduct thead tr:last")
                                .append(`<th><input class="search w-100" placeholder="Search ${title}" /></th>`);
                            $("#dtProduct thead tr:last th:last input")
                                .on("keyup change clear", function () {
                                    if (column.search() !== this.value) {
                                        column.search(this.value).draw();
                                    }
                                });
                        }
                    }

                    if (!searchable && isVisible) {
                        $("#dtProduct thead tr:last").append(`<th></th>`);
                    }
                });
            }
        });
    }
});

// --- BUNDLE ---
$(document).ready(function () {
    if ($("#dtBundle").length !== 0) {
        // Column definition
        var myColumns = [
            {
                data: "Name",
                searchable: true,
                orderable: true,
                responsivePriority: 3,
                render: $.fn.dataTable.render.text()
            },
            {
                data: "InventoryNumber",
                searchable: true,
                orderable: true,
                responsivePriority: 1,
                render: $.fn.dataTable.render.text()
            },
            {
                data: "Location.Name",
                searchable: true,
                orderable: true,
                responsivePriority: 30,
                render: $.fn.dataTable.render.text()
            },
            {
                data: "DefaultLocation.Name",
                searchable: true,
                orderable: true,
                responsivePriority: 31,
                render: $.fn.dataTable.render.text()
            },
            {
                data: "NumberOfUnits",
                searchable: false,
                orderable: false,
                responsivePriority: 20,
                render: $.fn.dataTable.render.text()
            },
            {
                data: "BundledUnits",
                searchable: true,
                orderable: false,
                responsivePriority: 2,
                render: function (data, type, row) {
                    var s = "";
                    for(const i in data) {
                        s = s + `${data[i].InventoryNumber} (${data[i].Product.ProductType.Name} - ${data[i].Product.Name})  <br />`;
                    }
                    return s;
                }
            },
            {
                data: "HasDefect",
                searchable: true,
                orderable: true,
                responsivePriority: 40,
                render: function (data, type, row) {
                    var s = "";
                    if (row.HasDefect === true) s = "checked";
                    return `<div class="form-check"><input class="form-check-input" type="checkbox" disabled ${s}></div>`;
                }
            },
            {
                data: "IsDeleted",
                searchable: true,
                orderable: true,
                responsivePriority: 200,
                render: function (data, type, row) {
                    var s = "";
                    if (row.IsDeleted === true) s = "checked";
                    return `<div class="form-check"><input class="form-check-input w-100" type="checkbox" disabled ${s}></div>`;
                }
            },
            {
                data: "Action",
                searchable: false,
                orderable: false,
                responsivePriority: 10,
                render: function (data, type, row) {
                    var s = `<a href="${row.Action.Edit}" class="text-primary">Edit</a> <br />`;
                    s = s + `<a href="${row.Action.Details}" class="text-primary">Details</a> <br />`;
                    if (!row.IsDeleted) {
                        s = s +
                            `<a href="#" class="text-primary" data-toggle="modal" data-target="#deleteRestoreModal" data-url="${row.Action.Delete}" data-name="${row.Name}">Delete</a>`;
                    } else {
                        s = s +
                            `<a href="#" class="text-primary" data-toggle="modal" data-target="#deleteRestoreModal" data-url="${row.Action.Restore}" data-name="${row.Name}">Restore</a>`;
                    }
                    return s;
                }
            }
        ];

        // DataTable initialization 
        var table = $("#dtBundle").DataTable({
            paging: false,
            processing: true,
            serverSide: true,
            ajax: {
                url: "Bundle/LoadTable",
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
            searchCols: [
                null, null, null, null, null, null, null, { search: "false" }, null
            ],
            order: [[0, "asc"]],
            initComplete: function () {
                $("#dtBundle thead tr").after("<tr>");
                this.api().columns().every(function () {
                    var column = this;
                    var index = column.index();
                    var searchable = myColumns[index].searchable;
                    var isVisible = column.responsiveHidden(); // returns true if visible
                    var data = myColumns[index].data;
                    var title = column.header().textContent;

                    if (searchable && isVisible) {
                        if (data.includes("IsDeleted")) {
                            $("#dtBundle thead tr:last")
                                .append(`<th><div class="form-check"><input class="form-check-input" type="checkbox" id="searchCheckbox"></div></th>`);
                            $("#dtBundle thead tr:last th:last input")
                                .on("click", function () {
                                    column.search(this.checked).draw();
                                });
                        } else if (data.includes("Date")) {
                            $("#dtBundle thead tr:last")
                                .append(`<th><input type="datetime-local" id="searchDate" placeholder="Search ${title}" /></th>`);
                            $("#dtBundle thead tr:last th:last input")
                                .on("change", function () {
                                    column.search(luxon.DateTime.fromISO(this.value).toUTC().toString()).draw(); // convert date from local time to UTC
                                });
                        } else {
                            $("#dtBundle thead tr:last")
                                .append(`<th><input class="search w-100" placeholder="Search ${title}" /></th>`);
                            $("#dtBundle thead tr:last th:last input")
                                .on("keyup change clear", function () {
                                    if (column.search() !== this.value) {
                                        column.search(this.value).draw();
                                    }
                                });
                        }
                    }

                    if (!searchable && isVisible) {
                        $("#dtBundle thead tr:last").append(`<th></th>`);
                    }
                });
            }
        });
    }
});

// --- UNIT ---
$(document).ready(function () {
    if ($("#dtUnit").length !== 0) {
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
                data: "Product.ProductType.Name",
                searchable: true,
                orderable: true,
                responsivePriority: 20,
                render: $.fn.dataTable.render.text()
            },
            {
                data: "Product.Manufacturer.Name",
                searchable: true,
                orderable: true,
                responsivePriority: 21,
                render: $.fn.dataTable.render.text()
            },
            {
                data: "Product.Name",
                searchable: true,
                orderable: true,
                responsivePriority: 22,
                render: $.fn.dataTable.render.text()
            },
            {
                data: "SerialNumber",
                searchable: true,
                orderable: true,
                responsivePriority: 2,
                render: $.fn.dataTable.render.text()
            },
            {
                data: "Location.Name",
                searchable: true,
                orderable: true,
                responsivePriority: 30,
                render: $.fn.dataTable.render.text()
            },
            {
                data: "DefaultLocation.Name",
                searchable: true,
                orderable: true,
                responsivePriority: 31,
                render: $.fn.dataTable.render.text()
            },
            {
                data: "PartOfBundle.Name",
                searchable: true,
                orderable: true,
                defaultContent: "",
                responsivePriority: 23,
                render: $.fn.dataTable.render.text()
            },
            {
                data: "Vendor.Name",
                searchable: true,
                orderable: true,
                defaultContent: "",
                responsivePriority: 32,
                render: $.fn.dataTable.render.text()
            },
            {
                data: "LastTransferTime",
                searchable: true,
                orderable: true,
                responsivePriority: 40,
                render: function (data, type, row) {
                    if (data)
                        return luxon.DateTime.fromISO(data, { zone: "utc" }).toLocal().toFormat("dd.LL.yyyy TT"); // Formats data from UTC to local time
                    else
                        return null;
                }
            },
            {
                data: "LastCheckTime",
                searchable: true,
                orderable: true,
                responsivePriority: 41,
                render: function (data, type, row) {
                    if (data)
                        return luxon.DateTime.fromISO(data, { zone: "utc" }).toLocal().toFormat("dd.LL.yyyy TT"); // Formats data from UTC to local time
                    else
                        return null;
                }
            },
            {
                data: "HasDefect",
                searchable: true,
                orderable: true,
                responsivePriority: 42,
                render: function (data, type, row) {
                    var s = "";
                    if (row.HasDefect === true) s = "checked";
                    return `<div class="form-check"><input class="form-check-input w-100" type="checkbox" disabled ${s}></div>`;
                }
            },
            {
                data: "IsDeleted",
                searchable: true,
                orderable: true,
                responsivePriority: 200,
                render: function (data, type, row) {
                    var s = "";
                    if (row.IsDeleted === true) s = "checked";
                    return `<div class="form-check"><input class="form-check-input w-100" type="checkbox" disabled ${s}></div>`;
                }
            },
            {
                data: "Action",
                searchable: false,
                orderable: false,
                responsivePriority: 10,
                render: function (data, type, row) {
                    var s = `<a href="${row.Action.Edit}" class="text-primary">Edit</a> <br />`;
                    s = s + `<a href="${row.Action.Details}" class="text-primary">Details</a> <br />`;
                    if (!row.IsDeleted) {
                        s = s +
                            `<a href="#" class="text-primary" data-toggle="modal" data-target="#deleteRestoreModal" data-url="${row.Action.Delete}" data-name="${row.InventoryNumber}">Delete</a>`;
                    } else {
                        s = s +
                            `<a href="#" class="text-primary" data-toggle="modal" data-target="#deleteRestoreModal" data-url="${row.Action.Restore}" data-name="${row.InventoryNumber}">Restore</a>`;
                    }
                    return s;
                }
            }
        ];

        // DataTable initialization 
        var table = $("#dtUnit").DataTable({
            paging: false,
            processing: true,
            serverSide: true,
            ajax: {
                url: "Unit/LoadTable",
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
            
            searchCols: [
                null, null, null, null, null, null, null, null, null, null, null, { search: "false" }, { search: "false" }, null
            ],
            order: [[0, "asc"]],
            initComplete: function () {
                $("#dtUnit thead tr").after("<tr>");
                this.api().columns().every(function () {
                    var column = this;
                    var index = column.index();
                    var searchable = myColumns[index].searchable;
                    var isVisible = column.responsiveHidden(); // returns true if visible
                    var data = myColumns[index].data;
                    var title = column.header().textContent;

                    if (searchable && isVisible) {
                        if (data.includes("IsDeleted") || data.includes("HasDefect")) {
                            $("#dtUnit thead tr:last")
                                .append(`<th><div class="form-check"><input class="form-check-input w-100" type="checkbox" id="searchCheckbox"></div></th>`);
                            $("#dtUnit thead tr:last th:last input")
                                .on("click", function () {
                                    column.search(this.checked).draw();
                                });
                        } else if (data.includes("Date")) {
                            $("#dtUnit thead tr:last")
                                .append(`<th><input type="datetime-local" id="searchDate" placeholder="Search ${title}" /></th>`);
                            $("#dtUnit thead tr:last th:last input")
                                .on("change", function () {
                                    column.search(luxon.DateTime.fromISO(this.value).toUTC().toString()).draw(); // convert date from local time to UTC
                                });
                        } else {
                            $("#dtUnit thead tr:last")
                                .append(`<th><input class="search w-100" placeholder="Search ${title}" /></th>`);
                            $("#dtUnit thead tr:last th:last input")
                                .on("keyup change clear", function () {
                                    if (column.search() !== this.value) {
                                        column.search(this.value).draw();
                                    }
                                });
                        }
                    }

                    if (!searchable && isVisible) {
                        $("#dtUnit thead tr:last").append(`<th></th>`);
                    }
                });
            }
        });
    }
});