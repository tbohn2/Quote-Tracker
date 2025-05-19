let newName = "";

async function saveNewTopic(e) {
    e.preventDefault()

    try {
        const response = await fetch('api/topic/', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ Name: newName })
        })

        const data = await response.json();
        if (!response.ok) {
            console.log(data);
        } else {
            $('#addTopic').modal('hide');
            resetState();
            $('#topic-list').remove();
            renderTopicList(data);
        }
    } catch (error) {
        console.error(error)
    }
};

function renderTopicList(topics) {
    console.log(topics);

    const topicsDisplay = `
    <ul id="topic-list" class="col-6 p-0 list">
        ${topics.map(topic =>
        `<li class="topic-item text-center p-2" data-id="${topic.id}">
            <a href="/topicPage/Details/${topic.id}">
               ${topic.name}
            </a>
        </li>`).join('')}
    </ul>
    `

    $('#topics').append(topicsDisplay);
};

function resetState() {
    newName = ""
    $('input[topicName]').val('');
}

$('#create-topic-form').on('change', (e) => { newName = e.target.value });
$('#create-topic-form').on('submit', saveNewTopic);
$('#close-topic-modal').on('click', resetState());