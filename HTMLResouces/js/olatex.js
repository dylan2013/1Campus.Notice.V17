if (!RedactorPlugins) var RedactorPlugins = {};

(function($) {
    RedactorPlugins.olatex = function() {
        return {
            getTemplate: function() {
                return ['<section id="redactor-modal-latex-insert">\
                            <div style="box-sizing:border-box;display:-webkit-flex;display:-ms-flexbox;display:flex;-webkit-flex-direction:row;-ms-flex-direction:row;flex-direction:row;">\
                                <div style="font-weight:bold">Type your equation in this box</div>\
                                <div style="-webkit-flex:1;-ms-flex:1;flex:1;"></div>\
                                <a href="http://docs.mathjax.org/en/latest/index.html" target="_blank">MathJax Documentation</a>\
                                <div style="margin:0 5px">|</div>\
                                <a href="http://www.codecogs.com/latex/eqneditor.php" target="_blank">LaTex Online Editor</a>\
                            </div>\
                            <textarea id="redactor-insert-latex-input" style="padding:10px;height:80px;width:600px"></textarea>\
                            <div id="redactor-insert-olatex-output" style="border:2px dashed #eee;margin-top:20px;padding:10px;height:80px;width:600px"></div>\
                        </section>'].join('');
            },
            init: function() {
                var button = this.button.add('latex', 'MathJax or LaTex');
                this.button.setAwesome('latex', 'fa-superscript');
                this.button.addCallback(button, this.olatex.show);
            },
            show: function() {
                this.modal.addTemplate('latex', this.olatex.getTemplate());

                this.modal.load('latex', 'MathJax or LaTex', 700);
                this.modal.createCancelButton();

                var button = this.modal.createActionButton(this.lang.get('insert'));
                button.on('click', this.olatex.insert);

                this.selection.save();
                this.modal.show();

                $('#redactor-insert-latex-input').focus();
                $('#redactor-insert-latex-input').on('keyup', function() {
                    $('#redactor-insert-olatex-output').html(
                        ['<script type="math/tex">', $('#redactor-insert-latex-input').val(), '</script>'].join('')
                    );
                    $('#redactor-modal-box footer').hide();
                    var element = document.querySelector('#redactor-insert-olatex-output');
                    MathJax.Hub.Queue(["Reprocess", MathJax.Hub, element, function() {
                        $('#redactor-modal-box footer').show();
                    }]);
                });
            },
            insert: function() {
                var data = $('#redactor-insert-olatex-output').html();

                this.selection.restore();
                this.modal.close();

                var current = this.selection.getBlock() || this.selection.getCurrent();

                if (current) $(current).after(data);
                else {
                    this.insert.html(data);
                }

                this.code.sync();
            }
        };
    };
})(jQuery);
