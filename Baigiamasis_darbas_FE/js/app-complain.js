const conclusionFormSbmBtn = document.querySelector('#conclusion-form-submit');
const conclusionForm = document.querySelector('#conclusion-form');
const addInvestigatorForm = document.querySelector('#add-investigator-form');
const investigatorFormSbmBtn = document.querySelector('#investigator-form-submit');
const addStageForm = document.querySelector('#stage-form');
const stageFormSbmBtn = document.querySelector('#stage-form-submit');
const queryString = window.location.search;
const urlParams = new URLSearchParams(queryString);
getComplain(urlParams.get('id'));
const complainsId = (urlParams.get('id'));



//get all investigators
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
function getComplain(complainId) {
    const url = 'https://localhost:7220/api/Complains/complains/' + complainId;
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
        const menuContainer = document.getElementById('complain');
        let header =
        `<div class="printedDataContainer">
        <div class="complainant">Complainant</div>` +
        `<div class="complainantPhoneNumer">Complainant Phone</div>` +
        `<div class="complaintDescription">Description</div>` +
        `<div class="companyDetails">Company details</div>` +
        `<div class="complainStage">Stages</div>` +
        `<div class="complainStartDate">Start date</div>` +
        `<div class="complainEndDate">End date</div>` +
        `<div class="conclusion">Conclusion</div>` +
        `<div class="investigator">Investigator</div>` +
        `<div class="investigatorPhoneNumber">Investigator Phone</div>` +
        `<div class="button_container">COMMANDS</div></div>`;
        
        menuContainer.innerHTML = header;
       
            let com =
            `<div id="${elements.complainId}" class="printedDataContainer">
            <div class="complainant">${elements.complainant}</div>` +
            `<div class="complainantPhoneNumer">${elements.complainantPhoneNumer}</div>` +
            `<div class="complaintDescription">${elements.complaintDescription}</div>` +
            `<div class="companyDetails">${elements.companyDetails}</div>` +
            `<div class="complainStage">${renderStages(elements.complainStage)}</div>` +
            `<div class="complainStartDate">${elements.complainStartDate}</div>` +
            `<div class="complainEndDate">${elements.complainEndDate}</div>` +
            `<div class="conclusion">${elements.conclusion}</div>` +
            `<div class="investigator">${elements.investigator}</div>` +
            `<div class="investigatorPhoneNumber">${elements.investigatorPhoneNumber}</div>`+
            '<div class="button_container"><input type="button" class="editButton" value="Update" onClick="updateComplain(' + elements.complainId + ')" />' +          
            '<input type="button" class="deleteButton" value="Delete" onClick="deleteComplain(' + elements.complainId + ')" /> </div> </div> '
            
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

const deleteComplain = (id) => {
    const url = `https://localhost:7220/api/Complains/complains/delete/` + id
    const optionsFetchPosts = {
        method: 'delete',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + window.localStorage.getItem('token')
        }
    }
    fetch(url, optionsFetchPosts)
        .then(() => printUserPosts())
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
    
        const url = 'https://localhost:7220/api/Complains/complains/investigator/update/' + id;
    
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
loadConclusionData()