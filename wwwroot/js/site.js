let sortByBook = true;
let quotes = [];
let dataByBook = [];
let dataByTopic = [];

function retrieveLocalData(name) {
    const storedData = sessionStorage.getItem(name);
    if (storedData !== null) {
        const data = JSON.parse(storedData)
        name === "dataByBook" ? dataByBook = data : dataByTopic = data;
        return true;
    }
    return false;
}

async function fetchAndSetDataByBook() {
    if (retrieveLocalData('dataByBook')) { return; }

    const response = await fetch('api/book', {
        method: 'GET',
        content: 'application/json'
    })
    const data = await response.json();
    dataByBook = data;
    sessionStorage.setItem('dataByBook', JSON.stringify(data));
}

async function fetchAndSetDataByTopic() {
    if (retrieveLocalData('dataByTopic')) { return; }

    const response = await fetch('api/topic', {
        method: 'GET',
        content: 'application/json'
    })
    const data = await response.json();
    dataByTopic = data;
    sessionStorage.setItem('dataByTopic', JSON.stringify(data));
}

function createBookDisplay(book, quoteDisplays) {
    const display = `
    <div id=${`book${book.id}`} class="d-flex justify-content-between mt-5 mb-2">
        <h2>${book.title}</h2>
        <h2>${book.author ?? ''}</h2>                
    </div>
    ${quoteDisplays}`

    return display;
}

function createQuoteDisplay(quote) {
    const topicDisplays = quote.topics ? quote.topics.map(topic => {
        return `<li class="text-center p-2 m-1">${topic}</li>`
    }).join('') : null;

    const quoteDisplay = `
            <div id=${quote.Id} class="d-flex justify-content-between align-items-center px-2 py-1 quote-row">
                <div class="d-flex flex-column align-items-start col-3">
                    <h3>${quote.chapter ?? ''}:${quote.verse ?? ''}</h3>                        
                    ${quote.page ? `<p>page ${quote.page ?? ''}</p>` : ''}
                </div>
                <p class="col-6 text-start quote-text mb-0">"${quote.text}" ${quote.person ? `by ${quote.person}` : ''}</p>
                ${topicDisplays ? `<ul class="col-3 d-flex flex-wrap justify-content-center align-items-center mb-0">${topicDisplays}</ul>` : ''}
            </div>`

    return quoteDisplay;
}

function renderBookData() {
    const bookHeaders = dataByBook.map(book => {
        const quoteDisplays = book.quotes.map(quote => {
            return createQuoteDisplay(quote);
        }).join('');

        return createBookDisplay(book, quoteDisplays)
    }).join('');

    $('#quotes').append(bookHeaders);
}

function renderTopicData() {
    const topicHeaders = dataByTopic.map(topic => {
        let book = {};
        const quoteDisplays = topic.quotes.map(quote => {
            if (Object.values(book) < 1 || book.Id !== quote.BookId) {
                book = {
                    id: quote.bookId,
                    title: quote.bookTitle,
                    author: quote.bookAuthor
                }
            }
            return createQuoteDisplay(quote);
        }).join('');
        const bookDisplays = createBookDisplay(book, quoteDisplays)

        return `
            <div id=${`topic${topic.id}`} class="mt-5">
                <h2>${topic.name}</h2>
            </div>
            ${bookDisplays}`
    }).join('');

    $('#quotes').append(topicHeaders);
}

async function renderByCategory() {
    if (sortByBook) {
        await fetchAndSetDataByBook();
        renderBookData();
    } else {
        await fetchAndSetDataByTopic();
        renderTopicData();
    }
}


$(document).ready(function () {
    $('#sort-select').on('change', function () {
        sortByBook = !sortByBook;
        $('#quotes').empty();
        renderByCategory();
    })

    renderByCategory();
})