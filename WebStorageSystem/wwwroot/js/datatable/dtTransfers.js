// --- TRANSFER ---
$(() => {
    if ($("#dtTransfer").length !== 0) {
        const table = $("#dtTransfer").DataTable({
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
            dom:
                '<"row"<"col-sm-12 col-md-6"B><"col-sm-12 col-md-6 text-right"l>><"row"<"col-sm-12"tr>><"row"<"col-sm-12 col-md-5"i><"col-sm-12 col-md-7"p>>',
            buttons: [
                {
                    text: "Export to Excel",
                    className: "btn btn-sm btn-dark",
                    action: function(e, dt, node, config) {
                        window.location.href = "/Home/GetExcel"; // TODO: Change path
                    },
                    init: function(api, node, config) {
                        $(node).removeClass("dt-button");
                    }
                }
            ],
            ajax: {
                type: "POST",
                url: "Transfer/LoadTable",
                contentType: "application/json; charset=utf-8",
                async: true,
                headers: {
                    "XSRF-TOKEN": document.querySelector('[name="__RequestVerificationToken"]').value
                },
                data: function(data) {
                    //let additionalValues = [];
                    //additionalValues[0] = "Additional Parameters 1";
                    //additionalValues[1] = "Additional Parameters 2";
                    //data.AdditionalValues = additionalValues;
                    return JSON.stringify(data);
                }
            },
            columns: [
                {
                    data: "TransferNumber",
                    name: "co"
                },
                {
                    data: "OriginLocation.Name",
                    name: "gte"
                },
                {
                    data: "DestinationLocation.Name",
                    name: "gte"
                },
                {
                    data: "OriginLocation.Description",
                    name: "co",
                    visible: false
                },
                {
                    data: "OriginLocation.LocationType.Name",
                    name: "co",
                    visible: false
                },
                {
                    data: "OriginLocation.LocationType.Description",
                    name: "co",
                    visible: false
                },
                {
                    data: "OriginLocation.LocationType.CreatedDate",
                    name: "co",
                    visible: false
                },
                {
                    data: "OriginLocation.LocationType.ModifiedDate",
                    name: "co",
                    visible: false
                },
                {
                    data: "OriginLocation.LocationType.IsDeleted",
                    name: "co",
                    visible: false
                },
                {
                    data: "OriginLocation.LocationType.Action",
                    name: "co",
                    visible: false
                },
                {
                    data: "OriginLocation.CreatedDate",
                    name: "co",
                    visible: false
                },
                {
                    data: "OriginLocation.ModifiedDate",
                    name: "co",
                    visible: false
                },
                {
                    data: "OriginLocation.IsDeleted",
                    name: "co",
                    visible: false
                },
                {
                    data: "OriginLocation.Action",
                    name: "co",
                    visible: false
                },
                {
                    data: "DestinationLocation.Description",
                    name: "co",
                    visible: false
                },
                {
                    data: "DestinationLocation.LocationType.Name",
                    name: "co",
                    visible: false
                },
                {
                    data: "DestinationLocation.LocationType.Description",
                    name: "co",
                    visible: false
                },
                {
                    data: "DestinationLocation.LocationType.CreatedDate",
                    name: "co",
                    visible: false
                },
                {
                    data: "DestinationLocation.LocationType.ModifiedDate",
                    name: "co",
                    visible: false
                },
                {
                    data: "DestinationLocation.LocationType.IsDeleted",
                    name: "co",
                    visible: false
                },
                {
                    data: "DestinationLocation.LocationType.Action",
                    name: "co",
                    visible: false
                },
                {
                    data: "DestinationLocation.CreatedDate",
                    name: "co",
                    visible: false
                },
                {
                    data: "DestinationLocation.ModifiedDate",
                    name: "co",
                    visible: false
                },
                {
                    data: "DestinationLocation.IsDeleted",
                    name: "co",
                    visible: false
                },
                {
                    data: "DestinationLocation.Action",
                    name: "co",
                    visible: false
                },
                {
                    data: "State",
                    name: "co"
                },
                {
                    data: "Units",
                    render: function(data, type, row) {
                        let s = "";
                        data.forEach(elem => {
                            s += elem.SerialNumber + "<br />";
                        });
                        return s;
                    }
                },
                {
                    data: "TransferTime",
                    render: function (data, type, row) {
                        if (data)
                            return moment(data).local().format("DD.MM.YYYY HH:mm:ss");
                        else
                            return null;
                    },
                    name: "co"
                },
                {
                    data: "User.UserName",
                    name: "co"
                },
                {
                    data: "User.IsAdmin",
                    name: "eq",
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
                            s = s + `<a href="#" class="text-primary" data-toggle="modal" data-target="#deleteRestoreModal" data-url="${row.Action.Delete}" data-name="${row.TransferNumber}">Delete</a>`;
                        } else {
                            s = s + `<a href="#" class="text-primary" data-toggle="modal" data-target="#deleteRestoreModal" data-url="${row.Action.Restore}" data-name="${row.TransferNumber}">Restore</a>`;
                        }
                        return s;
                    }
                }
            ]
        });

        table.columns().every(function (index) {
            $(`#dtTransfer thead tr:last th:eq(${index}) input`)
                .on("keyup",
                    function (e) {
                        if (e.keyCode === 13) {
                            table.column($(this).parent().index() + ":visible").search(this.value).draw();
                        }
                    }); // TODO: Add function to refresh when input is cleared
        });

        $("#dtTransfer > thead > tr:nth-child(1) > th:nth-child(2)").text("Origin Location");
        $("#dtTransfer > thead > tr:nth-child(1) > th:nth-child(3)").text("Destination Location");
    }
});