const moonBtn = document.getElementById("darkbtn");
const img = document.getElementById("darkimg");
const content = document.getElementById("main-content");
var flag;

if (moonBtn != null) {
    moonBtn.addEventListener('click', dark)
}

function dark(){
    if(flag==0){
        document.querySelector('body').setAttribute('data-bs-theme' , 'dark');
        try{
            content.classList.remove("bg-white");
            content.classList.add("bg-dark");
        }
        catch (err) { }
        if (img != null) {
            img.src = "/images/dark_moon.png";
        }
        document.cookie = "flag = " + flag;
        flag=1;
    }
    else{
        document.querySelector('body').setAttribute('data-bs-theme', 'light');
        if (img != null) {
            img.src = "/images/moon_light.png";
        }
        try{
            content.classList.remove("bg-dark");
            content.classList.add("bg-white");
        }
        catch(err){}
        document.cookie = "flag = " + flag;
        flag=0;
    }
}

window.onload = function(){
    var array = document.cookie.split("=");
    flag = parseInt(array[1]);
    dark()
}