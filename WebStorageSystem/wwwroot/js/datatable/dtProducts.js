// --- MANUFACTURER ---
$(() => {
    if ($("#dtManufacturer").length !== 0) {
        const table = $("#dtManufacturer").DataTable({
            language: {
                processing: "Loading Data...",
                zeroRecords: "No matching records found"
            },
            processing: true,
            serverSide: true,
            orderCellsTop: true,
            autoWidth: true,
            deferRender: true,
            //lengthMenu: [[5, 10, 15, 20, -1], [5, 10, 15, 20, "All"]],
            paging: false,
            dom: '<"row"<"col-sm-12 col-md-6"B><"col-sm-12 col-md-6 text-right"l>><"row"<"col-sm-12"tr>><"row"<"col-sm-12 col-md-5"i><"col-sm-12 col-md-7"p>>',
            buttons: [
                {
                    text: "Export to Excel",
                    className: "btn btn-sm btn-dark",
                    action: function (e, dt, node, config) {
                        window.location.href = "/Home/GetExcel"; // TODO: Change path
                    },
                    init: function (api, node, config) {
                        $(node).removeClass("dt-button");
                    }
                }
            ],
            ajax: {
                type: "POST",
                url: "Manufacturer/LoadTable",
                contentType: "application/json; charset=utf-8",
                async: true,
                headers: {
                    "XSRF-TOKEN": document.querySelector('[name="__RequestVerificationToken"]').value
                },
                data: function (data) {
                    //let additionalValues = [];
                    //additionalValues[0] = "Additional Parameters 1";
                    //additionalValues[1] = "Additional Parameters 2";
                    //data.AdditionalValues = additionalValues;
                    return JSON.stringify(data);
                }
            },
            columns: [
                {
                    data: "Name",
                    name: "co"
                },
                {
                    data: "Description",
                    name: "co"
                },
                {
                    data: "CreatedDate",
                    render: function (data, type, row) {
                        if (data)
                            return moment(data).local().format("DD.MM.YYYY HH:mm:ss"); // Formats data from UTC to local time
                        else
                            return null;
                    },
                    name: "gte"
                },
                {
                    data: "ModifiedDate",
                    render: function (data, type, row) {
                        if (data)
                            return moment(data).local().format("DD.MM.YYYY HH:mm:ss");
                        else
                            return null;
                    },
                    name: "gte"
                },
                {
                    data: "IsDeleted",
                    render: function (data, type, row) {
                        if (data)
                            return "Yes";
                        else
                            return "No";
                    }
                },
                {
                    data: "Action",
                    orderable: false,
                    width: 155,
                    render: function (data, type, row) {
                        let s = `<a href="${row.Action.Edit}" class="text-primary">Edit</a> | `;
                        s = s + `<a href="${row.Action.Details}" class="text-primary">Details</a> | `;
                        if (!row.IsDeleted) {
                            s = s + `<a href="#" class="text-primary" data-toggle="modal" data-target="#deleteRestoreModal" data-url="${row.Action.Delete}" data-name="${row.Name}">Delete</a>`;
                        } else {
                            s = s + `<a href="#" class="text-primary" data-toggle="modal" data-target="#deleteRestoreModal" data-url="${row.Action.Restore}" data-name="${row.Name}">Restore</a>`;
                        }
                        return s;
                    }
                }
            ]
        });

        table.columns().every(function (index) {
            $(`#dtManufacturer thead tr:last th:eq(${index}) input`)
                .on("keyup",
                    function (e) {
                        if (e.keyCode === 13) {
                            table.column($(this).parent().index() + ":visible").search(this.value).draw();
                        }
                    }); // TODO: Add function to refresh when input is cleared
        });
    }
});

