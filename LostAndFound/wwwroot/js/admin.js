function openModal() {
    const modal = document.getElementById("itemModal");

    if (modal) {
        modal.style.display = 'block';

        setTimeout(() => {
            modal.style.opacity = '1';
        }, 300);

    } else {
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

document.addEventListener('DOMContentLoaded', function () {

    window.addEventListener('click', function (event) {
        const modal = document.getElementById("itemModal");

        if (modal && event.target === modal) {
            closeModal();
        }
    });
});

let allBuildings = [];
let allRooms = [];
let allItems = [];

document.addEventListener('DOMContentLoaded', function () {
    loadBuildings();
    loadItems();
});

async function loadBuildings() {
    try {
        const response = await fetch('/api/buildings', {
            method: 'GET',
            headers: {
                'Accept': 'application/json'
            }
        });

        if (response.ok) {
            allBuildings = await response.json();
            console.log('Загружено зданий:', allBuildings.length);
            buildingsSelect(allBuildings);
            await loadAllRooms();
            await loadItems();
        } else {
            console.error('Ошибка загрузки зданий')
        }
    } catch (error) {
        console.error('Ошибка:', error);
    }
}

//загружать комнаты для отображения их в карточках
async function loadAllRooms() {
    allRooms = []; 

    for (const building of allBuildings) {
        try {
            const response = await fetch(`/api/buildings/${building.id}/rooms`, {
                method: 'GET',
                headers: { 'Accept': 'application/json' }
            });

            if (response.ok) {
                const rooms = await response.json();
                rooms.forEach(room => {
                    room.buildingId = building.id;
                    allRooms.push(room);
                });
                console.log(`Загружено ${rooms.length} комнат для здания ${building.id}`);
            }
        } catch (error) {
            console.error(`Ошибка загрузки комнат для здания ${building.id}:`, error);
        }
    }
}

function buildingsSelect(buildings) {
    const buildingSelect = document.getElementById('BuildingId');
    if (!buildingSelect) return;

    buildingSelect.innerHTML = '<option value="" disabled selected>Select building</option>';

    buildings.forEach(building => {
        const option = document.createElement('option');
        option.value = building.id;
        option.textContent = `${building.name} (${building.address})`;
        buildingSelect.appendChild(option);
    });

    buildingSelect.addEventListener('change', function () {
        const buildingId = this.value;
        if (buildingId) {
            loadRoomsForBuilding(buildingId);
        } else {
            clearRoomsSelect();
        }
    });
}

//загружить комнаты только для формы создания
async function loadRoomsForBuilding(buildingId) {
    const roomSelect = document.getElementById('RoomId');
    if (!roomSelect) return;

    roomSelect.innerHTML = '<option value="" disabled selected>Loading rooms...</option>';
    roomSelect.disabled = true;

    try {
        const response = await fetch(`/api/buildings/${buildingId}/rooms`, {
            method: 'GET',
            headers: {
                'Accept': 'application/json'
            }
        });

        if (response.ok) {
            allRooms = await response.json();
            console.log('Загружено комнат:', allRooms.length);
            roomsSelect(allRooms);
        } else {
            console.error('Ошибка загрузки комнат');
            roomSelect.innerHTML = '<option value="" disabled selected>Error loading rooms</option>';
        }
    } catch (error) {
        console.error('Ошибка:', error);
        roomSelect.innerHTML = '<option value="" disabled selected>Connection error</option>';
    } finally {
        roomSelect.disabled = false;
    }
}

function roomsSelect(rooms) {
    const roomSelect = document.getElementById('RoomId');
    if (!roomSelect) return;

    roomSelect.innerHTML = '<option value="" disabled selected>Select room</option>';

    if (rooms.length === 0) {
        const option = document.createElement('option');
        option.value = "";
        option.textContent = 'No rooms available';
        roomSelect.appendChild(option);
        return;
    }

    rooms.forEach(room => {
        const option = document.createElement('option');
        option.value = room.id;
        option.textContent = `${room.name} (Этаж ${room.floor})`;
        roomSelect.appendChild(option);
    });
}

//загрузка вещей
async function loadItems() {

    const container = document.getElementById('itemsContainer');
    console.log('itemsContainer:', document.getElementById('itemsContainer'));
    console.log('Контейнер найден:', container);

    if (container) {
        container.innerHTML = '<p class="no-users">Loading items...</p>';
    }

    try {
        const response = await fetch('/api/items', {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
            }
        });

        if (response.ok) {
            const items = await response.json();
            allItems = items;
            displayItems(items);
        } else {
            console.error('Failed to load items');
        }
    } catch (error) {
        console.error('Error loading:', error);
    }
}

function displayItems(items) {
    const container = document.getElementById('itemsContainer');

    if (!container) {
        console.error('Container for items not found');  
        return;
    }

    container.innerHTML = '';

    if (items.length === 0) {  
        container.innerHTML = '<p class="no-items">No items found</p>';
        return;
    }

    const sortedItems = items.sort((a, b) => b.id - a.id);

    sortedItems.forEach((item, index) => {
        console.log(`Создаю карточку ${index + 1}:`, item);
        const itemCard = createItemCard(item);
        container.appendChild(itemCard);
    });
}

function createItemCard(item) {
    const card = document.createElement('div'); 
    card.className = 'user-card';  
    card.id = `item-${item.id}`;


    const foundDate = new Date(item.foundAtUtc).toLocaleString('ru-RU', {
        year: 'numeric',
        month: 'long',
        day: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
    });

    const itemName = item.name || item.Name || "Unnamed Item";
    const itemInfo = item.info || item.Info || "No description";

    //находить комнату
    const room = allRooms.find(r => r.id == item.roomId);
    let locationInfo = "Location unknown";

    if (room) {
        console.log('Room found for item:', item.id, room);
        //находить здание
        const building = allBuildings.find(b => b.id == room.buildingId);
        if (building) {
            locationInfo = `${building.name}, ${room.name}`;
        } else {
            locationInfo = `${room.name} (Этаж ${room.floor})`;
        }
    } else {
        locationInfo = `Room ID: ${item.roomId}`;
        console.warn('Room not found for item:', item);
    }

    card.innerHTML = `
        <h2>${itemName}</h2>
        <p><strong>Description:</strong> ${itemInfo}</p>
        <p><strong>Found in:</strong> ${locationInfo}</p>
        <p><strong>Found at:</strong> ${foundDate}</p>
        <p><strong>Item ID:</strong> ${item.id}</p>
        <div class="item-actions">
            <button onclick="deleteItem(${item.id})" class="delete-btn">Delete</button>
        </div>
    `;

    return card;
}

async function createItem() {
    const name = document.getElementById('Name').value;
    const info = document.getElementById('Info').value;
    const roomId = document.getElementById('RoomId').value;
    
    if (!name || !roomId) {
        alert('Please fill name and select room');
        return;
    }
    
    const url = `/api/items?Name=${encodeURIComponent(name)}&Info=${encodeURIComponent(info || '')}&RoomId=${roomId}`;
    
    try {
        const response = await fetch(url, { method: 'POST' });
        
        if (response.status === 201) {
            const result = await response.json();
            allItems.unshift(result);
            const roomExists = allRooms.some(r => r.id == roomId);
            if (!roomExists) {
                //находить выбранную комнату в селекте
                const roomSelect = document.getElementById('RoomId');
                const selectedOption = roomSelect.options[roomSelect.selectedIndex];
                const buildingId = document.getElementById('BuildingId').value;

                //получать номер этажа
                const floorMatch = selectedOption.textContent.match(/Этаж (\d+)/);
                const floor = floorMatch ? parseInt(floorMatch[1]) : 1;

                allRooms.push({
                    id: parseInt(roomId),
                    name: selectedOption.textContent.split(' (Этаж')[0].trim(),
                    floor: floor,
                    buildingId: parseInt(buildingId)
                });

                console.log('Room added to allRooms:', allRooms[allRooms.length - 1]);
            }
            allItems.unshift(result);
            closeModal();

            displayItems(allItems);

        } else {
            alert('Failed to create item');
        }
    } catch {
        alert('Network error');
    }
}

async function deleteItem(itemId) {
    if (!confirm('Delete this item?')) {
        return;
    }

    try {
        const response = await fetch(`/api/items/${itemId}`, {
            method: 'DELETE'
        });

        if (response.status === 204) {
            const itemCard = document.getElementById(`item-${itemId}`);
            if (itemCard) {
                itemCard.remove();
            }

            alert('Item deleted successfully!');
            allItems = allItems.filter(item => item.id !== itemId);
            loadItems();
        } else {
            const error = await response.json();
            alert(`Error: ${error.error || 'Failed to delete'}`);
        }

    } catch (error) {
        console.error('Delete error:', error);
        alert('Network error. Please try again.');
    }
}

function searchItems() {
    const searchInput = document.getElementById('searchInput');
    if (!searchInput) return;

    const searchText = searchInput.value.toLowerCase().trim();

    if (!searchText) {
        displayItems(allItems);
        return;
    }

    // фильтр по полям
    const filteredItems = allItems.filter(item => {
        const name = (item.name || item.Name || "").toLowerCase();
        const info = (item.info || item.Info || "").toLowerCase();
        const id = String(item.id || "");
        const roomId = String(item.roomId || "");

        return name.includes(searchText) ||
            info.includes(searchText) ||
            id.includes(searchText) ||
            roomId.includes(searchText);
    });

    displayItems(filteredItems);
}

function handleSearchKeypress(event) {
    if (event.key === 'Enter') {
        event.preventDefault();
        searchItems();
    }
}