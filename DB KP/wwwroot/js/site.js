// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function wrapCards(cards) {
    alert(JSON.stringify(cards));
    var computerCardsHtml = "<p>";
    for (const card in cards.computer) {
        computerCardsHtml += card + " ";
    }
    computerCardsHtml += "</p>";
    return computerCardsHtml;
}

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
                    console.log(response);
                    if (response.operationStatus === true) {
                        alert(JSON.stringify(response.cards));
                        $("#user-chips").html(response.chips);   
                        $(".computer-score").html(response.score.computer);
                        $(".user-score").html(response.score.user);
                        $(".computer-cards").html(wrapCards(response.cards.computer));
                        $(".user-cards").html(wrapCards(response.cards.user));
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