// --- PRODUCT TYPE ---
$(() => {
    if ($("#dtProductType").length !== 0) {
        const table = $("#dtProductType").DataTable({
            language: {
                processing: "Loading Data...",
                zeroRecords: "No matching records found"
            },
            processing: true,
            serverSide: true,
            orderCellsTop: true,
            autoWidth: true,
            deferRender: true,
            //lengthMenu: [[5, 10, 15, 20, -1], [5, 10, 15, 20, "All"]],
            paging: false,
            dom: '<"row"<"col-sm-12 col-md-6"B><"col-sm-12 col-md-6 text-right"l>><"row"<"col-sm-12"tr>><"row"<"col-sm-12 col-md-5"i><"col-sm-12 col-md-7"p>>',
            buttons: [
                {
                    text: "Export to Excel",
                    className: "btn btn-sm btn-dark",
                    action: function (e, dt, node, config) {
                        window.location.href = "/Home/GetExcel"; // TODO: Change path
                    },
                    init: function (api, node, config) {
                        $(node).removeClass("dt-button");
                    }
                }
            ],
            ajax: {
                type: "POST",
                url: "ProductType/LoadTable",
                contentType: "application/json; charset=utf-8",
                async: true,
                headers: {
                    "XSRF-TOKEN": document.querySelector('[name="__RequestVerificationToken"]').value
                },
                data: function (data) {
                    //let additionalValues = [];
                    //additionalValues[0] = "Additional Parameters 1";
                    //additionalValues[1] = "Additional Parameters 2";
                    //data.AdditionalValues = additionalValues;
                    return JSON.stringify(data);
                }
            },
            columns: [
                {
                    data: "Name",
                    name: "co"
                },
                {
                    data: "Description",
                    name: "co"
                },
                {
                    data: "CreatedDate",
                    render: function (data, type, row) {
                        if (data)
                            return moment(data).local().format("DD.MM.YYYY HH:mm:ss"); // Formats data from UTC to local time
                        else
                            return null;
                    },
                    name: "gte"
                },
                {
                    data: "ModifiedDate",
                    render: function (data, type, row) {
                        if (data)
                            return moment(data).local().format("DD.MM.YYYY HH:mm:ss");
                        else
                            return null;
                    },
                    name: "gte"
                },
                {
                    data: "IsDeleted",
                    render: function (data, type, row) {
                        if (data)
                            return "Yes";
                        else
                            return "No";
                    }
                },
                {
                    data: "Action",
                    orderable: false,
                    width: 155,
                    render: function (data, type, row) {
                        let s = `<a href="${row.Action.Edit}" class="text-primary">Edit</a> | `;
                        s = s + `<a href="${row.Action.Details}" class="text-primary">Details</a> | `;
                        if (!row.IsDeleted) {
                            s = s + `<a href="#" class="text-primary" data-toggle="modal" data-target="#deleteRestoreModal" data-url="${row.Action.Delete}" data-name="${row.Name}">Delete</a>`;
                        } else {
                            s = s + `<a href="#" class="text-primary" data-toggle="modal" data-target="#deleteRestoreModal" data-url="${row.Action.Restore}" data-name="${row.Name}">Restore</a>`;
                        }
                        return s;
                    }
                }
            ]
        });

        table.columns().every(function (index) {
            $(`#dtProductType thead tr:last th:eq(${index}) input`)
                .on("keyup",
                    function (e) {
                        if (e.keyCode === 13) {
                            table.column($(this).parent().index() + ":visible").search(this.value).draw();
                        }
                    }); // TODO: Add function to refresh when input is cleared
        });
    }
});

