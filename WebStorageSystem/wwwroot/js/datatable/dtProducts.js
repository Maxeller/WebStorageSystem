// --- MANUFACTURER ---
$(document).ready(function () {
    if ($("#dtManufacturer").length !== 0) {
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
        var table = $("#dtManufacturer").DataTable({
            paging: false,
            processing: true,
            serverSide: true,
            ajax: {
                url: "Manufacturer/LoadTable",
                type: "POST"
            },
            layout: {
                topEnd: null
            },
            columns: myColumns
        });

        // Creation of search bars for searchable columns
        $("#dtManufacturer thead tr").after("<tr>");
        var counter = 0;
        $("#dtManufacturer thead th").each(function () {
            var title = $("#dtManufacturer thead th").eq($(this).index()).text();
            if (myColumns[counter].searchable) {
                if (myColumns[counter].data.includes("IsDeleted")) {
                    $("#dtManufacturer thead tr:last").append(`<th><div class="form-check"><input class="form-check-input" type="checkbox"></div></th>`);
                } else if (myColumns[counter].data.includes("Date")) {
                    $("#dtManufacturer thead tr:last").append(`<th><input type="datetime-local" id="searchDate" placeholder="Search ${title}" /></th>`);
                } else {
                    $("#dtManufacturer thead tr:last").append(`<th><input type="search" placeholder="Search ${title}" /></th>`);
                }
            } else {
                $("#dtManufacturer thead tr:last").append(`<th></th>`);
            }
            counter++;
        });
        $("#dtManufacturer thead th:last").after("</tr>");

        // Creation of trigger for search event
        table.columns().every(function (index) {
            var column = this;
            var elem = $(`#dtManufacturer thead tr:last th:eq(${index}) input`);
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

// --- PRODUCT TYPE ---
$(document).ready(function () {
    if ($("#dtProductType").length !== 0) {
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
        var table = $("#dtProductType").DataTable({
            paging: false,
            processing: true,
            serverSide: true,
            ajax: {
                url: "ProductType/LoadTable",
                type: "POST"
            },
            layout: {
                topEnd: null
            },
            columns: myColumns
        });

        // Creation of search bars for searchable columns
        $("#dtProductType thead tr").after("<tr>");
        var counter = 0;
        $("#dtProductType thead th").each(function () {
            var title = $("#dtProductType thead th").eq($(this).index()).text();
            if (myColumns[counter].searchable) {
                if (myColumns[counter].data.includes("IsDeleted")) {
                    $("#dtProductType thead tr:last").append(`<th><div class="form-check"><input class="form-check-input" type="checkbox"></div></th>`);
                } else if (myColumns[counter].data.includes("Date")) {
                    $("#dtProductType thead tr:last").append(`<th><input type="datetime-local" id="searchDate" placeholder="Search ${title}" /></th>`);
                } else {
                    $("#dtProductType thead tr:last").append(`<th><input type="search" placeholder="Search ${title}" /></th>`);
                }
            } else {
                $("#dtProductType thead tr:last").append(`<th></th>`);
            }
            counter++;
        });
        $("#dtProductType thead th:last").after("</tr>");

        // Creation of trigger for search event
        table.columns().every(function (index) {
            var column = this;
            var elem = $(`#dtProductType thead tr:last th:eq(${index}) input`);
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

// --- VENDOR ---
$(document).ready(function () {
    if ($("#dtVendor").length !== 0) {
        // Column definition
        var myColumns = [
            {
                data: "Name",
                searchable: true,
                orderable: true
            },
            {
                data: "Address",
                searchable: true,
                orderable: true
            },
            {
                data: "Phone",
                searchable: true,
                orderable: true
            },
            {
                data: "Email",
                searchable: true,
                orderable: true
            },
            {
                data: "Website",
                searchable: true,
                orderable: true
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
        var table = $("#dtVendor").DataTable({
            paging: false,
            processing: true,
            serverSide: true,
            ajax: {
                url: "Vendor/LoadTable",
                type: "POST"
            },
            layout: {
                topEnd: null
            },
            columns: myColumns
        });

        // Creation of search bars for searchable columns
        $("#dtVendor thead tr").after("<tr>");
        var counter = 0;
        $("#dtVendor thead th").each(function () {
            var title = $("#dtVendor thead th").eq($(this).index()).text();
            if (myColumns[counter].searchable) {
                if (myColumns[counter].data.includes("IsDeleted")) {
                    $("#dtVendor thead tr:last").append(`<th><div class="form-check"><input class="form-check-input" type="checkbox"></div></th>`);
                } else if (myColumns[counter].data.includes("Date")) {
                    $("#dtVendor thead tr:last").append(`<th><input type="datetime-local" id="searchDate" placeholder="Search ${title}" /></th>`);
                } else {
                    $("#dtVendor thead tr:last").append(`<th><input type="search" placeholder="Search ${title}" /></th>`);
                }
            } else {
                $("#dtVendor thead tr:last").append(`<th></th>`);
            }
            counter++;
        });
        $("#dtVendor thead th:last").after("</tr>");

        // Creation of trigger for search event
        table.columns().every(function (index) {
            var column = this;
            var elem = $(`#dtVendor thead tr:last th:eq(${index}) input`);
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

// --- PRODUCT ---
$(document).ready(function () {
    if ($("#dtProduct").length !== 0) {
        // Column definition
        var myColumns = [
            {
                data: "Name",
                searchable: true,
                orderable: true
            },
            {
                data: "ProductNumber",
                searchable: true,
                orderable: true
            },
            {
                data: "Description",
                searchable: true,
                orderable: true
            },
            {
                data: "Webpage",
                searchable: true,
                orderable: true
            },
            {
                data: "ProductType.Name",
                searchable: true,
                orderable: true
            },
            {
                data: "Manufacturer.Name",
                searchable: true,
                orderable: true
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
        var table = $("#dtProduct").DataTable({
            paging: false,
            processing: true,
            serverSide: true,
            ajax: {
                url: "Product/LoadTable",
                type: "POST"
            },
            layout: {
                topEnd: null
            },
            columns: myColumns
        });

        // Creation of search bars for searchable columns
        $("#dtProduct thead tr").after("<tr>");
        var counter = 0;
        $("#dtProduct thead th").each(function () {
            var title = $("#dtProduct thead th").eq($(this).index()).text();
            if (myColumns[counter].searchable) {
                if (myColumns[counter].data.includes("IsDeleted")) {
                    $("#dtProduct thead tr:last").append(`<th><div class="form-check"><input class="form-check-input" type="checkbox"></div></th>`);
                } else if (myColumns[counter].data.includes("Date")) {
                    $("#dtProduct thead tr:last").append(`<th><input type="datetime-local" id="searchDate" placeholder="Search ${title}" /></th>`);
                } else {
                    $("#dtProduct thead tr:last").append(`<th><input type="search" placeholder="Search ${title}" /></th>`);
                }
            } else {
                $("#dtProduct thead tr:last").append(`<th></th>`);
            }
            counter++;
        });
        $("#dtProduct thead th:last").after("</tr>");

        // Creation of trigger for search event
        table.columns().every(function (index) {
            var column = this;
            var elem = $(`#dtProduct thead tr:last th:eq(${index}) input`);
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

// --- BUNDLE ---
$(document).ready(function () {
    if ($("#dtBundle").length !== 0) {
        // Column definition
        var myColumns = [
            {
                data: "Name",
                searchable: true,
                orderable: true
            },
            {
                data: "InventoryNumber",
                searchable: true,
                orderable: true
            },
            {
                data: "NumberOfUnits",
                searchable: false,
                orderable: false
            },
            {
                data: "BundledUnits",
                searchable: true,
                orderable: false,
                render: function (data, type, row) {
                    var s = "";
                    for(const i in data) {
                        s = s + `${data[i].InventoryNumber} (${data[i].Product.ProductType.Name} - ${data[i].Product.Name})  <br />`;
                    }
                    return s;
                }
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
        var table = $("#dtBundle").DataTable({
            paging: false,
            processing: true,
            serverSide: true,
            ajax: {
                url: "Bundle/LoadTable",
                type: "POST"
            },
            layout: {
                topEnd: null
            },
            columns: myColumns
        });

        // Creation of search bars for searchable columns
        $("#dtBundle thead tr").after("<tr>");
        var counter = 0;
        $("#dtBundle thead th").each(function () {
            var title = $("#dtBundle thead th").eq($(this).index()).text();
            if (myColumns[counter].searchable) {
                if (myColumns[counter].data.includes("IsDeleted")) {
                    $("#dtBundle thead tr:last").append(`<th><div class="form-check"><input class="form-check-input" type="checkbox"></div></th>`);
                } else if (myColumns[counter].data.includes("Date")) {
                    $("#dtBundle thead tr:last").append(`<th><input type="datetime-local" id="searchDate" placeholder="Search ${title}" /></th>`);
                } else {
                    $("#dtBundle thead tr:last").append(`<th><input type="search" placeholder="Search ${title}" /></th>`);
                }
            } else {
                $("#dtBundle thead tr:last").append(`<th></th>`);
            }
            counter++;
        });
        $("#dtBundle thead th:last").after("</tr>");

        // Creation of trigger for search event
        table.columns().every(function (index) {
            var column = this;
            var elem = $(`#dtBundle thead tr:last th:eq(${index}) input`);
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

// --- UNIT ---
$(document).ready(function () {
    if ($("#dtUnit").length !== 0) {
        // Column definition
        var myColumns = [
            {
                data: "InventoryNumber",
                searchable: true,
                orderable: true
            },
            {
                data: "Product.ProductType.Name",
                searchable: true,
                orderable: true
            },
            {
                data: "Product.Manufacturer.Name",
                searchable: true,
                orderable: true
            },
            {
                data: "Product.Name",
                searchable: true,
                orderable: true
            },
            {
                data: "SerialNumber",
                searchable: true,
                orderable: true
            },
            {
                data: "Location.Name",
                searchable: true,
                orderable: true
            },
            {
                data: "DefaultLocation.Name",
                searchable: true,
                orderable: true
            },
            {
                data: "ShelfNumber",
                searchable: true,
                orderable: true
            },
            {
                data: "PartOfBundle.Name",
                searchable: true,
                orderable: true,
                defaultContent: ""
            },
            {
                data: "Notes",
                searchable: true,
                orderable: true
            },
            {
                data: "Vendor.Name",
                searchable: true,
                orderable: true,
                defaultContent: ""
            },
            {
                data: "LastTransferTime",
                searchable: true,
                orderable: true,
                render: function (data, type, row) {
                    if (data)
                        return luxon.DateTime.fromISO(data, { zone: "utc" }).toLocal().toFormat("dd.LL.yyyy TT") // Formats data from UTC to local time
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
                        return luxon.DateTime.fromISO(data, { zone: "utc" }).toLocal().toFormat("dd.LL.yyyy TT") // Formats data from UTC to local time
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
        var table = $("#dtUnit").DataTable({
            paging: false,
            processing: true,
            serverSide: true,
            ajax: {
                url: "Unit/LoadTable",
                type: "POST"
            },
            layout: {
                topEnd: null
            },
            columns: myColumns
        });

        // Creation of search bars for searchable columns
        $("#dtUnit thead tr").after("<tr>");
        var counter = 0;
        $("#dtUnit thead th").each(function () {
            var title = $("#dtUnit thead th").eq($(this).index()).text();
            if (myColumns[counter].searchable) {
                if (myColumns[counter].data.includes("IsDeleted")) {
                    $("#dtUnit thead tr:last").append(`<th><div class="form-check"><input class="form-check-input" type="checkbox"></div></th>`);
                } else if (myColumns[counter].data.includes("Date")) {
                    $("#dtUnit thead tr:last").append(`<th><input type="datetime-local" id="searchDate" placeholder="Search ${title}" /></th>`);
                } else {
                    $("#dtUnit thead tr:last").append(`<th><input type="search" placeholder="Search ${title}" /></th>`);
                }
            } else {
                $("#dtUnit thead tr:last").append(`<th></th>`);
            }
            counter++;
        });
        $("#dtUnit thead th:last").after("</tr>");

        // Creation of trigger for search event
        table.columns().every(function (index) {
            var column = this;
            var elem = $(`#dtUnit thead tr:last th:eq(${index}) input`);
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