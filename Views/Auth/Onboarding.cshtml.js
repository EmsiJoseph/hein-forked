$(document).ready(function () {
    const $submitButton = $('button[type="submit"]');
    const $preferenceCheckboxes = $('.preference-checkbox');
    const $preferenceValidation = $('#preferenceValidation');
    const $firstName = $('#FirstName');
    const $lastName = $('#LastName');
    const $dob = $('#Dob');
    const $gender = $('#Gender');

    function validateForm() {
        const isPreferencesValid = $preferenceCheckboxes.filter(':checked').length >= 5;
        const isFirstNameValid = $firstName.val().trim() !== '';
        const isLastNameValid = $lastName.val().trim() !== '';
        const isDobValid = $dob.val() !== '';
        const isGenderValid = $gender.val() !== '';

        const isFormValid = isPreferencesValid && isFirstNameValid && isLastNameValid && isDobValid && isGenderValid;

        if (!isPreferencesValid) {
            $preferenceValidation.show();
        } else {
            $preferenceValidation.hide();
        }

        $submitButton.prop('disabled', !isFormValid);
        $submitButton.attr('aria-disabled', (!isFormValid).toString());

        return isFormValid;
    }

    function updateStylePreferences() {
        const selectedPreferences = $preferenceCheckboxes
            .filter(':checked')
            .map(function () {
                return $(this).val();
            })
            .get()
            .join(',');

        $('#stylePreferenceInput').val(selectedPreferences);
    }

    // Add input event listeners to all required fields
    $firstName.on('input', validateForm);
    $lastName.on('input', validateForm);
    $dob.on('change', validateForm);
    $gender.on('change', validateForm);
    $preferenceCheckboxes.on('change', function () {
        updateStylePreferences();
        validateForm();
    });

    // Initial preferences update
    updateStylePreferences();

    // Get initial preferences from hidden input
    const initialPreferences = $('#stylePreferenceInput').val();
    if (initialPreferences) {
        const preferenceArray = initialPreferences.split(',');
        preferenceArray.forEach(preference => {
            $(`.preference-checkbox[value="${preference}"]`).prop('checked', true);
        });
    }

    // Initial validation
    validateForm();


    // Form submit handler
    $('form').on('submit', async function (e) {
        e.preventDefault();

        if (!validateForm()) {
            $preferenceValidation.show();
            return;
        }

        try {
            // Request geolocation
            await new Promise((resolve) => {
                navigator.geolocation.getCurrentPosition((position) => {
                    $('#Latitude').val(position.coords.latitude);
                    $('#Longitude').val(position.coords.longitude);
                    resolve();
                }, () => {
                    // If geolocation fails or is denied, continue without coordinates
                    resolve();
                });
            });
        } catch (error) {
            console.error('Geolocation error:', error);
        }

        // Submit the form after geolocation attempt
        this.submit();
    });
});