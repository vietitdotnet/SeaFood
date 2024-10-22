 
$(function () {
    var domain = document.getElementById("domain").value;
    $('#search-list').autocomplete({

        source: function (request, response) {
            document.getElementById("icon-load").style.display = "block";

            debugger
            $.ajax({
                url: "/Search/SearchProduct",
                data: { "prefix": request.term },
                dataType: "json",
                type: "POST",
                success: function (data) {
                    debugger
                    document.getElementById("icon-load").style.display = "none";
                    console.log(data);
                    response(data);
                },
                error: function (response) {
                    alert(response.responseText);
                },
                failure: function (response) {
                    alert(response.responseText);
                }
            })
        },
        appendTo: "#autocom-box",
        select: function (e, i) {

            debugger

            $("#hfProduct").val(i.item.slug);
            document.getElementById("submit-search").submit();
        },
        minLength: 1
    }).autocomplete("instance")._renderItem = function (ul, item) {

        var item = $('<div class="list_item_container d-flex flex-row bd-highlight mb-3"><div class="image-search-box"><img class="imageClass p-2 bd-highlight" src="' + item.img + '"></div><div class="label-item-search p-2 bd-highlight">' + item.name + '</div></div>')
        return $("<li>").append(item).appendTo(ul);
    };
});
