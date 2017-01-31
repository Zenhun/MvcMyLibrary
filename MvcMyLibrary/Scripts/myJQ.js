$(function () {
    var $title = $("#title");
    var $subtitle = $("#subtitle");
    var $author = $("#author");
    var $cover = $("#cover");
    var $genre = $("#genre");
    var $rating = $("#rating");
    var $description = $("#description");
    var selectedBook = 0;

    $(".flex-container .flex-item").each(function () { $(this).fadeIn(300);});

    //function to parse parameters from query string
    function getUrlParameter(name) {
        name = name.replace(/[\[]/, '\\[').replace(/[\]]/, '\\]');
        var regex = new RegExp('[\\?&]' + name + '=([^&#]*)');
        var results = regex.exec(location.search);
        return results === null ? '' : decodeURIComponent(results[1].replace(/\+/g, ' '));
    };

    $(".flex-item").not($(".book-icons")).click(function () {
        //don't change book info if clicked on the same book
        if (selectedBook !== $(this).attr("id")) {
            $("#bookInfo").hide();
            $cover.html("");
            $description.text("");
            $subtitle.text("");
            $rating.text("");

            $title.text($(this).children(".title").text());
            $author.html($(this).children(".author").text());
            //$cover.append("<img src='" + $(this).find("img").attr("src") + "' />");
            $genre.html($(this).children(".genre").text());


            //Google Books API version
            var bookSrc = "https://www.googleapis.com/books/v1/volumes?q='" + encodeURIComponent($title.text() + "' '" + $author.text()) + "'&maxResults=1";
            $.getJSON(bookSrc, function (data) {
                console.log(data);
                //$bookTitle.text(data.items["0"].volumeInfo.title);
                $subtitle.text(data.items["0"].volumeInfo.subtitle);
                $cover.append("<img src='" + data.items["0"].volumeInfo.imageLinks.thumbnail + "' />");
                $rating.text(data.items["0"].volumeInfo.averageRating);
                $description.text(data.items["0"].volumeInfo.description);
                $("#bookInfo").fadeIn(300);
                $("#bookInfo").offset({ top: $(window).scrollTop() + 100 });
            });


            ////Goodreads API version

            ////Goodreads API returns xml data but forces me to use dataType: "jsonp" because of cross-origin ajax request
            ////that's why I'm using yahoo's YQL (Yahoo Query Language) that's serves as a json proxy

            ////encodeURIComponents function encodes special characters, including: , / ? : @ & = + $ #
            //var bookSrc = "https://www.goodreads.com/search.xml?key=V6Iwmnm75zR91VuldBgZgw&q='" + encodeURIComponent($title.text() + "' '" + $author.text() + "'");

            //$.get("http://query.yahooapis.com/v1/public/yql",
            //    {
            //        q: "select * from xml where url=\"" + bookSrc + "\"",
            //        format: "xml"
            //    },
            //    function (xml) {
            //        // contains XML with the following structure:
            //        // <query>
            //        //   <results>
            //        //     <GoodreadsResponse>
            //        //        ...
            //        console.log(xml);
            //        console.log("Title: " +$(xml).find("work").first().find("title").text());
            //    }
            //);

            selectedBook = $(this).attr("id");
        }
    });

    //close bookInfo panel
    $("#close-bookInfo").click(function () {
        $("#bookInfo").fadeOut(200);
        //reset "selected" otherwise I can't reopen book Info of the same book I just closed with "close-bookInfo"
        //because "selected" still has the sam book id
        selectedBook = 0;
    });

    $(".to-top").click(function () {
        $('html, body').animate({ scrollTop: 0 }, 500);
    });

    //bookInfo panel stays fixed vertically but moves horizontally if window gets narrower
    // (I didn't use fixed position because bookInfo panel is taken out of the flow and if I shrink the window bookInfo panel covers the main book list)
    $(window).scroll(function () {
        $("#bookInfo").offset({ top: $(window).scrollTop() + 100 });

        //show-hide scroll-to-top button
        if($(this).scrollTop() > 100)
            $(".to-top").slideDown(300);
        else
            $(".to-top").slideUp(300);
    });

    //select active Genre side link
    $(".genre-link.active").removeClass("active");
    var activeGenreId = getUrlParameter("GenreId");
    if (activeGenreId > 0) {
        //find child of #sidebar ul element with id parameter equal to GenreId in query string
        $("#sidebar ul").find("[id=" + activeGenreId + "]").addClass("active");
    }

    ////prevent vertical scroll bar on New Book modal popup
    //$('#newBookModal').on('show.bs.modal', function () {
    //    $('body, .navbar').css("margin-right", "0px");
    //});

    //empty form fields on modal close (not working)
    $("#newBookModal").on('hidden.bs.modal', function () {
        //$(this).find('form')[0].reset();
        $(this).find("input[type=text]").val();
    })
});