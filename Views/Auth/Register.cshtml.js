$(document).ready(function () {
    const $passwordInput = $('#Password');
    const $confirmPasswordInput = $('#ConfirmPassword');
    const $passwordMatchValidation = $('#passwordMatchValidation');
    const $registerButton = $('button[type="submit"]');

    // Get requirement elements
    const $lengthCheck = $('#lengthCheck');
    const $lowercaseCheck = $('#lowercaseCheck');
    const $uppercaseCheck = $('#uppercaseCheck');
    const $digitCheck = $('#digitCheck');
    const $specialCheck = $('#specialCheck');

    const passwordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!#$%^&*(),.?\":{}|<>])[A-Za-z\d!#$%^&*(),.?\":{}|<>]{8,}$/;

    function updateRequirement($element, isValid) {
        $element
            .removeClass('text-success text-danger')
            .addClass(isValid ? 'text-success' : 'text-danger')
            .find('i')
            .removeClass('bi-circle bi-check-circle-fill')
            .addClass(isValid ? 'bi-check-circle-fill' : 'bi-circle');
    }

    function validatePassword(password) {
        const checks = {
            length: password.length >= 8,
            lowercase: /[a-z]/.test(password),
            uppercase: /[A-Z]/.test(password),
            digit: /\d/.test(password),
            special: /[!#$%^&*(),.?\":{}|<>]/.test(password)
        };

        // Update requirement indicators
        updateRequirement($lengthCheck, checks.length);
        updateRequirement($lowercaseCheck, checks.lowercase);
        updateRequirement($uppercaseCheck, checks.uppercase);
        updateRequirement($digitCheck, checks.digit);
        updateRequirement($specialCheck, checks.special);

        // Update strength meter
        const strength = Object.values(checks).filter(Boolean).length;
        const strengthPercentage = (strength / 5) * 100;
        const $strengthBar = $('#passwordStrengthBar');
        const $strengthText = $('#passwordStrengthText');

        $strengthBar.css('width', `${strengthPercentage}%`);

        if (strength === 5) {
            $strengthBar.removeClass().addClass('progress-bar bg-success');
            $strengthText.text('Very Strong').removeClass().addClass('text-success small');
        } else if (strength === 4) {
            $strengthBar.removeClass().addClass('progress-bar bg-success');
            $strengthText.text('Strong').removeClass().addClass('text-success small');
        } else if (strength === 3) {
            $strengthBar.removeClass().addClass('progress-bar bg-warning');
            $strengthText.text('Medium').removeClass().addClass('text-warning small');
        } else if (strength === 2) {
            $strengthBar.removeClass().addClass('progress-bar bg-danger');
            $strengthText.text('Weak').removeClass().addClass('text-danger small');
        } else {
            $strengthBar.removeClass().addClass('progress-bar bg-danger');
            $strengthText.text('Very Weak').removeClass().addClass('text-danger small');
        }

        return {
            isValid: passwordRegex.test(password),
            checks: checks
        };
    }

    function checkPasswordMatch() {
        return $passwordInput.val() === $confirmPasswordInput.val();
    }

    function updateRegisterButton(isValid) {
        $registerButton.prop('disabled', !isValid || !checkPasswordMatch());
        $registerButton.attr('aria-disabled', (!isValid || !checkPasswordMatch()).toString());
    }

    $passwordInput.on('input', function() {
        const validation = validatePassword($(this).val());
        updateRegisterButton(validation.isValid);

        const passwordsMatch = checkPasswordMatch();
        $passwordMatchValidation.prop('hidden', !$confirmPasswordInput.val());

        if (passwordsMatch) {
            $passwordMatchValidation
                .removeClass('text-danger')
                .addClass('text-success')
                .html('<i class="bi bi-check-circle-fill"></i> Passwords match');
            $confirmPasswordInput[0].setCustomValidity('');
        } else {
            $passwordMatchValidation
                .removeClass('text-success')
                .addClass('text-danger')
                .html('<i class="bi bi-circle"></i> Passwords do not match');
            $confirmPasswordInput[0].setCustomValidity('Passwords do not match.');
        }
    });

    $confirmPasswordInput.on('input', function() {
        const passwordsMatch = checkPasswordMatch();
        $passwordMatchValidation.prop('hidden', !$(this).val());

        if (passwordsMatch) {
            $passwordMatchValidation
                .removeClass('text-danger')
                .addClass('text-success')
                .html('<i class="bi bi-check-circle-fill"></i> Passwords match');
            $confirmPasswordInput[0].setCustomValidity('');
        } else {
            $passwordMatchValidation
                .removeClass('text-success')
                .addClass('text-danger')
                .html('<i class="bi bi-circle"></i> Passwords do not match');
            $confirmPasswordInput[0].setCustomValidity('Passwords do not match.');
        }

        const password = $passwordInput.val();
        let strength = 0;
        if (password.length >= 8) strength += 1;
        if (/[A-Z]/.test(password)) strength += 1;
        if (/[0-9]/.test(password)) strength += 1;
        if (/[!#$%^&*]/.test(password)) strength += 1;

        updateRegisterButton(strength);
    });
});


