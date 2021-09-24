module.exports = {
	login: '/api/auth/login',
	downloadArchiveByParticipant: '/api/blanks',
	uploadCsv: '/api/blanks/multi',
	donwloadZip: function (id) {
		return '/api/downloads/' + id;
	},
	getDownloads: '/api/downloads',
	blanksPage: '/blanks'
};