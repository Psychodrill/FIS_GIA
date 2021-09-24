var routes = {
    login: '/api/staff/login',
    logout: '/api/staff/logout',
    activate: '/api/staff/activate',
    deactivate: '/api/staff/deactivate',
    getSettingsForMyRegion: '/api/staff/examsettings',
    setSettingsForMyRegion: '/api/staff/examsettings',
    getSettings: function(regionId) {
        return '/api/staff/examsettings/' + regionId;
    },
    setSettings: function(regionId) {
        return '/api/staff/examsettings/' + regionId;
    },
    getGekDocument: function(examId) {
        return '/api/staff/examsettings/gek/' + examId;
    },
    editGekDocument: function(examId) {
        return '/api/staff/examsettings/gek/' + examId;
    },
    deleteGekDocument: function(examId) {
        return '/api/staff/examsettings/gek/' + examId;
    },
    getFctGekDocument: function(examId, regionId) {
        return '/api/staff/examsettings/gek/' + examId +
            '/' + regionId;
    },
    editFctGekDocument: function(examId, regionId) {
        return '/api/staff/examsettings/gek/' + examId +
            '/' + regionId;
    },
    deleteFctGekDocument: function(examId, regionId) {
        return '/api/staff/examsettings/gek/' + examId +
            '/' + regionId;
    },
    regionInfo: '/api/staff/regionInfo',
    setPassword: '/api/staff/setpassword',
    getCancelled: '/api/staff/cancel',
    getExamList: '/api/staff/examlist',
    cancel: '/api/staff/cancel',
    uncancel: '/api/staff/cancel/undo',
    documents: '/api/staff/documentUrls',
    getSubjects: '/api/staff/subject',
    taskSettings: function(subjectId) {
        return '/api/staff/tasksettings/' + subjectId;
    },
    getUsers: '/api/staff/user',
    createUser: '/api/staff/user',
    editUser: function(id) {
        return '/api/staff/user/' + id;
    },
    deleteUser: function(id) {
        return '/api/staff/user/' + id;
    },
    getAvailabilityStatus: '/api/servers',
    getStatusDetailsForRegion: function (id) {
        return '/api/servers/' + id + '/details';
    },
    checkRegion: function (id) {
        return '/api/servers/' + id + '/check';
    },
    checkAllRegions: '/api/servers/checkall',

    startPage: '/rcoi',
    settingsPage: '/rcoi/settings',
};

module.exports = routes;