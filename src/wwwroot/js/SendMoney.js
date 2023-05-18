const sendMoneyContainer = document.querySelector('.send-money-container');
let selectedSendMoneyItem = null;

function displayUsers(users) {
    users.forEach((item, index) => {
        const sendMoneyItem = document.createElement('div');
        sendMoneyItem.setAttribute('order-index', index);
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
        sendMoneyItem.appendChild(closeIconContainer);
        sendMoneyContainer.appendChild(sendMoneyItem);

        sendMoneyItem.addEventListener("click", (e) => {
            // if another element is selected, remove extra elements
            if (selectedSendMoneyItem) {
                selectedSendMoneyItem.classList.remove('send-money-item-selected');
                selectedSendMoneyItem.classList.add('send-money-item');
                selectedSendMoneyItem.querySelector('.close-icon')?.remove();
                document.querySelector('#amountContainer')?.remove();
            }

            // adding extra parts
            selectedSendMoneyItem = e.target.closest('.send-money-item');
            selectedSendMoneyItem.classList.remove('send-money-item');
            selectedSendMoneyItem.classList.add('send-money-item-selected');

            const closeIcon = document.createElement('img');
            closeIcon.classList.add('close-icon');
            closeIcon.src = '/img/x-white.png';
            closeIconContainer.appendChild(closeIcon);

            const amountContainer = document.createElement('div');
            amountContainer.classList.add(...['d-flex', 'align-items-center', 'w-50', 'p-3', 'mb-3']);
            amountContainer.id = 'amountContainer';
            const amountLabel = document.createElement('p');
            amountLabel.classList.add('me-3');
            amountLabel.textContent = 'Amount';
            amountContainer.appendChild(amountLabel);
            const amountInput = document.createElement('input');
            amountInput.classList.add(...['form-control', 'w-25']);
            amountInput.type = "text";
            amountContainer.appendChild(amountInput);
            selectedSendMoneyItem.insertAdjacentHTML("afterend", amountContainer.outerHTML);

            // on x click, remove extra parts from selected element
            closeIconContainer.addEventListener("click", (e) => {
                if (e.target.closest('.close-icon')) {
                    e.stopPropagation();
                    selectedSendMoneyItem.classList.remove('send-money-item-selected');
                    selectedSendMoneyItem.classList.add('send-money-item');
                    selectedSendMoneyItem.querySelector('.close-icon')?.remove();
                    document.querySelector('#amountContainer')?.remove();
                    selectedSendMoneyItem = null;
                }
            })
        })
    })
}

fetch("/SendMoney/GetSendMoneyData").then(data => data.json())
    .then(res => {
        displayUsers(res);
    })