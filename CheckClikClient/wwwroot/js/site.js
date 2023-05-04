var districtArray = [];
var cityArray = [];

$(document).ready(function () {
    districtArray = $('body').data('districts');
    cityArray = $('body').data('cities');
    $(".select").select2({ width: "100%" });
    var city = $('body').data('selectedCity');
    var district = $('body').data('selectedDist');
    if ((city == 0 || city == '') && (district == 0 || district == '')) {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(showPosition2, showError);
        }
    }
    else if ((city != 0 || city != '') && (district == 0 || district == '')) {
        var cities = cityArray;
        for (var i = 0; i < cities.length; i++) {
            if (cities[i].id == city) {
                $('#hdnidlat').val(cities[i].latitude);
                $('#hdnidlong').val(cities[i].longitude);
                $("#hdnlocation").val($("#hdnidlat").val() + ',' + $("#hdnidlong").val() + ',' + $("#select-district").val());
                $("#hdnIsLocationSet").val("1");
                $("#spncity").text(cities[i].nameEn);
                $("#select-district").val(district).trigger('change');
                $("#select-city").val(city).trigger('change');
                $('.select').select2({ width: "100%" }).trigger('change');
                $(document).trigger("my-event-afterLastDocumentReady");
            }
        }
    }
    else {
        $("#select-city").val(city).trigger('change');
        $("#spncity").text($("#select-city option:selected").text());
        var temp = districtArray;
        $("#select-district").empty();
        $('#select-district').append($('<option>', {
            value: "0",
            text: "All"
        }));
        for (var i = 0; i < temp.length; i++) {
            if (temp[i].cityId == city) {
                $('#select-district').append($('<option>', {
                    value: temp[i].id,
                    text: temp[i].nameEn
                }))
            }
        }
        $('.select').select2({ width: "100%" }).trigger('change');
        $("#select-district").val(district).trigger('change');
        for (var i = 0; i < temp.length; i++) {
            if (temp[i].id == district) {

                $('#hdnidlat').val(temp[i].latitude);
                $('#hdnidlong').val(temp[i].longitude);
                //console.log(temp[i].Latitude);
                $("#hdnlocation").val($("#hdnidlat").val() + ',' + $("#hdnidlong").val() + ',' + $("#select-district").val());
                $("#hdnIsLocationSet").val("1");
            }
        }
        $(document).trigger("my-event-afterLastDocumentReady");
    }
    readydropdown();
    typeAhead();
    $("#search").submit(function (e) {
        $("#hdnlocation").val($("#hdnidlat").val() + ',' + $("#hdnidlong").val() + ',' + $("#select-district").val());
    });
    $("#loader").hide();

});
$("#btnSearch").on("click", function () {
    debugger
    var searchText = $("#txtSearch").val();
    if (searchText != "") {
        $("#txtSearchTerm").val(searchText);
        $("#hdnSearchText").val(searchText);

        $("#search").submit();
    }
});
function showError(error) {
    switch (error.code) {
        case error.PERMISSION_DENIED:
            $("#select-city").val(3);
            $("#spncity").text($("#select-city option:selected").text());
            //var temp = Html.Raw(Session["DefaultDistrictListData"]);
            var temp = "";
            $("#select-district").empty();
            $('#select-district').append($('<option>', {
                value: "0",
                text: "All"
            }));
            for (var i = 0; i < temp.length; i++) {
                if (temp[i].CityId == 3) {
                    $('#select-district').append($('<option>', {
                        value: temp[i].id,
                        text: temp[i].nameEn
                    }))
                }
            }
            $("#select-district").val(3075);
            $('.select').select2({ width: "100%" }).trigger('change');
            for (var i = 0; i < temp.length; i++) {
                if (temp[i].id == 3075) {
                    $('#hdnidlat').val(temp[i].Latitude);
                    $('#hdnidlong').val(temp[i].Longitude);
                }
            }
            $(document).trigger("my-event-afterLastDocumentReady");
            break;
        case error.POSITION_UNAVAILABLE:
            $(document).trigger("my-event-afterLastDocumentReady");
            x.innerHTML = "Location information is unavailable."

            break;
        case error.TIMEOUT:
            $(document).trigger("my-event-afterLastDocumentReady");
            x.innerHTML = "The request to get user location timed out."
            break;
        case error.UNKNOWN_ERROR:
            $(document).trigger("my-event-afterLastDocumentReady");
            x.innerHTML = "An unknown error occurred."
            break;
    }
    $("#hdnlocation").val($("#hdnidlat").val() + ',' + $("#hdnidlong").val() + ',' + $("#select-district").val());
    $("#hdnIsLocationSet").val("1");
}

