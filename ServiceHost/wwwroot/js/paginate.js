function paginate(
    totalItems,
    currentPage,
    pageSize = 8,
    maxPages = 1000
) {
    // calculate total pages
    var totalPages = Math.ceil(totalItems / pageSize);

    // ensure current page isn't out of range
    if (currentPage < 1) {
        currentPage = 1;
    } else if (currentPage > totalPages) {
        currentPage = totalPages;
    }

    var startPage, endPage;
    if (totalPages <= maxPages) {
        // total pages less than max so show all pages
        startPage = 1;
        endPage = totalPages;
    } else {
        // total pages more than max so calculate start and end pages
        var maxPagesBeforeCurrentPage = Math.floor(maxPages / 2);
        var maxPagesAfterCurrentPage = Math.ceil(maxPages / 2) - 1;
        if (currentPage <= maxPagesBeforeCurrentPage) {
            // current page near the start
            startPage = 1;
            endPage = maxPages;
        } else if (currentPage + maxPagesAfterCurrentPage >= totalPages) {
            // current page near the end
            startPage = totalPages - maxPages + 1;
            endPage = totalPages;
        } else {
            // current page somewhere in the middle
            startPage = currentPage - maxPagesBeforeCurrentPage;
            endPage = currentPage + maxPagesAfterCurrentPage;
        }
    }

    // calculate start and end item indexes
    var startIndex = (currentPage - 1) * pageSize;
    var endIndex = Math.min(startIndex + pageSize - 1, totalItems - 1);

    // create an array of pages to ng-repeat in the pager control
    var pages = Array.from(Array((endPage + 1) - startPage).keys()).map(i => startPage + i);

    // return object with all pager properties required by the view
    return {
        totalItems: totalItems,
        currentPage: currentPage,
        pageSize: pageSize,
        totalPages: totalPages,
        startPage: startPage,
        endPage: endPage,
        startIndex: startIndex,
        endIndex: endIndex,
        pages: pages
    };
}

