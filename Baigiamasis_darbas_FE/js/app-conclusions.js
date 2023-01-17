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

const conclusionFormSbmBtn = document.querySelector('#conclusion-form-submit');
const conclusionForm = document.querySelector('#conclusion-form');

const queryString = window.location.search;
const urlParams = new URLSearchParams(queryString);



function addConclusion() {
    let data = new FormData(conclusionForm);
    let obj = {};

    console.log(data);

    data.forEach((value, key) => {
        obj[key] = value
    });

    fetch('https://localhost:7220/api/Conclusion/create', {
        method: 'post',
        headers: {
            'Accept': 'application/json, text/plain, */*',
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + window.localStorage.getItem('token')
        },
        // Naudojame JSON.stringify, nes objekte neturim .json() metodo
        body: JSON.stringify(obj) 
    })
    .then(async res => {
        console.log(res.status);

        if(res.ok)
        {
            // If success
            window.location.href = "./conclusions.html";
        }
        
        console.log(res);
        var resBody = await res.json();
        errorEle.textContent = resBody.message;
    })
    .catch((err) => console.log(err));
}





function getConclusions() {
    const url = 'https://localhost:7220/api/Conclusion/select/';
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
         for (const element of elements) {        
       
            let com =
            `<div id="${element.id}" class="printedDataContainer">
            <div class="name">${element.conclusion}</div>` +           
            '<div class="button_container"><input type="button" class="editButton" value="Edit" onClick="editConclusion(' + element.id + ')" />' +          
            '<input type="button" class="deleteButton" value="Delete" onClick="deleteConclusion(' + element.id + ')" /> </div> </div> '
            
            menuContainer.innerHTML += com;
        }                      
    });    
}
function renderStages(stages) {
    let result = "";
    stages?.forEach((value) => {
        result += value.stage + "<br/>";
    });
    return result
}

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



function updateCompany(companyId) {

    let data = new FormData(companyUpdateForm);
    let obj = {};

    // #1 iteracija -> obj {name: 'asd'}
    // #2 iteracija -> obj {type: 'asd'}
    data.forEach((value, key) => {
        // console.log(`${key}(Key): ${value}(Value)`);
        obj[key] = value
    });

    const url = 'https://localhost:7220/api/Company/update/' +companyId;

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
const editConclusion = (id) => {
    window.location.href = "./conclusion.html?id=" + id
}

conclusionFormSbmBtn.addEventListener('click', (e) => {
   // e.preventDefault(); // Breaks manual refresh after submit
   addConclusion();
})


getConclusions()




  

