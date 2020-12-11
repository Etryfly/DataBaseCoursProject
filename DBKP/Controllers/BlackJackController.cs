using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBKP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DBKP.Controllers
{
    public class BlackJackController : Controller
    {
        private ApplicationContext db;

        private readonly ILogger<BlackJackController> _logger;

        public BlackJackController(ApplicationContext db, ILogger<BlackJackController> logger)
        {
            _logger = logger;

            this.db = db;
        }

        [HttpGet]
        public IActionResult BlackJack()
        {
            if (!User.Identity.IsAuthenticated)
            {
                ModelState.AddModelError("Login", "Please, login in");
                return RedirectToAction("Loginin", "Authentication");
            }

          
            UserModel userModel = db.Users.FirstOrDefault(u => u.Login == User.Identity.Name);
            MoneyModel moneyModel = db.Money.FirstOrDefault(u => u.Id == userModel.Id);

            GameStatsModel gameStatsModel = db.GameStats.FirstOrDefault(u => u.Id == userModel.Id);
            
            userModel.Money = moneyModel;
            userModel.GameStats = gameStatsModel;

            return View(userModel);
        }

        #region CardOperation


        public List<HandModel> GetHandsForUserWithId(int id)
        {

            if (db.Hand.FirstOrDefault(c => c.user_id == id) != null)
                return db.Hand.Where(c => c.user_id == id).ToList();


            return new List<HandModel>();
        }

        public List<CardModel> GetAllCardsByHandModel(HandModel userHand)
        {
            List<CardModel> result = new List<CardModel>();




            List<HandCardModel> handCards = db.HandCard.Where(c => c.HandId == userHand.hand_id).ToList();
            foreach (var card in handCards)
            {
                result.Add(db.Card.FirstOrDefault(c => c.CardId == card.CardId));
            }

            return result;
        }


        private List<CardModel> getAllAvailableCardsByUserId(int id)
        {
            List<CardModel> usedCard = new List<CardModel>();
            foreach (var hand in GetHandsForUserWithId(id))
            {
                foreach (var card in GetAllCardsByHandModel(hand))
                {
                    usedCard.Add(card);
                }
            }

            List<CardModel> allCard = db.Card.ToList();
            return allCard.Except(usedCard).ToList();

        }

        public List<CardModel> getNCardForUserWithId(int n, int id)
        {
            List<CardModel> availableCards = getAllAvailableCardsByUserId(id);
            var rnd = new Random();
            return availableCards.OrderBy(x => rnd.Next()).Take(n).ToList();
        }

        public void setCardsForUserWithIdAndHandTypeId(List<CardModel> cards, int id, int typeId)
        {
            HandModel hand = db.Hand.FirstOrDefault(h => h.user_id == id && h.hand_type_id == typeId);
            if (hand == null)
            {
                hand = new HandModel() {user_id = id, hand_type_id = typeId};
                db.Hand.Add(hand);
                db.SaveChanges();
            }

            foreach (var card in cards)
            {
                db.HandCard.Add(new HandCardModel() {CardId = card.CardId, HandId = hand.hand_id});
            }

            // db.Hand.Add(hand);
            db.SaveChanges();
        }
        
        private void ClearHandCardsByHand(HandModel handModel)
        {
            foreach (var handCard in db.HandCard.Where(h => h.HandId == handModel.hand_id))
            {
                db.HandCard.Remove(handCard);
            }

            
            db.SaveChanges();
            db.Hand.Remove(handModel);
            db.SaveChanges();
        }

        #endregion

        private int CountScore(List<CardModel> cards)
        {
            int acesCount = 0;
            int score = 0;
            foreach (var card in cards)
            {
                if (card.Rank == 14) acesCount++;
                score += card.Score;
            }

            if (score > 21 && acesCount > 0)
            {
                for (int i = 0; i < acesCount; i++)
                {
                    if (score > 21)
                    {
                        score -= 10;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return score;
        }

        private enum gameState
        {
            userWin = 1,
            userLose = 2,
            ok = 3,
            error = 4
        }

        

        private void UserLoseIntoGameStatTable(int userId, decimal bet)
        {
            GameStatsModel gameStatsModel = db.GameStats.FirstOrDefault(g => g.Id == userId);
            gameStatsModel.chips_loosed += bet;
            gameStatsModel.Loses += 1;
            db.SaveChanges();
        }
        
        private void UserWinIntoGameStatTable(int userId, decimal bet)
        {
            GameStatsModel gameStatsModel = db.GameStats.FirstOrDefault(g => g.Id == userId);
            gameStatsModel.chips_earned += 2*bet;
            gameStatsModel.Wins += 1;
            db.SaveChanges();
        }

        public JsonResult Stand()
        {
            const int USER_HAND_TYPE = 1;
            const int COMPUTER_HAND_TYPE = 2;

            UserModel userModel = db.Users.FirstOrDefault(u => u.Login == User.Identity.Name);
            List<HandModel> hands = GetHandsForUserWithId(userModel.Id);
            if (hands.Count != 2)
            {
                _logger.LogInformation("Hands count != 2 in method Stand");
                return Json(new
                {
                    state = gameState.error,
                    message = "Hands count != 2 in method Stand"
                });
            }
            

            int computerScore = 0;
            int userScore = 0;

            List<CardModel> computerCards = new List<CardModel>();

            foreach (var hand in hands)
            {
                if (hand.hand_type_id == COMPUTER_HAND_TYPE)
                {
                    computerCards =  GetAllCardsByHandModel(hand);
                    computerScore = CountScore(computerCards);
                    while (computerScore < 17)
                    {
                        List<CardModel> addCards = getNCardForUserWithId(1, userModel.Id);
                        computerCards.Add(addCards[0]);
                        setCardsForUserWithIdAndHandTypeId(addCards, userModel.Id, COMPUTER_HAND_TYPE);
                        computerScore += CountScore(addCards);
                    }
                }

                if (hand.hand_type_id == USER_HAND_TYPE)
                {
                    userScore = CountScore(GetAllCardsByHandModel(hand));
                }

                ClearHandCardsByHand(hand);
            }

            BetsModel betModel = db.Bets.FirstOrDefault(b => b.UserId == userModel.Id);
            decimal bet = betModel.Bet;
            db.Remove(betModel);
            // GameStatsModel gameStatsModel = db.GameStats.FirstOrDefault(s => s.Id == userModel.Id);
            MoneyModel moneyModel = db.Money.FirstOrDefault(u => u.Id == userModel.Id);
            if (userScore > computerScore && userScore <= 21)
            {
                moneyModel.Chips += bet * 2;
                UserWinIntoGameStatTable(userModel.Id, bet);
                db.SaveChanges();
                _logger.LogDebug(string.Format("Stand: user win. User score - {0}, computer score - {1}", userScore, computerScore) );
                return Json(new
                {
                    ComputerCards = computerCards,
                    state = gameState.userWin,
                    message = "",
                    userBet = bet,
                    ComputerScore = computerScore
                });
            }

            if (computerScore > userScore && computerScore <= 21)
            {
                _logger.LogDebug(string.Format("Stand: user lose. User score - {0}, computer score - {1}", userScore, computerScore) );
                UserLoseIntoGameStatTable(userModel.Id, bet);
                return Json(new
                {
                    ComputerCards = computerCards,
                    state = gameState.userLose,
                    message = "",
                    userBet = bet,
                    ComputerScore = computerScore
                });
            }

            if (computerScore > 21)
            {
                moneyModel.Chips += bet * 2;
                db.SaveChanges();
                UserWinIntoGameStatTable(userModel.Id, bet);
                _logger.LogDebug(string.Format("Stand: user win (computer score > 21). User score - {0}, computer score - {1}", userScore, computerScore) );
                return Json(new
                {
                    ComputerCards = computerCards,
                    state = gameState.userWin,
                    message = "",
                    userBet = bet,
                    ComputerScore = computerScore
                });
            }

            if (userScore > 21)
            {
                _logger.LogDebug(string.Format("Stand: user lose (user score > 21). User score - {0}, computer score - {1}", userScore, computerScore) );
                UserLoseIntoGameStatTable(userModel.Id, bet);
                return Json(new
                {
                    ComputerCards = computerCards,
                    state = gameState.userLose,
                    message = "",
                    userBet = bet,
                    ComputerScore = computerScore
                });
            }

            if (userScore == computerScore)
            {
                UserWinIntoGameStatTable(userModel.Id, bet);
                _logger.LogDebug(string.Format("Stand: user win (user score == computer score). User score - {0}, computer score - {1}", userScore, computerScore) );
                return Json(new
                {
                    ComputerCards = computerCards,
                    state = gameState.userWin,
                    message = "",
                    userBet = bet,
                    ComputerScore = computerScore
                });
            }
            _logger.LogDebug(string.Format("Stand: OK. User score - {0}, computer score - {1}", userScore, computerScore) );

            return Json(new
            {
                state = gameState.ok,
                message = ""
            });

        }



        [HttpPost]
        public JsonResult Bet(int count)
        {
            const bool NOT_ENOUGH_CHIPS = false;
            const bool OK = true;
            const int USER_HAND_TYPE = 1;
            const int COMPUTER_HAND_TYPE = 2;
            UserModel userModel = db.Users.FirstOrDefault(u => u.Login == User.Identity.Name);
            MoneyModel moneyModel = db.Money.FirstOrDefault(u => u.Id == userModel.Id);
            _logger.LogDebug("BlackJack - Bet -  count: " + count);
            // ViewBag.Count = count;
            bool status;
            if (count > int.MaxValue)RedirectToAction("Error", "Home");
            if (moneyModel == null) RedirectToAction("Error", "Home");


            List<CardModel> userCards = new List<CardModel>();
            List<CardModel> computerCards = new List<CardModel>();
            if (moneyModel.Chips >= count && count > 0)
            {
                moneyModel.Chips -= count;
                status = OK;
                BetsModel betsModel = db.Bets.FirstOrDefault(b => b.UserId == userModel.Id);
                if (betsModel != null)
                {
                    List<HandModel> hands = GetHandsForUserWithId(userModel.Id);
                    foreach (HandModel hand in hands)
                    {
                        ClearHandCardsByHand(hand);
                    }
                    db.Bets.Remove((betsModel));
                   
                }
                betsModel = new BetsModel() {Bet = count, UserId = userModel.Id};
                db.Bets.Add(betsModel);
              
                db.SaveChanges();
                
                
                userCards = getNCardForUserWithId(2, userModel.Id);
                setCardsForUserWithIdAndHandTypeId(userCards, userModel.Id, USER_HAND_TYPE);
                computerCards = getNCardForUserWithId(1, userModel.Id);
                setCardsForUserWithIdAndHandTypeId(computerCards, userModel.Id, COMPUTER_HAND_TYPE);
            }
            else
            {
                status = NOT_ENOUGH_CHIPS;
            }
            
           

            int userScore = CountScore(userCards);
            int computerScore = CountScore(computerCards);
            
            StringBuilder userScoreStringBuilder = new StringBuilder(userScore.ToString());
            
            return Json(new
            {
                chips = moneyModel.Chips,
                operationStatus = status,
                cards = new
                {
                    user = userCards,
                    computer = computerCards
                },
                userScore = userScoreStringBuilder.ToString(),
                ComputerScore = computerScore
            });

        }

        private const int USER_HAND_TYPE = 1;
        private const int COMPUTER_HAND_TYPE = 2;

        public JsonResult IsUserScoreMoreThan21()
        {
            

            UserModel userModel = db.Users.FirstOrDefault(u => u.Login == User.Identity.Name);
            List<HandModel> hands = GetHandsForUserWithId(userModel.Id);
            if (hands.Count != 2)
            {
                string errorMessage = "Hands count != 2 in method IsUserScoreMoreThan21";
                _logger.LogInformation(errorMessage);
                return Json(new
                {
                    state = gameState.error,
                    message = errorMessage
                });
            }


            
            int userScore = 0;

            List<CardModel> userCards = new List<CardModel>();
            foreach (var hand in hands)
            {
                if (hand.hand_type_id == USER_HAND_TYPE)
                {
                    userCards = GetAllCardsByHandModel(hand);
                    userScore = CountScore(userCards);
                }
            }

            if (userScore > 21)
            {
                _logger.LogDebug(string.Format("Hit: user lose. User score - {0}", userScore) );
                ClearHandCardsByHand(hands[0]);
                ClearHandCardsByHand(hands[1]);
                BetsModel betModel = db.Bets.FirstOrDefault(b => b.UserId == userModel.Id);
                decimal bet = betModel.Bet;
                UserLoseIntoGameStatTable(userModel.Id, betModel.Bet);
                db.GameStats.FirstOrDefault(u => u.Id == userModel.Id).chips_loosed += betModel.Bet;
                db.Remove(betModel);
                return Json(new
                {
                    state = gameState.userLose,
                    UserCards = userCards,
                    UserScore = userScore,
                    userBet = bet
                    
                });
            }

            return Json(new
            {
                state = gameState.ok,
                UserCards = userCards,
                UserScore = userScore
            });
        }

        public async Task<JsonResult> GetChips()
        {
            UserModel user = await db.Users.FirstOrDefaultAsync(u => u.Login == User.Identity.Name);
            MoneyModel moneyModel = await db.Money.FirstOrDefaultAsync(m => m.Id == user.Id);
            decimal money = moneyModel.Chips;
            _logger.LogDebug(money.ToString());
            return Json(new
            {
                chips = money
            });
        }

        public JsonResult Hit()
        {
            UserModel userModel = db.Users.FirstOrDefault(u => u.Login == User.Identity.Name);

            List<CardModel> card = getNCardForUserWithId(1, userModel.Id);
            setCardsForUserWithIdAndHandTypeId(card, userModel.Id, USER_HAND_TYPE);

            return IsUserScoreMoreThan21();
        }

    }
}    