$(function () {
    var $title = $("#title");
    var $subtitle = $("#subtitle");
    var $author = $("#author");
    var $cover = $("#cover");
    var $genre = $("#genre");
    var $rating = $("#rating");
    var $description = $("#description");
    var selectedBook = 0;

    //fade in book list
    $(".flex-container .flex-item").each(function () { $(this).fadeIn(300); });

    function checkWdith() {
        if ($(window).width() > 860) {
            $("#myGenres").show();
            $("#btn-genre").hide();
        }
        else {
            $("#myGenres").hide();
            $("#btn-genre").show();
        }
    };

    //check window width on load
    checkWdith();
    //check widnow width on resize
    $(window).resize(function () {
        checkWdith();
    });

    //function to parse parameters from query string
    function getUrlParameter(name) {
        name = name.replace(/[\[]/, '\\[').replace(/[\]]/, '\\]');
        var regex = new RegExp('[\\?&]' + name + '=([^&#]*)');
        var results = regex.exec(location.search);
        return results === null ? '' : decodeURIComponent(results[1].replace(/\+/g, ' '));
    };

    //select active Genre side link
    $(".genre-link.active").removeClass("active");
    var activeGenreId = getUrlParameter("GenreId");
    if (activeGenreId > 0) {
        //find child of #sidebar ul element with id parameter equal to GenreId in query string
        $("#sidebar ul").find("[id=" + activeGenreId + "]").addClass("active");
    }

    $("#btn-genre").click(function () { $("#myGenres").slideToggle() });

    $("#chkSwitch").click(function () {
        if($(this).is(":checked"))
            $(this).closest(".switch").addClass("active");
        else
            $(this).closest(".switch").removeClass("active");
    });

    $(".flex-item").click(function () {
        //don't change book info if clicked on the same book
        if (selectedBook !== $(this).attr("id")) {
            $("#bookInfo").hide();
            $cover.html("");
            $description.text("");
            $subtitle.text("");
            $rating.text("");

            $title.text($(this).find(".title span").text());
            $author.html($(this).children(".author").text());
            $cover.append("<img src='" + $(this).find("img").attr("src") + "' height='200' width='auto' />");
            $genre.html("<b>Genre: </b>" + $(this).children(".genre").text());


            //Google Books API version
            ////encodeURIComponents function encodes special characters, including: , / ? : @ & = + $ #
            var bookSrc = "https://www.googleapis.com/books/v1/volumes?q='" + encodeURIComponent($title.text() + "' '" + $author.text()) + "'&maxResults=1";
            $.getJSON(bookSrc, function (data) {
                console.log(data);
                $subtitle.text(data.items["0"].volumeInfo.subtitle);
                $rating.html("<b>Rating: </b>" + data.items["0"].volumeInfo.averageRating);
                $description.html("<b>Description: </b>" + data.items["0"].volumeInfo.description);
                $("#bookInfo").fadeIn(300);
                $("#bookInfo").offset({ top: $(window).scrollTop() + 100 });
            });

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

    //bookInfo panel stays fixed vertically but moves horizontally if window gets narrower
    // (I didn't use fixed position because bookInfo panel is taken out of the flow and if I shrink the window bookInfo panel covers the main book list)
    $(window).scroll(function () {
        $("#bookInfo").offset({ top: $(window).scrollTop() + 100 });

        //show-hide scroll-to-top button
        if ($(this).scrollTop() > 100)
            $(".to-top").slideDown(300);
        else
            $(".to-top").slideUp(300);
    });

    $("a.book-edit").click(function () {
        var $book = $(this).closest(".flex-item");
        var $modal = $("#newBookModal");
        var $src = $book.find("img").attr("src");
        $modal.find("input#Title").val($book.find(".title span").text());
        $modal.find("input#AuthorName").val($book.find("#hidAuthorName").val());
        $modal.find("input#AuthorSurname").val($book.find("#hidAuthorSurname").val());
        $modal.find("select#GenreId").val($book.find("#hidGenreId").val());
        $modal.find("#update-cover img").attr("src", $src);
        //$modal.find("input#image-upload").val($book.find("img").attr("src"));

        $("#add-book-popup .top-bar span").text("Edit book");
        $("#update-cover").show();
        $("#upload-box").hide();
    });

    $("#change-cover").click(function () {
        $("#upload-box").slideToggle();
    });

//SECTION: new genre
    //show new genre input field
    $("#btn-add-genre").click(function () {
        $("#new-genre-box").fadeIn().find("#Genre").focus();
        //have to hide this elements because disabled Submit button is seethrough
        $("select.ddl-genre, input#btn-add-genre").hide();
    });
    //disable submit button if genre textbox is empty
    $("#Genre").keyup(function () {
        if ($(this).val() !== "")
            $("#btn-submit-genre").removeClass("disabled");
        else
            $("#btn-submit-genre").addClass("disabled");
    });
    //show dropdown when new genre box loses focus
    $("#new-genre-box").focusout(function () {
        //if none of the new-genre-box childs is in focus
        if ($(this).has(document.activeElement).length == 0)
        {
            $(this).fadeOut();
            $("select.ddl-genre, input#btn-add-genre").show();
        }
    });
    $("#btn-submit-genre").click(function () {
        if ($("#Genre").val() !== "") {
            var $newGenre = $("#Genre.txt-add-genre").val();

            $.ajax({
                url: '/BookActions/CreateGenre',
                data: { 'genre': $newGenre },
                type: 'POST',
                dataType: 'text'
            })
            .success(function (results) {
                $("#GenreId").append("<option value='" + results + "'>" + $newGenre + "</option>");
            })
            .error(function (xhr, status) {
                alert(status);
            });
        }
        //$("#new-genre-box").fadeOut();
        $("#Genre").val("");
        $("select.ddl-genre").focus();
    });
//new genre

    //scroll to top
    $(".to-top").click(function () {
        $('html, body').animate({ scrollTop: 0 }, 500);
    });

    //on modal close
    $("#newBookModal").on('hidden.bs.modal', function () {
        //empty form fields on modal close
        $(this).find("input.form-control, select, input[type=file]").val("");
        //remove validation messages
        $(this).find("[data-valmsg-replace]")
            .removeClass("field-validation-error")
            .addClass("field-validation-valid")
            .empty();
        $(this).find("img").attr("src", "/Content/Images/noimage.jpg");

        $("#add-book-popup .top-bar span").text("Add book");
        $("#update-cover").hide();
        $("#upload-box").show();
        $("#new-genre-box").hide();
    })

    //if new book: data-target = 0
    //if edit book: data-targer = bookId
    $("[data-target='#newBookModal']").click(function () {
        $("#hiddenId").val($(this).data('id'));
    });
});