
$(document).ready(function () {
    $("#imgPreview").on("change", function () {
        var files = this.files;
        console.log(files);
        for (var i = 0; i < files.length; i++) {
            let file = files[i];
            var uploadImg = new FileReader();
            uploadImg.onload = function (displayImg) {
                let div = $("<div>");
                let img = $("<img>")
                    .attr("src", displayImg.target.result)
                    .addClass("img-fluid")
                    .css({ width: "100px", height: "90px" });
                $(div).append(img);
                $("#CreatingImgPrew").append(div);
            }
            uploadImg.readAsDataURL(file);
        }
    })
   
})