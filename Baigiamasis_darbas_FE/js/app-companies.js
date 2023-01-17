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

const companyFormSbmBtn = document.querySelector('#company-form-submit');
const companyForm = document.querySelector('#company-form');

const queryString = window.location.search;
const urlParams = new URLSearchParams(queryString);

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





function getCompanies() {
    const url = 'https://localhost:7220/api/Company/companies/';
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
        <div class="companyName">Name</div>` +
        `<div class="companyEmail">Email Phone</div>` +
        `<div class="companyAdress">Adress</div>` +
        `<div class="companyPhone">Phone details</div>` +
        `<div class="companyRegistrationNumber">Reg. Nr.</div>` +
        `<div class="button_container">COMMANDS</div></div>`;
        
        menuContainer.innerHTML = header;
         for (const element of elements) {        
       
            let com =
            `<div id="${element.companyId}" class="printedDataContainer">
            <div class="companyName">${element.companyName}</div>` +
            `<div class="companyEmail">${element.companyEmail}</div>` +
            `<div class="companyAdress">${element.companyAdress}</div>` +
            `<div class="companyPhone">${element.companyPhone}</div>` +
            `<div class="companyRegistrationNumber">${element.companyRegistrationNumber}</div>` +            
            '<div class="button_container"><input type="button" class="editButton" value="Edit" onClick="editCompany(' + element.companyId + ')" />' +          
            '<input type="button" class="deleteButton" value="Delete" onClick="deleteCompany(' + element.companyId + ')" /> </div> </div> '
            
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

const deleteCompany = (id) => {
    const url = `https://localhost:7220/api/Company/delete/` + id
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

companyFormSbmBtn.addEventListener('click', (e) => {
    e.preventDefault(); // Breaks manual refresh after submit
    addCompany();
})


const editCompany = (id) => {
    window.location.href = "./company.html?id=" + id
}


getCompanies()



  

