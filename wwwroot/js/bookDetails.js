import { saveNew, deleteObject, toggleEdit } from "./bookTopicOperations.js";

const bookId = $('#book-details').data("id");
let title = $('#book-header').text();
let author = $('#author').text() || '';
let editing = false;
let newTitle = $('#book-header').text();
let newAuthor = $('#author').text() || '';

function resetState() {
    title = $('#book-header').text();
    author = $('#author').text() || '';
    editing = false;
    newTitle = $('#book-header').text();
    newAuthor = $('#author').text() || '';
}

function saveTitle() {
    const bookToUpdate = {
        Id: bookId,
        Title: newTitle,
        Author: newAuthor
    };

    saveNew('/api/book/', bookToUpdate, () => {
        $('#book-header').text(newTitle);
        $('#author').text(newAuthor);

        toggleBookEdit();
        resetState();
    });
};

function toggleBookEdit() {
    editing = !editing;
    toggleEdit(editing);

    if (editing) {
        $('#book-header').hide();
        $('#author').hide();

        $('#title-input').fadeIn();
        $('#author-input').fadeIn();
    } else {
        $('#title-input').hide();
        $('#author-input').hide();

        $('#book-header').fadeIn();
        if (author) {
            $('#author').fadeIn().text(author);
            if (!$('#author').html()) {
                $("#book-header").after(`<h2 id="author">${author}</h2>`);
            }
        } else {
            $("#author").remove();
        };

        resetState();
    };
};

$('#title-input').on('change', (e) => { newTitle = e.target.value });
$('#author-input').on('change', (e) => { newAuthor = e.target.value });
$("#save").on('click', saveTitle);
$("#delete").on('click', () => deleteObject(`/api/book/${bookId}`, "/BookPage"));
$("#toggle-edit").on('click', toggleBookEdit);