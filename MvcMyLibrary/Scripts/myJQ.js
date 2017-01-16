$(function () {
    var $title = $("#title");
    var $subtitle = $("#subtitle");
    var $author = $("#author");
    var $cover = $("#cover");
    var $genre = $("#genre");
    var $rating = $("#rating");
    var $description = $("#description");
    var selected = 0;

    //function to parse parameters from query string
    function getUrlParameter(name) {
        name = name.replace(/[\[]/, '\\[').replace(/[\]]/, '\\]');
        var regex = new RegExp('[\\?&]' + name + '=([^&#]*)');
        var results = regex.exec(location.search);
        return results === null ? '' : decodeURIComponent(results[1].replace(/\+/g, ' '));
    };

    $(".flex-item").not($(".book-icons")).click(function () {
        //don't change book info if clicked on the same book
        if (selected !== $(this).attr("id")) {
            $("#bookInfo").hide();
            $cover.html("");
            $description.text("");
            $subtitle.text("");
            $rating.text("");

            $title.text($(this).children(".title").text());
            $author.html($(this).children(".author").text());
            //$cover.append("<img src='" + $(this).find("img").attr("src") + "' />");
            $genre.html($(this).children(".genre").text());

            //var bookSrc = "https://www.googleapis.com/books/v1/volumes?q=" + $title.text() + " " + $author.text() + "&maxResults=1";
            var bookSrc = "https://www.goodreads.com/search.xml?key=V6Iwmnm75zR91VuldBgZgw&q=" + $title.text() + " " + $author.text() + "&callback=?";
            //var bookSrc = "https://www.goodreads.com/search.xml?key=V6Iwmnm75zR91VuldBgZgw&q=" + $title.text() + " " + $author.text();

            //$.getJSON(bookSrc, function (data) {
            //    console.log(data);
            //    //$bookTitle.text(data.items["0"].volumeInfo.title);
            //    $subtitle.text(data.items["0"].volumeInfo.subtitle);
            //    $cover.append("<img src='" + data.items["0"].volumeInfo.imageLinks.thumbnail + "' />");
            //    $rating.text(data.items["0"].volumeInfo.averageRating);
            //    $description.text(data.items["0"].volumeInfo.description);
            //    //$("#bookInfo").show();
            //});

            $.ajax(bookSrc, {
                dataType: "jsonp",
                //jsonpCallback: "success",
                success: function (data) {
                    console.log(data);
                    //$bookTitle.text(data.items["0"].volumeInfo.title);
                    $subtitle.text(data.items["0"].volumeInfo.subtitle);
                    $cover.append("<img src='" + data.items["0"].volumeInfo.imageLinks.thumbnail + "' />");
                    $rating.text(data.items["0"].volumeInfo.averageRating);
                    $description.text(data.items["0"].volumeInfo.description);
                    $("#bookInfo").fadeIn();
                }
            });

            selected = $(this).attr("id");
        }
    });

    //bookInfo panel stays fixed vertically but moves horizontally if window gets narrower
    // (I didn't use fixed position because bookInfo panel is taken out of the flow and if I shrink the window bookInfo panel covers the main book list)
    $(window).scroll(function () {
        $("#bookInfo").offset({ top: $(window).scrollTop() + 100});
    });

    //select active Genre side link
    $(".genre-link.active").removeClass("active");
    var activeGenreId = getUrlParameter("GenreId");
    if (activeGenreId > 0) {
        //find child of #sidebar ul element with id parameter equal to GenreId in query string
        $("#sidebar ul").find("[id=" + activeGenreId + "]").addClass("active");
    }
});