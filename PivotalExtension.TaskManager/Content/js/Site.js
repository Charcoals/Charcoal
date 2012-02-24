function SignUpForTask() {
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
                    $('.flippable').quickFlip();
                }
            });
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
            $('.flippable').quickFlip();
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
            $('.flippable').quickFlip();
        }
    });
}

function RefreshStories() {
    $('tr.story-row').each(function () {
        var id = $(this).attr('id');
        var items = id.split('-');
        $.ajax({
            type: 'GET',
            url: '/Stories/Get',
            data: 'projectId=' + items[0] + '&storyId=' + items[1],
            success: function (html) {
                $('#' + id).replaceWith(html);
                $('.flippable').quickFlip();
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
            $('.flippable').quickFlip();
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
            $('.flippable').quickFlip();
        }
    });
}

function Toggle(elem, selector) {
    $(selector).each(function () {
        var showCompleted = !elem.checked //this should eventually be checked = show
        $(this).toggle(showCompleted);
    });
}

//facebox stuff
//requires jquery.form.js and facebox
var updateTargetId;

function BindFaceboxLinks(unbind) {
    //$.live() doesn't work w/ facebox, need to re-bind when content is reloaded
    if (unbind) {
        $('a.facebox').unbind();
    }
    $('a.facebox').bind('click', function () {
        updateTargetId = this.rel;
    }).facebox();

    //on reveal, need to bind new async forms and cancellation links
    $(document).bind('reveal.facebox', function () {
        $('.async-form').ajaxForm(function (responseText) {
            $('#' + updateTargetId).replaceWith(responseText);
            $('.flippable').quickFlip();
            $.facebox.close();
            BindFaceboxLinks(true);
            updateTargetId = null;
        });

        $('.facebox-cancel').bind('click', function () {
            $.facebox.close();
            updateTargetId = null;
            return false;
        });
    });
}

function ExpandNotes(id) {
    var $button = $('#link-' + id);
    if ($button.hasClass("expand")) {
        $button.switchClass("expand", "collapse", 200);
    } else {
        $button.switchClass("collapse", "expand", 200);
    }
    $('#' + id).toggle("blind", null, 400);
};