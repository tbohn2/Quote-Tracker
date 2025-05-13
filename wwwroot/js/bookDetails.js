const bookId = $('#book-details').data("id");
let title = $('#book-header').text();
let author = $('#author').text() || '';
let editing = false;
let newTitle = $('#book-header').text();
let newAuthor = $('#author').text() || '';

async function saveTitle() {
    const bookToUpdate = {
        Id: bookId,
        Title: newTitle,
        Author: newAuthor
    }

    try {
        const response = await fetch('/api/book/', {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(bookToUpdate)
        });

        const result = await response.json();

        if (!response.ok) {
            console.error('Update failed:', result);
        } else {
            console.log('Update successful:', result);
            title = newTitle;
            author = newAuthor;
            toggleEdit();
        }
    } catch (error) {
        console.error('Fetch error:', error);
    }
}

async function deleteBook() {
    try {
        const response = await fetch(`/api/book/${bookId}`, {
            method: 'DELETE',
        });

        if (!response.ok) {
            console.error('Delete failed');
        } else {
            console.log('Delete successful');
            $("#book-buttons").remove();
            $("#book-details").remove();
        }
    } catch (error) {
        console.error('Fetch error:', error);
    }
}

function toggleEdit() {
    editing = !editing;

    if (editing) {
        $("#toggle-title-edit").html("Cancel Edit").removeClass("primary").addClass("secondary");
        const saveButton = `<button id="save-title" class="button success my-2 fs-5 col-3">Save</button>`
        const deleteButton = `<button id="delete-book" class="button danger my-2 fs-5 col-3">Delete Book</button>`
        $("#toggle-title-edit").before(saveButton);
        $("#toggle-title-edit").after(deleteButton);
        $("#save-title").on('click', saveTitle)
        $("#delete-book").on('click', deleteBook)

        const titleInput = `<input id="title-input" class="toggle-hidden" type="text" value="${newTitle}" />`
        const authorInput = `<input id="author-input" class="toggle-hidden" type="text" placeholder="Author name if present" value="${newAuthor}" />`
        $('#book-header').after(authorInput);
        $('#book-header').after(titleInput);

        $('#title-input').on('change', (e) => { newTitle = e.target.value })
        $('#author-input').on('change', (e) => { newAuthor = e.target.value })

        $('#book-header').hide();
        $('#author').hide();
    } else {
        $("#toggle-title-edit").html("Edit Book").removeClass("secondary").addClass("primary");
        $("#save-title").remove();
        $("#delete-book").remove();
        $('#title-input').remove();
        $('#author-input').remove();

        $('#book-header').show().text(title);
        if (author) {
            $('#author').show().text(author);
            if (!$('#author').html()) {
                $("#book-header").after(`<h2 id="author">${author}</h2>`);
            }
        } else {
            $("#author").remove();
        }

        newTitle = $('#book-header').text();
        newAuthor = $('#author').text() || '';
    };
};

$("#toggle-title-edit").on('click', toggleEdit);