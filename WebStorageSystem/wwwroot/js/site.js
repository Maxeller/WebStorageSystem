// Tippy Initialization
tippy(document.querySelectorAll(".tippy"));

// --- MODALS ---
// Function for filling modal window for deleting/restoring with appropriate data
$("#deleteRestoreModal").on("show.bs.modal", function(event) {
    const url = $(event.relatedTarget).data("url");
    const name = $(event.relatedTarget).data("name");
    const array = url.split("/");
    const action = array[2];

    $(this).find(".modal-header h5").text(action); // Changes action name (e.g. Delete/Restore) in header of modal window 
    $(this).find(".modal-body span").text(action.toLowerCase()); // Changes action name in body of modal window 
    $(this).find(".modal-body b").text(name); // Changes name of the item
    $(this).find(".modal-footer .btn-danger").text(action); // Change action name on the button
    $(this).find(".modal-footer form").attr("action", url); // Change action to proper route
});

// Shows error modal window when loaded in partial
$(function () {
    $('#errorModal').modal('show');
});