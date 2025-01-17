// Show/hide password functionality
$('.toggle-password').on('click', function() {
    const $button = $(this);
    const $input = $button.parent().find('input');
    const $icon = $button.find('i');

    if ($input.attr('type') === 'password') {
        $input.attr('type', 'text');
        $icon.removeClass('bi-eye').addClass('bi-eye-slash');
    } else {
        $input.attr('type', 'password');
        $icon.removeClass('bi-eye-slash').addClass('bi-eye');
    }
});