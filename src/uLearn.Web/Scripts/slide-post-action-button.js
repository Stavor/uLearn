var $postActionButton = $(".postAction-peerAssasement-button");


$postActionButton.click(function () {
    var form = $postActionButton.parent();
    if (form.is("form")) {
        codeMirrorEditorAutoSave(form);
        var formData = form.serializeObject();
        var submitData = JSON.stringify(formData);
        var text = $postActionButton.text();
        $postActionButton.text("...running...").addClass("active");
        $.ajax(
            {
                type: "POST",
                url: $postActionButton.data("url"),
                data: submitData
            })
            .success(function (ans) {
                var el = $("#" + $postActionButton.data("update-id"));
                var funcName = $postActionButton.data("on-success");
                $updateFuncs[funcName](el, ans);
            })
            .fail(function (req) {
                setSimpleResult($serviceError, req.status + " " + req.statusText);
                console.log(req.responseText);
            })
            .always(function() {
                $postActionButton.text(text).removeClass("active");
            });
    }
});

function codeMirrorEditorAutoSave(context) {
    var textareas = context.find("textarea");
    $.each(textareas, function() {
        if (this.codeMirrorEditor) {
            this.codeMirrorEditor.save();
        }
    });
}

var $updateFuncs = {
    reloadProposition: reloadProposition
};


function reloadProposition(context, ans) {
    if (ans.IsReadonly == true) {
        context.find("#readonly-view").attr("hidden", false);
        context.find("#editing-view").attr("hidden", true);
        context.find("#readonly-proposition").html(ans.Text);
    }
    if (ans.isReadonly == false) {
        context.find("#readonly-view").attr("hidden", true);
        context.find("#editing-view").attr("hidden", false);
    }
}


$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name] !== undefined) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};