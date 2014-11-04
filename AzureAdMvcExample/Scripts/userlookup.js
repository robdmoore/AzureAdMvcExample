(function ($) {
    $.fn.userlookup = function(options) {
        var settings = $.extend({
            lookupButtonShow: true,
            lookupButtonHtml: "<span class='input-group-addon'><i class='glyphicon glyphicon-expand' style='cursor: pointer;'></i></span>",
            lookupKeypress: function(e) {
                return (e.ctrlKey || e.metaKey) && String.fromCharCode(e.which).toLowerCase() === "k";
            },
            classes: {
                matchFound: "matchfound",
                noMatchFound: "nomatchfound",
                loading: "loading"
            },
            idHiddenFieldPopulate: true,
            idHiddenFieldIdFoundInDataAttribute: "user-lookup",
            apiUrl: "/api/userlookup",
            apiNameProperty: "DisplayName",
            apiIdProperty: "ObjectId",
            typeaheadOptions: {
                hint: true,
                highlight: true,
                minLength: 0
            }
        }, options);

        var userNameMatcher = function (strs) {
            return function findMatches(q, cb) {
                var matches = [];
                var substrRegex = new RegExp(q, "i");
                $.each(strs, function (i, user) {
                    if (substrRegex.test(user[settings.apiNameProperty])) {
                        matches.push(user);
                    }
                });
                cb(matches);
            };
        };

        var lookupUsers = function (partialName) {
            return $.get(settings.apiUrl, {q: partialName});
        };

        this.each(function() {
            var $input = $(this);
            var $field;
            if (settings.idHiddenFieldPopulate) {
                $field = $("#" + $input.data(settings.idHiddenFieldIdFoundInDataAttribute));
                if ($field.val()) {
                    $input.addClass(settings.classes.matchFound);
                }
            }

            var setUser = function(user) {
                $input.addClass(settings.classes.matchFound);
                $input.val(user[settings.apiNameProperty]);
                if (settings.idHiddenFieldPopulate) {
                    $field.val(user[settings.apiIdProperty]);
                }
            };

            var clear = function() {
                $input.removeClass(settings.classes.matchFound);
                $input.removeClass(settings.classes.noMatchFound);
                if (settings.idHiddenFieldPopulate) {
                    $field.val("");
                }
            }

            var multiSelect = function(users) {
                $input.typeahead("destroy");
                $input.typeahead(
                    settings.typeaheadOptions,
                    {
                        name: "users",
                        displayKey: settings.apiNameProperty,
                        source: userNameMatcher(users)
                    }
                );
                var val = $input.typeahead("val");
                $input.typeahead("val", "")
                    .focus()
                    .typeahead("val", val)
                    .focus();
                $input.on("typeahead:selected", function(e, selectedUser) {
                    setUser(selectedUser);
                    $input.typeahead("destroy");
                });
            };

            var search = function() {
                if ($input.val() === "")
                    return;

                clear();
                $input.addClass(settings.classes.loading);
                lookupUsers($input.val())
                    .done(function(users) {
                        $input.removeClass(settings.classes.loading);
                        if (users.length === 0) {
                            $input.addClass(settings.classes.noMatchFound);
                        } else if (users.length === 1) {
                            setUser(users[0]);
                            $input.focus();
                        } else {
                            multiSelect(users);
                        }
                    })
                    .fail(function() {
                        $input.removeClass(settings.classes.loading);
                        $input.addClass(settings.classes.noMatchFound);
                    });
            };

            if (settings.lookupButtonShow) {
                var $button = $(settings.lookupButtonHtml);
                $input.after($button);
                $button.click(search);
            }

            $input.keydown(function(e) {
                if (e.which === 13 || e.which === 9 || e.which >= 37 && e.which <= 40) {
                    return true;
                } else if (settings.lookupKeypress(e)) {
                    search();
                    e.defaultPrevented = true;
                    return false;
                } else if ($input.hasClass(settings.classes.matchFound)) {
                    clear();
                    if (e.which === 8 || e.which === 46) {
                        $input.val("");
                    }
                } else if ($input.hasClass(settings.classes.noMatchFound)) {
                    $input.removeClass(settings.classes.noMatchFound);
                }
                return true;
            });
        });
    };
})(jQuery);