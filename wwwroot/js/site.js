let sortByBook = true;
let quotes = [];
let dataByBook = [];
let dataByTopic = [];

async function fetchAndSetDataByBook() {
    const response = await fetch('api/book', {
        method: 'GET',
        content: 'application/json'
    })
    const data = await response.json();
    dataByBook = data;
}

async function fetchAndSetQuotes() {
    const response = await fetch('api/quote', {
        method: 'GET',
        content: 'application/json'
    })
    const data = await response.json();
    quotes = data;
}

function renderBookData() {
    const bookHeaders = dataByBook.map(book => {
        const quoteDisplays = book.quotes.map(quote => {
            const topicDisplays = quote.topics.map(topic => {
                return `<li>${topic}</li>`
            }).join('');

            const quoteDisplay = `
                <div id=${quote.Id} class="d-flex justify-content-between">
                    <div class="d-flex flex-column">
                        <h3>${quote.chapter ?? ''}:${quote.verse ?? ''}</h3>
                        <p>${quote.page ?? ''}</p>
                    </div>
                    <p>${quote.text} ${quote.person ? `by ${quote.person}` : ''}</p>
                    <ul>
                        ${topicDisplays}
                    </ul>
                </div>`

            return quoteDisplay;
        }).join('');

        const display = `
            <div id=${`book${book.id}`} class="d-flex justify-content-between">
                <h2>${book.title}</h2>
                <h2>${book.author ?? ''}</h2>                
            </div>
            ${quoteDisplays}`

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

async function renderByCategory() {
    if (sortByBook) {
        await fetchAndSetDataByBook();
        renderBookData();
    }
}


$(document).ready(function () {
    renderByCategory();
})