// --- VENDOR ---
$(() => {
    if ($("#dtVendor").length !== 0) {
        const table = $("#dtVendor").DataTable({
            language: {
                processing: "Loading Data...",
                zeroRecords: "No matching records found"
            },
            processing: true,
            serverSide: true,
            orderCellsTop: true,
            autoWidth: true,
            deferRender: true,
            //lengthMenu: [[5, 10, 15, 20, -1], [5, 10, 15, 20, "All"]],
            paging: false,
            dom: '<"row"<"col-sm-12 col-md-6"B><"col-sm-12 col-md-6 text-right"l>><"row"<"col-sm-12"tr>><"row"<"col-sm-12 col-md-5"i><"col-sm-12 col-md-7"p>>',
            buttons: [
                {
                    text: "Export to Excel",
                    className: "btn btn-sm btn-dark",
                    action: function (e, dt, node, config) {
                        window.location.href = "/Home/GetExcel"; // TODO: Change path
                    },
                    init: function (api, node, config) {
                        $(node).removeClass("dt-button");
                    }
                }
            ],
            ajax: {
                type: "POST",
                url: "Vendor/LoadTable",
                contentType: "application/json; charset=utf-8",
                async: true,
                headers: {
                    "XSRF-TOKEN": document.querySelector('[name="__RequestVerificationToken"]').value
                },
                data: function (data) {
                    //let additionalValues = [];
                    //additionalValues[0] = "Additional Parameters 1";
                    //additionalValues[1] = "Additional Parameters 2";
                    //data.AdditionalValues = additionalValues;
                    return JSON.stringify(data);
                }
            },
            columns: [
                {
                    data: "Name",
                    name: "co"
                },
                {
                    data: "Address",
                    name: "co"
                },
                {
                    data: "Phone",
                    name: "co"
                },
                {
                    data: "Email",
                    name: "co"
                },
                {
                    data: "Website",
                    name: "co"
                },
                {
                    data: "CreatedDate",
                    render: function (data, type, row) {
                        if (data)
                            return moment(data).local().format("DD.MM.YYYY HH:mm:ss"); // Formats data from UTC to local time
                        else
                            return null;
                    },
                    name: "gte"
                },
                {
                    data: "ModifiedDate",
                    render: function (data, type, row) {
                        if (data)
                            return moment(data).local().format("DD.MM.YYYY HH:mm:ss");
                        else
                            return null;
                    },
                    name: "gte"
                },
                {
                    data: "IsDeleted",
                    render: function (data, type, row) {
                        if (data)
                            return "Yes";
                        else
                            return "No";
                    }
                },
                {
                    data: "Action",
                    orderable: false,
                    width: 155,
                    render: function (data, type, row) {
                        let s = `<a href="${row.Action.Edit}" class="text-primary">Edit</a> | `;
                        s = s + `<a href="${row.Action.Details}" class="text-primary">Details</a> | `;
                        if (!row.IsDeleted) {
                            s = s + `<a href="#" class="text-primary" data-toggle="modal" data-target="#deleteRestoreModal" data-url="${row.Action.Delete}" data-name="${row.Name}">Delete</a>`;
                        } else {
                            s = s + `<a href="#" class="text-primary" data-toggle="modal" data-target="#deleteRestoreModal" data-url="${row.Action.Restore}" data-name="${row.Name}">Restore</a>`;
                        }
                        return s;
                    }
                }
            ]
        });

        table.columns().every(function (index) {
            $(`#dtVendor thead tr:last th:eq(${index}) input`)
                .on("keyup",
                    function (e) {
                        if (e.keyCode === 13) {
                            table.column($(this).parent().index() + ":visible").search(this.value).draw();
                        }
                    }); // TODO: Add function to refresh when input is cleared
        });
    }
});

