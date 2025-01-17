$(document).ready(function () {
    const $mainInput = $("#mainInput");
    const $emailField = $("#emailField");
    const $phoneField = $("#phoneField");
    const $activeField = $("#activeField");
    const $countryCode = $("#countryCode");

    // Correct setCursorPosition function usage
    function setCursorPosition(input, pos) {
        if (input.setSelectionRange) {
            input.focus();
            input.setSelectionRange(pos, pos);
        } else if (input.createTextRange) {
            var range = input.createTextRange();
            range.collapse(true);
            range.moveEnd('character', pos);
            range.moveStart('character', pos);
            range.select();
        }
    }

    function isNumeric(str) {
        return /^\d+$/.test(str);
    }

    function togglePhoneMode(isPhone) {
        $countryCode.css("display", isPhone ? "block" : "none");
        $mainInput.attr("placeholder", isPhone ? "Phone number" : "Email address");
        $mainInput.attr("type", isPhone ? "text" : "email");
        $activeField.val(isPhone ? "phone" : "email");
    }

    $mainInput.on("input", function (e) {

        const value = e.target.value.trim();

        if (value === "") {
            togglePhoneMode(false);
            $emailField.val("");
            $phoneField.val("");
            return;
        }

        if (isNumeric(value)) {
            togglePhoneMode(true);
            $phoneField.val(value);
            $emailField.val("");
        } else {
            togglePhoneMode(false);
            $emailField.val(value);
            $phoneField.val("");
        }
    });

    // Initial setup as email type
    togglePhoneMode(false);

    const $email = $emailField.val();

    if ($email) {
        $mainInput.val($email);
        setCursorPosition($mainInput[0], $email.length);
        togglePhoneMode(false);
    } else if ($phoneField.val()) {
        $mainInput.val($phoneField.val());
        setCursorPosition($mainInput[0], $phoneField.val().length);
        togglePhoneMode(true);
    }
    
});
