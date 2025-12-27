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

//загрузка юзеров
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

    //для обработки @ почему-то получает json??
    const login = user.login || user["login"] || "No login";
    const fullName = user.fullName || login;
    const role = user.role || "@";

    const roleDisplay = String(role);

    //карточка юзера
    card.innerHTML = `
        <h2>${fullName}</h2>
        <p><strong>Login:</strong> ${login}</p>
        <p><strong>Role:</strong> ${roleDisplay}</p>
        <p><strong>ID:</strong> ${user.id}</p>
        <small>Created: ${new Date().toLocaleDateString()}</small>
        <div class="user-actions">
            <button onclick="deleteUser(${user.id})" class="delete-btn">Delete</button>
        </div>
    `;

    return card;
}

async function createUser() {
    const login = document.getElementById('Login').value;
    const fullName = document.getElementById('FullName').value;
    const password = document.getElementById('Password').value;
    const role = document.getElementById('Role').value;

    if (!login || !fullName || !password || !role) {
        alert('Please fill all fields');
        return; 
    }

    try {
        const formData = new FormData();
        formData.append('Login', login);
        formData.append('FullName', fullName);
        formData.append('Password', password);
        formData.append('Role', role);

        const response = await fetch('/api/users', {
            method: 'POST',
            body: new URLSearchParams(formData),
            headers: {
                'Accept': 'application/json'
            }
        });

        if (response.status === 201) {
            const result = await response.json();
            closeModal();
            document.getElementById('itemForm').reset();
            loadUsers(); 
        } else {
            const error = await response.json();
            alert(`Error: ${error.error || 'Failed to create user'}`);
        }
    } catch (error) {
        console.error('Error:', error);
        alert('Network error. Please try again.');
    }
}

async function deleteUser(userId) {
    if (!confirm('Delete this user?')) {
        return;
    }

    try {
        const response = await fetch(`/api/users/${userId}`, {
            method: 'DELETE'
        });

        if (response.status === 204) {
            const userCard = document.getElementById(`user-${userId}`);
            if (userCard) {
                userCard.remove();
            }

            alert('User deleted successfully!');
            loadUsers();
        } else {
            const error = await response.json();
            alert(`Error: ${error.error || 'Failed to delete user'}`);
        }

    } catch (error) {
        console.error('Delete error:', error);
        alert('Network error. Please try again.');
    }
}



