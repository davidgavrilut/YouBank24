﻿const sendMoneyContainer = document.querySelector('.items-container');
const sendMoneyButton = document.querySelector('.action-buttons > .btn-primary');
const spinner = document.getElementById('loading-icon');
const itemsContainer = document.querySelector('.items-container');
let selectedSendMoneyItem = null;


function displayUsers(users) {
    users.forEach((item, index) => {
        const sendMoneyItem = document.createElement('div');
        sendMoneyItem.setAttribute('order-index', index);
        sendMoneyItem.classList.add(...['item-send', 'd-flex', 'justify-content-evenly', 'align-items-center', 'mb-3']);
        const userImageContainer = document.createElement('div');
        userImageContainer.classList.add(...['col-2', 'd-flex', 'justify-content-center', 'align-items-center', 'flex-column']);
        const userImage = document.createElement('img');
        userImage.classList.add('item-img');
        userImage.src = '/img/user.png';
        userImageContainer.appendChild(userImage);
        sendMoneyItem.appendChild(userImageContainer);
        const userDetailsContainer = document.createElement('div');
        userDetailsContainer.classList.add(...['col-8', 'd-flex', 'justify-content-center', 'flex-column', 'ms-2']);
        const userName = document.createElement('p');
        userName.classList.add(...['item-name', 'text-uppercase']);
        userName.textContent = `${item.firstName} ${item.lastName}`;
        userDetailsContainer.appendChild(userName);
        const userEmail = document.createElement('p');
        userEmail.classList.add('item-email');
        userEmail.textContent = item.email;
        userDetailsContainer.appendChild(userEmail);
        sendMoneyItem.appendChild(userDetailsContainer);
        const closeIconContainer = document.createElement('div');
        closeIconContainer.classList.add(...['col-2', 'd-flex', 'justify-content-center', 'align-items-center']);
        sendMoneyItem.appendChild(closeIconContainer);
        sendMoneyContainer.appendChild(sendMoneyItem);

        sendMoneyItem.addEventListener("click", (e) => {
            if (selectedSendMoneyItem) {
                selectedSendMoneyItem.classList.remove('send-money-item-selected');
                selectedSendMoneyItem.classList.add('item-send');
                selectedSendMoneyItem.querySelector('.close-icon')?.remove();
                document.querySelector('#inputContainer')?.classList.add("d-none");
            }

            selectedSendMoneyItem = e.target.closest('.item-send');
            selectedSendMoneyItem.classList.remove('item-send');
            selectedSendMoneyItem.classList.add('send-money-item-selected');

            const closeIcon = document.createElement('img');
            closeIcon.classList.add('close-icon');
            closeIcon.src = '/img/close.png';
            closeIconContainer.appendChild(closeIcon);

            const inputContainer = document.querySelector("#inputContainer");
            inputContainer.classList.remove("d-none");
            inputContainer.parentNode.removeChild(inputContainer);
            selectedSendMoneyItem.insertAdjacentHTML("afterend", inputContainer.outerHTML);
            const inputEmail = document.querySelector("#inputEmail");
            inputEmail.value = item.email;

            closeIconContainer.addEventListener("click", (e) => {
                if (e.target.closest('.close-icon')) {
                    e.stopPropagation();
                    if (selectedSendMoneyItem) {
                    selectedSendMoneyItem.classList.remove('send-money-item-selected');
                    selectedSendMoneyItem.classList.add('item-send');
                    selectedSendMoneyItem.querySelector('.close-icon')?.remove();
                    }
                    document.querySelector('#inputContainer')?.classList.add("d-none");
                    selectedSendMoneyItem = null;
                }
            })
        })
    })
}

function displayAlert(title, message) {
    Swal.fire({
        icon: 'error',
        title: title,
        text: message
    })
}

spinner.style.display = 'block';

fetch("/SendMoney/GetSendMoneyData").then(data => data.json())
    .then(res => {
        displayUsers(res);
        spinner.style.display = 'none';
        itemsContainer.classList.remove('blur-spin');
    })

fetch("/SendMoney/GetUserBalance").then(data => data.json())
    .then(res => {
        sendMoneyButton.addEventListener("click", (e) => {
            const inputAmount = document.querySelector('#inputAmount');
            const inputContainer = document.querySelector('#inputContainer');
            if (inputContainer.classList.contains('d-none')) {
                e.preventDefault();
                displayAlert("No recipient selected", "You need to select a recipient.");
            }
            else if (inputAmount.value == '' || inputAmount.value < 0 || inputAmount.value == 'e' || inputAmount.value == '.') {
                e.preventDefault();
                displayAlert("Invalid amount", "Please enter an amount value greater than 0");
            }
            else if (inputAmount.value > res) {
                e.preventDefault();
                displayAlert("Insufficient balance", "Amount cannot be greater than your current balance");
            }
        })
    })