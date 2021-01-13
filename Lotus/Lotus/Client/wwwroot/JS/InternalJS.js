function StartCarousel() {
   
    $("#carouselLogin").carousel(
        {
            interval: 2000
        });
}

function setCookies(username,password) {
    document.cookie = "username=" + username;
    document.cookie = "password=" + password;

}
function checkRendering(id) {
    try {
        document.getElementById(id)
        return true;
    }
    catch (err) {
        return false;
    }
}
function getCookies() {
    return document.cookie;
}
function FileInputClick() {
    document.querySelector('input[type=file]').click()
}
const toBase64 = file => new Promise((resolve, reject) => {
    const reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = () => resolve(reader.result);
    reader.onerror = error => reject(error);
});

function previewFile(ID) {
    
    const preview = document.getElementById(ID);
    const file = document.querySelector('input[type=file]').files[0];
    const reader = new FileReader();

    reader.addEventListener("load", function () {
        // convert image file to base64 string
        preview.src = reader.result;
        
    }, false);

    if (file) {
        reader.readAsDataURL(file);
        
    }
}

function DisplayGetImage(ID) {
    const preview = document.getElementById(ID);
    return preview.src;
}
async function GetImage() {
    const file = document.querySelector('input[type=file]').files[0];
    var result = await toBase64(file);
    console.log(result);
    return result;
}

function GetText(id) {
    return document.getElementById(id).value;
}
function SetText(id, value) {
    console.log(value);
    document.getElementById(id).value = value;
}
function SetDate(ID,DateValue) {
    document.getElementById(ID).value = DateValue;
}