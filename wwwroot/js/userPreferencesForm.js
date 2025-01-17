$(document).ready(function() {
    let initialState = getFormState();
    let hasChanges = false;

    function getFormState() {
        return $('#preferenceForm').serialize();
    }

    function checkFormChanges() {
        const currentState = getFormState();
        hasChanges = currentState !== initialState;
        $('#savePreferences').prop('disabled', !hasChanges);
    }

    // Handle checkbox clicks
    $(document).on('click', '.preference-box', function(e) {
        e.preventDefault();
        $(this).toggleClass('active');
        const checkbox = $(this).find('input[type="checkbox"]');
        checkbox.prop('checked', !checkbox.prop('checked'));
        checkFormChanges();
    });

    // Handle form submission
    $('#preferenceForm').on('submit', function(e) {
        if (!hasChanges) {
            e.preventDefault();
            return;
        }

        const $button = $('#savePreferences');
        const $spinner = $button.find('.spinner-border');
        const $buttonText = $button.find('.button-text');

        // Show loading state
        $button.prop('disabled', true);
        $spinner.removeClass('d-none');
        $buttonText.text('Saving...');

        // Let the form submit normally
        return true;
    });
});
