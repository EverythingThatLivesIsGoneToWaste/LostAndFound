document.querySelector('form').addEventListener('submit', function (e) {
    const login = document.getElementById('inputLogin').value;
    const password = document.getElementById('inputPassword').value;

    if (!login || !password) {
        e.preventDefault();
        alert('Заполните все поля!');
        return false;
    }

    return true;
});