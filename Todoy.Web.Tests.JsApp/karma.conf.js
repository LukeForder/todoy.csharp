// Karma configuration
// Generated on Wed Dec 17 2014 12:19:55 GMT+0200 (South Africa Standard Time)

module.exports = function(config) {
  config.set({

    // base path that will be used to resolve all patterns (eg. files, exclude)
    basePath: '',


    // frameworks to use
    // available frameworks: https://npmjs.org/browse/keyword/karma-adapter
    frameworks: ['jasmine'],


    // list of files / patterns to load in the browser
    files: [
		"../Todoy.Web/Scripts/underscore.js",
		"../Todoy.Web/Scripts/jquery-2.1.1.js",
		"../Todoy.Web/Scripts/bootstrap.js",
		"../Todoy.Web/Scripts/angular.js",
		"../Todoy.Web/Scripts/angular-route.js",
		"../Todoy.Web/Scripts/angular-mocks.js",
		"../Todoy.Web/Scripts/angular-input-match.js",
		"../Todoy.Web/JsApp/authentication/**/*.js",
		"../Todoy.Web/JsApp/todo/**/*.js",
		"../Todoy.Web/JsApp/module.js",
		"./*.js"
    ],


    // list of files to exclude
    exclude: [
    ],


    // preprocess matching files before serving them to the browser
    // available preprocessors: https://npmjs.org/browse/keyword/karma-preprocessor
    preprocessors: {
    },


    // test results reporter to use
    // possible values: 'dots', 'progress'
    // available reporters: https://npmjs.org/browse/keyword/karma-reporter
    reporters: ['progress'],


    // web server port
    port: 9876,


    // enable / disable colors in the output (reporters and logs)
    colors: true,


    // level of logging
    // possible values: config.LOG_DISABLE || config.LOG_ERROR || config.LOG_WARN || config.LOG_INFO || config.LOG_DEBUG
    logLevel: config.LOG_INFO,


    // enable / disable watching file and executing tests whenever any file changes
    autoWatch: true,


    // start these browsers
    // available browser launchers: https://npmjs.org/browse/keyword/karma-launcher
    browsers: ['Chrome'],


    // Continuous Integration mode
    // if true, Karma captures browsers, runs the tests and exits
    singleRun: false
  });
};
