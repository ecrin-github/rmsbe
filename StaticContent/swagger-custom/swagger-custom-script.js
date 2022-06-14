(function () {
    window.addEventListener("load", function () {
        setTimeout(function () {
            var logo = document.getElementsByClassName('link'); 
            logo[0].href = "https://ecrin.org";
            logo[0].target = "_blank";
            logo[0].children[0].alt = "Link to ECRIN main page";
            logo[0].children[0].src = "/StaticContent/Images/ecrin-logo.jpg"; 
        });
    });
})();