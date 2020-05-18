
for (let i = 0; i < stars.length; i++) {
    stars[i].addEventListener("mouseover", function () {
        for (let j = index; j < stars.length; j++) {
            stars[j].classList.remove("fa-star");
            stars[j].classList.add("fa-star-o");
        }
        for (let j = 0; j <= i; j++) {
            stars[j].classList.remove("fa-star-o");
            stars[j].classList.add("fa-star");
        }
    })
    stars[i].addEventListener("click", function () {
        index = i;
        ratingValue = i + 1;
        $.ajax({
            type: "GET",
            url: '@Url.Page("details", "rating")',
            data: { value: ratingValue, tmdbid: '@Model.Movie.TmdbId' },
            contentType: "application/json; charset=utf-8",
            dataType: "json"
        })
            .done(function (data) {
                //alert("success");
                console.log(data);
            })
            .fail(function (errorThrown) {
                alert("error");
            });
    })
    stars[i].addEventListener("mouseout", function () {
        for (let j = 0; j < stars.length; j++) {
            stars[j].classList.remove("fa-start");
            stars[j].classList.add("fa-star-o");
        }
        for (let j = 0; j <= index; j++) {
            stars[j].classList.remove("fa-star-o");
            stars[j].classList.add("fa-star");
        }
    })
}