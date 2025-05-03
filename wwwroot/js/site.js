let quotes = [];
let books = [];

async function fetchAndSetBooks() {
    const response = await fetch('api/book', {
        method: 'GET',
        content: 'application/json'
    })
    const data = await response.json();
    books = data;
}

async function fetchAndSetQuotes() {
    const response = await fetch('api/quote', {
        method: 'GET',
        content: 'application/json'
    })
    const data = await response.json();
    quotes = data;
}

function renderBooks() {
    const bookHeaders = books.map(book => {
        const display = `
            <div id=${`book${book.id}`} class="d-flex justify-content-between">
                <h2>${book.title}</h2>
                <h2>${book.author}</h2>
            </div>`

        return display;
    }).join('');

    $('#quotes').append(bookHeaders);
}

function renderQuotes() {
    quotes.forEach(quote => {
        const bookId = quote.bookId;

        const topicsDisplay = quote.topics.map(topic => {
            const display = `<p>${topic}</p>`;
            return display;
        }).join('');

        const display = `
            <div id=${quote.id} class="d-flex quote-row">
            <div class="col-3 text-start ps-1">
                <p>${quote.chapter} : ${quote.verse}</p>
                <p>${quote.page}</p>
            </div>
            <p class="col-6 text-start">${quote.text} by ${quote.person}</p>
            <div class="col-3 text-start">
                ${topicsDisplay}
            </div>
        </div>`

        $(`book${bookId}`).append(display);
    });

}


$(document).ready(async function () {
    await fetchAndSetBooks();
    await fetchAndSetQuotes();
    renderBooks();
    renderQuotes();
}
)