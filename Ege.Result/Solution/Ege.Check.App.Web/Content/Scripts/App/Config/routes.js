var routes = {
    startPage: '/',
    examsPage: '/exams',
    examPage: function(id) {
        return '/exams/' + id;
    },
    appealPage: function(id) {
        return '/appeal/' + id;
    },
    login: '/api/participant/login',
    logout: '/api/participant/logout',
    captcha: '/api/captcha',
    region: '/api/region',
    getExams: '/api/exam',
    getExam: function(id) {
        return '/api/exam/' + id;
    },
    getAppeal: function(id) {
        return '/api/appeal/' + id;
    }
};

module.exports = routes;