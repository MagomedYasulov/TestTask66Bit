// Базовые URL для API
const API_BASE = '/api/v1';
const URL_INTERNS = `${API_BASE}/interns`;
const URL_INTERNSHIPS = `${API_BASE}/internships`;
const URL_PROJECTS = `${API_BASE}/projects`;

// Кэш для всех interns (используется в формах)
let allInterns = [];

// DataTables объекты
let internshipsTable, projectsTable;

// Текущая активная сущность ('internship' или 'project')
let currentEntityType = 'internship';

// Получаем список всех interns (для форм создания/редактирования)
function loadAllInterns() {
    return fetch(URL_INTERNS)
        .then(res => res.json())
        .then(data => {
            allInterns = data || [];
        })
        .catch(err => console.error('Ошибка загрузки interns:', err));
}

// Показ уведомления (всплывающее сообщение)
function showNotification(message, duration = 3000) {
    const $notif = $('#notification');
    $notif.text(message).fadeIn(200);
    setTimeout(() => {
        $notif.fadeOut(200);
    }, duration);
}

// Инициализация DataTable для заданного селектора
function initDataTable(selector, entityType) {
    return $(selector).DataTable({
        stateSave: true, // сохраняем параметры (поиск, сортировка, пагинация) в localStorage
        stateSaveCallback: function (settings, data) {
            localStorage.setItem(
                entityType + '-tableState',
                JSON.stringify(data)
            );
        },
        stateLoadCallback: function (settings) {
            const state = localStorage.getItem(entityType + '-tableState');
            return state ? JSON.parse(state) : null;
        },
        columns: [
            { data: 'name' },
            { data: 'internCount', orderable: true },
            { data: 'actions', orderable: false, searchable: false }
        ],
        order: [[0, 'asc']],
        language: {
            url: '//cdn.datatables.net/plug-ins/1.13.5/i18n/Русский.json'
        }
    });
}

// Подготовка и отрисовка строки таблицы
function prepareRowData(entity, entityType) {
    const id = entity.id;
    const name = entity.name;
    const internsArr = entity.interns || [];
    const internCount = internsArr.length;

    // Кнопка просмотра списка interns
    const viewBtn = `<button class="btn btn-view" data-type="${entityType}" data-id="${id}" data-action="view">Стажёры (${internCount})</button>`;

    // Кнопка редактирования
    const editBtn = `<button class="btn btn-edit" data-type="${entityType}" data-id="${id}" data-action="edit">Редактировать</button>`;

    // Кнопка удаления
    const deleteBtn = `<button class="btn btn-delete" data-type="${entityType}" data-id="${id}" data-action="delete">Удалить</button>`;

    // Формируем HTML для выпадающего списка interns (скрытая по умолчанию)
    let internListHtml = `<div class="intern-list" id="${entityType}-intern-list-${id}"><ul>`;
    internsArr.forEach(i => {
        const fullName = `${i.name} ${i.surname}`.trim();
        internListHtml += `<li>${fullName || '(без имени)'}</li>`;
    });
    internListHtml += '</ul></div>';

    return {
        id,
        name,
        internCount,
        actions: viewBtn + editBtn + deleteBtn + internListHtml
    };
}

// Загрузка данных и заполнение таблицы entityType ('internship' или 'project')
function loadAndRenderTable(entityType) {
    const URL = entityType === 'internship' ? URL_INTERNSHIPS : URL_PROJECTS;
    const table = entityType === 'internship' ? internshipsTable : projectsTable;

    fetch(URL)
    .then(res => res.json())
    .then(data => {
        table.clear();
        data.forEach(entity => {
            const row = prepareRowData(entity, entityType);
            table.row.add(row);
        });
        table.draw(false);
    })
    .catch(err => {
        console.error(`Ошибка загрузки ${entityType}:`, err);
        showNotification(`Не удалось загрузить ${entityType}`, 3000);
    });
}

// Открывает модальное окно с формой для создания/редактирования
function openFormModal(entityType, mode, entityData = null) {
    // mode: 'add' или 'edit'
    $('#entity-form')[0].reset();
    $('#entity-id').val('');
    $('#entity-interns').empty();

    // Заполняем селект всеми interns
    allInterns.forEach(intern => {
        const fullName = `${intern.name} ${intern.surname}`.trim();
        $('#entity-interns').append(
            `<option value="${intern.id}">${fullName}</option>`
        );
    });

    if (mode === 'add') {
        $('#modal-title').text(
            entityType === 'internship'
                ? 'Добавить Internship'
                : 'Добавить Project'
        );
    } else if (mode === 'edit' && entityData) {
        $('#modal-title').text(
            entityType === 'internship'
                ? 'Редактировать Internship'
                : 'Редактировать Project'
        );
        $('#entity-id').val(entityData.id);
        $('#entity-name').val(entityData.name || '');

        // Отмечаем в селекте тех interns, которые уже привязаны
        const boundInternIds = (entityData.interns || []).map(i => i.id);
        $('#entity-interns option').each(function () {
            if (boundInternIds.includes(parseInt($(this).val()))) {
                $(this).prop('selected', true);
            }
        });
    }

    // Сохраним в data текущий режим и тип
    $('#entity-form').data('mode', mode);
    $('#entity-form').data('type', entityType);

    $('#modal-overlay')
    .css('display', 'flex')  // указываем, что при показе должен быть flex
    .hide()                  // скрываем, чтобы fadeIn начал с opacity:0
    .fadeIn(200);
}

