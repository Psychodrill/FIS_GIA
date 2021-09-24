var dest = './Content/Build',
    src = './Content';

module.exports = {
    browserSync: {
        server: {
            baseDir: src
        },
        open: false
    },
    less: {
        src: src + '/Styles/Pages/*.less',
        watch: [
            src + '/Styles/**',
        ],
        dest: dest + '/css'
    },
    html: {
        src: src + '/Pages/*.html',
        dest: dest + '/html',
        watch: [src + '/Pages/**']
    },
    jshint: {
        src: src + '/Scripts/App/**'
    },
    browserify: {
        dest: dest + '/js/',
        bundleConfigs: [{entries: src + '/Scripts/App/Participant/Start/entry.js',
                outputName: 'start.js'
            }, {
                entries: src + '/Scripts/App/Participant/Exams/entry.js',
                outputName: 'exams.js'
            }, {
                entries: src + '/Scripts/App/Participant/Exam/entry.js',
                outputName: 'exam.js'
            }, {
                entries: src + '/Scripts/App/Participant/Appeal/entry.js',
                outputName: 'appeal.js'
            }, {
                entries: src + '/Scripts/App/Staff/Start/entry.js',
                outputName: 'staff-start.js'
            }, {
                entries: src + '/Scripts/App/Staff/Settings/entry.js',
                outputName: 'settings.js'
            }, {
                entries: src + '/Scripts/App/Staff/Cancelled/entry.js',
                outputName: 'cancelled.js'
            }, {
                entries: src + '/Scripts/App/Staff/Profile/entry.js',
                outputName: 'profile.js'
            }, {
                entries: src + '/Scripts/App/Staff/Documents/entry.js',
                outputName: 'documents.js'
            }, {
                entries: src + '/Scripts/App/Staff/TaskSettings/entry.js',
                outputName: 'tasksettings.js'
            }, {
                entries: src + '/Scripts/App/Staff/Users/entry.js',
                outputName: 'users.js'
            }, {
                entries: src + '/Scripts/App/Staff/Activation/entry.js',
                outputName: 'activation.js'
            }, {
                entries: src + '/Scripts/App/Staff/ServersAccessibility/entry.js',
                outputName: 'servers-accessibility.js'
            }]
    },
    mssql: {
        user: 'sa',
        password: '2222',
        server: 'localhost',
        database: 'CheckEge2020',
        port: 1433
    },
    regions: {
        folder: src + '/Scripts/App/Config/',
        file: 'regions.json'
    },
    appealStatuses: {
        folder: src + '/Scripts/App/Config/',
        file: 'appeal-statuses.json'
    }
};