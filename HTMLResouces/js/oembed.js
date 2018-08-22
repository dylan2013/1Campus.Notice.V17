if (!RedactorPlugins) var RedactorPlugins = {};

(function($) {
    RedactorPlugins.oembed = function() {
        return {
            getTemplate: function() {
                return ['<section id="redactor-modal-oembed-insert">\
                            <label>Embed Code</label>\
                            <textarea id="redactor-insert-oembed" style="height:160px"></textarea>\
                        </section>'].join('');
            },
            init: function() {
                var button = this.button.add('oembed', 'Insert Embed Code');
                this.button.setAwesome('oembed', 'fa-code');
                this.button.addCallback(button, this.oembed.show);
            },
            show: function() {
                this.modal.addTemplate('oembed', this.oembed.getTemplate());

                this.modal.load('oembed', 'Insert Embed Code', 700);
                this.modal.createCancelButton();

                var button = this.modal.createActionButton(this.lang.get('insert'));
                button.on('click', this.oembed.insert);

                this.selection.save();
                this.modal.show();
            },
            insert: function() {
                var data = $('#redactor-insert-oembed').val();
                data = $(data).attr('width', '560');
                data = $(data).attr('height', '315');

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
