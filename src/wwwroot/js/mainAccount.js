function showButton(balance) {
    let button = document.querySelector("#balanceButton");
    button.addEventListener("click", () => {
            if (button.textContent === "SHOW") {
                return button.textContent = "$" + balance;
            } else {
                return button.textContent = "SHOW";
            }
    })
}

function displayHiddenCardNumber(text) {
    let hiddenCardNumber = document.querySelector("#hiddenCardNumber");
    let textLast = text.substring(11, 15);
    let asterixString = "**** **** **** ";
    let finalString = asterixString + textLast;
    hiddenCardNumber.textContent = finalString;
}

fetch("/Home/GetAccountCardNumberBalance").then(res => res.json())
    .then(data => {
        showButton(data.balance);
        displayHiddenCardNumber(data.cardNumber);
})
