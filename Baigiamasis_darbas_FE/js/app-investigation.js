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
            <div class="company">${elements.company}</div>` +
            `<div class="nvestigationStage">${elements.investigationStage}</div>` +
            `<div class="investigationStarted">${elements.investigationStarted}</div>` +
            `<div class="investigationEnded">${elements.investigationEnded}</div>` +
            `<div class="Conclusion">${elements.conclusion}</div>` +
            `<div class="Investigator">${elements.investigator}</div>` +
            `<div class="penalty">${elements.penalty}</div>` +
            '<div class="button_container"><input type="button" class="editButton" value="Update" onClick="updateComplain(' + elements.complainId + ')" />' +          
            '<input type="button" class="deleteButton" value="Delete" onClick="deleteComplain(' + elements.complainId + ')" /> </div> </div> '
            
            menuContainer.innerHTML += investigation;
        }                      
        );    
}