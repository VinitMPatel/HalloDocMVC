// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

try {
    const btn1 = document.getElementById("upload");
    var names = "";
    const file = document.getElementById("file-choosen");

    btn1.addEventListener('change', function () {
    
        for (var i = 0; i < this.files.length; i++) {
            names += this.files[i].name + ", ";
        }
        file.textContent = names;
        file.style.fontSize = "large";
    })
}
catch (err) { }

