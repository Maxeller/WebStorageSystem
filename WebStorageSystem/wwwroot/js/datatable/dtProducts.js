﻿// --- MANUFACTURER ---
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

        $("#dtProduct > thead > tr:nth-child(1) > th:nth-child(2)").text("Product Type");
        $("#dtProduct > thead > tr:nth-child(1) > th:nth-child(3)").text("Manufacturer");
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
                    data: "Name",
                    name: "co"
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

        //$("#dtProduct > thead > tr:nth-child(1) > th:nth-child(2)").text("Product Type");
        //$("#dtProduct > thead > tr:nth-child(1) > th:nth-child(3)").text("Manufacturer");
    }
});