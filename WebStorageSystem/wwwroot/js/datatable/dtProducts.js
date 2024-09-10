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
                responsivePriority: 3,
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
                render: function (data, type, row) {
                    return luxon.DateTime.fromISO(data, { zone: "utc" }).toLocal().toFormat("dd.LL.yyyy TT"); // Formats data from UTC to local time
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
            responsive: true,
            columns: myColumns,
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
                                .append(`<th><div class="form-check"><input class="form-check-input" type="checkbox" id="searchCheckbox"></div></th>`);
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
                                .append(`<th><input type="search" placeholder="Search ${title}" /></th>`);
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
        table.columns(4).search("false").draw();
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
                responsivePriority: 3,
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
                render: function (data, type, row) {
                    return luxon.DateTime.fromISO(data, { zone: "utc" }).toLocal().toFormat("dd.LL.yyyy TT"); // Formats data from UTC to local time
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
            responsive: true,
            columns: myColumns,
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
                                .append(`<th><div class="form-check"><input class="form-check-input" type="checkbox" id="searchCheckbox"></div></th>`);
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
                                .append(`<th><input type="search" placeholder="Search ${title}" /></th>`);
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
                table.columns(4).search("false").draw();
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
                render: $.fn.dataTable.render.text()
            },
            {
                data: "Phone",
                searchable: true,
                orderable: true,
                responsivePriority: 4,
                render: $.fn.dataTable.render.text()
            },
            {
                data: "Email",
                searchable: true,
                orderable: true,
                responsivePriority: 3,
                render: $.fn.dataTable.render.text()
            },
            {
                data: "Website",
                searchable: true,
                orderable: true,
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
                render: function (data, type, row) {
                    return luxon.DateTime.fromISO(data, { zone: "utc" }).toLocal().toFormat("dd.LL.yyyy TT"); // Formats data from UTC to local time
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
            responsive: true,
            columns: myColumns,
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
                                .append(`<th><div class="form-check"><input class="form-check-input" type="checkbox" id="searchCheckbox"></div></th>`);
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
                                .append(`<th><input type="search" placeholder="Search ${title}" /></th>`);
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
        table.columns(7).search("false").draw();
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
                responsivePriority: 3,
                render: $.fn.dataTable.render.text()
            },
            {
                data: "Description",
                searchable: true,
                orderable: true,
                render: $.fn.dataTable.render.text()
            },
            {
                data: "Webpage",
                searchable: true,
                orderable: true,
                render: $.fn.dataTable.render.text()
            },
            {
                data: "ProductType.Name",
                searchable: true,
                orderable: true,
                responsivePriority: 4,
                render: $.fn.dataTable.render.text()
            },
            {
                data: "Manufacturer.Name",
                searchable: true,
                orderable: true,
                responsivePriority: 5,
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
                render: function (data, type, row) {
                    return luxon.DateTime.fromISO(data, { zone: "utc" }).toLocal().toFormat("dd.LL.yyyy TT"); // Formats data from UTC to local time
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
            responsive: true,
            columns: myColumns,
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
                                .append(`<th><div class="form-check"><input class="form-check-input" type="checkbox" id="searchCheckbox"></div></th>`);
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
                                .append(`<th><input type="search" placeholder="Search ${title}" /></th>`);
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
        table.columns(8).search("false").draw();
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
                responsivePriority: 4,
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
                render: $.fn.dataTable.render.text()
            },
            {
                data: "DefaultLocation.Name",
                searchable: true,
                orderable: true,
                render: $.fn.dataTable.render.text()
            },
            {
                data: "NumberOfUnits",
                searchable: false,
                orderable: false,
                render: $.fn.dataTable.render.text()
            },
            {
                data: "BundledUnits",
                searchable: true,
                orderable: false,
                responsivePriority: 3,
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
            responsive: true,
            columns: myColumns,
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
                                .append(`<th><input type="search" placeholder="Search ${title}" /></th>`);
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
        table.columns(7).search("false").draw();
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
                responsivePriority: 6,
                render: $.fn.dataTable.render.text()
            },
            {
                data: "Product.Manufacturer.Name",
                searchable: true,
                orderable: true,
                responsivePriority: 5,
                render: $.fn.dataTable.render.text()
            },
            {
                data: "Product.Name",
                searchable: true,
                orderable: true,
                responsivePriority: 4,
                render: $.fn.dataTable.render.text()
            },
            {
                data: "SerialNumber",
                searchable: true,
                orderable: true,
                responsivePriority: 3,
                render: $.fn.dataTable.render.text()
            },
            {
                data: "Location.Name",
                searchable: true,
                orderable: true,
                render: $.fn.dataTable.render.text()
            },
            {
                data: "DefaultLocation.Name",
                searchable: true,
                orderable: true,
                render: $.fn.dataTable.render.text()
            },
            {
                data: "ShelfNumber",
                searchable: true,
                orderable: true,
                render: $.fn.dataTable.render.text()
            },
            {
                data: "PartOfBundle.Name",
                searchable: true,
                orderable: true,
                defaultContent: "",
                responsivePriority: 7,
                render: $.fn.dataTable.render.text()
            },
            {
                data: "Notes",
                searchable: true,
                orderable: true,
                render: $.fn.dataTable.render.text()
            },
            {
                data: "Vendor.Name",
                searchable: true,
                orderable: true,
                defaultContent: "",
                render: $.fn.dataTable.render.text()
            },
            {
                data: "LastTransferTime",
                searchable: true,
                orderable: true,
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
            responsive: true,
            columns: myColumns,
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
                                .append(`<th><div class="form-check"><input class="form-check-input" type="checkbox" id="searchCheckbox"></div></th>`);
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
                                .append(`<th><input type="search" placeholder="Search ${title}" /></th>`);
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
        table.columns(14).search("false").draw();
    }
});