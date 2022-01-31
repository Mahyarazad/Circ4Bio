$(document).ready(function () {
    $('#main-table').DataTable({
        language: { searchPlaceholder: "Search Everthing ..." }
    });
    $('input[type="search"]').addClass('mt-1');
    $('input[type="search"]').addClass('w-1/2');
    $('input[type="search"]').addClass('border-gray-300');
    $('input[type="search"]').addClass('rounded-md');
    $('input[type="search"]').addClass('shadow-sm');
    $('input[type="search"]').addClass('py-2');
    $('input[type="search"]').addClass('mb-2');
    $('input[type="search"]').addClass('px-3');
    $('input[type="search"]').addClass('focus:outline-none');
    $('input[type="search"]').addClass('focus:border-sky-500');
    $('input[type="search"]').addClass('focus:ring-sky-500');
    $('input[type="search"]').addClass('sm:text-sm');
    $('input[type="search"]').addClass('float-left');
    $('input[type="search"]').addClass('font-medium');

    $('select[name="main-table_length"]').addClass('mt-1');
    $('select[name="main-table_length"]').addClass('w-full');
    $('select[name="main-table_length"]').addClass('border-gray-300');
    $('select[name="main-table_length"]').addClass('rounded-md');
    $('select[name="main-table_length"]').addClass('shadow-sm');
    $('select[name="main-table_length"]').addClass('mb-2');
    $('select[name="main-table_length"]').addClass('px-3');
    $('select[name="main-table_length"]').addClass('focus:outline-none');
    $('select[name="main-table_length"]').addClass('focus:border-sky-500');
    $('select[name="main-table_length"]').addClass('focus:ring-sky-500');
    $('select[name="main-table_length"]').addClass('sm:text-sm');
    $('select[name="main-table_length"]').addClass('font-medium');


    $('div[id="main-table_length"]').addClass('sm:text-sm');
    $('div[id="main-table_length"]').addClass('mx-3');
    $('div[id="main-table_length"]').addClass('pt-2');
    $('div[id="main-table_length"]').addClass('text-gray-700');
    $('div[id="main-table_length"]').addClass('font-medium');
    $('div[id="main-table_length"]').addClass('float-right');

    $('div[id="main-table_info"]').addClass('sm:text-sm');
    $('div[id="main-table_info"]').addClass('mx-3');
    $('div[id="main-table_info"]').addClass('mt-1');
    $('div[id="main-table_info"]').addClass('text-gray-700');
    $('div[id="main-table_info"]').addClass('font-medium');
    $('div[id="main-table_info"]').addClass('float-left');
    $('div[id="main-table_info"]').addClass('px-3');
    $('div[id="main-table_info"]').addClass('py-3');

    $('div[id="main-table_paginate"]').addClass('sm:text-sm');
    $('div[id="main-table_paginate"]').addClass('text-gray-700');
    $('div[id="main-table_paginate"]').addClass('font-medium');
    $('div[id="main-table_paginate"]').addClass('float-right');
    $('div[id="main-table_paginate"]').addClass('px-6');
    $('div[id="main-table_paginate"]').addClass('py-3');

    $('div[class="row"]').addClass('border-t');

    $('td[valign="top"]').addClass('sm:text-sm');
    $('td[valign="top"]').addClass('text-gray-700');
    $('td[valign="top"]').addClass('pl-6');
    $('td[valign="top"]').addClass('pt-2');

    $('ul[class="pagination"]').addClass('flex');
    $('li[class="active"]').addClass('mx-1');
    $('li[class="next"]').addClass('mx-1');
});