const itemsContainer = document.querySelector('.items-container');
const filterButtons = document.querySelector('#filterButtons');
const spinner = document.getElementById('loading-icon');

function displayTransactions(item, index, color, symbol) {
    const transaction = document.createElement('div');
    transaction.setAttribute('order-index', index);
    transaction.classList.add(...['item-transaction', 'd-flex', 'justify-content-evenly', 'align-items-center', 'mb-3']);
    const userImageContainer = document.createElement('div');
    userImageContainer.classList.add(...['col-2', 'd-flex', 'justify-content-center', 'align-items-center', 'flex-column']);
    const userImage = document.createElement('img');
    userImage.classList.add('item-img');
    userImage.src = '/img/user.png';
    userImageContainer.appendChild(userImage);
    transaction.appendChild(userImageContainer);
    const userDetailsContainer = document.createElement('div');
    userDetailsContainer.classList.add(...['col-7', 'd-flex', 'justify-content-center', 'flex-column', 'ms-2']);
    const userName = document.createElement('p');
    userName.classList.add(...['item-name', 'text-uppercase']);
    userName.textContent = item.name;
    userDetailsContainer.appendChild(userName);
    const userEmail = document.createElement('p');
    userEmail.classList.add('item-email');
    userEmail.textContent = item.email;
    userDetailsContainer.appendChild(userEmail);
    const hr = document.createElement('hr');
    hr.classList.add('hr-transaction');
    userDetailsContainer.appendChild(hr);
    const timestamp = document.createElement('p');
    timestamp.classList.add('item-timestamp');
    timestamp.textContent = item.timestamp;
    userDetailsContainer.appendChild(timestamp);
    transaction.appendChild(userDetailsContainer);
    const amountContainer = document.createElement('div');
    amountContainer.classList.add(...['col-3', 'd-flex', 'justify-content-center', 'align-items-center']);
    const amount = document.createElement('p');
    amount.id = 'amount';
    amount.classList.add(...['item-name', 'text-uppercase']);
    amount.style.color = color;
    amount.textContent = symbol + " $" + item.amount;
    amountContainer.appendChild(amount);
    transaction.appendChild(amountContainer);
    itemsContainer.appendChild(transaction);

    transaction.addEventListener('click', () => {
        const detailsBtn = document.querySelector('#details');
        detailsBtn.href = '/TransactionList/TransactionDetails/' + item.id;
        detailsBtn.click();
    })
}

function noTransactions(text) {
    const noTransaction = document.createElement('div');
    noTransaction.classList.add(...['no-transactions', 'd-flex', 'justify-content-center', 'align-items-center']);
    const noTransactionText = document.createElement('p');
    noTransactionText.classList.add('no-transactions-text');
    noTransactionText.textContent = text;
    noTransaction.appendChild(noTransactionText);
    itemsContainer.appendChild(noTransaction);
}

spinner.style.display = 'block';

fetch('/GetTransactions')
    .then(data => data.json())
    .then(res => {
        console.log(res);
        const sentBtn = document.createElement('button');
        sentBtn.classList.add(...['btn', 'btn-outline-danger', 'me-2', 'w-25']);
        sentBtn.textContent = 'Sent';
        filterButtons.appendChild(sentBtn);
        const receivedBtn = document.createElement('button');
        receivedBtn.classList.add(...['btn', 'btn-outline-success', 'me-2', 'w-25']);
        receivedBtn.textContent = 'Received';
        filterButtons.appendChild(receivedBtn);
        sentBtn.addEventListener('click', (e) => {
            sentBtn.classList.add('active');
            receivedBtn.classList.remove('active');
            itemsContainer.innerHTML = '';
            if (res.transactionsSent.length == 0) {
                noTransactions('No transactions sent');
            }
            res.transactionsSent.forEach((item, index) => {
                displayTransactions(item, index, '#ff7373', '-');
            });
        });
        receivedBtn.addEventListener('click', (e) => {
            receivedBtn.classList.add('active');
            sentBtn.classList.remove('active');
            itemsContainer.innerHTML = '';
            if (res.transactionsReceived.length == 0) {
                noTransactions('No transactions received');
            }
            res.transactionsReceived.forEach((item, index) => {
                displayTransactions(item, index, '#6fff6f', '+');
            });
        });
        sentBtn.click();
        spinner.style.display = 'none';
        itemsContainer.classList.remove('blur-spin');
    });