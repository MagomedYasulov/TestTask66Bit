const API_BASE = '/api/v1';
const URL_INTERN = `${API_BASE}/interns`;
const URL_INTERNSHIPS = `${API_BASE}/internships`;
const URL_PROJECTS = `${API_BASE}/projects`;

let internships = [];
let projects = [];
let internsTable;

const connection = new signalR.HubConnectionBuilder()
    .withUrl("/interns") // Укажите URL вашего SignalR хаба
    .configureLogging(signalR.LogLevel.Information)
    .build();

// Загрузить направления и проекты
function loadInternshipsAndProjects() {
    return Promise.all([
        fetch(URL_INTERNSHIPS).then((r) => r.json()),
        fetch(URL_PROJECTS).then((r) => r.json())
    ]).then(([interData, projData]) => {
        internships = interData || [];
        projects = projData || [];
    });
}

// Заполнить селекты направлениями
function populateInternshipSelects() {
    const $filter = $('#filter-direction');
    const $edit = $('#edit-direction');
    $filter.empty().append(`<option value="">Все направления</option>`);
    $edit.empty().append(`<option value="">Выберите направление</option>`);
    internships.forEach((d) => {
        const opt = `<option value="${d.id}">${d.name}</option>`;
        $filter.append(opt);
        $edit.append(opt);
    });
}

// Заполнить селекты проектами
function populateProjectSelects() {
    const $filter = $('#filter-project');
    const $edit = $('#edit-project');
    $filter.empty().append(`<option value="">Все проекты</option>`);
    $edit.empty().append(`<option value="">Выберите проект</option>`);
    projects.forEach((p) => {
        const opt = `<option value="${p.id}">${p.name}</option>`;
        $filter.append(opt);
        $edit.append(opt);
    });
}

// Показ уведомления
function showNotification(msg, duration = 3000) {
    const $n = $('#notification');
    $n.text(msg).fadeIn(200);
    setTimeout(() => {
        $n.fadeOut(200);
    }, duration);
}

// Инициализация DataTable с horizontal scrollX
function initTable() {
    internsTable = $('#interns-table').DataTable({
        scrollX: true,
        autoWidth: false,
        columns: [
            { data: 'name' },
            { data: 'surname' },
            { data: 'gender' },
            { data: 'email' },
            { data: 'phone' },
            { data: 'birthDate' },
            { data: 'internshipName' },
            { data: 'projectName' },
            { data: 'actions', orderable: false, searchable: false }
        ],
        language: {
            url: '//cdn.datatables.net/plug-ins/1.13.5/i18n/Русский.json'
        },
        order: [[0, 'asc']]
    });
}

// Подготовка данных для строки
function prepareRowData(intern) {
    return {
        id: intern.id,
        name: intern.name,
        surname: intern.surname,
        gender: intern.gender === 0 ? 'Мужской' : 'Женский',
        email: intern.email,
        phone: intern.phone || '',
        birthDate: intern.birthDate ? intern.birthDate.split('T')[0] : '',
        internshipId: intern.internshipId,
        internshipName: intern.internship.name,
        projectId: intern.projectId,
        projectName: intern.project.name,
        actions:
            `<button class="btn btn-edit" data-id="${intern.id}" data-update="upd" data-action="edit">Редактировать</button>` +
            `<button class="btn btn-delete" data-id="${intern.id}" data-update="upd" data-action="delete">Удалить</button>`
    };
}

// Загрузка и отрисовка стажёров с учётом фильтров
function loadAndRenderInterns() {
    const dirId = $('#filter-direction').val();
    const projId = $('#filter-project').val();
    let url = URL_INTERN;
    const params = [];
    if (dirId) params.push(`InternshipId=${dirId}`);
    if (projId) params.push(`ProjectId=${projId}`);
    if (params.length) url += `?${params.join('&')}`;

    fetch(url)
    .then((res) => res.json())
    .then((data) => {
        internsTable.clear();
        data.forEach((intern) => {
            internsTable.row.add(prepareRowData(intern));
        });
        internsTable.draw(false);
    })
    .catch((err) => {
        console.error(err);
        showNotification('Ошибка при загрузке списка стажёров', 3000);
    });
}