// --- PRODUCT ---
$(() => {
    if ($("#dtProduct").length !== 0) {
        const table = $("#dtProduct").DataTable({
            language: {
                processing: "Loading Data...",
                zeroRecords: "No matching records found"
            },
            processing: true,
            serverSide: true,
            orderCellsTop: true,
            autoWidth: true,
            deferRender: true,
            //lengthMenu: [[5, 10, 15, 20, -1], [5, 10, 15, 20, "All"]],
            paging: false,
            dom: '<"row"<"col-sm-12 col-md-6"B><"col-sm-12 col-md-6 text-right"l>><"row"<"col-sm-12"tr>><"row"<"col-sm-12 col-md-5"i><"col-sm-12 col-md-7"p>>',
            buttons: [
                {
                    text: "Export to Excel",
                    className: "btn btn-sm btn-dark",
                    action: function (e, dt, node, config) {
                        window.location.href = "/Home/GetExcel"; // TODO: Change path
                    },
                    init: function (api, node, config) {
                        $(node).removeClass("dt-button");
                    }
                }
            ],
            ajax: {
                type: "POST",
                url: "Product/LoadTable",
                contentType: "application/json; charset=utf-8",
                async: true,
                headers: {
                    "XSRF-TOKEN": document.querySelector('[name="__RequestVerificationToken"]').value
                },
                data: function (data) {
                    //let additionalValues = [];
                    //additionalValues[0] = "Additional Parameters 1";
                    //additionalValues[1] = "Additional Parameters 2";
                    //data.AdditionalValues = additionalValues;
                    return JSON.stringify(data);
                }
            },
            columns: [
                {
                    data: "Name",
                    name: "co"
                },
                {
                    data: "ProductNumber",
                    name: "co"
                },
                {
                    data: "Description",
                    name: "co",
                    visible: false
                },
                {
                    data: "Webpage",
                    name: "co",
                    render: function (data, type, row) {
                        return `<a href="${data}">LINK</a>`;
                    },
                    visible: false
                },
                {
                    data: "ProductType.Name",
                    name: "co"
                },
                {
                    data: "ProductType.Description",
                    name: "co",
                    visible: false
                },
                {
                    data: "ProductType.CreatedDate",
                    name: "co",
                    visible: false
                },
                {
                    data: "ProductType.ModifiedDate",
                    name: "co",
                    visible: false
                },
                {
                    data: "ProductType.IsDeleted",
                    name: "co",
                    visible: false
                },
                {
                    data: "ProductType.Action",
                    name: "co",
                    visible: false
                },
                {
                    data: "Manufacturer.Name",
                    name: "co"
                },
                {
                    data: "Manufacturer.Description",
                    name: "co",
                    visible: false
                },
                {
                    data: "Manufacturer.CreatedDate",
                    name: "co",
                    visible: false
                },
                {
                    data: "Manufacturer.ModifiedDate",
                    name: "co",
                    visible: false
                },
                {
                    data: "Manufacturer.IsDeleted",
                    name: "co",
                    visible: false
                },
                {
                    data: "Manufacturer.Action",
                    name: "co",
                    visible: false
                },
                {
                    data: "CreatedDate",
                    render: function (data, type, row) {
                        if (data)
                            return moment(data).local().format("DD.MM.YYYY HH:mm:ss"); // Formats data from UTC to local time
                        else
                            return null;
                    },
                    name: "gte"
                },
                {
                    data: "ModifiedDate",
                    render: function (data, type, row) {
                        if (data)
                            return moment(data).local().format("DD.MM.YYYY HH:mm:ss");
                        else
                            return null;
                    },
                    name: "gte"
                },
                {
                    data: "IsDeleted",
                    render: function (data, type, row) {
                        if (data)
                            return "Yes";
                        else
                            return "No";
                    }
                },
                {
                    data: "Action",
                    orderable: false,
                    width: 155,
                    render: function (data, type, row) {
                        let s = `<a href="${row.Action.Edit}" class="text-primary">Edit</a> | `;
                        s = s + `<a href="${row.Action.Details}" class="text-primary">Details</a> | `;
                        if (!row.IsDeleted) {
                            s = s + `<a href="#" class="text-primary" data-toggle="modal" data-target="#deleteRestoreModal" data-url="${row.Action.Delete}" data-name="${row.Name}">Delete</a>`;
                        } else {
                            s = s + `<a href="#" class="text-primary" data-toggle="modal" data-target="#deleteRestoreModal" data-url="${row.Action.Restore}" data-name="${row.Name}">Restore</a>`;
                        }
                        return s;
                    }
                }
            ]
        });

        table.columns().every(function (index) {
            $(`#dtVendor thead tr:last th:eq(${index}) input`)
                .on("keyup",
                    function (e) {
                        if (e.keyCode === 13) {
                            table.column($(this).parent().index() + ":visible").search(this.value).draw();
                        }
                    }); // TODO: Add function to refresh when input is cleared
        });

        $("#dtProduct > thead > tr:nth-child(1) > th:nth-child(3)").text("Product Type");
        $("#dtProduct > thead > tr:nth-child(1) > th:nth-child(4)").text("Manufacturer");
    }
});

