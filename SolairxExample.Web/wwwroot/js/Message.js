(function ($) {
    function HomeIndex() {
        var $this = this;

        function initialize() {
            $('#Message').summernote({
                focus: true,
                height: 250,
                toolbar: [
                    // ['cleaner', ['cleaner']], // The Button
                    ['style', ['style']],
                    ['font', ['bold', 'italic']],
                    // ['fontname', ['fontname']],
                    //['color', ['color']],
                    ['para', ['ul', 'ol']],
                    //['height', ['height']],
                    //['table', ['table']],
                   // ['insert', ['media', 'link']],
                    //['view', ['fullscreen', 'codeview']],
                    //['help', ['help']]
                ],
                //link: [
                //    ['link', ['linkDialogShow', 'unlink']]],
                styleTags: ['p', 'h4'],
                cleaner: {
                    notTime: 2400, // Time to display Notifications.
                    action: 'paste', // both|button|paste 'button' only cleans via toolbar button, 'paste' only clean when pasting content, both does both options.
                    newline: '<br>', // Summernote's default is to use '<p><br></p>'
                    notStyle: 'position:absolute;top:0;left:0;right:0', // Position of Notification
                    icon: '<i class="fa fa-fire-extinguisher"></i>',
                    keepHtml: true, // Remove all Html formats
                    keepOnlyTags: ['<p>', '<br>', '<ul>', '<li>', '<b>', '<strong>', '<i>', '<a>'], // If keepHtml is true, remove all tags except these
                    keepClasses: false, // Remove Classes
                    badTags: ['style', 'script', 'applet', 'embed', 'img', 'noframes', 'noscript', 'link', 'media', 'html'], // Remove full tags with contents
                    badAttributes: ['style', 'start'] // Remove attributes from remaining tags
                }

            });
        }

        $this.init = function () {
            initialize();
        }
    }
    $(function () {
        var self = new HomeIndex();
        self.init();
    })
}(jQuery))