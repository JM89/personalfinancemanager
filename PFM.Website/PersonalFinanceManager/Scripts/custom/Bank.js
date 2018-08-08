function readURL(input) {

    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            preview("File", e.target.result);
        }

        reader.readAsDataURL(input.files[0]);
    }
}

function preview(prefixe, img)
{
    $("#" + prefixe + "Preview18").attr("src", img);
    $("#" + prefixe + "Preview50").attr("src", img);
    $("#" + prefixe + "PreviewDiv").css('display', 'block');
}

function checkIconPath()
{
    var displayIconFlags = $("#DisplayIconFlags").val();

    $("#IconPathDiv").hide();
    $("#IconPathPreviewDiv").hide();
    $("#UploadImageDiv").hide();
    $("#FilePreviewDiv").hide();

    if (strContains(displayIconFlags,"DisplayUploader")) {
        $("#UploadImageDiv").show();
    }
    if (strContains(displayIconFlags, "DisplayExistingIcon")) {
        $("#IconPathDiv").show();
    }
    if (strContains(displayIconFlags, "DisplayIconPathPreview")) {
        $("#IconPathPreviewDiv").show();
        preview("IconPath", $("#IconPath").val());
    }
    if (strContains(displayIconFlags, "DisplayFilePreview")) {
        $("#FilePreviewDiv").show();
    }
}

function showUploadImage()
{
    $("#UploadImageDiv").css('display', 'block');
}