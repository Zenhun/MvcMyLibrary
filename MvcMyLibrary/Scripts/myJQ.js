$(function () {
    var $title = $("#title");
    var $subtitle = $("#subtitle");
    var $author = $("#author");
    var $cover = $("#cover");
    var $genre = $("#genre");
    var $rating = $("#rating");
    var $description = $("#description");

    $(".flex-item").click(function () {
        $("#bookInfo").show();
        $cover.html("");
        $description.text("");
        $subtitle.text("");
        $rating.text("");
        $title.html($(this).find(".title").text());
        $author.html($(this).find(".author").text());
        //$cover.append("<img src='" + $(this).find("img").attr("src") + "' />");
        $genre.html($(this).find(".genre").text());

        var bookSrc = "https://www.googleapis.com/books/v1/volumes?q=" + $title.text() + " " + $author.text() + "&maxResults=1";

        $.getJSON(bookSrc, function (data) {
            console.log(data);
            //$bookTitle.text(data.items["0"].volumeInfo.title);
            $subtitle.text(data.items["0"].volumeInfo.subtitle);
            $cover.append("<img src='" + data.items["0"].volumeInfo.imageLinks.thumbnail + "' />");
            $rating.text(data.items["0"].volumeInfo.averageRating);
            $description.text(data.items["0"].volumeInfo.description);
        });
    });
});