// Tippy Initialization
tippy(document.querySelectorAll(".tippy"));

// Select2
$(document).ready(function() { // TODO: Move when select2 moved to separate partial
    $(".selectTwo").select2({
        theme: "bootstrap4"
    }); 
    $(".selectTwoAllowClear").select2({
        theme: "bootstrap4",
        allowClear: true
    });
    $(".selectTwoTransferUnits").select2({
        theme: "bootstrap4",
        ajax: {
            url: "/Transfer/UnitLoc",
            dataType: "json",
            data: function(params) {
                var query = {
                    loc: $("#OriginLocationId :selected").val(),
                    sn: params.term
                };
                return query;
            },
            delay: 250
            // TODO: change container look (ex https://select2.org/data-sources/ajax)
        }
    });
});

// --- MODALS ---
// Function for filling modal window for deleting/restoring with appropriate data
$("#deleteRestoreModal").on("show.bs.modal", function(event) {
    const url = $(event.relatedTarget).data("url");
    const name = $(event.relatedTarget).data("name");
    const array = url.split("/");
    let action = array[2];
    if (!(action === "Delete" || action === "Restore")) {
        action = array[3]; // For Actions in Areas
    }

    $(this).find(".modal-header h5").text(action); // Changes action name (e.g. Delete/Restore) in header of modal window 
    $(this).find(".modal-body span").text(action.toLowerCase()); // Changes action name in body of modal window 
    $(this).find(".modal-body b").text(name); // Changes name of the item
    $(this).find(".modal-footer .btn-danger").text(action); // Change action name on the button
    $(this).find(".modal-footer form").attr("action", url); // Change action to proper route
});

// Shows error modal window when loaded in partial
$(function () {
    $("#errorModal").modal("show");
});