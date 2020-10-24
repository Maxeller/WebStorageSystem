// --- LOCATION TYPE ---
$(() => {
    if ($("#dtLocationType").length !== 0) {
        var table = $("#dtLocationType").DataTable({
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
                },
                {
                    text: "createText",
                    className: "btn btn-sm btn-success",
                    action: function (e, dt, node, config) {
                        $("#createModal").modal("show");
                    },
                    init: function (api, node, config) {
                        $(node).removeClass("dt-button");
                    }
                }
            ],
            ajax: {
                type: "POST",
                url: "LocationType/LoadTable",
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
            $("#dtLocationType thead tr:last th:eq(" + index + ") input")
                .on("keyup",
                    function(e) {
                        if (e.keyCode === 13) {
                            table.column($(this).parent().index() + ":visible").search(this.value).draw();
                        }
                    }); // TODO: Add function to refresh when input is cleared
        });

        $(document)
            .off("click", "#btnCreate")
            .on("click", "#btnCreate", function () {
                fetch("/Home/Create/",
                    {
                        method: "POST",
                        cache: "no-cache",
                        body: new URLSearchParams(new FormData(document.querySelector("#frmCreate")))
                    })
                    .then((response) => {
                        table.ajax.reload();
                        $("#createModal").modal("hide");
                        document.querySelector("#frmCreate").reset();
                    })
                    .catch((error) => {
                        console.log(error);
                    });
            });

        $(document)
            .off("click", ".btnEdit")
            .on("click", ".btnEdit", function () {
                const id = $(this).attr("data-key");

                fetch(`/Home/Edit/${id}`,
                    {
                        method: "GET",
                        cache: "no-cache"
                    })
                    .then((response) => {
                        return response.text();
                    })
                    .then((result) => {
                        $("#editPartial").html(result);
                        $("#editModal").modal("show");
                    })
                    .catch((error) => {
                        console.log(error);
                    });
            });

        $(document)
            .off("click", "#btnUpdate")
            .on("click", "#btnUpdate", function () {
                fetch("/Home/Edit/",
                    {
                        method: "PUT",
                        cache: "no-cache",
                        body: new URLSearchParams(new FormData(document.querySelector("#frmEdit")))
                    })
                    .then((response) => {
                        table.ajax.reload();
                        $("#editModal").modal("hide");
                        $("#editPartial").html("");
                    })
                    .catch((error) => {
                        console.log(error);
                    });
            });

        $(document)
            .off("click", ".btnDelete")
            .on("click", ".btnDelete", function () {
                const id = $(this).attr("data-key");

                if (confirm("Are you sure?")) {
                    fetch(`/Home/Delete/${id}`,
                        {
                            method: "DELETE",
                            cache: "no-cache"
                        })
                        .then((response) => {
                            table.ajax.reload();
                        })
                        .catch((error) => {
                            console.log(error);
                        });
                }
            });

        $("#btnExternalSearch").click(function () {
            table.column("0:visible").search($("#txtExternalSearch").val()).draw();
        });
    }
});