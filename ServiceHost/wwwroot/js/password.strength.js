var timeout;
var password = document.getElementById('password');
var strengthBadge = document.getElementById('strength-display');

var strongPassword = new RegExp('(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[^A-Za-z0-9])(?=.{8,})')
var mediumPassword = new RegExp('((?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[^A-Za-z0-9])(?=.{6,}))|((?=.*[a-z])(?=.*[A-Z])(?=.*[^A-Za-z0-9])(?=.{8,}))')
var googleRecommender = new RegExp(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{6,20}$/);

function StrengthChecker(password) {

    if (strongPassword.test(password)) {
        strengthBadge.style.backgroundColor = "green"
        strengthBadge.textContent = 'Strong'
    } else if (mediumPassword.test(password)) {
        strengthBadge.style.backgroundColor = 'rgb(7,89,133)'
        strengthBadge.textContent = 'Medium'
    } else if (googleRecommender.test(password)) {
        strengthBadge.style.backgroundColor = "green"
        strengthBadge.textContent = 'Strong'
    } else {
        strengthBadge.style.backgroundColor = 'red'
        strengthBadge.textContent = 'Weak'
    }
}

password.addEventListener("input", () => {

    strengthBadge.style.display = 'block'
    clearTimeout(timeout);

    timeout = setTimeout(() => StrengthChecker(password.value), 60);

    if (password.value.length !== 0) {
        strengthBadge.style.display != 'block'
    } else {
        strengthBadge.style.display = 'none'
    }
});