function createPagination(totalPages, page) {
    hideItems();
    var $totalPages = totalPages;
    var liTag = '';
    var active;
    var beforePage = page - 1;
    var afterPage = page + 1;

    if (page > 1) {
        liTag += `<p class="btn prev text-sm font-medium leading-none cursor-pointer text-gray-600 hover:text-sky-700 border-t border-transparent hover:border-sky-400 pt-3 mr-4 px-2" onclick="createPagination(_totalPages, ${page - 1})"><span><i class="fas fa-angle-left"></i> Previous</span></p>`;
        //show the next button if the page value is greater than 1
    }
    if (_totalPages > 5) {
        if (page > 2) { //if page value is less than 2 then add 1 after the previous button
            liTag += `<p class="first numb text-sm font-medium leading-none cursor-pointer text-gray-600 hover:text-sky-700 border-t border-transparent hover:border-sky-400 pt-3 mr-4 px-2" onclick="createPagination(_totalPages, 1)"><span>1</span></p>`;
            //if page value is greater than 3 then add this (...) after the first li or page

            liTag += `<p class="dots text-sm font-medium leading-none cursor-pointer text-gray-600 hover:text-sky-700 border-t border-transparent hover:border-sky-400 pt-3 mr-4 px-2"><span>...</span></p>`;
        }
    }

    // how many pages or li show before the current li
    if (page == totalPages) {
        beforePage = beforePage - 2;
    } else if (page == totalPages - 1) {
        beforePage = beforePage - 1;
    }
    // how many pages or li show after the current li
    if (page == 1) {
        afterPage = afterPage + 2;
    } else if (page == 2) {
        afterPage = afterPage + 1;
    }

    if (totalPages > 1) {
        for (var plength = beforePage; plength <= afterPage; plength++) {
            if (plength > totalPages) { //if plength is greater than totalPage length then continue
                continue;
            }
            if (plength == 0) { //if plength is 0 than add +1 in plength value
                plength = plength + 1;
            }
            if (page == plength) {
                //                    console.log(paginate(totalPages, page))
                //if page is equal to plength than assign active string in the active variable
                active = "active";

            } else { //else leave empty to the active variable
                active = "";
            }
            if (plength !== -1) {
                liTag += `<p class="numb ${active} text-sm font-medium leading-none cursor-pointer ${active == "active" ? 'text-sky-700 border-sky-400' : 'text-gray-600 hover:text-sky-700 over:border-sky-400 border-transparent'}  border-t pt-3 mr-4 px-2" onclick="createPagination(_totalPages, ${plength})"><span>${plength}</span></p>`;
            }
        }
    }


    var pageSize = paginateObject.pageSize;
    var maxPages = 100;
    if (totalPages <= maxPages) {
        // total pages less than max so show all pages
        startPage = 1;
        endPage = totalPages;
    } else {
        // total pages more than max so calculate start and end pages
        var maxPagesBeforeCurrentPage = Math.floor(maxPages / 2);
        var maxPagesAfterCurrentPage = Math.ceil(maxPages / 2) - 1;
        if (currentPage <= maxPagesBeforeCurrentPage) {
            // current page near the start
            startPage = 1;
            endPage = maxPages;
        } else if (currentPage + maxPagesAfterCurrentPage >= totalPages) {
            // current page near the end
            startPage = totalPages - maxPages + 1;
            endPage = totalPages;
        } else {
            // current page somewhere in the middle
            startPage = currentPage - maxPagesBeforeCurrentPage;
            endPage = currentPage + maxPagesAfterCurrentPage;
        }
    }
    var startIndex = (page - 1) * pageSize;
    var endIndex = Math.min(startIndex + pageSize - 1, filtered.length - 1);
    for (var i = startIndex; i <= endIndex; i++) {
        $(`div[data-item=${filtered[i].Id}]`).css('display', 'block');
    }

    if (_totalPages > 5) {
        if (page < totalPages - 1) { //if page value is less than totalPage value by -1 then show the last li or page
            if (page < totalPages - 2) { //if page value is less than totalPage value by -2 then add this (...) before the last li or page
                liTag += `<p class="dots"><span>...</span></p>`;
            }
            liTag += `<p class="last numb text-sm font-medium leading-none cursor-pointer text-gray-600 hover:text-sky-700 border-t border-transparent hover:border-sky-400 pt-3 mr-4 px-2" onclick="createPagination(_totalPages, ${totalPages})"><span>${totalPages}</span></p>`;
        }
    }


    if (page < totalPages) {
        liTag += `<p class="btn next text-sm font-medium leading-none cursor-pointer text-gray-600 hover:text-sky-700 border-t border-transparent hover:border-sky-400 pt-3 mr-4 px-2" onclick="createPagination(_totalPages, ${page + 1})"><span>Next <i class="fas fa-angle-right"></i></span></p>`;
        //show the next button if the page value is less than totalPage(20)
    }

    element.innerHTML = liTag; //add li tag inside ul tag

    return liTag; //reurn the li tag
}

var cacheType = new Array();
var cacheNaceType = new Set();


var typeFilter = function (f1, f2, f3, array) {
    return array.filter(function (data) {
        switch (f1 + "-" + f2 + "-" + f3) {
            case "true-false-false":
                return data.Type === "Technology Provider";
            case "false-true-false":
                return data.Type === "Plant";
            case "false-false-true":
                return data.Type === "Supplier of Raw Material";
            case "true-false-true":
                return data.Type !== "Plant";
            case "false-true-true":
                return data.Type !== "Technology Provider";
            case "true-true-false":
                return data.Type !== "Supplier of Raw Material";
            default:
                return array;
        }
    });
}