// --- BUNDLE ---
$(() => {
    if ($("#dtBundle").length !== 0) {
        const table = $("#dtBundle").DataTable({
            language: {
                processing: "Loading Data...",
                zeroRecords: "No matching records found"
            },
            processing: true,
            serverSide: true,
            orderCellsTop: true,
            autoWidth: true,
            deferRender: true,
            //lengthMenu: [[5, 10, 15, 20, -1], [5, 10, 15, 20, "All"]],
            paging: false,
            dom: '<"row"<"col-sm-12 col-md-6"B><"col-sm-12 col-md-6 text-right"l>><"row"<"col-sm-12"tr>><"row"<"col-sm-12 col-md-5"i><"col-sm-12 col-md-7"p>>',
            buttons: [
                {
                    text: "Export to Excel",
                    className: "btn btn-sm btn-dark",
                    action: function (e, dt, node, config) {
                        window.location.href = "/Home/GetExcel"; // TODO: Change path
                    },
                    init: function (api, node, config) {
                        $(node).removeClass("dt-button");
                    }
                }
            ],
            ajax: {
                type: "POST",
                url: "Bundle/LoadTable",
                contentType: "application/json; charset=utf-8",
                async: true,
                headers: {
                    "XSRF-TOKEN": document.querySelector('[name="__RequestVerificationToken"]').value
                },
                data: function (data) {
                    //let additionalValues = [];
                    //additionalValues[0] = "Additional Parameters 1";
                    //additionalValues[1] = "Additional Parameters 2";
                    //data.AdditionalValues = additionalValues;
                    return JSON.stringify(data);
                }
            },
            columns: [
                {
                    data: "Name",
                    name: "co"
                },
                {
                    data: "SerialNumber",
                    name: "co"
                },
                {
                    data: "NumberOfUnits",
                    name: "gte"
                },
                {
                    data: "CreatedDate",
                    render: function (data, type, row) {
                        if (data)
                            return moment(data).local().format("DD.MM.YYYY HH:mm:ss"); // Formats data from UTC to local time
                        else
                            return null;
                    },
                    name: "gte"
                },
                {
                    data: "ModifiedDate",
                    render: function (data, type, row) {
                        if (data)
                            return moment(data).local().format("DD.MM.YYYY HH:mm:ss");
                        else
                            return null;
                    },
                    name: "gte"
                },
                {
                    data: "IsDeleted",
                    render: function (data, type, row) {
                        if (data)
                            return "Yes";
                        else
                            return "No";
                    }
                },
                {
                    data: "Action",
                    orderable: false,
                    width: 155,
                    render: function (data, type, row) {
                        let s = `<a href="${row.Action.Edit}" class="text-primary">Edit</a> | `;
                        s = s + `<a href="${row.Action.Details}" class="text-primary">Details</a> | `;
                        if (!row.IsDeleted) {
                            s = s + `<a href="#" class="text-primary" data-toggle="modal" data-target="#deleteRestoreModal" data-url="${row.Action.Delete}" data-name="${row.Name}">Delete</a>`;
                        } else {
                            s = s + `<a href="#" class="text-primary" data-toggle="modal" data-target="#deleteRestoreModal" data-url="${row.Action.Restore}" data-name="${row.Name}">Restore</a>`;
                        }
                        return s;
                    }
                }
            ]
        });

        table.columns().every(function (index) {
            $(`#dtBundle thead tr:last th:eq(${index}) input`)
                .on("keyup",
                    function (e) {
                        if (e.keyCode === 13) {
                            table.column($(this).parent().index() + ":visible").search(this.value).draw();
                        }
                    }); // TODO: Add function to refresh when input is cleared
        });
    }
});

