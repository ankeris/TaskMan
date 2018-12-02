// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
// Write your JavaScript code.
// on page load...
moveProgressBar();
// on browser resize...

window.addEventListener('resize', function () {
    moveProgressBar();
});

// SIGNATURE PROGRESS
function moveProgressBar() {
    var allProjects = document.querySelectorAll('.progress-wrap');
    allProjects.forEach(prj => {
        var getPercent = (prj.getAttribute("data-progress-percent") / 100);
        console.log(getPercent);
        prj.firstElementChild.style.width = prj.getAttribute("data-progress-percent") + '%';
        prj.firstElementChild.style.backgroundColor = getColor(getPercent);
    })
}

function getColor(p) {
    if (p <= 0.1 && p >= 0) {
        return "rgb(" + 255 + "," + 20 + "," + 20 + ")";
    }
    if (p <= 0.3 && p >= 0.1) {
        return "rgb(" + 220 + "," + 40 + "," + 20 + ")";
    }
    if (p <= 0.5 && p >= 0.3) {
        return "rgb(" + 255 + "," + 110 + "," + 30 + ")";
    }
    if (p <= 0.7 && p >= 0.5) {
        return "rgb(" + 255 + "," + 150 + "," + 51 + ")";
    }
    if (p <= 0.9 && p >= 0.7) {
        return "rgb(" + 160 + "," + 225 + "," + 20 + ")";
    }
    if (p < 1 && p >= 0.9) {
        return "rgb(" + 20 + "," + 235 + "," + 20 + ")";
    }
    if (p == 1) {
        return "rgb(" + 40 + "," + 255 + "," + 40 + ")";
    }
}