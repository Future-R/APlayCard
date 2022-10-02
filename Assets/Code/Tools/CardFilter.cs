using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static BattleField;

public static class CardFilter
{
    public static BattleField BF;

    public static List<Card> UsableCards(Player AP)
    {
        List<Card> usableCards = new List<Card>();
        foreach (var item in ZoneFilter.GetField())
        {
            if (item.cards.Count == 0)
            {
                usableCards.AddRange(AP.Hand.Where(c => c.colors.Contains(item.color)));
            }
            else
            {
                Card popCard = item.cards.Last();
                usableCards.AddRange(AP.Hand.Where(c => c.colors.Contains(item.color) && c.points > popCard.points));
            }
        }
        return usableCards;
    }

    public static List<Card> UsableCardsToZone(Card.CardColor cardColor,Player AP)
    {
        List<Card> result = new List<Card>();
        Zone targetZone = BF.FindZoneByColor(cardColor);
        int popPoint = targetZone.GetPopPoint();
        result.AddRange(AP.Hand.Where(c => c.points > popPoint));
        return result;
    }

    public static List<Card> OnFieldCards()
    {
        List<Card> result = new List<Card>();
        foreach (var item in ZoneFilter.GetField())
        {
            result.AddRange(item.cards);
        }
        return result;
    }
}