function handelFilter(id, val) {
    var _filter1 = $("#filter-technology").prop('checked');
    var _filter2 = $("#filter-plant").prop('checked');
    var _filter3 = $("#filter-supplier").prop('checked');

    if (typeof (id) === 'undefined' & typeof (val) === 'undefined') {
        if (cacheNaceType.size > 0) {
            var temp = new Array();
            cacheNaceType.forEach(val => {
                temp.push(...items.filter(data => { return data.NaceId === val }));
            });
            filtered = [...temp];
            filtered = typeFilter(_filter1, _filter2, _filter3, filtered);
        } else {
            filtered = typeFilter(_filter1, _filter2, _filter3, items);
        }

    }

    if (typeof (id) !== 'undefined' & val) {
        cacheNaceType.add(id);
        if (cacheNaceType.size > 1) {
            var temp = new Array();
            cacheNaceType.forEach(val => {
                temp.push(...items.filter(data => { return data.NaceId === val }));
            });
            filtered = [...temp];
        } else {
            filtered = filtered.filter(function (data) {
                return data.NaceId === id;
            });
            if (cacheType.length === 0) {
                cacheType = filtered;
            };
        }
    }


    if (typeof (id) !== 'undefined' & !val) {
        if (cacheNaceType.has(id)) {
            cacheNaceType.delete(id);
            cacheType.pop(cacheType.find(x => x.NaceId === id));
        }

        var temp = new Array();
        if (cacheNaceType.size > 0) {
            var temp = new Array();
            cacheNaceType.forEach(val => {
                temp.push(...items.filter(data => { return data.NaceId === val }));
            });
            filtered = [...temp];
        } else {
            filtered = typeFilter(_filter1, _filter2, _filter3, items);
        }
    }

    if (!_filter1 & !_filter2 & !_filter3 & typeof (id) === 'undefined' & typeof (val) === 'undefined') {
        if (cacheNaceType.size > 0) {
            var temp = new Array();
            cacheNaceType.forEach(val => {
                temp.push(...items.filter(data => { return data.NaceId === val }));
            });
            filtered = [...temp];
        } else {
            filtered = items;
        }

    }

    element.innerHTML = '';
    paginateObject = paginate(filtered.length, 1);
    _totalPages = paginateObject.pages.length;
    checkNoResult(_totalPages);
};



function handleGridItem(pageSize) {
    $("#grid-size-container").addClass('invisible');
    $("#grid-size-container").addClass('scale-95');
    $('#grid-size-container').css('opacity', '0');

    $("#grid-size-container-unlogged").addClass('invisible');
    $("#grid-size-container-unlogged").addClass('scale-95');
    $('#grid-size-container-unlogged').css('opacity', '0');

    element.innerHTML = '';
    paginateObject = paginate(filtered.length, 1, pageSize);
    _totalPages = paginateObject.pages.length;
    if (_totalPages > 1) {
        element.innerHTML = createPagination(_totalPages, page);
    } else {
        showItems();
    }
}

var checkNoResult = function (tp) {
    if (_totalPages === 1) {
        hideItems();
        showItems();
        element.innerHTML = ''
        $("#no-result").addClass('hidden');
    } else if (filtered.length === 0) {
        hideItems();
        $("#no-result").removeClass('hidden');
        $("#no-result").html("No Result");
    } else {
        hideItems();
        showItems();
        $("#no-result").addClass('hidden');
        element.innerHTML = createPagination(tp, page);
    }
}

function search(input) {

    filtered = items.filter(function (data) {
        return data.Name.toLowerCase().includes(input.value.toLowerCase()) ||
            data.Description.toLowerCase().includes(input.value.toLowerCase()) ||
            data.Email.toLowerCase().includes(input.value.toLowerCase()) ||
            data.FullName.toLowerCase().includes(input.value.toLowerCase())
    });


    if (input.value.length === 0) {
        handelFilter();
    } else {
        element.innerHTML = '';
        paginateObject = paginate(filtered.length, 1);
        _totalPages = paginateObject.pages.length;
        checkNoResult(_totalPages);

    }
}
