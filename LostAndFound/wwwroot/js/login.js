document.getElementById('SingInButton').addEventListener('click', async function(e) {
    e.preventDefault();
    
    const userData = {
        username: document.getElementById('inputName').value,
        password: document.getElementById('inputPassword').value
    };

    if(userData.username == 'admin' && userData.password == '1'){
        //cохраняет в localStorage браузера пользователя.
        //! это пока заглушка потом нужно с бд получать данные 
        localStorage.setItem('isAuthenticated', 'true');
        localStorage.setItem('userRole', 'admin');
        localStorage.setItem('username', userData.username);
        window.location.href = 'admin-panel.html';

    } else if(userData.username && userData.password) {
        // Проверка для обычного пользователя
        // здесь нужен запрос на сервер
        localStorage.setItem('isAuthenticated', 'true');
        localStorage.setItem('userRole', 'user');
        localStorage.setItem('username', userData.username);
        
        // Редирект на стороне сервера
        //window.location.href = 'user-dashboard.html';
    }
     else {
        alert('Заполните все поля!');
    }
});


// // login.js - правильный подход
// document.getElementById('SingUpButton').addEventListener('click', async function(e) {
//     e.preventDefault();
    
//     const userData = {
//         username: document.getElementById('inputName').value,
//         password: document.getElementById('inputPassword').value
//     };

//     try {
//         // 1. Отправляем данные на сервер
//         const response = await fetch('/api/auth/login', {
//             method: 'POST',
//             headers: { 'Content-Type': 'application/json' },
//             body: JSON.stringify(userData)
//         });
        
//         // 2. Сервер проверяет пароль и возвращает токен
//         if (response.ok) {
//             const result = await response.json();
            
//             // 3. Сохраняем только токен (не пароль!)
//             localStorage.setItem('authToken', result.token);
//             localStorage.setItem('userRole', result.role);
            
//             // 4. Переходим на нужную страницу
//             if (result.role === 'admin') {
//                 window.location.href = 'admin-panel.html';
//             } else {
//                 window.location.href = 'user-dashboard.html';
//             }
//         } else {
//             alert('Неверный логин или пароль');
//         }
        
//     } catch (error) {
//         console.error('Ошибка входа:', error);
//         alert('Ошибка соединения с сервером');
//     }
// });