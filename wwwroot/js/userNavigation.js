$(document).ready(function () {
    // Expand all menus by default
    $('.collapse').addClass('show');
    $('.menu-header i').removeClass('bi-plus').addClass('bi-dash');

    // Handle menu header clicks for collapse/expand
    $('.menu-header').click(function () {
        const icon = $(this).find('i');
        const isExpanded = $(this).attr('aria-expanded') === 'true';
        icon.toggleClass('bi-plus bi-dash');
    });

    function scrollToTop() {
        window.scrollTo({
            top: 0,
            behavior: 'smooth'
        });
    }

    function loadPartialView(view) {
        $.get(`/User/GetPartialView?view=${view}`, function(data) {
            $('#mainContent').html(data);
            scrollToTop();
            history.pushState({ view: view }, '', `/User?view=${view}`);
        }).fail(function(error) {
            scrollToTop();
            console.error('Error loading partial view:', error);
            toastr.error('Error loading content');
        });
    }

    // Handle menu title click (Personal Center)
    $('.menu-title').click(function (e) {
        e.preventDefault();
        // Remove active state from all menu items
        $('.menu-item').removeClass('active');
        // Update URL to Index view
        history.pushState({}, '', '/User');
        // Load Index content
        $.get('/User/GetPartialView?view=Index', function (data) {
            $('#mainContent').html(data);
            scrollToTop();
        });
    });

    // Handle menu item clicks
    $('.menu-item[data-view]').click(function(e) {
        e.preventDefault();
        const view = $(this).data('view');
        $('.menu-item').removeClass('active');
        $(this).addClass('active');
        loadPartialView(view);
    });

    // Handle browser back/forward
    $(window).on('popstate', function () {
        location.reload();
    });

    // Initialize active state based on URL
    const urlParams = new URLSearchParams(window.location.search);
    const currentView = urlParams.get('view') || 'Index';
    $(`.menu-item[data-view="${currentView}"]`).addClass('active');

});