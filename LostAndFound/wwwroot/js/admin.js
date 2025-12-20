function openModal(){
    const modal = document.getElementById("itemModal");

    if(modal){
        modal.style.display = 'block';

        setTimeout(() => {
            modal.style.opacity = '1';
        }, 300);

        document.getElementById('taskForm').reset();
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
    
    // Обработчик отправки формы
    const form = document.getElementById('itemForm');
    if (form) {
        form.addEventListener('submit', function(event) {
            event.preventDefault(); 

            const itemData = {
                name: document.getElementById('itemName').value.trim(),
                info: document.getElementById('InfoId').value.trim(),
                foundAt: document.getElementById('FoundAt').value.trim(),
                room: document.getElementById('RoomId').value.trim(),
                date: new Date().toISOString().split('T')[0] 
            };
            
            console.log('Сохранение предмета:', itemData);
            
            // Здесь будет отправка на сервер
            // saveItem(itemData);
            
            closeModal();
            
            alert('Предмет успешно добавлен!');
            
            form.reset();
        });
    }
});