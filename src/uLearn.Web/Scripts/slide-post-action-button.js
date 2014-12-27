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
    reloadReview: reloadReview
};


function reloadProposition(context, param) {
//    if (param.IsReadonly == true ) {
//        context.find("#readonly-view").attr("hidden", false);
//        context.find("#editing-view").attr("hidden", true);
    context.find("#readonly-proposition").html(param.RenderedText);
//    }
    if (param.IsReadonly == false) {
        context.find("#readonly-view").attr("hidden", true);
        context.find("#editing-view").attr("hidden", false);
    }
}

function reloadReview(context, param) {
    alert(1);
}