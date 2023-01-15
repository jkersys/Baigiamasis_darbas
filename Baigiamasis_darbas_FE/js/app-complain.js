const btnToggleTodo = document.querySelector('#addTodo')
let inputForm = document.querySelector('#todo-form')

//get all investigators
let dropdown = document.querySelector('#InvestigatorId');
//dropdown.length = 0;
let defaultOption = document.createElement('option');
defaultOption.text = 'select';
dropdown.add(defaultOption);
dropdown.selectedIndex = 0;

function loadInvestigatorsData() {
    const url = 'https://localhost:7220/api/Investigator/investigators';
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
                data.text = elements[i].name;
                data.value = elements[i].id;
                dropdown.add(data);                   

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

const queryString = window.location.search;
const urlParams = new URLSearchParams(queryString);
getComplain(urlParams.get('id'));



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
    if (inputForm.style.display === 'none' || inputForm.style.display === '') {
        inputForm.style.display = 'grid';
    } else {
        inputForm.style.display = 'none';
    }
}






function updateComplain(id) {

        let data = new FormData(inputForm);
        let obj = {};
    
        // #1 iteracija -> obj {name: 'asd'}
        // #2 iteracija -> obj {type: 'asd'}
        data.forEach((value, key) => {
            // console.log(`${key}(Key): ${value}(Value)`);
            obj[key] = value
        });
    
        const url = 'https://localhost:7220/api/Complains/complains/update/' + id;
    
        fetch(url, {
            method: 'put',
            headers: {
                'Accept': 'application/json, text/plain, */*',
                'Content-Type': 'application/json'
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

  

   const editTodo = (id) => {
 //   e.preventDefault(); // Breaks manual refresh after submit
    updateComplain(id);}
loadInvestigatorsData()