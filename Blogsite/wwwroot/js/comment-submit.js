$(() => {
    $(".form-control").on('input', function () {
        ensureFormValidity();
    });

    function ensureFormValidity() {
        const name = Boolean($("#name").val().trim());
        const content = Boolean($("#content").val().trim());
        const isFormValid = name && content;
        $(".btn-primary").prop('disabled', !isFormValid);
    }
})