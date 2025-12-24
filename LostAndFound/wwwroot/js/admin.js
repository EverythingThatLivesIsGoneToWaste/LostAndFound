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

// Закрытие при клике вне окна
document.addEventListener('DOMContentLoaded', function () {

    window.addEventListener('click', function (event) {
        const modal = document.getElementById("itemModal");

        if (modal && event.target === modal) {
            closeModal();
        }
    });
});