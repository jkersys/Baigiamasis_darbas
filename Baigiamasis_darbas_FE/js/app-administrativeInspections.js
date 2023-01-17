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

const investigationForm = document.querySelector('#investigation-form');
const investigationFormSbmBtn = document.querySelector('#investigation-form-submit');
const errorEle = document.querySelector(".error-message");




function addInvestigation() {
    let data = new FormData(investigationForm);
    let obj = {};

    console.log(data);

    data.forEach((value, key) => {
        obj[key] = value
    });

    fetch('https://localhost:7220/api/AdministrativeInspection/inspection/create', {
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
            window.location.href = "./investigation.html";
        }
        
        console.log(res);
        var resBody = await res.json();
        errorEle.textContent = resBody.message;
    })
    .catch((err) => console.log(err));
}




//Load companies list
let dropdownCompanies = document.querySelector('#companyId');
//dropdown.length = 0;
let defaultOptionCompanies = document.createElement('option');
defaultOptionCompanies.text = 'select';
//dropdownCompanies.add(defaultOption);
dropdownCompanies.selectedIndex = 0;
function loadCompaniesData() {
    const url = 'https://localhost:7220/api/Company/companies/list';
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
            let data          
            for (let i = 0; i < elements.length; i++) {
                data = document.createElement('option');
                data.text = elements[i].companyName;
                data.value = elements[i].companyId;
                dropdownCompanies.add(data);                   

        }
                        
        });   
}




// function sendData() {
//     let data = new FormData(complainForm);
//     let obj = {};

//     console.log(data);

//     data.forEach((value, key) => {
//         obj[key] = value
//     });

//     fetch('https://localhost:7220/api/Investigation/investigations', {
//         method: 'post',
//         headers: {
//             'Accept': 'application/json, text/plain, */*',
//             'Content-Type': 'application/json',
//             'Authorization': 'Bearer ' + window.localStorage.getItem('token')
//         },
//         // Naudojame JSON.stringify, nes objekte neturim .json() metodo
//         body: JSON.stringify(obj) 
//     })
//     .then(async res => {
//         console.log(res.status);

//         if(res.ok)
//         {
//             // If success
//             window.location.href = "./index.html";
//         }
        
//         console.log(res);
//         var resBody = await res.json();
//         errorEle.textContent = resBody.message;
//     })
//     .catch((err) => console.log(err));
// }

function renderStages(stages) {
    let result = "";
    stages?.forEach((value) => {
        result += value.stage + "<br/>";
    });
    return result;
}

function renderInvestigators(investigator) {
    let result = "";
    investigator?.forEach((value) => {
        result += value.nameAndLastName + "<br/>";
    });
    return result;
}

investigationFormSbmBtn.addEventListener('click', (e) => {
    e.preventDefault(); // Breaks manual refresh after submit
    addInvestigation();
})


//get all complains


function loadData() {
    const url = 'https://localhost:7220/api/AdministrativeInspection/inspections';
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
        const menuContainer = document.getElementById('menu-items');
        let header =
        `<div class="printedDataContainerInvestigations">
        <div class="company">Company</div>` +
        `<div class="investigationStage">Investigation Stage</div>` +
        `<div class="investigationStarted">Investigation Started</div>` +
        `<div class="investigationEnded">Investigation  Ended</div>` +
        `<div class="conclusion">Conclusion</div>` +
        `<div class="investigator">Investigators</div>` +
        `<div class="button_container">COMMANDS</div></div>`;
        
        menuContainer.innerHTML = header;
        // a.forEach(element => {
        //     console.log(element);
        for (const element of elements) {                 
                      
            let complains =
            `<div id="${element.id}" class="printedDataContainerInvestigations">
            <div class="company">${element.company}</div>` +
            `<div class="investigationStage">${renderStages(element.investigationStages)}</div>` +
            `<div class="investigationStarted">${element.startDate}</div>` +
            `<div class="investigationEnded">${element.endDate}</div>` +
            `<div class="conclusion">${element.conclusion}</div>` +
            `<div class="investigator">${renderInvestigators(element.investigators)}</div>` +
            '<div class="button_container"><input type="button" class="editButton" value="Edit" onClick="editInvestigation(' + element.id + ')" />' +
            '<input type="button" class="updateButton" style="display:none" value="Update" onClick="updateTodo(' + element.id + ')" />' +
            '<input type="button" class="cancelButton" style="display:none" value="Cancel" onClick="cancelTodo(' + element.id + ')" />' +
            '<input type="button" class="yesButton" style="display:none" value="YES" onClick="deleteTodo(' + element.id + ')" />' +
            '<input type="button" class="noButton" style="display:none" value="NO" onClick="cancelDelete(' + element.id + ')" />' +
            //'<input type="button" value="Delete" onClick="deleteTodo(' + post.id + ')" /> </div> </div> '
            '<input type="button" class="deleteButton" value="Delete" onClick="aproveDelete(' + element.id + ')" /> </div> </div> '
            
            

            menuContainer.innerHTML += complains;
        }
            
            
        });    
}


const editInvestigation = (id) => {
    window.location.href = "./administrativeInspection.html?id=" + id
}

loadData();
loadCompaniesData()