// Открыть модалку редактирования
function openEditModal(internId) {
    $('#edit-error-detail').text('');
    fetch(`${URL_INTERN}/${internId}`)
    .then((res) => res.json())
    .then((intern) => {
        $('#edit-id').val(intern.id);
        $('#edit-first-name').val(intern.name || '');
        $('#edit-last-name').val(intern.surname || '');
        $('#edit-gender').val(intern.gender);
        $('#edit-email').val(intern.email || '');
        $('#edit-phone').val(intern.phone || '');
        $('#edit-birthdate').val(intern.birthDate ? intern.birthDate.split('T')[0] : '');
        $('#edit-direction').val(intern.internshipId);
        $('#edit-project').val(intern.projectId);

        $('#modal-edit-overlay')
            .css('display', 'flex')
            .hide()
            .fadeIn(200);
    })
    .catch((err) => {
        console.error(err);
        showNotification('Ошибка при получении данных стажёра', 3000);
    });
}

// Закрыть модалку редактирования
function closeEditModal() {
    $('#modal-edit-overlay').fadeOut(200);
}

// Сохранить изменения в стажёре
$('#edit-form').on('submit', function (e) {
    e.preventDefault();
    $('#edit-error-detail').text('');

    const id = $('#edit-id').val();
    const payload = {
        name: $('#edit-first-name').val().trim(),
        surname: $('#edit-last-name').val().trim(),
        gender: parseInt($('#edit-gender').val(), 10),
        email: $('#edit-email').val().trim(),
        phone: $('#edit-phone').val().trim() || null,
        birthDate: $('#edit-birthdate').val(),
        internshipId: parseInt($('#edit-direction').val(), 10),
        projectId: parseInt($('#edit-project').val(), 10)
    };

    $('#save-edit').prop('disabled', true);
    fetch(`${URL_INTERN}/${id}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(payload)
    })
    .then((res) => {
        if (!res.ok) {
            return res.json().then((err) => {
                throw err;
            });
        }
        return res.json();
    })
    .then(() => {
        showNotification('Данные стажёра успешно обновлены');
        closeEditModal();
        loadAndRenderInterns();
    })
    .catch((err) => {
        const detail = err.detail || err.message || 'Ошибка при обновлении';
        $('#edit-error-detail').text(detail);
    })
    .finally(() => {
        $('#save-edit').prop('disabled', false);
    });
});

// Делегированный клик на кнопку «Редактировать»
$('body').on('click', 'button[data-update="upd"]', function () { 
    const action = $(this).data('action');
    const id = $(this).data('id');
    if (action === 'edit') {
        openEditModal(id);
    }
    else if (action === 'delete') {
        if (confirm('Вы уверены, что хотите удалить этого стажёра?')) {
            fetch(`${URL_INTERN}/${id}`, { method: 'DELETE' })
            .then((res) => {
                if (!res.ok) {
                    return res.json().then((err) => { throw err; });
                }
                showNotification('Стажёр успешно удалён');
                loadAndRenderInterns();
            })
            .catch((err) => {
                const detail = err.detail || err.message || 'Ошибка при удалении';
                showNotification(detail, 4000);
            });
        }
    }
});

// Отмена редактирования
$('#cancel-edit').on('click', closeEditModal);

// Обработчики фильтров
$('#filter-direction, #filter-project').on('change', loadAndRenderInterns);
$('#clear-filters').on('click', function () {
    $('#filter-direction').val('');
    $('#filter-project').val('');
    loadAndRenderInterns();
});

// Start the connection
connection.start().then(() => {
    console.log("SignalR connection established.");
}).catch(err => {
    console.error("SignalR connection error: ", err);
});

// Handle incoming messages
connection.on("OnInternCreate", (intern) => {
    loadAndRenderInterns();
});

connection.on("OnInternUpdate", (message) => {
    loadAndRenderInterns()
});

connection.on("OnInternDelete", (messageId) => {
    loadAndRenderInterns()
});

// Инициализация страницы
loadInternshipsAndProjects().then(() => {
    populateInternshipSelects();
    populateProjectSelects();
    initTable();
    loadAndRenderInterns();
});

