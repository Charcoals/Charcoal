function buildReplaceCallback(id, additionalFunction) {
    return function (html) {
        var rootSelector = '#' + id;
        $(rootSelector).replaceWith(html);
        $(rootSelector + ' .flippable').quickFlip();
        $(rootSelector + ' .task-column').selectable({
            filter: 'div.task:not(.complete)',
            selected: function (event, ui) {
                var deselector = 'td.task-column:not(#' + $(ui.selected).parent().attr('id') + ')';
                $(deselector).find('.ui-selected').removeClass('ui-selected');
            }
        });
        if (additionalFunction != undefined) {
            additionalFunction();
        }
        BindFaceboxLinks(true);
    };
}

function SignUpForTask() {
    initials = prompt('Enter your initials in AA/BB format (an empty string will clear existing assignment):', '');
    var array = [];
    var storyId;
    if (initials != null) {
        $('div.ui-selected').each(function () {
            var id = $(this).attr('id');
            var items = id.split('-');
            storyId = items[0] + '-' + items[1];
            array.push(id);
        });
        var queryString = $.param({ initials: initials, fullIds: array }, true);
        $.ajax({
            type: 'POST',
            url: '/Task/SignUp',
            data: queryString,
            success: buildReplaceCallback(storyId)
        });
    }
}

function RemoveTask(id) {
    var items = id.split('-');
    $.ajax({
        type: 'POST',
        url: '/Stories/DeleteTask',
        data: 'projectId=' + items[0] + '&storyId=' + items[1] + '&taskId=' + items[2],
        success: buildReplaceCallback(items[0] + '-' + items[1])
    });
}

function CompleteTask(id, completed) {
    var items = id.split('-');
    $.ajax({
        type: 'POST',
        url: '/Task/Complete',
        data: 'projectId=' + items[0] + '&storyId=' + items[1] + '&id=' + items[2] + '&completed=' + completed,
        success: buildReplaceCallback(id)
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
            success: buildReplaceCallback(id)
        });
    });
}

function StartStory(id) {
    var items = id.split('-');
    $.ajax({
        type: 'POST',
        url: '/Stories/Start',
        data: 'projectId=' + items[0] + '&storyId=' + items[1],
        success: buildReplaceCallback(id)
    });
}

function FinishStory(id) {
    var items = id.split('-');
    $.ajax({
        type: 'POST',
        url: '/Stories/Finish',
        data: 'projectId=' + items[0] + '&storyId=' + items[1],
        success: buildReplaceCallback(id)
    });
}

function Toggle(elem, selector) {
    $(selector).each(function () {
        $(this).toggle(elem.checked);
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
        //this could probably use buildReplaceCallback if we added ability to pass in an additional function
        //would be slightly awkward because it needs to be constructed when updateTargetId is available so buildReplaceCallback(updateTargetId)(responseText);
        $('.async-form').ajaxForm(function (responseText) {
            buildReplaceCallback(updateTargetId, function () {
                $.facebox.close();
                updateTargetId = null;
            })(responseText);

//            $('#' + updateTargetId).replaceWith(responseText);
//            $('.flippable').quickFlip();
//            $.facebox.close();
//            BindFaceboxLinks(true);
//            updateTargetId = null;
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