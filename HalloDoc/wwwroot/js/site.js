// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

try {
    const btn1 = document.getElementById("upload");
    var names = "";
    const file = document.getElementById("filename");
    // console.log(file);
    // console.log("Hello");
    btn1.addEventListener('change', function () {
        file.placeholder = this.files[0].name;
        for (var i = 0; i < this.files.length; i++) {
            names += this.files[i].name;
            }
        file.value = names;
        console.log(file.value);
        console.log(btn1.value);
        file.style.fontSize = "large";
    })
}
catch (err) { }

