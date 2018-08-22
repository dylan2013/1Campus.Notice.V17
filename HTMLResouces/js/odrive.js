if (!RedactorPlugins) var RedactorPlugins = {};

(function($) {
    RedactorPlugins.odrive = function() {
        return {
            init: function() {
                // var dropdown = {
                //     'point1': {
                //         title: 'Google Drive',
                //         func: this.odrive.google
                //     },
                //     'point2': {
                //         title: 'Dropbox',
                //         func: this.odrive.dropbox
                //     },
                //     'point3': {
                //         title: '1know',
                //         func: this.odrive.oneknow
                //     }
                // };

                // var button = this.button.add('drive', 'Insert File');
                // this.button.setAwesome('drive', 'fa-paperclip');
                // this.button.addDropdown(button, dropdown);

                var button = this.button.add('drive', 'Google Drive');
                this.button.addCallback(button, this.odrive.google);
            },
            google: function() {
                var _this = this;

                if (!_this.odrive.GOOGLE_OAUTH_TOKEN) {
                    gapi.auth.authorize({
                        'client_id': '706992035442.apps.googleusercontent.com',
                        'scope': ['https://www.googleapis.com/auth/drive'],
                        'immediate': false
                    }, function(token) {
                        if (token && !token.error) {
                            _this.odrive.GOOGLE_OAUTH_TOKEN = token.access_token;
                            _this.odrive.google();
                        }
                    });
                } else {
                    var picker = new google.picker.PickerBuilder()
                        .addView(google.picker.ViewId.DOCS)
                        .addView(new google.picker.DocsUploadView())
                        .enableFeature(google.picker.Feature.MULTISELECT_ENABLED)
                        .setLocale(window.localStorage.getItem('1know_lang') === 'zh-tw' ? 'zh-TW' : 'en')
                        .setOAuthToken(_this.odrive.GOOGLE_OAUTH_TOKEN)
                        .setDeveloperKey('AIzaSyD6T5tkhbonhN4HuthHUhe5PIkR33CoF-E')
                        .build();
                    picker.setVisible(true);

                    picker.setCallback(function(data) {
                        if (data.action == google.picker.Action.PICKED) {
                            var content = [];
                            
                            data.docs.forEach(function(item) {
                                if (item.type === 'photo') {
                                    content.push(['<div><img src="https://drive.google.com/uc?export=view&id=', item.id, '" style="max-width:100%" /></div>'].join(''));
                                } else {
                                    content.push(['<div><a href="', item.embedUrl, '" target="_blank"><img src="', item.iconUrl, '"/> <span>', item.name, '</span></a></div>'].join(''));
                                }

                                var request = gapi.client.drive.permissions.list({
                                    'fileId': item.id
                                });
                                request.execute(function(resp) {
                                    gapi.client.drive.permissions.insert({
                                        'fileId': item.id,
                                        resource: {
                                            role: 'reader',
                                            type: 'anyone'
                                        }
                                    }).execute(function(resp){});
                                });
                            });

                            _this.selection.restore();

                            var current = _this.selection.getBlock() || _this.selection.getCurrent();

                            if (current) $(current).after(content.join(''));
                            else {
                                _this.insert.html(content.join(''));
                            }

                            _this.code.sync();
                            _this.observe.images();
                        }
                    });
                }
            },
            dropbox: function() {
                var _this = this;

                var options = {
                    linkType: "preview",
                    multiselect: true,
                    success: function(files) {
                        var content = [];
                        files.forEach(function(item) {
                            content.push(['<div><a href="', item.link, '" target="_blank">', item.name, '</a></div>'].join(''));
                        });

                        _this.selection.restore();

                        var current = _this.selection.getBlock() || _this.selection.getCurrent();

                        if (current) $(current).after(content.join(''));
                        else {
                            _this.insert.html(content.join(''));
                        }

                        _this.code.sync();
                    },
                    cancel: function() {}
                };
                Dropbox.choose(options);
            },
            oneknow: function() {
                var _this = this;

                var options = {
                    host: window.location.origin,
                    success: function(items) {
                        var content = [];
                        items.forEach(function(item) {
                            content.push(['<div><a href="', item.url[1], '" target="_blank">', item.title, '</a></div>'].join(''));
                        });

                        _this.selection.restore();

                        var current = _this.selection.getBlock() || _this.selection.getCurrent();

                        if (current) $(current).after(content.join(''));
                        else {
                            _this.insert.html(content.join(''));
                        }

                        _this.code.sync();
                    },
                    cancel: function() {}
                };
                OneKnow.choose(options);
            }
        };
    };
})(jQuery);
