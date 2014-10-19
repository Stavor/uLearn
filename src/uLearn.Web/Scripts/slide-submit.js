var $submitButton = $(".submit-peerAssasement-button");

$submitButton.click(function () {
    var proposition = $(".markdown-input")[0].codeMirrorEditor.getValue();
    $submitButton.text("...running...").addClass("active");
    $.ajax(
    {
        type: "POST",
        url: $submitButton.data("url"),
        data: proposition
    }).always(function () {
        $submitButton.text("Submit").removeClass("active");
    });
});

//JSON.stringify(new Proposition(proposition))