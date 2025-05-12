let editing = false;
const bookId = $('#book-details').data("id");

function toggleEdit() {
    editing = !editing;

    if (editing) {
        $("#toggle-title-edit").html("Cancel Edit").removeClass("primary").addClass("secondary");
        const saveButton = `<button id="save-title" class="button success my-2 fs-5">Save New Title</button>`
        $("#toggle-title-edit").before(saveButton);
        $("#save-title").on('click', saveTitle)

        const title = $('#book-header').text();
        const input = `<input id="title-input" class="toggle-hidden" type="text" value="${title}" />`
        $('#book-header').after(input);
        $('#book-header').hide();
    } else {
        $("#toggle-title-edit").html("Edit Book").removeClass("secondary").addClass("primary");
        $("#save-title").remove();
        $('#title-input').remove();
        $('#book-header').show();
    }
}

$("#toggle-title-edit").on('click', toggleEdit);