// --- UNIT ---
$(() => {
    if ($("#dtUnit").length !== 0) {
        const table = $("#dtUnit").DataTable({
            language: {
                processing: "Loading Data...",
                zeroRecords: "No matching records found"
            },
            processing: true,
            serverSide: true,
            orderCellsTop: true,
            autoWidth: true,
            deferRender: true,
            //lengthMenu: [[5, 10, 15, 20, -1], [5, 10, 15, 20, "All"]],
            paging: false,
            dom: '<"row"<"col-sm-12 col-md-6"B><"col-sm-12 col-md-6 text-right"l>><"row"<"col-sm-12"tr>><"row"<"col-sm-12 col-md-5"i><"col-sm-12 col-md-7"p>>',
            buttons: [
                {
                    text: "Export to Excel",
                    className: "btn btn-sm btn-dark",
                    action: function (e, dt, node, config) {
                        window.location.href = "/Home/GetExcel"; // TODO: Change path
                    },
                    init: function (api, node, config) {
                        $(node).removeClass("dt-button");
                    }
                }
            ],
            ajax: {
                type: "POST",
                url: "Unit/LoadTable",
                contentType: "application/json; charset=utf-8",
                async: true,
                headers: {
                    "XSRF-TOKEN": document.querySelector('[name="__RequestVerificationToken"]').value
                },
                data: function (data) {
                    //let additionalValues = [];
                    //additionalValues[0] = "Additional Parameters 1";
                    //additionalValues[1] = "Additional Parameters 2";
                    //data.AdditionalValues = additionalValues;
                    return JSON.stringify(data);
                }
            },
            columns: [
                {
                    data: "SerialNumber",
                    name: "co"
                },
                {
                    data: "Product.Name",
                    name: "co"
                },
                {
                    data: "Product.ProductNumber",
                    name: "co"
                },
                {
                    data: "Product.Description",
                    name: "co",
                    visible: false
                },
                {
                    data: "Product.Webpage",
                    name: "co",
                    visible: false
                },
                {
                    data: "Product.ProductType.Name",
                    name: "co"
                },
                {
                    data: "Product.ProductType.Description",
                    name: "co",
                    visible: false
                },
                {
                    data: "Product.ProductType.CreatedDate",
                    name: "co",
                    visible: false
                },
                {
                    data: "Product.ProductType.ModifiedDate",
                    name: "co",
                    visible: false
                },
                {
                    data: "Product.ProductType.IsDeleted",
                    name: "co",
                    visible: false
                },
                {
                    data: "Product.ProductType.Action",
                    name: "co",
                    orderable: false,
                    visible: false
                },
                {
                    data: "Product.Manufacturer.Name",
                    name: "co"
                },
                {
                    data: "Product.Manufacturer.Description",
                    name: "co",
                    visible: false
                },
                {
                    data: "Product.Manufacturer.CreatedDate",
                    name: "co",
                    visible: false
                },
                {
                    data: "Product.Manufacturer.ModifiedDate",
                    name: "co",
                    visible: false
                },
                {
                    data: "Product.Manufacturer.IsDeleted",
                    name: "co",
                    visible: false
                },
                {
                    data: "Product.Manufacturer.Action",
                    name: "co",
                    orderable: false,
                    visible: false
                },
                {
                    data: "Product.CreatedDate",
                    name: "co",
                    visible: false
                },
                {
                    data: "Product.ModifiedDate",
                    name: "co",
                    visible: false
                },
                {
                    data: "Product.IsDeleted",
                    name: "co",
                    visible: false
                },
                {
                    data: "Product.Action",
                    name: "co",
                    orderable: false,
                    visible: false
                },
                {
                    data: "Location.Name",
                    name: "co"
                },
                {
                    data: "DefaultLocation.Name",
                    name: "co"
                },
                {
                    data: "Location.Description",
                    name: "co",
                    visible: false
                },
                {
                    data: "DefaultLocation.Description",
                    name: "co",
                    visible: false
                },
                {
                    data: "Location.Address",
                    name: "co",
                    visible: false
                },
                {
                    data: "DefaultLocation.Address",
                    name: "co",
                    visible: false
                },
                {
                    data: "Location.LocationType.Name",
                    name: "co",
                    visible: false
                },
                {
                    data: "DefaultLocation.LocationType.Name",
                    name: "co",
                    visible: false
                },
                {
                    data: "Location.LocationType.Description",
                    name: "co",
                    visible: false
                },
                {
                    data: "DefaultLocation.LocationType.Description",
                    name: "co",
                    visible: false
                },
                {
                    data: "Location.LocationType.CreatedDate",
                    name: "co",
                    visible: false
                },
                {
                    data: "DefaultLocation.LocationType.CreatedDate",
                    name: "co",
                    visible: false
                },
                {
                    data: "Location.LocationType.ModifiedDate",
                    name: "co",
                    visible: false
                },
                {
                    data: "DefaultLocation.LocationType.ModifiedDate",
                    name: "co",
                    visible: false
                },
                {
                    data: "Location.LocationType.IsDeleted",
                    name: "co",
                    visible: false
                },
                {
                    data: "DefaultLocation.LocationType.IsDeleted",
                    name: "co",
                    visible: false
                },
                {
                    data: "Location.LocationType.Action",
                    name: "co",
                    orderable: false,
                    visible: false
                },
                {
                    data: "DefaultLocation.LocationType.Action",
                    name: "co",
                    orderable: false,
                    visible: false
                },
                {
                    data: "Location.CreatedDate",
                    name: "co",
                    visible: false
                },
                {
                    data: "DefaultLocation.CreatedDate",
                    name: "co",
                    visible: false
                },
                {
                    data: "Location.ModifiedDate",
                    name: "co",
                    visible: false
                },
                {
                    data: "DefaultLocation.ModifiedDate",
                    name: "co",
                    visible: false
                },
                {
                    data: "Location.IsDeleted",
                    name: "co",
                    visible: false
                },
                {
                    data: "DefaultLocation.IsDeleted",
                    name: "co",
                    visible: false
                },
                {
                    data: "Location.Action",
                    name: "co",
                    orderable: false,
                    visible: false
                },
                {
                    data: "DefaultLocation.Action",
                    name: "co",
                    orderable: false,
                    visible: false
                },
                {
                    data: "ShelfNumber",
                    name: "co",
                    visible: true,
                    render: function (data, type, row) {
                        if (data)
                            return data;
                        else
                            return null;
                    }
                },
                {
                    data: "Vendor.Name",
                    render: function (data, type, row) {
                        if (data)
                            return data;
                        else
                            return null;
                    },
                    name: "co",
                    visible: false
                },
                {
                    data: "Vendor.Address",
                    render: function (data, type, row) {
                        if (data)
                            return data;
                        else
                            return null;
                    },
                    name: "co",
                    visible: false
                },
                {
                    data: "Vendor.Phone",
                    render: function (data, type, row) {
                        if (data)
                            return data;
                        else
                            return null;
                    },
                    name: "co",
                    visible: false
                },
                {
                    data: "Vendor.Email",
                    render: function (data, type, row) {
                        if (data)
                            return data;
                        else
                            return null;
                    },
                    name: "co",
                    visible: false
                },
                {
                    data: "Vendor.Website",
                    render: function (data, type, row) {
                        if (data)
                            return data;
                        else
                            return null;
                    },
                    name: "co",
                    visible: false
                },
                {
                    data: "Vendor.CreatedDate",
                    render: function (data, type, row) {
                        if (data)
                            return data;
                        else
                            return null;
                    },
                    name: "gte",
                    visible: false
                },
                {
                    data: "Vendor.ModifiedDate",
                    render: function (data, type, row) {
                        if (data)
                            return data;
                        else
                            return null;
                    },
                    name: "gte",
                    visible: false
                },
                {
                    data: "Vendor.IsDeleted",
                    render: function (data, type, row) {
                        if (data)
                            return data;
                        else
                            return null;
                    },
                    name: "eq",
                    visible: false
                },
                {
                    data: "Vendor.Action",
                    render: function (data, type, row) {
                        if (data)
                            return data;
                        else
                            return null;
                    },
                    orderable: false,
                    visible: false
                },
                {
                    data: "PartOfBundle.Name",
                    name: "co",
                    render: function (data, type, row) {
                        if (data)
                            return data;
                        else
                            return null;
                    },
                },
                {
                    data: "PartOfBundle.SerialNumber",
                    name: "co",
                    visible: false,
                    render: function (data, type, row) {
                        if (data)
                            return data;
                        else
                            return null;
                    },
                },
                {
                    data: "PartOfBundle.NumberOfUnits",
                    name: "co",
                    visible: false,
                    render: function (data, type, row) {
                        if (data)
                            return data;
                        else
                            return null;
                    }
                },
                {
                    data: "PartOfBundle.CreatedDate",
                    name: "co",
                    visible: false,
                    render: function (data, type, row) {
                        if (data)
                            return data;
                        else
                            return null;
                    }
                },
                {
                    data: "PartOfBundle.ModifiedDate",
                    name: "co",
                    visible: false,render: function (data, type, row) {
                        if (data)
                            return data;
                        else
                            return null;
                    }
                },
                {
                    data: "PartOfBundle.IsDeleted",
                    name: "co",
                    visible: false,
                    render: function (data, type, row) {
                        if (data)
                            return data;
                        else
                            return null;
                    }
                },
                {
                    data: "PartOfBundle.Action",
                    name: "co",
                    visible: false,
                    orderable: false,
                    render: function (data, type, row) {
                        if (data)
                            return data;
                        else
                            return null;
                    }
                },
                {
                    data: "Notes",
                    name: "co",
                    visible: true,
                    render: function (data, type, row) {
                        if (data)
                            return data;
                        else
                            return null;
                    }
                },
                {
                    data: "LastTransferTime",
                    name: "co",
                    visible: true,
                    render: function (data, type, row) {
                        if (data)
                            return moment(data).local().format("DD.MM.YYYY HH:mm:ss");
                        else
                            return null;
                    }
                },
                {
                    data: "LastCheckTime",
                    name: "co",
                    visible: true,
                    render: function (data, type, row) {
                        if (data)
                            return moment(data).local().format("DD.MM.YYYY HH:mm:ss");
                        else
                            return null;
                    }
                },
                {
                    data: "CreatedDate",
                    render: function (data, type, row) {
                        if (data)
                            return moment(data).local().format("DD.MM.YYYY HH:mm:ss"); // Formats data from UTC to local time
                        else
                            return null;
                    },
                    name: "gte",
                    visible: false
                },
                {
                    data: "ModifiedDate",
                    render: function (data, type, row) {
                        if (data)
                            return moment(data).local().format("DD.MM.YYYY HH:mm:ss");
                        else
                            return null;
                    },
                    name: "gte",
                    visible: false
                },
                {
                    data: "IsDeleted",
                    render: function (data, type, row) {
                        if (data)
                            return "Yes";
                        else
                            return "No";
                    },
                    visible: false
                },
                {
                    data: "Action",
                    orderable: false,
                    width: 155,
                    render: function (data, type, row) {
                        let s = `<a href="${row.Action.Edit}" class="text-primary">Edit</a> | `;
                        s = s + `<a href="${row.Action.Details}" class="text-primary">Details</a> | `;
                        if (!row.IsDeleted) {
                            s = s + `<a href="#" class="text-primary" data-toggle="modal" data-target="#deleteRestoreModal" data-url="${row.Action.Delete}" data-name="${row.Name}">Delete</a>`;
                        } else {
                            s = s + `<a href="#" class="text-primary" data-toggle="modal" data-target="#deleteRestoreModal" data-url="${row.Action.Restore}" data-name="${row.Name}">Restore</a>`;
                        }
                        return s;
                    }
                }
            ]
        });

        table.columns().every(function (index) {
            $(`#dtUnit thead tr:last th:eq(${index}) input`)
                .on("keyup",
                    function (e) {
                        if (e.keyCode === 13) {
                            table.column($(this).parent().index() + ":visible").search(this.value).draw();
                        }
                    }); // TODO: Add function to refresh when input is cleared
        });

        $("#dtUnit > thead > tr:nth-child(1) > th:nth-child(2)").text("Product");
        $("#dtUnit > thead > tr:nth-child(1) > th:nth-child(4)").text("Product Type");
        $("#dtUnit > thead > tr:nth-child(1) > th:nth-child(5)").text("Manufacturer");
        $("#dtUnit > thead > tr:nth-child(1) > th:nth-child(6)").text("Location");
        $("#dtUnit > thead > tr:nth-child(1) > th:nth-child(7)").text("Default Location");
        $("#dtUnit > thead > tr:nth-child(1) > th:nth-child(9)").text("Bundle");
    }
});