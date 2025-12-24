function openModal(){
    const modal = document.getElementById("itemModal");

    if(modal){
        modal.style.display = 'block';

        setTimeout(() => {
            modal.style.opacity = '1';
        }, 300);

    }else{
        console.error('Модальное окно не найдено!');
    }
}

function closeModal() {
    const modal = document.getElementById("itemModal");
    
    if (modal) {
        modal.style.opacity = '0';
        
        setTimeout(() => {
            modal.style.display = 'none';
        }, 300); 
    }
}

// Закрытие при клике вне окна
document.addEventListener('DOMContentLoaded', function() {
    
    window.addEventListener('click', function(event) {
        const modal = document.getElementById("itemModal");
        
        if (modal && event.target === modal) {
            closeModal();
        }
    });
});

document.addEventListener('DOMContentLoaded', function () {
    console.log('DOM полностью загружен!');
    loadUsers();
});

async function loadUsers() {

    const container = document.getElementById('usersContainer');
    console.log('usersContainer:', document.getElementById('usersContainer'));
    console.log('Контейнер найден:', container);

    if (container) {
        container.innerHTML = '<p class="no-users">Loading users...</p>';
    }

    try {
        const response = await fetch('/api/users', {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
            }
        });

        if (response.ok) {
            const users = await response.json();
            console.log('Загружено пользователей:', users.length);
            displayUsers(users);
        } else {
            console.error('Failed to load users');
        }
    } catch (error) {
        console.error('Error loading users:', error);
    }
}

function displayUsers(users) {
    const container = document.getElementById('usersContainer');

    if (!container) {
       console.error('Container for users not found');
    return;
    }

    container.innerHTML = '';

    if (users.length === 0) {
        container.innerHTML = '<p class="no-users">No users found</p>';
        return;
    }
    const sortedUsers = users.sort((a, b) => b.id - a.id);

    sortedUsers.forEach((user, index) => {
        console.log(`Создаю карточку ${index + 1}:`, user);
        const userCard = createUserCard(user);
        container.appendChild(userCard);
    });
}

function createUserCard(user) {
    const card = document.createElement('div');
    card.className = 'user-card';
    card.id = `user-${user.id}`;

    // Простейшее получение данных
    const login = user.login || user["login"] || "No login";
    const fullName = user.fullName || login;
    const role = user.role || "@";

    // Просто отображаем роль как есть
    const roleDisplay = String(role);

    card.innerHTML = `
        <h2>${fullName}</h2>
        <p><strong>Login:</strong> ${login}</p>
        <p><strong>Role:</strong> ${roleDisplay}</p>
        <p><strong>ID:</strong> ${user.id}</p>
        <small>Created: ${new Date().toLocaleDateString()}</small>
        <div class="user-actions">
            <button onclick="editUser(${user.id})" class="edit-btn">Edit</button>
            <button onclick="deleteUser(${user.id})" class="delete-btn">Delete</button>
        </div>
    `;

    return card;
}


//todo
async function deleteUser(userId) {
}

//function setupForm() {
//    const form = document.getElementById('itemForm');
//    if (!form) return;

//    form.addEventListener('submit', async function (e) {
//        e.preventDefault();

//        // Собираем данные из формы
//        const formData = new FormData(this);

//        try {
//            // Отправляем POST запрос
//            const response = await fetch('/api/users', {
//                method: 'POST',
//                body: new URLSearchParams(formData), // Это создаст application/x-www-form-urlencoded
//                headers: {
//                    // Если нужен anti-forgery token, раскомментируйте:
//                    // 'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
//                }
//            });

//            if (response.status === 201) { // 201 Created
//                const result = await response.json();
//                alert(`User created successfully! ID: ${result.id}`);
//                closeModal();
//                form.reset();

//                // Обновляем список пользователей, если есть такая функция
//                if (typeof searchUsers === 'function') {
//                    searchUsers();
//                }
//            } else {
//                const error = await response.json();
//                alert(`Error: ${error.error || 'Unknown error'}`);
//            }
//        } catch (error) {
//            console.error('Error:', error);
//            alert('Network error. Please try again.');
//        }
//    });
//}

//// Вызываем при загрузке страницы
//document.addEventListener('DOMContentLoaded', setupForm);


