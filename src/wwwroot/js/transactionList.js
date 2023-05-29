const sentBtn = document.querySelector('.btn-outline-danger');
const receivedBtn = document.querySelector('.btn-outline-success');

fetch('/TransactionList/GetTransactions').then(data => data.json())
    .then(res => {
        console.log(res);
    });