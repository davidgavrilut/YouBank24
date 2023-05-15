// <img class="close-icon" src="/img/x-white.png">

const sendMoneyItems = document.querySelectorAll(".send-money-item");

sendMoneyItems.forEach(item => {
    item.addEventListener("click", (e) => {
        item.classList.add("send-money-item-selected");
        item.classList.remove("send-money-item");
    })
})