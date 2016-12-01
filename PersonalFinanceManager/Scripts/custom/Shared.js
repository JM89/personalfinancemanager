function buildAccountList(data) {

    var link = "";
    var ddData = [];

    $.each(data, function (index) {
        ddData.push({
            text: data[index].Name,
            value: data[index].Id,
            imageSrc: data[index].BankIconPath
        });
    })

    var indexAccountList = $("#indexAccountList").val();
    if (!indexAccountList) {
        indexAccountList = 0;
    }

    var selected = false;

    $('#availableAccounts').ddslick({
        data: ddData,
        width: 250,
        imagePosition: "left",
        defaultSelectedIndex: indexAccountList,
        onSelected: function (data) {
            console.log(data);
            var url = "/Home/SaveCurrentAccount?accountId=" + data.selectedData.value + "&indexAccountList=" + data.selectedIndex;
            $.get(url, null, function (data) {
                if (data.Data.reloadPage == true) {
                    location.reload();
                }
            });
        }
    });
}


function getAccountsForCurrentUser() {

    $.get("/BankAccount/GetAccounts")
        .done(function (result) {
            if (result.length > 0) {
                $("#userBankAccounts").show();
                $("#dashboard").show();
                $("#movements").show();
                $("#budget").show();
                buildAccountList(result);
            }
            else {
                $("#userBankAccounts").hide();
                $("#dashboard").hide();
                $("#movements").hide();
                $("#budget").hide();
            }
        })
        .fail(function (error) {
            console.error("Something is wrong");
        });
}