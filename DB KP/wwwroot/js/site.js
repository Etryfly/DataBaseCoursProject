// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var send = function ()
{
    var count = $("#betCount").val();

    // alert('@Url.Action("Bet", "BlackJack")');
    $.ajax(
        {
            type: 'POST',
            dataType: 'json',
            url: "/BlackJack/Bet",
            data: { count:  count },
            success:
                function (response)
                {
                    if (response.operationStatus == true) {
                        $("#user-chips").html(response.chips);    
                    } else {
                        alert("smthng wrng with bet value");
                    }
                    
                },
            error: function(xhr, status, error) {
                alert(error.message)
            }
        });

};  


function afterBet() {
    
}