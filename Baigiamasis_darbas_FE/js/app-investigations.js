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

    fetch('https://localhost:7220/api/Investigation/create', {
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



//Load investigators list
let dropdown = document.querySelector('#investigatorId');
//dropdown.length = 0;
let defaultOption = document.createElement('option');
defaultOption.text = 'select';
//dropdown.add(defaultOption);
dropdown.selectedIndex = 0;
function loadInvestigatorsData() {
    const url = 'https://localhost:7220/api/Investigator/select';
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
                data.text = elements[i].nameAndLastName;
                data.value = elements[i].id;
                dropdown.add(data);                   

        }
                        
        });   
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




function sendData() {
    let data = new FormData(complainForm);
    let obj = {};

    console.log(data);

    data.forEach((value, key) => {
        obj[key] = value
    });

    fetch('https://localhost:7220/api/Investigation/investigations', {
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
            window.location.href = "./index.html";
        }
        
        console.log(res);
        var resBody = await res.json();
        errorEle.textContent = resBody.message;
    })
    .catch((err) => console.log(err));
}

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
    const url = 'https://localhost:7220/api/Investigation/investigations';
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
        `<div class="investigationStage">Snvestigation Stage</div>` +
        `<div class="investigationStarted">Investigation Started</div>` +
        `<div class="investigationEnded">Investigation  Ended</div>` +
        `<div class="conclusion">Conclusion</div>` +
        `<div class="investigator">Investigators</div>` +
        `<div class="penalty">Penalty</div>` +
        `<div class="button_container">COMMANDS</div></div>`;
        
        menuContainer.innerHTML = header;
        // a.forEach(element => {
        //     console.log(element);
        for (const element of elements) {                 
                      
            let complains =
            `<div id="${element.investigationId}" class="printedDataContainerInvestigations">
            <div class="company">${element.company}</div>` +
            `<div class="investigationStage">${renderStages(element.investigationStage)}</div>` +
            `<div class="investigationStarted">${element.investigationStarted}</div>` +
            `<div class="investigationEnded">${element.investigationEnded}</div>` +
            `<div class="conclusion">${element.conclusion}</div>` +
            `<div class="investigator">${renderInvestigators(element.investigator)}</div>` +
            `<div class="penalty">${element.penalty}</div>` +
            '<div class="button_container"><input type="button" class="editButton" value="Edit" onClick="editInvestigation(' + element.investigationId + ')" />' +
            '<input type="button" class="updateButton" style="display:none" value="Update" onClick="updateTodo(' + element.investigationId + ')" />' +
            '<input type="button" class="cancelButton" style="display:none" value="Cancel" onClick="cancelTodo(' + element.investigationId + ')" />' +
            '<input type="button" class="yesButton" style="display:none" value="YES" onClick="deleteTodo(' + element.investigationId + ')" />' +
            '<input type="button" class="noButton" style="display:none" value="NO" onClick="cancelDelete(' + element.investigationId + ')" />' +
            //'<input type="button" value="Delete" onClick="deleteTodo(' + post.id + ')" /> </div> </div> '
            '<input type="button" class="deleteButton" value="Delete" onClick="aproveDelete(' + element.investigationId + ')" /> </div> </div> '
            
            

            menuContainer.innerHTML += complains;
        }
            
            
        });    
}


const editInvestigation = (id) => {
    window.location.href = "./investigation.html?id=" + id
}

loadData();
loadInvestigatorsData()
loadCompaniesData()
