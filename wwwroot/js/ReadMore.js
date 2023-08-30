
var mainimage = document.querySelector('.First img');
var modalimage = document.querySelectorAll('.Morepic img');

modalimage.forEach(function (x) {
    x.addEventListener('click', function () {

        mainimage.src = x.src

    });
});