// Закрыть модальное окно
function closeFormModal() {
    $('#modal-overlay').fadeOut(200);
}

// Обработчик отправки формы (создать/редактировать)
$('#entity-form').on('submit', function (e) {
    e.preventDefault();
    const mode = $(this).data('mode'); // 'add' или 'edit'
    const entityType = $(this).data('type'); // 'internship' или 'project'
    const id = $('#entity-id').val();
    const name = $('#entity-name').val().trim();
    const selectedInternIds = $('#entity-interns').val().map(idStr => parseInt(idStr));

    if (!name) {
        alert('Название не может быть пустым.');
        return;
    }

    // URL и метод для создания/редактирования
    const URL =
        mode === 'add'
            ? entityType === 'internship'
                ? URL_INTERNSHIPS
                : URL_PROJECTS
            : entityType === 'internship'
                ? `${URL_INTERNSHIPS}/${id}`
                : `${URL_PROJECTS}/${id}`;
    const method = mode === 'add' ? 'POST' : 'PUT';

    const payload = { name, interns: selectedInternIds };
    fetch(URL, {
        method: method,
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(payload)
    })
    .then(res => {
        if (!res.ok) {
            return res.json().then(errData => {
                throw new Error(`Ошибка создания: ${errData.message || res.statusText}`);
            });
        }
        return res.json();
    })
    .then(() => {
        showNotification(mode == 'add' ? 'Сущность успешно создана' : "Сущность успешно обновлена");
        closeFormModal();
        // Перезагрузим таблицу
        loadAndRenderTable(entityType);
    })
    .catch(err => {
        console.error(err);
        alert(err.message);
    });
});

// Обработчик кнопки отмены в модальном окне
$('#modal-cancel').on('click', function () {
    closeFormModal();
});

// Обработчик кликов по табам
$('.tab').on('click', function () {
    $('.tab').removeClass('active');
    $(this).addClass('active');
    const target = $(this).data('target');
    $('.table-container').addClass('hidden');
    $(target).removeClass('hidden');

    // Обновим текущий тип сущности
    currentEntityType = target.includes('internships') ? 'internship' : 'project';
});

// Обработчик кнопок Добавить
$('#add-internship').on('click', function () {
    openFormModal('internship', 'add');
});
$('#add-project').on('click', function () {
    openFormModal('project', 'add');
});

// Делегированные обработчики для действий внутри таблиц (View, Edit, Delete)
$('body').on('click', 'button[data-action]', async function () {
    const action = $(this).data('action');
    const entityType = $(this).data('type');
    const id = $(this).data('id');

    if (action === 'view') {
        // Показываем/скрываем div с intern-list
        const $list = $(`#${entityType}-intern-list-${id}`);
        $list.toggle();
    }
    else if (action === 'edit') {
        // Получить данные сущности по ID и открыть форму редактирования
        const URL =
            entityType === 'internship'
                ? `${URL_INTERNSHIPS}/${id}`
                : `${URL_PROJECTS}/${id}`;
        fetch(URL)
        .then(res => res.json())
        .then(data => {
            openFormModal(entityType, 'edit', data);
        })
        .catch(err => {
            console.error('Ошибка получения данных для редактирования:', err);
            showNotification('Не удалось загрузить данные для редактирования', 3000);
        });
    }
    else if (action === 'delete') {
        // Перед удалением проверим, есть ли связанные interns
        const URL =
            entityType === 'internship'
                ? `${URL_INTERNSHIPS}/${id}`
                : `${URL_PROJECTS}/${id}`;

        // Выполняем DELETE
        fetch(URL, { method: 'DELETE' })
        .then(res => {
            if (!res.ok) {
                res.json().then(data => {
                    showNotification('Ошибка при удалении ' + data.detail, 3000);
                })
            }
            else { 
                showNotification('Удалено успешно');
                loadAndRenderTable(entityType);
            }
        })
        .catch(err => {
            console.error('Ошибка удаления:', err);
            showNotification('Ошибка при удалении', 3000);
        });
    }
});

// Инициализация: сначала загрузим всех interns, затем таблицы
Promise.all([loadAllInterns()]).then(() => {
    internshipsTable = initDataTable('#internships-table', 'internships');
    projectsTable = initDataTable('#projects-table', 'projects');

    // Загрузим и отрисуем данные
    loadAndRenderTable('internship');
    loadAndRenderTable('project');
});