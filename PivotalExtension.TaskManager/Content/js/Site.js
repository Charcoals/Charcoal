function SignUp() {
    initials = prompt('Enter your initials in AA/BB format (an empty string will clear existing assignment):', '');
    $('div.ui-selected').each(function () {
        if (initials != null) {
            var id = $(this).attr('id');
            var items = id.split('-');
            $.ajax({
                type: 'POST',
                url: '/Task/SignUp',
                data: 'projectId=' + items[0] + '&storyId=' + items[1] + '&id=' + items[2] + '&initials=' + initials,
                success: function (html) {
                    $('#' + id).replaceWith(html);
                }
            });
        }
    });
}

function AddTask(id) {
    details = prompt('Enter new task detail:', '');
    var items = id.split('-');
    $.ajax({
        type: 'POST',
        url: '/Stories/AddTask',
        data: 'projectId=' + items[0] + '&storyId=' + items[1] + '&details=' + details,
        success: function (html) {
            $('#' + id).replaceWith(html);
        }
    });
}

function RemoveTask(id) {
    var items = id.split('-');
    $.ajax({
        type: 'POST',
        url: '/Stories/DeleteTask',
        data: 'projectId=' + items[0] + '&storyId=' + items[1] + '&taskId=' + items[2],
       success: function (html) {
           $('#' + items[0] + '-' + items[1]).replaceWith(html);
        }
    });
}

function CompleteTask(id, completed) {
    var items = id.split('-');
    $.ajax({
        type: 'POST',
        url: '/Task/Complete',
        data: 'projectId=' + items[0] + '&storyId=' + items[1] + '&id=' + items[2] + '&completed=' + completed,
        success: function (html) {
            $('#' + id).replaceWith(html);
        }
    });
}

function RefreshAll() {
    $('tr.story-row').each(function () {
        var id = $(this).attr('id');
        var items = id.split('-');
        $.ajax({
            type: 'GET',
            url: '/Stories/Get',
            data: 'projectId=' + items[0] + '&storyId=' + items[1],
            success: function (html) {
                $('#' + id).replaceWith(html);
            }
        });
    });
}

function StartStory(id) {
    var items = id.split('-');
    $.ajax({
        type: 'POST',
        url: '/Stories/Start',
        data: 'projectId=' + items[0] + '&storyId=' + items[1],
        success: function (html) {
            $('#' + id).replaceWith(html);
        }
    });
}

function FinishStory(id) {
    var items = id.split('-');
    $.ajax({
        type: 'POST',
        url: '/Stories/Finish',
        data: 'projectId=' + items[0] + '&storyId=' + items[1],
        success: function (html) {
            $('#' + id).replaceWith(html);
        }
    });
}

function Toggle(elem, selector) {
    $(selector).each(function () {
        var showCompleted = !elem.checked //this should eventually be checked = show
        $(this).toggle(showCompleted);
    });
}