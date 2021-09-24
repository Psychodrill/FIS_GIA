var dest = './Content/Build',
    src = './Content';

module.exports = {
  browserSync: {
    server: {
      baseDir: src
    }
  },
  less: {
    src: src + '/Styles/Pages/*.less',
    watch: [
      src + '/Styles/**'
    ],
    dest: dest + '/css'
  },
  jshint: {
    src: src + '/Scripts/App/**'
  },
  browserify: {
    debug: true,
    dest: dest + '/js',
    bundleConfigs: [{
        entries: src + '/Scripts/App/Login/entry.js',
        outputName: 'login.js'
    }, {
        entries: src + '/Scripts/App/Blanks/entry.js',
        outputName: 'start.js'
    }, {
        entries: src + '/Scripts/App/Downloads/entry.js',
        outputName: 'downloads.js'
    }]
  }
};
