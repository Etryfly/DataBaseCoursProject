using System;
using System.Collections.Generic;
using System.Linq;
using DB_KP.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DB_KP.Controllers
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
            
            // db.GetService<ILoggerFactory>().AddProvider(new DBLoggerProvider());
            UserModel userModel = db.Users.FirstOrDefault(u => u.Login == User.Identity.Name );
            MoneyModel moneyModel = db.Money.FirstOrDefault(u => u.Id == userModel.Id);
            
            GameStatsModel gameStatsModel = db.GameStats.FirstOrDefault(u => u.Id == userModel.Id);
            _logger.LogInformation("BlackJack: real userId: " + userModel.Id + " id from money: "
            + moneyModel.Id + " money: " + moneyModel.Chips);
            userModel.Money = moneyModel;
            userModel.GameStats = gameStatsModel;
            
            return View(userModel);
        }

        #region CardOperation

        
        public List<HandModel> GetHandsForUserWithId( int id)
        {
            
            if (db.Hand.FirstOrDefault(c => c.user_id == id) != null) 
                return  db.Hand.Where(c => c.user_id == id).ToList();
            
           
            return new List<HandModel>();
        }
        
        public List<CardModel> GetAllCardsByHandModel( HandModel userHand)
        {
            List<CardModel> result = new List<CardModel>();
            
            
            
        
            List<HandCardModel> handCards = db.HandCard.Where(c => c.HandId == userHand.hand_id).ToList();
            foreach (var card in handCards)
            {
                result.Add(db.Card.FirstOrDefault( c => c.CardId == card.CardId));
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
                hand = new HandModel(){user_id =  id, hand_type_id = typeId};
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

        #endregion
        
        [HttpPost]
        public JsonResult Bet(int count)
        {
           
            const bool NOT_ENOUGH_CHIPS = false;
            const bool OK = true;
            const int USER_HAND_TYPE = 1;
            const int COMPUTER_HAND_TYPE = 2;
            UserModel userModel = db.Users.FirstOrDefault(u => u.Login == User.Identity.Name );
            MoneyModel moneyModel = db.Money.FirstOrDefault(u => u.Id == userModel.Id);
            _logger.LogDebug("BlackJack - Bet -  count: " + count);
            // ViewBag.Count = count;
            bool status;
            if (moneyModel == null) RedirectToAction("Error", "Home");
            if (moneyModel.Chips >= count && count > 0)
            {
                moneyModel.Chips -= count;
                status = OK;
                BetsModel betsModel = db.Bets.FirstOrDefault(b => b.UserId == userModel.Id);
                if (betsModel == null)
                {
                    betsModel = new BetsModel(){Bet = count, UserId = userModel.Id};
                    db.Bets.Add(betsModel);
                }
                else
                {
                    betsModel.Bet += count;
                }
                db.SaveChanges();
            }
            else
            {
                status = NOT_ENOUGH_CHIPS;
            }
            
            
            List<CardModel> userCards = getNCardForUserWithId(2, userModel.Id);
            setCardsForUserWithIdAndHandTypeId(userCards, userModel.Id, USER_HAND_TYPE);
            List<CardModel> computerCards = getNCardForUserWithId(2, userModel.Id);
            setCardsForUserWithIdAndHandTypeId(computerCards, userModel.Id, COMPUTER_HAND_TYPE);
            int computerScore = 0;
            int userScore = 0;
            foreach (var card in userCards)
            {
                userScore += card.Score;
            }
            
            foreach (var card in computerCards)
            {
                computerScore += card.Score;
            }
            return Json(new
            {
                chips = moneyModel.Chips,
                operationStatus = status,
                cards = new
                {
                    user = userCards,
                    computer = computerCards
                },
                score = new
                {
                    computer = computerScore,
                    user = userScore
                }
            });

        }
        
    }
}    