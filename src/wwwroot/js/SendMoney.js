//const sendMoneyItems = document.querySelectorAll(".send-money-item");

//sendMoneyItems.forEach(item => {
//    item.addEventListener("click", (e) => {
//        item.classList.add("send-money-item-selected");
//        item.classList.remove("send-money-item");
//    })
//})

const sendMoneyContainer = document.querySelector('.send-money-container');

function displayUsers(users) {
    users.forEach(item => {
        const sendMoneyItem = document.createElement('div');
        sendMoneyItem.classList.add(...['send-money-item', 'd-flex', 'justify-content-evenly', 'align-items-center', 'mb-3']);
        const userImageContainer = document.createElement('div');
        userImageContainer.classList.add(...['col-2', 'd-flex', 'justify-content-center', 'align-items-center', 'flex-column']);
        const userImage = document.createElement('img');
        userImage.classList.add('send-money-img');
        userImage.src = '/img/user.png';
        userImageContainer.appendChild(userImage);
        sendMoneyItem.appendChild(userImageContainer);
        const userDetailsContainer = document.createElement('div');
        userDetailsContainer.classList.add(...['col-8', 'd-flex', 'justify-content-center', 'flex-column', 'ms-2']);
        const userName = document.createElement('p');
        userName.classList.add(...['send-money-name', 'text-uppercase']);
        userName.textContent = `${item.firstName} ${item.lastName}`;
        userDetailsContainer.appendChild(userName);
        const userEmail = document.createElement('p');
        userEmail.classList.add('send-money-email');
        userEmail.textContent = item.email;
        userDetailsContainer.appendChild(userEmail);
        sendMoneyItem.appendChild(userDetailsContainer);
        const closeIconContainer = document.createElement('div');
        closeIconContainer.classList.add(...['col-2', 'd-flex', 'justify-content-center', 'align-items-center']);
        // <img class="close-icon" src="/img/x-white.png">
        sendMoneyItem.appendChild(closeIconContainer);
        sendMoneyContainer.appendChild(sendMoneyItem);
    })
}

fetch("/SendMoney/GetSendMoneyData").then(data => data.json())
    .then(res => {
        displayUsers(res);
    })