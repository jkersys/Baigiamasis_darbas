//Tikrinam ar prisijungęs
let isConected = () => {
    KeyName = Object.getOwnPropertyNames(localStorage)
    if (KeyName != "token") {
        localStorage.clear();
        window.location.href = '../index.html'
    }
}
//nuskaitom raktą
let KeyName = Object.getOwnPropertyNames(localStorage)
setInterval(isConected, 1000)

const conclusionUpdateFormSbmBtn = document.querySelector('#conclusion-form-submit');
const conclusionUpdateForm = document.querySelector('#conclusion-form');

const queryString = window.location.search;
const urlParams = new URLSearchParams(queryString);
getConclusion(urlParams.get('id'));


function getConclusion(conclusionId) {
    const url = 'https://localhost:7220/api/Conclusion/' + conclusionId;
const options = {
    headers: {
        'Accept': 'application/json, text/plain, */*',
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + window.localStorage.getItem('token')
    },
    method: 'get',    
}
    fetch(url, options)
    .then((response) => response.json())
    .then((complains) => {
        const elements = complains;
        console.log(elements);
        const menuContainer = document.getElementById('conclusions');
        let header =
        `<div class="printedDataContainer">
        <div class="name">Name</div>` +       
        `<div class="button_container">COMMANDS</div></div>`;
        
        menuContainer.innerHTML = header;
       
            let com =
            `<div id="${elements.id}" class="printedDataContainer">
            <div class="name">${elements.conclusion}</div>` +           
      
            '<input type="button" class="deleteButton" value="Delete" onClick="deleteConclusion(' + urlParams.get('id') + ')" /> </div> </div> '
            
            menuContainer.innerHTML += com;
        }                      
    );    
}


function updateConclusion(companyId) {

    let data = new FormData(conclusionUpdateForm);
    let obj = {};

    // #1 iteracija -> obj {name: 'asd'}
    // #2 iteracija -> obj {type: 'asd'}
    data.forEach((value, key) => {
        // console.log(`${key}(Key): ${value}(Value)`);
        obj[key] = value
    });

    const url = 'https://localhost:7220/api/Conclusion/update/' +companyId;

    fetch(url, {
        method: 'put',
        headers: {
            'Accept': 'application/json, text/plain, */*',
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + window.localStorage.getItem('token')
        },
        // Naudojame JSON.stringify, nes objekte neturim .json() metodo
        body: JSON.stringify(obj) 
    })
    .then(obj => {
        const res = obj.json()
        console.log(res);
        return res;
    })
    .catch((klaida) => console.log(klaida));
}

conclusionUpdateFormSbmBtn.addEventListener('click', (e) => {
   // e.preventDefault(); // Breaks manual refresh after submit
   updateConclusion(urlParams.get('id'));
})

const deleteConclusion = (id) => {
    const url = `https://localhost:7220/api/Conclusion/delete/` + id
    const optionsFetchPosts = {
        method: 'delete',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + window.localStorage.getItem('token')
        }
    }
    fetch(url, optionsFetchPosts)
        .then(() => getConclusions())
        .catch((error) => {
            console.log(`Request failed with error: ${error}`);
        })
}


urlParams.get('id')