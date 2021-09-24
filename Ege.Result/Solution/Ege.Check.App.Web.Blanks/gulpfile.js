/// <binding BeforeBuild='less' />
var requireDir = require('require-dir');
requireDir('./gulp/tasks', { recurse: true });
