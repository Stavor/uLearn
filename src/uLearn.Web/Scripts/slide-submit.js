var $submitButton = $(".submit-peerAssasement-button");

$submitButton.click(function () {
    var submitData = $submitButton.parent().parent().find(".pa-submit-data")[0].codeMirrorEditor.getValue();
    $submitButton.text("...running...").addClass("active");
    $.ajax(
    {
        type: "POST",
        url: $submitButton.data("url"),
        data: submitData
    }).always(function () {
        $submitButton.text("Submit").removeClass("active");
    });
});