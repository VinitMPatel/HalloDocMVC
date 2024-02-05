const btn = document.getElementById("darkbtn");
const img = document.getElementById("darkimg");
const content = document.getElementById("main-content");
var flag ;
btn.addEventListener('click', dark)

function dark(){
    if(flag==0){
        document.querySelector('body').setAttribute('data-bs-theme' , 'dark');
        try{
            content.classList.remove("bg-white");
            content.classList.add("bg-dark");
        }
        catch(err){}
        img.src = "/images/dark_moon.png";
        document.cookie = "flag = " + flag;
        flag=1;
    }
    else{
        document.querySelector('body').setAttribute('data-bs-theme' , 'light');
        img.src = "/images/moon_light.png";
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
    console.log(array);
    flag = parseInt(array[1]);
    dark()
}