$(function () {
    var $title = $("#title");
    var $subtitle = $("#subtitle");
    var $author = $("#author");
    var $cover = $("#cover");
    var $genre = $("#genre");
    var $rating = $("#rating");
    var $description = $("#description");
    var selected = 0;

    $(".flex-item").not($(".book-icons")).click(function () {
        //don't change book info if clicked on the same book
        if (selected !== $(this).attr("id")) {

            $("#bookInfo").hide();
            $cover.html("");
            $description.text("");
            $subtitle.text("");
            $rating.text("");

            $title.html($(this).find(".title").text());
            $author.html($(this).find(".author").text());
            //$cover.append("<img src='" + $(this).find("img").attr("src") + "' />");
            $genre.html($(this).find(".genre").text());

            var bookSrc = "https://www.googleapis.com/books/v1/volumes?q=" + $title.text() + " " + $author.text() + "&maxResults=1";
            //var bookSrc = "https://www.goodreads.com/search.xml?key=V6Iwmnm75zR91VuldBgZgw&q=" + $title.text() + " " + $author.text();
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
                //dataType: "jsonp",
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
});