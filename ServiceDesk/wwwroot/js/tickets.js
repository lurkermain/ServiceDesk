document.addEventListener("DOMContentLoaded", function () {
    const container = document.getElementById("ticket-list");

    fetch("/api/tickets")
        .then(response => response.json())
        .then(data => {
            container.innerHTML = "";
            data.forEach(ticket => {
                const card = document.createElement("div");
                card.className = "col-md-4";
                card.innerHTML = `
                    <div class="card h-100 shadow-sm ticket-card" data-id="${ticket.id}">
                        <div class="card-body">
                            <h5 class="card-title">${ticket.name}</h5>
                            <p class="card-text">${ticket.description}</p>
                            <p class="text-muted priority-cell">Приоритет: <strong>${ticket.priority}</strong></p>
                            <p class="text-muted">Статус: <strong>${ticket.status}</strong></p>
                            <p class="text-muted small">Создано: ${new Date(ticket.createdDate).toLocaleString()}</p>
                            <p class="text-muted small">Обновлено: ${new Date(ticket.updatedDate).toLocaleString()}</p>
                            ${ticket.imageUrl ? `<a href="${ticket.imageUrl}" target="_blank" class="btn btn-sm btn-outline-primary">Картинка</a>` : ''}
                        </div>
                    </div>
                `;
                container.appendChild(card);
            });

            // Навешиваем обработчик после вставки карточек
            document.querySelectorAll(".ticket-card").forEach(card => {
                card.addEventListener("click", () => {
                    const id = card.getAttribute("data-id");
                    openEditModal(id);
                });
            });
        });
});

function openEditModal(id) {
    // Простой fetch, можно расширить до получения полной инфы
    fetch(`/api/tickets`)
        .then(response => response.json())
        .then(data => {
            const ticket = data.find(t => t.id == id);
            if (!ticket) return;

            document.getElementById("edit-id").value = ticket.id;
            document.getElementById("edit-name").value = ticket.name;
            document.getElementById("edit-description").value = ticket.description;
            document.getElementById("edit-priority").value = ticket.priority;
            document.getElementById("edit-status").value = ticket.status;

            const modal = new bootstrap.Modal(document.getElementById("editTicketModal"));
            modal.show();
        });
}

document.getElementById("edit-form").addEventListener("submit", function (e) {
    e.preventDefault();

    const formData = new FormData();
    formData.append("Name", document.getElementById("edit-name").value);
    formData.append("Description", document.getElementById("edit-description").value);
    formData.append("Priority", document.getElementById("edit-priority").value);
    formData.append("Status", document.getElementById("edit-status").value);
    formData.append("File", document.getElementById("edit-file").files[0] || ""); // файл не обязателен

    const id = document.getElementById("edit-id").value;

    // ownerId и adminId захардкожены для примера
    fetch(`/api/tickets?id=${id}&ownerId=1&adminId=1`, {
        method: "PATCH",
        body: formData
    })
        .then(res => res.ok ? location.reload() : alert("Ошибка при обновлении"))
});

document.getElementById("delete-ticket").addEventListener("click", function () {
    const id = document.getElementById("edit-id").value;
    if (confirm("Вы уверены, что хотите удалить эту заявку?")) {
        fetch(`/api/tickets?id=${id}`, {
            method: "DELETE"
        }).then(res => {
            if (res.ok) {
                location.reload();
            } else {
                alert("Ошибка при удалении");
            }
        });
    }
});

function openCreateModal() {
    document.getElementById("create-form").reset();
    const modal = new bootstrap.Modal(document.getElementById("createTicketModal"));
    modal.show();
}

document.getElementById("create-form").addEventListener("submit", function (e) {
    e.preventDefault();

    const formData = new FormData();
    formData.append("Name", document.getElementById("create-name").value);
    formData.append("Description", document.getElementById("create-description").value);
    formData.append("Priority", document.getElementById("create-priority").value);
    formData.append("Status", document.getElementById("create-status").value);
    formData.append("File", document.getElementById("create-file").files[0] || "");

    // временно ownerId и adminId хардкодим
    fetch(`/api/tickets?ownerId=1&adminId=1`, {
        method: "POST",
        body: formData
    }).then(res => {
        if (res.ok) {
            location.reload();
        } else {
            alert("Ошибка при создании");
        }
    });
});

let isSortedByPriorityAsc = true; // Начинаем с сортировки по убыванию

function sortTicketsByPriority() {
    const priorityOrder = { "Critical": 1, "High": 2, "Medium": 3, "Low": 4 };
    const container = document.getElementById("ticket-list");
    const cards = Array.from(container.querySelectorAll(".ticket-card"));

    // Сортируем карточки по приоритету в зависимости от текущего состояния сортировки
    cards.sort((a, b) => {
        const prioA = a.querySelector(".priority-cell strong")?.textContent.trim();
        const prioB = b.querySelector(".priority-cell strong")?.textContent.trim();

        const valueA = priorityOrder[prioA] || Infinity;
        const valueB = priorityOrder[prioB] || Infinity;

        return isSortedByPriorityAsc ? valueA - valueB : valueB - valueA;
    });

    // Очистка контейнера
    container.innerHTML = "";

    // Перерисовываем карточки в новом порядке
    cards.forEach(card => {
        const wrapper = document.createElement("div");
        wrapper.className = "col-md-4";
        wrapper.appendChild(card);
        container.appendChild(wrapper);
    });

    // Переключаем состояние сортировки
    isSortedByPriorityAsc = !isSortedByPriorityAsc;
}

function sortTicketsByNew() {
    isNewestFirst = !isNewestFirst;
    updateSortButton();

    const container = document.getElementById("ticket-list");
    const cards = Array.from(container.querySelectorAll(".ticket-card"));

    cards.sort((a, b) => {
        const dateA = new Date(a.getAttribute('data-created-date'));
        const dateB = new Date(b.getAttribute('data-created-date'));
        return isNewestFirst ? dateB - dateA : dateA - dateB;
    });

    container.innerHTML = "";
    cards.forEach(card => container.appendChild(card));
}


function filterTicketsByName() {
    const query = document.getElementById("searchInput").value.toLowerCase();
    const cardContainers = document.querySelectorAll("#ticket-list .col-md-4"); // Ищем именно контейнеры колонок

    cardContainers.forEach(container => {
        const card = container.querySelector(".ticket-card");
        if (!card) return;

        const nameElement = card.querySelector(".card-title");
        const name = nameElement ? nameElement.textContent.toLowerCase() : "";

        // Скрываем/показываем всю колонку (col-md-4), а не только карточку
        container.style.display = name.includes(query) ? "" : "none";
    });
}