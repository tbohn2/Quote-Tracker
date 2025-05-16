
let draggedItem = null;
let start = $("#book-list .book-item").length - 1;
let end = 0;
let newBook = {
    Title: "",
    Author: "",
    PriorityIndex: 0
}

async function saveChangedReorder() {
    const booksToReorder = [];

    for (let i = start; i < end + 1; i++) {
        const id = $("#book-list .book-item").eq(i).data("id");

        booksToReorder.push({
            Id: id,
            PriorityIndex: i,
        });
    }

    try {
        const response = await fetch('api/book/reorder', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(booksToReorder)
        })

        const data = await response.json();
        if (response.ok) {
            $('#save-reorder').remove();
            $('#book-list').remove();
            renderBookList(data);
            start = 0;
            end = $("#book-list .book-item").length - 1;
        } else {
            console.error(data);
        }
    } catch (error) {
        console.error(error);
    }
}

async function saveNewBook(e) {
    e.preventDefault()

    console.log(newBook);

    try {
        const response = await fetch('api/book/', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(newBook)
        })

        const data = await response.json();
        if (!response.ok) {
            console.log(data);
        } else {
            $('#addBook').modal('hide');
            resetFormState();
            $('#book-list').remove();
            renderBookList(data);
        }
    } catch (error) {
        console.error(error)
    }
}

function updateForm(e) {
    const name = e.target.name;
    const newValue = name === "PriorityIndex" ? parseInt(e.target.value) : e.target.value;
    newBook[name] = newValue;
};

function resetFormState() {
    newBook = {
        Title: "",
        Author: "",
        PriorityIndex: 0
    };

    $('input[name="Title"]').val("");
    $('input[name="Title"]').val("");
    $('select[name="PriorityIndex"]').val("0");
}

function renderBookList(books) {
    const booksDisplay = `
    <ul id="book-list" class="col-6 p-0">
        ${books.map(book =>
        `<li class="book-item text-center p-2" data-id="${book.id}" data-pi="${book.priorityIndex}" draggable="true">
            <a href="/BookPage/Details/${book.id}">
               ${book.title}
            </a>
        </li>`).join('')}
    </ul>
    `

    $('#books').append(booksDisplay);
    applyDragEventListeners();
};

$('#create-book-form').on('change', updateForm);
$('#create-book-form').on('submit', saveNewBook);
$('#close-book-modal').on('click', resetFormState);

function applyDragEventListeners() {
    $('#book-list').on('dragstart', '.book-item', function (e) {
        draggedItem = $(this);
        draggedItem.css('opacity', 0.5);
    });

    $('#book-list').on('dragend', '.book-item', function (e) {
        $(this).css('opacity', '');
    });

    $('#book-list').on('dragover', function (e) {
        e.preventDefault();
    });

    $('#book-list').on('drop', '.book-item', function (e) {
        e.preventDefault();

        if (draggedItem[0] !== this) {
            if (!$('#save-reorder').html()) {
                $('#books').append(`<button id="save-reorder" class="button primary my-3">Save Changes</button>`)
                $('#save-reorder').on('click', saveChangedReorder)
            }

            const draggedIndex = $("#book-list .book-item").index($(draggedItem));
            const targetIndex = $("#book-list .book-item").index($(this));

            start = Math.min(Math.min(draggedIndex, targetIndex), start);
            end = Math.max(Math.max(draggedIndex, targetIndex), end);

            if (draggedIndex < targetIndex) {
                $(this).after(draggedItem);
            } else {
                $(this).before(draggedItem);
            }
        }
    });
}

applyDragEventListeners();