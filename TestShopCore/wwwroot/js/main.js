$(document).ready(function() {
    "use strict";

    // Mobile Nav toggle
    $('.menu-toggle > a').on('click', function (e) {
        e.preventDefault();
        $('#responsive-nav').toggleClass('active');
    });

    // Fix cart dropdown from closing
    $('.cart-dropdown').on('click', function (e) {
        e.stopPropagation();
    });

    /////////////////////////////////////////

    // Products Slick
    $('.products-slick').each(function () {
        var $this = $(this),
            $nav = $this.attr('data-nav');

        $this.slick({
            slidesToShow: 4,
            slidesToScroll: 1,
            autoplay: true,
            infinite: true,
            speed: 300,
            dots: false,
            arrows: true,
            appendArrows: $nav ? $nav : false,
            responsive: [{
                breakpoint: 991,
                settings: {
                    slidesToShow: 2,
                    slidesToScroll: 1
                }
            },
            {
                breakpoint: 480,
                settings: {
                    slidesToShow: 1,
                    slidesToScroll: 1
                }
            }
            ]
        });
    });

    // Products Widget Slick
    $('.products-widget-slick').each(function () {
        var $this = $(this),
            $nav = $this.attr('data-nav');

        $this.slick({
            infinite: true,
            autoplay: true,
            speed: 300,
            dots: false,
            arrows: true,
            appendArrows: $nav ? $nav : false
        });
    });

    /////////////////////////////////////////

    // Product Main img Slick
    $('#product-main-img').slick({
        infinite: true,
        speed: 300,
        dots: false,
        arrows: true,
        fade: true,
        asNavFor: '#product-imgs'
    });

    // Product imgs Slick
    $('#product-imgs').slick({
        slidesToShow: 3,
        slidesToScroll: 1,
        arrows: true,
        centerMode: true,
        focusOnSelect: true,
        centerPadding: 0,
        vertical: true,
        asNavFor: '#product-main-img',
        responsive: [{
            breakpoint: 991,
            settings: {
                vertical: false,
                arrows: false,
                dots: true
            }
        }
        ]
    });

    // Product img zoom
    var zoomMainProduct = document.getElementById('product-main-img');
    if (zoomMainProduct) {
        $('#product-main-img .product-preview').zoom();
    }

    /////////////////////////////////////////
    InitaializeUpDown();

    var priceInputMax = document.getElementById('price-max'),
        priceInputMin = document.getElementById('price-min');

    if (priceInputMax) {
        priceInputMax.addEventListener('change', function () {
            updatePriceSlider($(this).parent(), this.value);
        });
    }

    if (priceInputMin) {
        priceInputMin.addEventListener('change', function () {
            updatePriceSlider($(this).parent(), this.value);
        });
    }

    function updatePriceSlider(elem, value) {
        if (elem.hasClass('price-min')) {
            console.log('min');
            priceSlider.noUiSlider.set([value, null]);
        } else if (elem.hasClass('price-max')) {
            console.log('max');
            priceSlider.noUiSlider.set([null, value]);
        }
    }

    // Price Slider
    var priceSlider = document.getElementById('price-slider');
    if (priceSlider) {
        noUiSlider.create(priceSlider, {
            start: [1, 99999],
            connect: true,
            step: 1,
            range: {
                'min': 1,
                'max': 99999
            }
        });

        priceSlider.noUiSlider.on('update', function (values, handle) {
            var value = values[handle];
            handle ? priceInputMax.value = value : priceInputMin.value = value;
        });
    }
    /*--Конец функций шаблона--*/
    Storage.prototype.setObject = function (key, value) {
        this.setItem(key, JSON.stringify(value));
    };

    Storage.prototype.getObject = function (key) {
        var value = this.getItem(key);
        return value && JSON.parse(value);
    };

    var productArray = localStorage.getObject("shoping-card") === null ? new Array() : localStorage.getObject("shoping-card");
    $("#shopingItemsCount").html(productArray.length);

    $(".logout").click(function (e) {
        e.preventDefault();
        Logout();
    });

    function Logout() {
        var tokenKey = "tokenInfo";
        var initialInner = $("#login").html();
        $(".logout").html("<img src='/Img/loader.gif' />");

        $.ajax({
            type: 'GET',
            url: '/api/Account/Logout',
            beforeSend: function (xhr) {
                var token = localStorage.getItem(tokenKey);
                xhr.setRequestHeader("Authorization", "Bearer " + token);
            },
            complete: function (data) {
                if (data.responseText === "") {
                    localStorage.removeItem(tokenKey);
                    $(".logout").html(initialInner);
                    location.reload();
                }
                else {
                    alert(data.responseText);
                }
            }
        });
    }

    SetActiveMenuItem();
});

function SetActiveMenuItem() {
    $(".main-nav li").each(function () {
        if ($(this).html().indexOf($("#pageName").val()) !== -1) {
            $(this).addClass("active");
            $(".main-nav li:first").removeClass("active");
        }
    });
}

function InitaializeUpDown() {
    // Input number
    $('.input-number').each(function () {
        var $this = $(this),
            $input = $this.find('input[type="number"]'),
            up = $this.find('.qty-up'),
            down = $this.find('.qty-down');

        down.on('click', function () {
            var value = parseInt($input.val()) - 1;
            value = value < 1 ? 1 : value;
            $input.val(value);
            $input.change();
            updatePriceSlider($this, value);
        });

        up.on('click', function () {
            var value = parseInt($input.val()) + 1;
            $input.val(value);
            $input.change();
            updatePriceSlider($this, value);
        });
    });
}

function updatePriceSlider(elem, value) {
    if (elem.hasClass('price-min')) {
        console.log('min');
        priceSlider.noUiSlider.set([value, null]);
    } else if (elem.hasClass('price-max')) {
        console.log('max');
        priceSlider.noUiSlider.set([null, value]);
    }
}

function ResetBuyButtons() {
    $(".add-to-cart-btn").click(function () {
        var productCount = $("#productCount").val() === undefined ? 1 : $("#productCount").val();
        var selectedProduct = {
            Id: $(this).data("prodict-id"),
            Name: $(this).data("product-name"),
            Count: productCount,
            Price: $(this).data("product-price")
        };
        var productArray = localStorage.getObject("shoping-card") === null ? new Array() : localStorage.getObject("shoping-card");
        productArray.push(selectedProduct);
        localStorage.setObject("shoping-card", productArray);
        $("#shopingItemsCount").html(productArray.length);
    });
}

$(document).ready(function () {
    $("#filter").click(function () {
        CreateFilter();
    });

    $("#sortType").on("change", function () {
        CreateFilter();
    });

    ResetBuyButtons();
});

function CreateFilter() {
    var category = function () { };
    var selected = new Array();
    $(".checkbox-filter input[name='filter']:checked").each(function () {
        var cat = new category();
        cat.Id = $(this).data("id");
        cat.Name = $(this).data("name");
        selected.push(cat);
    });

    var filter = {
        Categories: selected,
        MinPrice: $("#price-min").val().replace(".", ","),
        MaxPrice: $("#price-max").val().replace(".", ","),
        Sort: $("#sortType").val()
    };

    ReloadList(filter);
}

function ReloadList(filter) {
    $.ajax({
        async: true,
        type: 'POST',
        dataType: 'json',
        contentType: "application/json;charset=utf-8",
        cache: false,
        url: '/Home/SortedProducts',
        data: JSON.stringify(filter),
        complete: function (data) {
            if (data.status === 200) {
                $("#productList").html(data.responseText);
                ResetBuyButtons();
            }
            else {
                $("#productList").html("Не удалось загрузить. ");
            }
        }
    });
}