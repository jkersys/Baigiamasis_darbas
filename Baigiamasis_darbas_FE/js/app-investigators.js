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

const investigatorFormSbmBtn = document.querySelector('#investigator-form-submit');
const investigatorForm = document.querySelector('#investigator-form');
const editFormSbmBtn = document.querySelector('#investigatorEdit-form-submit');
const editInvestigatorForm = document.getElementById('investigatorId');


const queryString = window.location.search;
const urlParams = new URLSearchParams(queryString);

function addInvestigator() {
    let data = new FormData(investigatorForm);
    let obj = {};

    console.log(data);

    data.forEach((value, key) => {
        obj[key] = value
    });

    fetch('https://localhost:7220/api/Investigator/create', {
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
            window.location.href = "./investigators.html";
        }
        
        console.log(res);
        var resBody = await res.json();
        errorEle.textContent = resBody.message;
    })
    .catch((err) => console.log(err));
}

investigatorFormSbmBtn.addEventListener('click', (e) => {
    e.preventDefault(); // Breaks manual refresh after submit
    addInvestigator();
})


function getInvestigators() {
    const url = 'https://localhost:7220/api/Investigator/investigators/';
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
        const menuContainer = document.getElementById('investigators');
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
         for (const element of elements) {        
       
            let com =
            `<div id="${element.investigatorId}" class="printedDataContainer">
            <div class="name">${element.name}</div>` +
            `<div class="lastname">${element.lastname}</div>` +
            `<div class="phoneNumber">${element.phoneNumber}</div>` +
            `<div class="email">${element.email}</div>` +
            `<div class="cabinetNumber">${element.cabinetNumber}</div>` +
            `<div class="workplaceAdress">${element.workplaceAdress}</div>` +
            `<div class="certificateNumber">${element.certificateNumber}</div>` +            
            '<div class="button_container"><input type="button" class="editButton" value="Edit" onClick="editInvestigator(' + element.investigatorId + ')" />' +          
            '<input type="button" class="deleteButton" value="Delete" onClick="deleteInvestigator(' + element.investigatorId + ')" /> </div> </div> '
            
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


const editInvestigator = (id) => {
    window.location.href = "./investigator.html?id=" + id
}


// //veikia
// editFormSbmBtn.addEventListener('click', (e) => {
//     e.preventDefault(); // Breaks manual refresh after submit
//     editInvestigation(editInvestigatorForm);
// })

// const editInvestigation = (editInvestigatorForm) => {
//     window.location.href = "./investigator.html?id=" + editInvestigatorForm.value
// }


getInvestigators()



  