function showPosition2(position) {
    lat = position.coords.latitude;
    lon = position.coords.longitude;
    //console.log(location.hostname);
    $("#hdnidlat").val(position.coords.latitude);
    $('#hdnidlong').val(position.coords.longitude);

    var latlng = lat + ',' + lon;
    $.ajax({
        async: false,
        type: "get",
        url: '/Home/UpdateStoreLocation',
        datatype: "json",
        data: { coordinate: latlng },
        traditional: true,
        success: function (data) {
            var region = data[0];
            var city = data[1];
            var district = data[2];

            $('#hdnisusrloc').val(1);
            $('#select-city').val(city);
            $('#hdncity').val(city);
            $('#hdndistrict').val(district);
            //var temp = Html.Raw(Session["DefaultDistrictListData"]);
            //var temp = "";
            //console.log(AppUtils.DistrictList.ToList());
            //console.log(AppUtils.DistrictList.ToString());
            var temp = districtArray;//AppUtils.DistrictList.ToList();
            $("#select-district").empty();
            //$("#select-district1").empty();
            $('#select-district').append($('<option>', {
                value: "0",
                text: "All"
            }));
            for (var i = 0; i < temp.length; i++) {
                if (temp[i].cityId == city) {
                    console.log(temp[i].id);
                    $('#select-district').append($('<option>', {
                        value: '' + temp[i].id + '',
                        text: '' + temp[i].nameEn + ''

                    }));
                    //$('#select-district1').append($('<option>', {
                    //    value: ''+temp[i].id+'',
                    //    text: '' + temp[i].nameEn + ''

                    //}));
                    //$('#select-district').append($('<option>', {
                    //        value: '10',
                    //        text: 'asfsad'
                    //    }));
                    //$('#select-district1').append($('<option>', {
                    //        value: '10',
                    //        text: 'asfsad'
                    //    }));
                }
            }
        },
        error: function (xhr, status, error) {
            return false;
        },
        complete: function () {
            //readydropdown();
            var a = $('#hdncity').val();
            var b = $('#hdndistrict').val();
            $('#hdnisusrloc').val(1);
            $("#select-district").val(b).trigger('change');
            $("#select-city").val(a).trigger('change');
            $('.select').select2({ width: "100%" }).trigger('change');
            $("#spncity").text($("#select-city option:selected").text());
            $("#hdnlocation").val($("#hdnidlat").val() + ',' + $("#hdnidlong").val() + ',' + $("#select-district").val());
            $("#hdnIsLocationSet").val("1");
            //$(document).ready(function () {
            // This event will fire after all the other $(document).ready() functions have completed.
            // Usefull when your script is at the top of the page, but you need it run last
            $(document).trigger("my-event-afterLastDocumentReady");
            //});
        }
    });
}

function readydropdown() {
}
function typeAhead() {
    var $input = $("#txtSearchTerm");
    $input.typeahead({
        source: function (query, process) {
            var results = [];
            map = {};
            if (query.length > 1) {
                return $.post('/Home/Autocomplete)', { q: query, lang: 'en' }, function (data) {
                    $.each(data, function (i, result) {
                        map[result] = result;
                        results.push(result);
                    });

                    process(results);
                });
            }
        },
        updater: function (item) {
            $("#txtSearchTerm").text(item.trim());
            //window.location = "search?q=" + item.trim();
            //$("#search").submit();
            return item;
        },
        autoSelect: false
        //autoSelect: true
    });
    $input.change(function (e) {
        if (e.which == 13) {
            //alert('You pressed enter!');
        }
        var current = $input.typeahead("getActive");
        if (current) {
            if (current.toLowerCase() == $input.val().toLowerCase()) {
                // This means the exact match is found. Use toLowerCase() if you want case insensitive match.
            } else {
                // This means it is only a partial match, you can either add a new item
                // or take the active if you don't want new items
            }
        } else {
            // Nothing is active so it is a new value (or maybe empty value)
        }
    });
}


function removeL1(id) {

    $.ajax({
        async: false,
        type: "post",
        url: '/Cart/UpdateProductStatus',
        datatype: "json",
        data: { CartId: id, quantity: 0, flagid: 2 },
        traditional: true,
        success: function (data) {
            var v = data.success;
            if (v == true) {
                location.reload();
            }
        },
        error: function (xhr, status, error) {
            return false;
        }
    });
}

function MinusQtyL1(id) {

    var min = $('#hdnMinL1_' + id).val();
    var max = $('#hdnMaxL1_' + id).val();
    var act = $('#txtvalL1_' + id).val();
    var price = $('#lblpriceL1_' + id).text();

    if (min < act && max >= act) {

        var a = parseInt(act);
        if (a >= 1) {
            a--;
        }
        var fnl = a * parseFloat(price);

        $('#txtvalL1_' + id).val(a);

        $.ajax({
            async: false,
            type: "post",
            url: '/Cart/UpdateProductStatus',
            datatype: "json",
            data: { CartId: id, quantity: a, flagid: 1 },
            traditional: true,
            success: function (data) {
                var v = data.success;
                if (v == true) {
                    location.reload();
                }
            },
            error: function (xhr, status, error) {
                return false;
            }
        });
    }
    else {
        $('#loader').hide();
    }
}
function PlusQtyL1(id) {

    var min = $('#hdnMinL1_' + id).val();
    var max = $('#hdnMaxL1_' + id).val();
    var act = $('#txtvalL1_' + id).val();
    var price = $('#lblpriceL1_' + id).text();

    if (min <= act && max > act) {
        var a = parseInt(act);
        a++;
        $('#txtvalL1_' + id).val(a);
        var fnl = a * parseFloat(price);
        //$('#spnL1_' + id).html(fnl);

        $.ajax({
            async: false,
            type: "post",
            url: '/Cart/UpdateProductStatus',
            datatype: "json",
            data: { CartId: id, quantity: a, flagid: 1 },
            traditional: true,
            success: function (data) {
                var v = data.success;
                if (v == true) {
                    location.reload();
                }
            },
            error: function (xhr, status, error) {
                return false;
            }
        });
    }
    else {
    }
}


