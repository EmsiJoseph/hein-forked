// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    const $container = $('.nav-items-container');
    const $leftBtn = $('#scrollLeft');
    const $rightBtn = $('#scrollRight');

    function getNextVisibleItem(direction) {
        const containerRect = $container[0].getBoundingClientRect();
        const items = $('.nav-items .nav-link');

        if (direction === 'right') {
            // Existing right scroll logic
            for (let i = 0; i < items.length; i++) {
                const itemRect = items[i].getBoundingClientRect();
                if (itemRect.right > containerRect.right) {
                    return {
                        elem: items[i],
                        offset: itemRect.left - containerRect.left
                    };
                }
            }
        } else {
            // New left scroll logic
            for (let i = items.length - 1; i >= 0; i--) {
                const itemRect = items[i].getBoundingClientRect();
                if (itemRect.left < containerRect.left) {
                    // Find the last fully hidden item on the left
                    let targetIndex = i;
                    while (targetIndex > 0) {
                        const prevRect = items[targetIndex - 1].getBoundingClientRect();
                        if (prevRect.right < containerRect.left) {
                            targetIndex--;
                        } else {
                            break;
                        }
                    }
                    return {
                        elem: items[targetIndex],
                        offset: itemRect.left - containerRect.right + itemRect.width
                    };
                }
            }
        }
        return null;
    }

    function updateScrollButtons() {
        const scrollLeft = $container.scrollLeft();
        const scrollWidth = $container[0].scrollWidth;
        const containerWidth = $container.width();

        // Check if content is scrollable
        const isScrollable = scrollWidth > containerWidth;

        // Disable left button if at start
        $leftBtn.prop('disabled', scrollLeft <= 0);

        // Disable right button if at end or if content isn't scrollable
        $rightBtn.prop('disabled', !isScrollable || scrollLeft >= scrollWidth - containerWidth);
    }

    // Update buttons on scroll and window resize
    $container.on('scroll', updateScrollButtons);
    $(window).on('resize', updateScrollButtons);

    // Initial state check
    updateScrollButtons();

    $leftBtn.on('click', () => {
        const target = getNextVisibleItem('left');
        if (target) {
            $container.animate({
                scrollLeft: $container.scrollLeft() + target.offset
            }, 300);
        }
    });

    $rightBtn.on('click', () => {
        const target = getNextVisibleItem('right');
        if (target) {
            $container.animate({
                scrollLeft: $container.scrollLeft() + target.offset
            }, 300);
        }
    });

    // Hover functionality for icons and menus
    $('.icon-with-menu').hover(
        function () {
            $(this).find('.menu').show();
        },
        function () {
            $(this).find('.menu').hide();
        }
    );

    $('.menu').hover(
        function () {
            $(this).show();
        },
        function () {
            $(this).hide();
        }
    );

    // Update menu positions on window resize
    let resizeTimer;
    $(window).on('resize', function () {
        clearTimeout(resizeTimer);
        resizeTimer = setTimeout(function () {
            $('.menu:visible').each(function () {
                // No need to adjust menu position
            });
        }, 250);
    });
});
