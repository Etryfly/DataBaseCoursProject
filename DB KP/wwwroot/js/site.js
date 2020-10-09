// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

gameStatus =
{
    "userWin" : 1,
    "userLose" : 2,
    "ok" : 3,
    "error" : 3
}

function rankToCardName(rank) {
   
    if (rank < 11) return rank;
    switch (rank) {
        case 11: 
            return "J";
            
        case 12:
            return "Q";
            
        case 13:
            return "K";
            
        case 14:
            return "A";
    }
}

function wrapCards(cards) {
    
    var computerCardsHtml = "";
    for (let value of Object.entries(cards)) {
        
        computerCardsHtml += rankToCardName(value["1"]["rank"]) + " ";
    }
    return computerCardsHtml;
}



var bet = function ()
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
                        $(".game-interface").show();
                        $(".bet-interface").hide();
                        $("#user-chips").html(response.chips);   
                        // alert(JSON.stringify(response.cards.computer))
                        $(".user-score").html(response.score);
                        $(".computer-cards").html(wrapCards(response.cards.computer) );
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


function stand()
{
    var count = $("#betCount").val();

    // alert('@Url.Action("Bet", "BlackJack")');
    $.ajax(
        {
            type: 'POST',
            dataType: 'json',
            url: "/BlackJack/Stand",
            success:
                function (response)
                {
                    console.log(response);
                    if (response.operationStatus === true) {
                        $(".game-interface").show();
                        $(".bet-interface").hide();
                        $("#user-chips").html(response.chips);
                        // alert(JSON.stringify(response.cards.computer))
                        $(".user-score").html(response.score);
                        $(".computer-cards").html(wrapCards(response.cards.computer) );
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
