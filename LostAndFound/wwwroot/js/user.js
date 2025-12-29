let allBuildings = [];
let allRooms = [];
let allItems = [];

document.addEventListener('DOMContentLoaded', function () {
    loadBuildings();
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
            await loadAllRooms();
            await loadItems();
        } else {
            console.error('Ошибка загрузки зданий')
        }
    } catch (error) {
        console.error('Ошибка:', error);
    }
}

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

// Загрузка вещей
async function loadItems() {
    const container = document.getElementById('itemsContainer');

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

    const foundAt =
        new Intl.DateTimeFormat('ru-RU', {
            year: 'numeric',
            month: '2-digit',
            day: '2-digit',
            hour: '2-digit',
            minute: '2-digit',
            hour12: false
        }).format(new Date(item.foundAtUtc));

    const itemName = item.name || item.Name || "Unnamed Item";
    const itemInfo = item.info || item.Info || "No description";

    const room = allRooms.find(r => r.id == item.roomId);
    let locationInfo = "Location unknown";

    if (room) {
        const building = allBuildings.find(b => b.id == room.buildingId);
        const buildingName = building ? building.name : "Unknown building";

        locationInfo = `${buildingName}, ${room.name}`;
    } else {
        locationInfo = `Room ID: ${item.roomId}`;
        console.warn('Room not found for item:', item.id, 'roomId:', item.roomId);
    }

    card.innerHTML = `
        <h2>${itemName}</h2>
        <p><strong>Description:</strong> ${itemInfo}</p>
        <p><strong>Found in:</strong> ${locationInfo}</p>
        <p><strong>Found at:</strong> ${foundAt}</p>
    `;

    return card;
}

function searchItems() {
    const searchInput = document.getElementById('searchInput');
    if (!searchInput) return;

    const searchText = searchInput.value.toLowerCase().trim();

    if (!searchText) {
        displayItems(allItems);
        return;
    }
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