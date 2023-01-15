const loginForm  = document.querySelector('#login-form');
const loginFormSbmBtn = document.querySelector('#login-form-submit');
const errorEle = document.querySelector(".error-message");

function sendData() {
    let data = new FormData(loginForm);
    let obj = {};

    console.log(data);

    data.forEach((value, key) => {
        obj[key] = value
    });

    fetch('https://localhost:7220/api/User/login', {
        method: 'post',
        headers: {
            'Accept': 'application/json, text/plain, */*',
            'Content-Type': 'application/json'
        },
        // Naudojame JSON.stringify, nes objekte neturim .json() metodo
        body: JSON.stringify(obj) 
    })
    .then(async res => {
        console.log(res.status);
        if(res.ok)
        {
            // If success
            window.location.href = "./index.html";
        }
        
        console.log(res);
        var resBody = await res.json();
        window.localStorage.setItem('token', resBody.token);
        
    })
    .catch((err) => console.log(err));
    
}

loginFormSbmBtn.addEventListener('click', (e) => {
    e.preventDefault(); // Breaks manual refresh after submit
    sendData();
})