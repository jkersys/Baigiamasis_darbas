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

const companyUpdateFormSbmBtn = document.querySelector('#company-form-submit');
const companyUpdateForm = document.querySelector('#company-form');

const queryString = window.location.search;
const urlParams = new URLSearchParams(queryString);
getInvestigator(urlParams.get('id'));


function addCompany() {
    let data = new FormData(companyForm);
    let obj = {};

    console.log(data);

    data.forEach((value, key) => {
        obj[key] = value
    });

    fetch('https://localhost:7220/api/Company/create', {
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
            window.location.href = "./company.html";
        }
        
        console.log(res);
        var resBody = await res.json();
        errorEle.textContent = resBody.message;
    })
    .catch((err) => console.log(err));
}





function getInvestigator(investigatorId) {
    const url = 'https://localhost:7220/api/Investigator/' + investigatorId;
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
    const menuContainer = document.getElementById('investigator');
    let header =
        `<div class="printedDataContainer">
        <div class="name">Name</div>` +
        `<div class="lastname">Lastname Phone</div>` +
        `<div class="phoneNumber">Phone</div>` +
        `<div class="email">Phone Email</div>` +
        `<div class="cabinetNumber">Cabinet Nr.</div>` +
        `<div class="workplaceAdress">Adress</div>` +
        `<div class="certificateNumber">Sertificate Nr</div>` +
        `<div class="button_container">COMMANDS</div></div>`;
        
        menuContainer.innerHTML = header;
        
       
            let com =
            `<div id="${elements.investigatorId}" class="printedDataContainer">
            <div class="name">${elements.name}</div>` +
            `<div class="lastname">${elements.lastname}</div>` +
            `<div class="phoneNumber">${elements.phoneNumber}</div>` +
            `<div class="email">${elements.email}</div>` +
            `<div class="cabinetNumber">${elements.cabinetNumber}</div>` +
            `<div class="workplaceAdress">${elements.workplaceAdress}</div>` +
            `<div class="certificateNumber">${elements.certificateNumber}</div>` +            
            '<div class="button_container"><input type="button" class="editButton" value="Edit" onClick="editInvestigator(' + elements.investigatorId + ')" />' +          
            '<input type="button" class="deleteButton" value="Delete" onClick="deleteInvestigator(' + urlParams.get('id') + ')" /> </div> </div> '   
            menuContainer.innerHTML += com;
        }                      
);    
}
function renderStages(stages) {
    let result = "";
    stages?.forEach((value) => {
        result += value.stage + "<br/>";
    });
    return result
}

const deleteInvestigator = (id) => {
    const url = `https://localhost:7220/api/Investigator/delete/` + id
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


let toggleTodo = () => {    
    if (addInvestigatorForm.style.display === 'none' || addInvestigatorForm.style.display === '') {
        addInvestigatorForm.style.display = 'grid';
    } else {
        addInvestigatorForm.style.display = 'none';
    }
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

companyUpdateFormSbmBtn.addEventListener('click', (e) => {
   // e.preventDefault(); // Breaks manual refresh after submit
   updateCompany(urlParams.get('id'));
})