var city = "";

/* Function to Open the side nav panel */
function openNav() {
    $("#sideNavUserGuide").toggleClass("open");
    var overlay = $('.overlay');
    overlay.show();
}
/* Function to close the side nav panel */
$(".user-guide-header button.close").click(function closeNav() {
    $("#sideNavUserGuide").removeClass("open");
    var overlay = $('.overlay');
    overlay.hide();
});

$(document).ready(function () {
    $("#select-city option:contains(" + ('Riyadh').trim() + ")").attr('selected', 'selected');
    //$('.customer-logos').slick({
    //    slidesToShow: 5,
    //    slidesToScroll: 1,
    //    autoplay: true,
    //    autoplaySpeed: 2500,
    //    arrows: false,
    //    dots: false,
    //    pauseOnHover: false,
    //    responsive: [{
    //        breakpoint: 1367,
    //        settings: {
    //            slidesToShow: 5
    //        }
    //    }, {
    //        breakpoint: 1024,
    //        settings: {
    //            slidesToShow: 4
    //        }
    //    }, {
    //        breakpoint: 768,
    //        settings: {
    //            slidesToShow: 3
    //        }
    //    }, {
    //        breakpoint: 520,
    //        settings: {
    //            slidesToShow: 2
    //        }
    //    }]
    //});
});

function fnupdatecitydistrict() {
    var cityid = $("#select-city option:selected").val();
    var districtid = $("#select-district option:selected").val();

    $.ajax({
        url: '/Base/ChangeCityDistrict',
        data: { CityId: cityid, DistrictId: districtid },
        success: function (response) {
            if (districtid == 0 || districtid == "") {
                var tempcity = "";
                //var tempcity = Html.Raw(Session["DefaultCityListData"]);
                for (var i = 0; i < tempcity.length; i++) {
                    if (tempcity[i].id == cityid) {
                        debugger
                        $('#hdnidlat').val(tempcity[i].latitude);
                        $('#hdnidlong').val(tempcity[i].longitude);
                    }
                }
            }
            location.reload();
        },
        error: function (response) {

        }
    });
    //var temp = Html.Raw(Session["DefaultDistrictListData"]);
    var temp = districtArray;
    for (var i = 0; i < temp.length; i++) {
        if (temp[i].id == districtid) {
            $('#hdnidlat').val(temp[i].latitude);
            $('#hdnidlong').val(temp[i].longitude);
        }
    }
}
$("#select-city").change(function (e) {
    $("#spncity").text($("#select-city option:selected").text());
    var cityid = $("#select-city option:selected").val();
    $("#select-district").empty();
    //$("#select-district1").empty();
    //var temp = Html.Raw(Session["DefaultDistrictListData"]);
    var temp = districtArray;
    $('#select-district').append($('<option>', {
        value: "0",
        text: "All"
    }));
    //$('#select-district1').append($('<option>', {
    //    value: "0",
    //    text: "All"
    //}));
    for (var i = 0; i < temp.length; i++) {
        if (temp[i].cityId == cityid) {
            $("#select-district").append($('<option>', {
                value: temp[i].id,
                text: temp[i].nameEn
            }));
            //$("#select-district1").append($('<option>', {
            //    value: temp[i].id,
            //    text: temp[i].nameEn
            //}));
        }
    }
    if ($('#hdnisusrloc').val() == 0) {
        var districtid = $("#select-district option:selected").val();
        for (var i = 0; i < temp.length; i++) {
            if (temp[i].id == districtid) {
                $('#hdnidlat').val(temp[i].latitude);
                $('#hdnidlong').val(temp[i].longitude);
            }
        }
    }
    //}
});

function binddistrict() {
    $("#select-district").empty();
    var cityid = $("#select-city option:selected").val();
    //var temp = Html.Raw(Session["DefaultDistrictListData"]);
    var temp = districtArray;
    $('#select-district').append($('<option>', {
        value: "0",
        text: "All"
    }));
    //$('#select-district1').append($('<option>', {
    //    value: "0",
    //    text: "All"
    //}));
    for (var i = 0; i < temp.length; i++) {
        if (temp[i].cityId == cityid) {
            $("#select-district").append($('<option>', {
                value: temp[i].id,
                text: temp[i].nameEn
            }));
            //$("#select-district1").append($('<option>', {
            //    value: temp[i].id,
            //    text: temp[i].nameEn
            //}));
        }
    }
}

$('.city.dropdown-menu').click(function (e) {
    e.stopPropagation();
});

$(document).on("click", "input[class='select2-search__field']", function (e) {
    e.stopPropagation();
});
