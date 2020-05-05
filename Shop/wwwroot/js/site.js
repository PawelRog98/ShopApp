// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function ShowPreview(input)
{
    if (input.files && input.files[0])
    {
        var ImageDir = new FileReader();
        ImageDir.onload = function (e)
        {
            $('#impPrev').attr('src', e.target.result);
        }
        ImageDir.readAsDataURL(input.files[0]);
    }
}

$(window).on("load", function () {
    $(".lds-ripple-wrapper").fadeOut("slow");
});

var scrollButton = document.getElementById("scroll-up-button");

window.onscroll = function() { scrollFunction() };

function scrollFunction() {
    if (document.body.scrollTop > 20 || document.documentElement.scrollTop > 20) {
        scrollButton.style.display = "block";
    } else {
        scrollButton.style.display = "none";
    }
}
function topFunction() {
    document.body.scrollTop = 0;
    document.documentElement.scrollTop = 0;
}