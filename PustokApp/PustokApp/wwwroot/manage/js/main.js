$(document).ready(function () {
    $("#Photo").change(function (ev) {
        let file=ev.target.files[0]
        var uploadimg = new FileReader();
        uploadimg.onload = function (displayimg) {
            $("#imgPreview").attr('src', displayimg.target.result);
            $("#imgPreview").show();
        }
        uploadimg.readAsDataURL(file);
    })
})