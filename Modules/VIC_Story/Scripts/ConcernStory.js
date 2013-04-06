$(function () {

    $('#StoryDivID').hide();
    $('#SubmitID').hide();

    $('#ConcernID').change(function () {
        var URL = $('#ConcernStoryFormID').data('stateListAction');
        $.getJSON(URL + '/' + $('#ConcernID').val(), function (data) {
            var items = '<option>Select a story</option>';
            $.each(data, function (i, state) {
                items += "<option value='" + state.Value + "'>" + state.Text + "</option>";
                // state.Value cannot contain ' character. We are OK because state.Value = cnt++;
            });
            $('#StoryID').html(items);
            $('#StoryDivID').show();

        });
    });

    $('#StatesID').change(function () {
        $('#SubmitID').show();
    });
});