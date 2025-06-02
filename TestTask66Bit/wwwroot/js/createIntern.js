const API_BASE = '/api/v1';
const URL_INTERN = `${API_BASE}/interns`;
const URL_INTERNSHIPS = `${API_BASE}/internships`;
const URL_PROJECTS = `${API_BASE}/projects`;

let internships = [];
let projects = [];

function loadInitialData() {
    return Promise.all([
        fetch(URL_INTERNSHIPS).then((r) => r.json()),
        fetch(URL_PROJECTS).then((r) => r.json())        
    ]).then(([dirData, projData]) => {
        internships = dirData || [];
        projects = projData || [];
    });
}

function populateInternships() {
    const $sel = $('#internship-direction');
    $sel.empty();
    $sel.append(`<option value="">Выберите направление</option>`);
    internships.forEach((d) => {
        $sel.append(`<option value="${d.id}">${d.name}</option>`);
    });
}

function populateProjects() {
    const $sel = $('#current-project');
    $sel.empty();
    $sel.append(`<option value="">Выберите проект</option>`);
    projects.forEach((p) => {
        $sel.append(`<option value="${p.id}">${p.name}</option>`);
    });
}

function showNotification(msg, duration = 3000) {
    const $n = $('#notification');
    $n.text(msg).fadeIn(200);
    setTimeout(() => {
        $n.fadeOut(200);
    }, duration);
}

function openIntershipModal() {
    $('#new-internship-name').val('');
    $('#error-new-internship').text('');
    $('#modal-internship-overlay')
        .css('display', 'flex')
        .hide()
        .fadeIn(200);
}
function closeInternshipModal() {
    $('#modal-internship-overlay').fadeOut(200);
}

$('#save-new-direction').on('click', function () {
    const name = $('#new-internship-name').val().trim();
    $('#error-new-internship').text('');
    if (!name) {
        $('#error-new-internship').text('Название обязательно.');
        return;
    }
    fetch(URL_INTERNSHIPS, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ name, interns: [] })
    })
    .then((res) => {
        if (!res.ok) {
            return res.json().then((err) => {
                throw err;
            });
        }
        return res.json();
    })
    .then((newDirOrArray) => {
        const newDir = Array.isArray(newDirOrArray)
            ? newDirOrArray.find((d) => d.name === name)
            : newDirOrArray;
        if (!newDir) throw { detail: 'Не удалось получить созданное направление' };
        internships.push(newDir);
        populateInternships();
        $('#internship-direction').val(newDir.id);
        closeInternshipModal();
        showNotification('Направление создано');
    })
    .catch((err) => {
        const detail = err.detail || err.message || 'Ошибка при создании направления';
        $('#error-new-internship').text(detail);
    });
});
$('#cancel-new-direction').on('click', closeInternshipModal);

// Модалки для Project
function openProjectModal() {
    $('#new-project-name').val('');
    $('#error-new-project').text('');
    $('#modal-project-overlay')
        .css('display', 'flex')
        .hide()
        .fadeIn(200);
}
function closeProjectModal() {
    $('#modal-project-overlay').fadeOut(200);
}

$('#save-new-project').on('click', function () {
    const name = $('#new-project-name').val().trim();
    $('#error-new-project').text('');
    if (!name) {
        $('#error-new-project').text('Название обязательно.');
        return;
    }
    fetch(URL_PROJECTS, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ name, interns: [] })
    })
    .then((res) => {
        if (!res.ok) {
            return res.json().then((err) => {
                throw err;
            });
        }
        return res.json();
    })
    .then((newProject) => {
        projects.push(newProject);
        populateProjects();
        $('#current-project').val(newProject.id);
        closeProjectModal();
        showNotification('Проект создан');
    })
    .catch((err) => {
        const detail = err.detail || err.message || 'Ошибка при создании проекта';
        $('#error-new-project').text(detail);
    });
});
$('#cancel-new-project').on('click', closeProjectModal);

$('#new-direction-btn').on('click', openIntershipModal);
$('#new-project-btn').on('click', openProjectModal);

$('#intern-form').on('submit', function (e) {
    e.preventDefault();
    $('#error-detail').text('');

    const payload = {
        name: $('#first-name').val().trim(),
        surname: $('#last-name').val().trim(),
        gender: $('#gender').val(),
        email: $('#email').val().trim(),
        phone: $('#phone').val().trim() || null,
        birthDate: $('#birthdate').val(),
        internshipId: parseInt($('#internship-direction').val(), 10),
        projectId: parseInt($('#current-project').val(), 10)
    };

    $('#submit-btn').prop('disabled', true);

    fetch(URL_INTERN, {
        method: 'POST',
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
    .then((newIntern) => {
        showNotification('Новый стажёр успешно добавлен');
        $('#intern-form')[0].reset();
    })
    .catch((err) => {
        const detail = err.detail || err.message || 'Ошибка при создании стажёра';
        $('#error-detail').text(detail);
    })
    .finally(() => {
        $('#submit-btn').prop('disabled', false);
    });
});

loadInitialData().then(() => {
    populateInternships();
    populateProjects();
});