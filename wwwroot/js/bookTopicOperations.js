async function saveNew(url, objectToCreate, resetState) {
    try {
        const response = await fetch(url, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(objectToCreate)
        });

        const result = await response.json();

        if (!response.ok) {
            console.error('Update failed:', result);
        } else {
            console.log('Update successful:', result);
            resetState();
        }
    } catch (error) {
        console.error('Fetch error:', error);
    }
}

async function deleteObject(url, reassignUrl) {
    try {
        const response = await fetch(url, {
            method: 'DELETE',
        });

        if (!response.ok) {
            console.error('Delete failed');
        } else {
            console.log('Delete successful');
            location.assign(reassignUrl);
        }
    } catch (error) {
        console.error('Fetch error:', error);
    }
}

function toggleEdit(editing) {
    if (editing) {
        $("#toggle-edit").html("Cancel Edit").removeClass("primary").addClass("secondary");
        $("#save").fadeIn();
        $("#delete").fadeIn();
    } else {
        $("#toggle-edit").html("Edit").removeClass("secondary").addClass("primary");
        $("#save").hide();
        $("#delete").hide();
    };
};

export { saveNew, deleteObject, toggleEdit };