// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

try {
    const btn1 = document.getElementById("upload");
    const file = document.getElementById("filename");
    // console.log(file);
    // console.log("Hello");
    btn1.addEventListener('change', function () {
        file.textContent = this.files[0].name;
        file.style.fontSize = "large";
    })
}
catch (err) { }

