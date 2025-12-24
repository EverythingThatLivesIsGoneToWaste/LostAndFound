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

async function loadUsers() {
    try {
        const response = await fetch('/api/users', {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
            }
        });

        if (response.ok) {
            const users = await response.json();
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
    users.forEach(user => {
        const userCard = createUserCard(user);
        container.appendChild(userCard);
    });
}

function createUserCard(user) {
    const card = document.createElement('div');
    card.className = 'user-item'; 

    card.innerHTML = `
        <h3>${user.fullName}</h3>
        <p><strong>Login:</strong> ${user.login}</p>
        <p><strong>Role:</strong> ${roleFormatted}</p>
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


