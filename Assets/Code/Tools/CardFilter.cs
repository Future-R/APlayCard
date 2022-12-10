using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static BattleField;

public static class CardFilter
{
    public static BattleField BF;

    /// <summary>
    /// 从玩家手牌中检查有哪些牌可打出
    /// </summary>
    /// <param name="cardColor"></param>
    /// <param name="AP"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 从卡牌列表中检查有哪些牌可打出到指定区域
    /// </summary>
    /// <param name="cards"></param>
    /// <param name="targetZone"></param>
    /// <param name="currentPlayer"></param>
    /// <returns></returns>
    public static List<Card> UsableCardsToZone(List<Card> cards, Zone targetZone, Player currentPlayer)
    {
        List<Card> result = new List<Card>();
        int popPoint = targetZone.GetPopPoint();
        result.AddRange(cards.Where(c => c.colors.Contains(targetZone.color) && c.points > popPoint));
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
