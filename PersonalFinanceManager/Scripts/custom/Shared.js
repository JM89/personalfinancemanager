function initDeletionConfirmation() {
    $('.btn_delete').click(function () {
        var url = $(this).data('url');
        $(".modal-body #ItemUrl").val(url);

        var description = $(this).data('description');
        $(".modal-body #ItemDescription").text(description);
    });
    $('.btn_delete_confirm').click(function () {
        var url = $(".modal-body #ItemUrl").val();
        var form = $('#__AjaxAntiForgeryForm');
        var token = $('input[name="__RequestVerificationToken"]', form).val();
        console.log(token);

        $.ajax({
            url: url,
            type: 'POST',
            data: {
                __RequestVerificationToken: token
            },
            success: function (data) {
                window.location = data;
            }
        });
    });
}