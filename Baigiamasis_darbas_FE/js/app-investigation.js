const addInvestigatorForm = document.querySelector('#add-investigator-form');
const investigatorFormSbmBtn = document.querySelector('#investigator-form-submit');
const conclusionFormSbmBtn = document.querySelector('#conclusion-form-submit');
const conclusionForm = document.querySelector('#conclusion-form');
const addStageForm = document.querySelector('#stage-form');
const stageFormSbmBtn = document.querySelector('#stage-form-submit');

const queryString = window.location.search;
const urlParams = new URLSearchParams(queryString);
getInvestigation(urlParams.get('id'));


function getInvestigation(investigationId) {
    const url = 'https://localhost:7220/api/Investigation/' + investigationId;
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
        const menuContainer = document.getElementById('investigation');
        let header =
        `<div class="printedDataContainerInvestigation">
        <div class="company">Company</div>` +
        `<div class="nvestigationStage">Investigation Stage</div>` +
        `<div class="investigationStarted">Investigation Started</div>` +
        `<div class="investigationEnded">Investigation Ended</div>` +
        `<div class="Conclusion">Conclusion</div>` +
        `<div class="Investigator">Investigator</div>` +
        `<div class="penalty">Penalty</div>` +
        `<div class="button_container">COMMANDS</div></div>`;
        
        menuContainer.innerHTML = header;
       
            let investigation =
            `<div id="${elements.investigationId}" class="printedDataContainerInvestigation">
            <div class="company">${"Pavadinimas:" + "&nbsp" + elements.company.companyName + "<br/>" +
                     "Reg. Nr.:" + "&nbsp" + elements.company.companyRegistrationNumber + "<br/>" +
                    "El. p.:" + "&nbsp" + elements.company.companyEmail + "<br/>" +
                     "Tel. Nr." + "&nbsp" + elements.company.companyPhone + "<br/>" +
                    "Adresas:" + "&nbsp" + elements.company.companyAdress + "<br/>" }</div>` +
                    
            `<div class="nvestigationStage">${renderStages(elements.investigationStage)}</div>` +
            `<div class="investigationStarted">${elements.investigationStarted}</div>` +
            `<div class="investigationEnded">${elements.investigationEnded}</div>` +
            `<div class="Conclusion">${elements.conclusion}</div>` +
            `<div class="Investigator">${renderInvestigators(elements.investigator)}</div>` +
            `<div class="penalty">${elements.penalty}</div>` +
            '<div class="button_container"><input type="button" class="editButton" value="Update" onClick="updateComplain(' + elements.complainId + ')" />' +          
            '<input type="button" class="deleteButton" value="Delete" onClick="deleteComplain(' + elements.complainId + ')" /> </div> </div> '
            
            menuContainer.innerHTML += investigation;
        }                      
        );    
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
        result += "Vardas:" + "&nbsp" + value.name + "<br/>" +
         "Pavarde:" + "&nbsp" + value.lastname + "<br/>" +
         "Tel:" + "&nbsp" + value.phoneNumber + "<br/>" +
         "El. Paštas:" + "&nbsp" + value.email + "<br/>" +
         "Kabineto Nr." + "&nbsp" + value.cabinetNumber + "<br/>" +
         "Adresas:" + "&nbsp" + value.workplaceAdress + "<br/>" +
         "Pazymejimo Nr.:" + "&nbsp" + value.certificateNumber + "<br/>"
    });
    return result;
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

//get all conclusions
let dropdownConclusion = document.querySelector('#conclusionId');
//dropdown.length = 0;
let defaultOptionConclusion = document.createElement('option');
defaultOptionConclusion.text = 'select';
//dropdown.add(defaultOption);
dropdownConclusion.selectedIndex = 0;

function loadConclusionData() {
    const url = 'https://localhost:7220/api/Conclusion/select';
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
                data.text = elements[i].conclusion;
                data.value = elements[i].id;
                dropdownConclusion.add(data);                   

        }
                        
        });   
}


function addConclusion(complainsId) {

    let data = new FormData(conclusionForm);
    let obj = {};

    // #1 iteracija -> obj {name: 'asd'}
    // #2 iteracija -> obj {type: 'asd'}
    data.forEach((value, key) => {
        // console.log(`${key}(Key): ${value}(Value)`);
        obj[key] = value
    });

    const url = 'https://localhost:7220/api/ComplainConclusion/complains/' +complainsId+ '/conclusions';

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

conclusionFormSbmBtn.addEventListener('click', (e) => {
   // e.preventDefault(); // Breaks manual refresh after submit
   addConclusion(urlParams.get('id'));
})


function addInvestigator(id) {

        let data = new FormData(addInvestigatorForm);
        let obj = {};
    
        // #1 iteracija -> obj {name: 'asd'}
        // #2 iteracija -> obj {type: 'asd'}
        data.forEach((value, key) => {
            // console.log(`${key}(Key): ${value}(Value)`);
            obj[key] = value
        });
    
        const url = 'https://localhost:7220/api/Investigation/investigator/update/' + id;
    
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

   investigatorFormSbmBtn.addEventListener('click', (e) => {
    // e.preventDefault(); // Breaks manual refresh after submit
    addInvestigator(urlParams.get('id'));
 })

 function addStage(id) {

    let data = new FormData(addStageForm);
    let obj = {};

    // #1 iteracija -> obj {name: 'asd'}
    // #2 iteracija -> obj {type: 'asd'}
    data.forEach((value, key) => {
        // console.log(`${key}(Key): ${value}(Value)`);
        obj[key] = value
    });

    const url = 'https://localhost:7220/api/ComplainStage/investigator/complains/stage/update/' + id;

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
stageFormSbmBtn.addEventListener('click', (e) => {
// e.preventDefault(); // Breaks manual refresh after submit
addStage(urlParams.get('id'));
})






loadInvestigatorsData()
loadCompaniesData()
loadConclusionData()
