CodeMirror.commands.autocomplete = function (cm) {
	cm.showHint({ hint: CodeMirror.hint.csharp });
}

function createSharpConfiguration(editable) {
    var extraKeys = {
        "Ctrl-Space": "autocomplete",
        ".": function(cm) {
            setTimeout(function() { cm.execCommand("autocomplete"); }, 100);
            return CodeMirror.Pass;
        }
    };
    return createCodeMirrorConfiguration("text/x-csharp", "cobalt", editable, extraKeys);
}

function createMarkdownConfiguration() {
    var extraKeys = {
        "Ctrl-S": function (cm) {
            return;
        }
    };
    return createCodeMirrorConfiguration("markdown", "default", true, extraKeys);
}

function createCodeMirrorConfiguration(mode, editorTheme, editable, extraKeys) {
    return {
        mode: mode,
        lineNumbers: true,
        theme: editable ? editorTheme : "default",
        indentWithTabs: true,
        tabSize: 4,
        indentUnit: 4,
        extraKeys: extraKeys,
        readOnly: !editable,
        //autoCloseBrackets: true, // bug: autoCloseBracket breakes indentation after for|while|...
        styleActiveLine: editable,
        matchBrackets: true,
    };
}

function codeMirrorClass(c, config) {
	var codes = document.getElementsByClassName(c);
	for (var i = 0; i < codes.length; i++) {
	    var element = codes[i];
		var editor = CodeMirror.fromTextArea(element, config);
	    element.codeMirrorEditor = editor;
	    if (!config.readOnly)
	        editor.focus();
	}
}

codeMirrorClass("code-exercise", createSharpConfiguration(true));
codeMirrorClass("code-sample", createSharpConfiguration(false));
codeMirrorClass("markdown-input", createMarkdownConfiguration());

function refreshPreviousDraft(ac, id) {
    window.onbeforeunload = function () {
        if (ac == 'False')
            localStorage[id] = $('.code-exercise')[0].codeMirrorEditor.getValue();
    }
    if (localStorage[id] != undefined && ac == 'False') {
        $('.code-exercise')[0].codeMirrorEditor.setValue(localStorage[id]);
    }
}