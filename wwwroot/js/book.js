
let draggedItem = null;
let start = 0;
let newBook = {
    Title: "",
    Author: "",
    PriorityIndex: 0
}

async function saveChangedReorder() {
    const bookOrder = [];

    $('#book-list .book-item').each(function (i) {
        if (i >= start) {
            bookOrder.push({
                Id: $(this).data('id'),
                PriorityIndex: i
            });
        }
    });

    const response = await fetch('api/book/reorder', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(bookOrder)
    })

    const message = await response.text();
    console.log(message);

    $('#save-reorder').remove();
}

async function saveNewBook(e) {
    e.preventDefault()

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
            console.log(data);
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

$('#create-book-form').on('change', updateForm);
$('#create-book-form').on('submit', saveNewBook);

$('#book-list').on('dragstart', '.book-item', function (e) {
    draggedItem = $(this);
    draggedItem.css('opacity', 0.5);
    $('#save-reorder').remove();
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
        $('#books').append(`<button id="save-reorder" class="button primary my-3">Save Changes</button>`)

        $('#save-reorder').on('click', saveChangedReorder)

        const draggedIndex = draggedItem.index();
        const targetIndex = $(this).index();

        if (draggedIndex < targetIndex) {
            $(this).after(draggedItem);
        } else {
            $(this).before(draggedItem);
        }
    }
});