var $postActionButton = $(".postAction-peerAssasement-button");


$postActionButton.click(function (e) {
    var postActionButton = $(e.target);
    var form = postActionButton.parent();
    if (form.is("form")) {
        codeMirrorEditorAutoSave(form);
        var formData = form.serializeArray();
        formData.push({ name: "X-Requested-With", value: "XMLHttpRequest" });
        formData.push({ name: "X-HTTP-Method-Override", value: "POST" });
        var text = postActionButton.text();
        postActionButton.text("...running...").addClass("active");
        $.ajax(
            {
                type: "POST",
                url: postActionButton.data("url"),
                data: formData
            })
            .success(function (ans) {
                var actionName = postActionButton.data("on-success");
                var el = $("#" + postActionButton.data("update-id"));
                $updateFuncs[actionName](el, ans);
            })
            .fail(function (req) {
                setSimpleResult($serviceError, req.status + " " + req.statusText);
                console.log(req.responseText);
            })
            .always(function() {
                postActionButton.text(text).removeClass("active");
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
    reloadProposition: reloadProposition,
    reloadReviewBeforeSave: reloadReviewBeforeSave,
    reloadReviewBeforeSubmit: reloadReviewBeforeSubmit
};

function reloadProposition(context, param) {
    context.find("#readonly-proposition").html(param.RenderedText);
}

function reloadReviewBeforeSave(context, param) {
    context.find("#other-proposition").html(param.TextForReview);
    var textarea = context.find("#review-text")[0];
    var editor = textarea.codeMirrorEditor;
    editor.getDoc().setValue(param.Text == null ? "" : param.Text);
    context.find("#reviewCnt").html(param.ReviewCnt);
    $("editing-view", context).attr("hidden", param.IsReadonly);
}

function reloadReviewBeforeSubmit(context, param) {
    context.find("#other-proposition").html(param.TextForReview);
    var textarea = context.find("#review-text")[0];
    var editor = textarea.codeMirrorEditor;
    editor.getDoc().setValue(param.Text == null ? "" : param.Text);
    var marks = $(".mark .value", context);
    $.each(marks, function() {
        var cur = $(this);
        cur.val($($("option", cur).get(0)).attr("value"));
    });
    context.find("#reviewCnt").html(param.ReviewCnt);
    if (param.ReviewCnt == 0)
        context.attr("hidden", true);
}