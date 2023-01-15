const complainForm = document.querySelector('#complain-form');
const complainFormSbmBtn = document.querySelector('#complain-form-submit');
const errorEle = document.querySelector(".error-message");


function sendData() {
    let data = new FormData(complainForm);
    let obj = {};

    console.log(data);

    data.forEach((value, key) => {
        obj[key] = value
    });

    fetch('https://localhost:7220/api/Complains/create/complain', {
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
    stages?.forEach((value, key) => {
        result += value.stage + ', ';
    });

    //TODO:trim last
    return result.trimEnd(',');
}

complainFormSbmBtn.addEventListener('click', (e) => {
    e.preventDefault(); // Breaks manual refresh after submit
    sendData();
})


//get all complains


function loadData() {
    const url = 'https://localhost:7220/api/Complains/complains';
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
        // a.forEach(element => {
        //     console.log(element);
        for (const element of elements) {                 
                      
            let complains =
            `<div id="${element.complainId}" class="printedDataContainer">
            <div class="complainant">${element.complainant}</div>` +
            `<div class="complainantPhoneNumer">${element.complainantPhoneNumer}</div>` +
            `<div class="complaintDescription">${element.complaintDescription}</div>` +
            `<div class="companyDetails">${element.companyDetails}</div>` +
            `<div class="complainStage">${renderStages(element.complainStage)}</div>` +
            `<div class="complainStartDate">${element.complainStartDate}</div>` +
            `<div class="complainEndDate">${element.complainEndDate}</div>` +
            `<div class="conclusion">${element.conclusion}</div>` +
            `<div class="investigator">${element.investigator}</div>` +
            `<div class="investigatorPhoneNumber">${element.investigatorPhoneNumber}</div>`+
            '<div class="button_container"><input type="button" class="editButton" value="Edit" onClick="editTodo(' + element.complainId + ')" />' +
            '<input type="button" class="updateButton" style="display:none" value="Update" onClick="updateTodo(' + element.complainId + ')" />' +
            '<input type="button" class="cancelButton" style="display:none" value="Cancel" onClick="cancelTodo(' + element.complainId + ')" />' +
            '<input type="button" class="yesButton" style="display:none" value="YES" onClick="deleteTodo(' + element.complainId + ')" />' +
            '<input type="button" class="noButton" style="display:none" value="NO" onClick="cancelDelete(' + element.complainId + ')" />' +
            //'<input type="button" value="Delete" onClick="deleteTodo(' + post.id + ')" /> </div> </div> '
            '<input type="button" class="deleteButton" value="Delete" onClick="aproveDelete(' + element.complainId + ')" /> </div> </div> '
            
            

            menuContainer.innerHTML += complains;
        }
            
            
        });

    
}




const editTodo = (id) => {
    window.location.href = "./complain.html?id=" + id
}

loadData();
loadInvestigatorsData();