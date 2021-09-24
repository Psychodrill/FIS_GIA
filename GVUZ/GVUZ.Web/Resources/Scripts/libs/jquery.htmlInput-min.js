/*
HTML Input for jQuery
Copyright (c) 2008-2009 Anthony Johnston
http://www.antix.co.uk    
        
version 1.3.0

$Revision: 67 $

Now requires jquery.ui.core.js

Use and distibution http://www.opensource.org/licenses/bsd-license.php
*/

/// <reference path="http://ajax.googleapis.com/ajax/libs/jquery/1.4/jquery.js" />
/// <reference path="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.js" />
/// <reference path="http://jquery-clean.googlecode.com/svn/tags/1.2/jquery.htmlClean.js" />

(function($) {
    var frames = 0, // counter incremented as frames added for identification
        htmlInput = {
            _create: function() {
                $.extend(this.options, $.ui.htmlInput.defaults);
            },
            _init: function() {
                var self = this;

                // set up the toolbar and editor
                this.toolbar = new Toolbar("<div class='ui-widget-header'></div>", function(e, $command) {
                    switch ($command.attr("name")) {
                        case "link":
                            var link = findFirst(self.selectedElement, ["a"]);
                            if (link && !$.browser.msie) { self.setSelected(link); }
                            self.options.editLink(e, self, link, "insertLink");

                            break;
                        case "image":
                            self.options.editImage(e, self, findFirst(self.selectedElement, ["img"]), "insertImage");

                            break;
                        default:
                            var values = $command.attr("value")
                            ? $command.attr("value").split(".")
                            : "";
                            self._applyCommand($command.attr("name"), values[0]);
                            if (values.length == 2) {
                                $(self.getSelected()).addClass(values[1]);
                            }
                            break;
                    }
                });
                this.toolbar.addButtons(this.options);
                if ($.browser.safari) this.toolbar.element.css("height", "21px");

                // create the container, add the toolbar, and the container to the document
                this.container =
                $("<div class='ui-htmlInput ui-widget' style='position:relative;overflow:hidden;'></div>")
                .addClass(this.element.attr("class"))
                .attr("id", this.element.attr("id").concat("_container"))
                .append(this.toolbar.element);
                this.element.after(this.container);

                // work out toolbar and iframe heights
                var toolbarHeight = this.toolbar.element.outerHeight();
                if (!this.options.showToolbar) {
                    this.toolbar.element.css("display", "none");
                    toolbarHeight = 0;
                }
                var frameHeight = this.element.outerHeight() - toolbarHeight - 1;

                frames++;
                this._frameId = "htmlInput_".concat(frames);
                this._frame = $("<iframe frameborder='no' border='0' marginWidth='0' marginHeight='0' leftMargin='0' topMargin='0' rightMargin='0' bottomMargin='0' width='100%' height='".concat(frameHeight, "px' allowTransparency='true' scroll='yes'>"))
                .attr("id", this._frameId)
                .attr("src", $.browser.msie ? "javascript:false;" : "javascript:;");

                if (!this.options.debug) {
                    this.element.css("display", "none");
                }

                // add the iframe to the container
                this.container.append($("<div class='ui-widget-content'/>").append(this._frame));

                // set the content
                this._setContent(this.element.val());

                // size new elements
                this.container
                .height(this.element.outerHeight())
                .width(this.options.widthAuto ? "auto" : this.element.width());
                this._frame
                .css({
                    "width": "100%",
                    "height": frameHeight - 1 + "px"
                });
                frameHeight = this._frame.height();

                // bind events
                this._bindEvents();

                // moz and safari go blank if the frame is moved,
                // this detects the move, rebinds events and resets the content
                if ($.browser.mozilla || $.browser.safari) {
                    this._frame.parents().bind("DOMNodeInserted.htmlInput", function(e) {
                        if ($(e.target).find("#".concat(self._frameId)).length == 1) {
                            setTimeout(function() {
                                self._setContent(self.element.val());
                                self._bindEvents();
                            }, 0);
                            e.stopPropagation();
                        }
                    });
                }

                // Selection functions
                if (window.getSelection) {
                    this.bookmark = function() {
                        self._bookmark = self._window.getSelection();
                    }
                    this.restoreBookmark = function() {
                        if (self._bookmark) {
                            var selection = self._window.getSelection();
                            selection.removeAllRanges();
                            selection.addRange(self._bookmark);
                            self._bookmark = null;
                        }
                    }
                    this.getSelected = function() {
                        var selection = self._window.getSelection();
                        if (selection.rangeCount > 0) {
                            var range = selection.getRangeAt(0);
                            var element = range.collapsed || range.startContainer.childNodes.length == 0
                            ? selection.focusNode
                            : range.startContainer.childNodes[range.startOffset];
                            return element.tagName == undefined ? element.parentNode : element;
                        }
                    }
                    this.getSelectedHTML = function() {
                        var selection = self._window.getSelection();
                        if (selection.rangeCount > 0) {
                            var range = selection.getRangeAt(0);
                            var el = document.createElement("div");
                            el.appendChild(range.cloneContents());
                            return el.innerHTML;
                        }
                    }
                    this.setSelected = function(item) {
                        var selection = self._window.getSelection();
                        selection.removeAllRanges();
                        if (item) {
                            var range = self._document.createRange();
                            range.selectNodeContents(item);
                            selection.addRange(range);
                        }
                    }
                    this.insertHtml = function(html) {
                        self._document.execCommand("insertHTML", null, html);
                        self._update();
                    }
                } else { // ie
                    this.bookmark = function() {
                        try {
                            self._bookmark = self._document.selection.createRange();
                        } catch (ex) { }
                    }
                    this.getSelected = function() {
                        self._window.focus();
                        var range = self._document.selection.createRange();
                        return range.item
                        ? range.item(0)
                        : range.parentElement();
                    }
                    this.getSelectedHTML = function() {
                        self._window.focus();
                        var range = self._document.selection.createRange();
                        return range.htmlText ? range.htmlText : range.item ? range.item(0).outerHTML : "";
                    }
                    this.setSelected = function(item) {
                        try {
                            self._window.focus();
                            var range = self._document.selection.createRange();
                            range = self._document.body.createTextRange();
                            range.moveToElementText(item);
                            range.select();
                        } catch (e) { }
                    }
                    this.insertHtml = function(html) {
                        var range = this._document.selection.createRange();
                        if (range.item) {
                            range.item(0).outerHTML = html;
                        } else {
                            range.pasteHTML(html);
                        }
                        self._update();
                    }
                    this.restoreBookmark = function() {
                        if (self._bookmark) {
                            self._bookmark.select();
                            self._bookmark = null;
                        }
                    }
                }
            },

            value: function(html) {
                /// <summary>Get/Set the html</summary>
                if (arguments.length) {
                    html = this._htmlClean(html);
                    if (html != this.element.val()) {
                        this.element.val(html);
                        this._document.body.innerHTML = html;
                        this._triggerChange();
                    }
                    return this;
                }

                return this._htmlClean(this._document.body.innerHTML);
            },

            clean: function() {
                /// <summary>Cleans the html</summary>

                html = this._htmlClean(this.value());
                if (html != this.element.val()) {
                    this.element.val(html);
                    this._document.body.innerHTML = html;
                    this._triggerChange();
                }
            },

            insertLink: function(hRef, target, type) {
                if (hRef == "http:/".concat("/") || hRef == "") { hRef = null; }
                // set focus so that execCommand works
                this._window.focus();
                if(this.selectedHTML == null) this.selectedHTML = ""
                // check for a current link
                var link = findFirst(this.selectedElement, ["a"]);
                if (hRef == null) {
                    if (link != null) {
                        // remove link
                        this._document.execCommand("unLink", false, null);
                    }
                    return;
                } else if (link == null) {
                    // add a new link
                    var spaceCount = 0;
                    var content = this.selectedHTML;
                    for(var i = content.toString().length - 1; i >= 0; i--)
                    {
                    	if(/\s/.test(content.charAt(i).toString())) spaceCount++
                    	else break
                    }
	                var after = "",
                    afterIndex = content.length -spaceCount;
                    if (afterIndex > -1) {
                        after = content.substring(afterIndex);
                        content = content.substring(0, afterIndex);
                    }
                    this.insertHtml("<a href='".concat(hRef, "'>", content, "</a>", after));
                    link = findFirst(this.getSelected(), ["a"]);
                }

                //if(target == undefined) target = "_blank";
                // set properties
                link = $(link);

                link.attr("href", hRef);
                target != null && target.length > 0
                ? link.attr("target", target)
                : link.removeAttr("target");
                type != null && type.length > 0
                ? link.attr("type", type)
                : link.removeAttr("type");

                this._update();
            },

            insertImage: function(src) {
                // set focus so that execCommand works
                this._window.focus();

                // check for a current image
                var image = findFirst(this.selectedElement, ["img"]);
                if (image == null && src != null) {
                    // add a new image
                    this._document.execCommand("insertimage", false, src);
                    this.selectedElement = this.getSelected();
                    image = findFirst(this.selectedElement, ["img"]);
                } else if (image != null && src == null) {
                    // do nothing
                    return;
                }

                // set properties
                image = $(image);
                image.attr("src", src);
                image.attr("alt", image.attr("alt") || "");

                this._update();
            },

            focus: function() {
                /// <summary>Pass focus to the editor window</summary>
                this._window.focus();
            },

            destroy: function() {
                /// <summary>Destroy this control</summary>
                $.Widget.prototype.destroy.apply(this, arguments);
                $(this._document)
                .add(this._document.body)
                .add(this._frame)
                .unbind(".htmlInput");
            },

            _update: function() {
                /// <summary>Update the selection, show the status on the toolbar</summary>
                if (this.getSelected().tagName == "BODY"
                    || ($.browser.safari && this.getSelected().tagName == "DIV")) {
                    this._document.execCommand("formatblock", false, "<p>");
                }

                // get the currently selected element
                this.selectedElement = this.getSelected();
                this.selectedHTML = this.getSelectedHTML();
                //check for parargraph selection in text
                if(new RegExp('<P>').test(this.selectedHTML)) this.selectedHTML = ""
                if (this.options.debug) this._showStatus("_update");

                // show button statuses
                for (var name in this.toolbar.tools) {
                    var $tool = this.toolbar.tools[name];
                    var selected = false;
                    var enabled = true;
                    switch (name) {
                        case Tools.bulletList.command:
                        case Tools.numberList.command:
                            var listItem = findFirstTag(this.selectedElement, ["ul", "ol"]);
                            selected = (name == Tools.bulletList.command && listItem == "ul")
                                || (name == Tools.numberList.command && listItem == "ol")
                            break;
                        case Tools.bold.command:
                            selected = findFirst(this.selectedElement, ["strong", "b", ["span", { style: /weight:\s*bold/i}]]);
                            break;
                        case Tools.italic.command:
                            selected = findFirst(this.selectedElement, ["em", "i", ["span", { style: /style:\s*italic/i}]]);
                            break;
                        case Tools.superscript.command:
                            selected = findFirst(this.selectedElement, ["sup", ["span", { style: /-align:\s*super/i}]]);
                            break;
                        case Tools.subscript.command:
                            selected = findFirst(this.selectedElement, ["sub", ["span", { style: /-align:\s*sub/i}]]);
                            break;
                        case Tools.block.command:
                            $tool.command.val(findFirstTag(this.selectedElement, $tool.data("tags")));
                            continue;
                        case Tools.link.command:
                            selected = findFirst(this.selectedElement, ["a"]);
                            enabled = selected
                            || (this.selectedHTML && this.selectedHTML.length > 0)
                            || findFirst(this.selectedElement, ["img"]);
                            break;
                        case Tools.image.command:
                            selected = findFirst(this.selectedElement, ["img"]);
                            break;
                        case Tools.leftAlign.command:
                            selected = $(findFirst(this.selectedElement, this.options.canAlign)).hasClass("left");
                            break;
                        case Tools.middleAlign.command:
                            selected = $(findFirst(this.selectedElement, this.options.canAlign)).hasClass("middle");
                            break;
                        case Tools.rightAlign.command:
                            selected = $(findFirst(this.selectedElement, this.options.canAlign)).hasClass("right");
                            break;
                    }

                    selected ? $tool.addClass("ui-state-active") : $tool.removeClass("ui-state-active");
                    enabled ? $tool.removeClass("ui-state-disabled") : $tool.addClass("ui-state-disabled");
                }
            },

            _updateElement: function() {
                /// <summary>Update the original element with the value in the editor</summary>
                var html = this._htmlClean(this._document.body.innerHTML);
                if (html != this.element.val()) {
                    this.element.val(html);
                    this._triggerChange();
                }
            },

            _showStatus: function(t) {
                /// <summary>Shows the current selection on the status bar, used for debugging</summary>
                window.status = t.concat(": selected html: '",
                this.selectedHTML, "' element: ",
                (this.selectedElement ? this.selectedElement.tagName == undefined ? this.selectedElement.toString() : this.selectedElement.tagName : ""));
            },

            _htmlClean: function(html, replace) {
                /// <summary>Clean the passed html, checks for htmlClean plug-in, if not present nothing is done</summary>
                return this.options.clean && $.htmlClean
                ? $.htmlClean(html, {
                    allowedClasses: this.options.allowedClasses,
                    replace: replace,
                    format: this.options.format,
                    formatIndent: this.options.formatIndent
                })
                : html;
            },

            _triggerChange: function(e) {
                /// <summary>raise change event on the element</summary>
                this.element.trigger("change", e, {
                    value: this.value()
                });
                if (this.options.debug) this._showStatus("changed");
            },

            _bindEvents: function() {
                /// <summary>Bind events to the element's form, iframe, document and document body</summary>
                var self = this;
                $(this._document)
                .unbind(".htmlInput")
                .bind("keydown.htmlInput keypress.htmlInput keyup.htmlInput mouseup.htmlInput focus.htmlInput blur.htmlInput", function(e) { self._bubbleEvent(e) });

                if (this.element[0].form) {
                    // clean on submit
                    $(this.element[0].form)
                    .unbind(".htmlInput")
                    .bind("submit.htmlInput", function() { self.clean() });
                }

                if ($.browser.msie) {
                    this._frame
                    .unbind(".htmlInput")
                    .bind("focus.htmlInput blur.htmlInput", function(e) { self._bubbleEvent(e) });
                    this._document.body.onpaste = function() { setTimeout(function() { self.clean() }, 0) };
                    this._document.onbeforedeactivate = function() { self.bookmark() };
                    // if a book mark has been saved restore its selection
                    this._document.onactivate = function() { self.restoreBookmark() };
                }

                if ($.browser.safari) {
                    $.each("keydown keypress keyup mouseup focus blur".split(" "), function(type) {
                        self._window.removeEventListener(this);
                        self._window.addEventListener(this, function(e) { self._bubbleEvent(e) }, false);
                    });
                }

                if ($.browser.mozilla) {
                    try {
                        // catch paste(ish) event for moz
                        $(this._document.body)
                        .unbind(".htmlInput")
                        .bind("input.htmlInput", function() { self.clean() });
                    } catch (ex) { }
                }
            },

            _bubbleEvent: function(e) {
                /// <summary>Bubble events to the original element</summary>
                if (e) {
                    switch (e.type) {
                        case "blur":
                            this._updateElement();
                            break;
                        case "mouseup":
                        case "keyup":
                            this._update();
                            break;
                    }
                    this.element.triggerHandler(e.type, e);
                }
            },

            _setContent: function(html) {
                /// <summary>Set the content of the editor with the html passed</summary>
                this._window = this._frame.attr("contentWindow")
                ? this._frame.attr("contentWindow")
                : window.frames[this._frameId].window;
                this._document = this._window.document;

                html = "<html><head>".concat(
                    "<link href='", this.options.styleUrl, "' rel='stylesheet' type='text/css' />",
                    this.options.baseUrl ? "<base href='".concat(this.options.baseUrl, "' />") : "",
                    "<style type='text/css'>body{overflow:auto;}</style></head><body class='editor'>", this._htmlClean(html, [
                            [["strong", "big", /span.*?weight:\s*bold/i], "b"],
                            [["em", /span.*?style:\s*italic/i], "i"],
                            [[/span.*?-align:\s*super/i], "sup"],
                            [[/span.*?-align:\s*sub/i], "sub"]
                        ]
                    ), "</body></html>");
                try {
                    this._document.designMode = "on";
                    this._document.open();
                    this._document.write(html);
                    this._document.close();
                } catch (e) { }

                try {
                    // stop moz using inline styles, can't do anything about webkit at the mo
                    this._document.execCommand("useCSS", false, true); // off (true=off!)            
                    this._document.execCommand("styleWithCSS", false, false); // new implementation of the same thing
                } catch (ex) { }
            },

            _applyCommand: function(command, value) {
                /// <summary>Apply the command and value, largely using execCommand</summary>
                this.focus();
                switch (command) {
                    default:
                        this._document.execCommand(command, false, null);

                        break;
                    case Tools.block.command:
                        this._document.execCommand(command, false, "<" + value + ">");

                        break;
                    case Tools.leftAlign.command:
                        $(findFirst(this.selectedElement, this.options.canAlign))
                        .removeClass(Tools.middleAlign.command)
                        .removeClass(Tools.rightAlign.command)
                        .toggleClass(command);

                        break;
                    case Tools.middleAlign.command:
                        $(findFirst(this.selectedElement, this.options.canAlign))
                        .removeClass(Tools.leftAlign.command)
                        .removeClass(Tools.rightAlign.command)
                        .toggleClass(command);

                        break;
                    case Tools.rightAlign.command:
                        $(findFirst(this.selectedElement, this.options.canAlign))
                        .removeClass(Tools.leftAlign.command)
                        .removeClass(Tools.middleAlign.command)
                        .toggleClass(command);

                        break;
                    case Tools.bulletList.command:
                    case Tools.numberList.command:
                        this._document.execCommand(command, false, null);

                        break;
                }
            }
        };

    /* Helpers */
    function findFirstTag(element, tags) {
        /// <summary>Find the first matching tag up the heirachy starting from, and including, the element passed</summary>
        /// <param name="element">Element to start search from</param>
        /// <param name="tags">A string array of tags to look for</param>
        /// <returns>Element tagName found, or null if not found</returns>

        while (element != null) {
            var tagIndex = findFirstMatch(element, tags);
            if (tagIndex > -1) return tags[tagIndex];
            element = element.parentNode;
        }

        return "";
    }

    function findFirst(element, tags) {
        /// <summary>Find the first matching element up the heirachy starting from, and including, the element passed</summary>
        /// <param name="element">Element to start search from</param>
        /// <param name="tags">A string array of tags to look for</param>
        /// <returns>Element found, or null if not found</returns>

        while (element != null
                && (findFirstMatch(element, tags) == -1)) {
            element = element.parentNode;
        }

        return element;
    }
    function findFirstMatch(element, tags) {
        if (element.tagName) {
            var tag = element.tagName.toLowerCase();
            for (var i = 0; i < tags.length; i++) {
                if (tags[i].constructor == Array) {
                    if (tags[i][0] == tag) {
                        var found = true;
                        for (var name in tags[i][1]) {
                            if (!tags[i][1][name].test(element.getAttribute(name))) {
                                found = false;
                                continue;
                            }
                        }

                        if (found) return i;
                    }
                } else {
                    var tagName = tags[i].split(".");
                    if (tag == tagName[0]
                        && (tagName.length == 1 || element.getAttribute("class") == tagName[1])) return i;
                }
            }
        }
        return -1;
    }
    function findHRef(element) {
        element = findFirst(element, ["a"]);
        return element == null ? "" : element.href;
    }

    var popup;

    function editPopup(e, control, element, urlProperty, showRemove, callBack) {
        /// <summary>Shows a popup in the toolbar requesting a Url for an image or link</summary>
        if (!popup) {
            popup = $("<div id='Popup' style='position:relative;zoom:1;overflow:auto'><label style='float:left;width:35px;padding:3px 5px 0 0;text-align:right'>".concat(control.options.textUrl, "</label></div>"));
            popup.input = $("<input style='float:left;height:100%' />");
            popup.ok = $("<span class='ui-tool ui-state-default'><a style='width:55px'>".concat(control.options.textOk, "</a></span>"));
            popup.cancel = $("<span class='ui-tool ui-state-default'><a style='width:55px'></a></span>");

            popup
                .append(popup.input)
                .append(popup.ok)
                .append(popup.cancel)
                .bind("click.htmlInput", function(e) { e.stopPropagation(); });
        }

        var toolbarWidth = control.toolbar.element.width(),
            toolbarHeight = control.toolbar.element.outerHeight();
        control.toolbar.element.append(popup);
        popup.input.width(toolbarWidth > 500 ? 335 : toolbarWidth - 175);
        popup.width(toolbarWidth - 2);
        popup.cancel.contents("a").text(control.options.textCancel);
        if (element) {
            if (showRemove) popup.cancel.contents("a").text(control.options.textRemove);
            popup.input.val(element[urlProperty]);
        } else {
            popup.input.val("");
        }

        control.toolbar.element.contents("span")
            .animate({ marginTop: -toolbarHeight }, {
                duration: 500,
                complete: function() {
                    popup.input.focus();
                }
            });

        popup.buttons = popup.ok.add(popup.cancel);
        popup.buttons
                .bind("mouseover.htmlInput", function() { $(this).addClass("ui-state-hover"); })
                .bind("mouseout.htmlInput", function() { $(this).removeClass("ui-state-hover"); })
                .bind("click.htmlInput", function(e) {
                    control[callBack]($(this).contents("a").text() == "ok" ? popup.input.val() : null);
                    control.toolbar.element.contents("span").animate({ marginTop: 0 }, {
                        duration: 250,
                        complete: function() {
                            $(document)
                                .add(control.document)
                                .add(popup.buttons)
                                .add(popup.input)
                                .add(popup)
                                .unbind(".htmlInput");
                            popup.remove();
                        }
                    });
                    e.preventDefault();
                });
        $(document)
            .add(control.document)
            .bind("click.htmlInput", function() { popup.cancel.click() });
        popup.input.keydown(function(e) {
            switch (e.which) {
                case $.ui.keyCode.ENTER: popup.ok.click(); break;
                case $.ui.keyCode.ESCAPE: popup.cancel.click(); break;
            }
        });
    }

    function editLink(e, control, anchor, callBack) {
        editPopup(e, control, anchor, "href", true, callBack);
    }

    function editImage(e, control, image, callBack) {
        editPopup(e, control, image, "src", false, callBack);
    }

    // objects
    function Toolbar(element, onCommand) {
        this.element = $(element);
        this.tools = {};

        this.element
            .bind("click.htmlInput", function(e) { e.stopPropagation(); })
            .addClass("ui-toolbar");

        this.add = function(name, command, tags) {
            var $tool = $("<span class='ui-tool ".concat(name, "'></span>"));

            this.element.append($tool);
            if (command) {
                $tool.append(command);

                command.attr("name", name);
                $tool.command = command;
            }
            if (tags) $tool.data("tags", tags);

            this.tools[name] = $tool;
            return $tool;
        }

        this.addButton = function(tool) {
            var $command = $("<a title='".concat(tool.tooltip, "' tabindex='1000'>", tool.content, "</a>"));

            var $tool = this.add(tool.command, $command)
                .addClass("ui-state-default");

            $command.bind("click.htmlInput", function(e) { $tool.hasClass("ui-state-disabled") || onCommand(e, $command) });
            $tool.mouseover(function() { $(this).addClass("ui-state-hover"); });
            $tool.mouseout(function() { $(this).removeClass("ui-state-hover"); });
        }

        this.addList = function(tool) {
            var $command = $("<select title='".concat(tool.tooltip, "' tabindex='1000' style='height:100%;width:auto'></select>"));

            var $tool = this.add(tool.command, $command, [])
                .addClass("ui-state-default");

            $command.bind("change.htmlInput", function(e) { $tool.hasClass("ui-state-disabled") || onCommand(e, $command) });
            for (var value in tool.content) {
                var item = "<option value='".concat(value, "'>", tool.content[value], "</option>");
                $command.append(item);
                $tool.data("tags").push(value);
            }
        }

        this.addSeparator = function(tool) {
            this.add(tool.command, null);
        }

        this.addButtons = function(options) {
            for (var name in options.tools) {
                var tool = options.tools[name];
                if (tool.add) { this["add" + tool.type](tool); }
            }
        }
    }

    // Tool object
    function Tool(type, command, content, tooltip) {
        this.type = type;
        this.command = command;
        this.content = content;
        this.add = true;
        this.tooltip = tooltip;
    }

    // types of tools
    var ToolTypes = {
        Button: "Button",
        List: "List",
        Separator: "Separator"
    };

    // Tools
    var Tools = {
        bold: new Tool(ToolTypes.Button, "bold", "<strong>B</strong>", "жирный"),
        italic: new Tool(ToolTypes.Button, "italic", "<em>I</em>", "курсив"),
        superscript: new Tool(ToolTypes.Button, "superscript", "X<sup>2</sup>", "надстрочный"),
        subscript: new Tool(ToolTypes.Button, "subscript", "X<sub>2</sub>", "подстрочный"),
        subSeparator: new Tool(ToolTypes.Separator),
        removeFormat: new Tool(ToolTypes.Button, "removeformat", "~", "удалить форматирование"),
        removeFormatSeparator: new Tool(ToolTypes.Separator),
        block: new Tool(ToolTypes.List, "formatBlock", {
            "p": "Обычный",
            "h1": "Заголовок 1",
            "h2": "Заголовок 2",
            "h3": "Заголовок 3",
            "h4": "Заголовок 4",
            "h5": "Заголовок 5",
            "h6": "Заголовок 6",
            "pre": "Preformatted"
        }, "Отформатировать как..."),
        blockSeparator: new Tool(ToolTypes.Separator),
        leftAlign: new Tool(ToolTypes.Button, "left", "&lArr;", "влево"),
        middleAlign: new Tool(ToolTypes.Button, "middle", "&hArr;", "посередине"),
        rightAlign: new Tool(ToolTypes.Button, "right", "&rArr;", "вправо"),
        rightAlignSeparator: new Tool(ToolTypes.Separator),
        bulletList: new Tool(ToolTypes.Button, "insertUnorderedList", "&bull;&equiv;", "выделение как список"),
        numberList: new Tool(ToolTypes.Button, "insertOrderedList", "1&equiv;", "выделение как нумерованный список"),
        increaseIndent: new Tool(ToolTypes.Button, "indent", "&gt;&equiv;", "увеличить отступ"),
        decreaseIndent: new Tool(ToolTypes.Button, "outdent", "&lt;&equiv;", "уменьшить отступ"),
        decreaseIndentSeparator: new Tool(ToolTypes.Separator),
        link: new Tool(ToolTypes.Button, "link", "<strong>&infin;</strong>", "добавить/редактировать ссылку"),
        image: new Tool(ToolTypes.Button, "image", "&#10065;", "добавить/редактировать изображение")
    };

    $.widget("ui.htmlInput", htmlInput);

    $.extend($.ui.htmlInput, {
        getter: "value",
        version: "1.3.0",
        eventPrefix: "htmlInput",
        defaults: {
            debug: false,
            clean: true,
            format: true,
            formatIndent: 0,
            styleUrl: "/content/editor.css",
            editLink: editLink,
            editImage: editImage,
            showToolbar: true,
            tools: Tools,
            widthAuto: false,
            baseUrl: false,
            allowedClasses: ["left", "middle", "right"],
            canAlign: ["img", "p", "h1", "h2", "h3", "h4", "h5", "h6", "th", "td", "li", "dt"],
            textCancel: "отмена",
            textOk: "ok",
            textRemove: "удалить",
            textUrl: "url"
        }
    });

})(jQuery);