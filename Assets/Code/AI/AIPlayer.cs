using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static BattleField;

public class AIPlayer : MonoBehaviour
{
    //BattleManager battleManager;
    //BattleField battleField;

    private void Start()
    {
        //battleManager = FindObjectOfType<BattleManager>();
        EventManager.Instance.AddListener(GameEvents.ActionPhase, Action);
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveListener(GameEvents.ActionPhase, Action);
    }

    private void Action(object sender, EventArgs e)
    {
        Player self = (e as ActionPhaseEventArgs).activePlayer;
        if (self.gameObject != gameObject)
        {
            return;
        }
        Debug.Log($"轮到{self.NickName}#{self.AccountID}行动！");
        // 筛选当前玩家手中的可用卡牌
        List<Card> useableCards = CardFilter.UsableCards(self);
        if (useableCards.Count == 0)
        {
            if (self.Hand.Count > 1 && self.Deck.Count > 0)
            {
                // 随机选2张牌丢掉，之后可以优化
                List<Card> discards = GameHelper.SelectAny(self.Hand, 2);
                self.Buy(discards.FirstOrDefault(), discards.Last());
                return;
            }
            else
            {
                Card discard = GameHelper.SelectOne(self.Hand);
                self.Discard(discard);
                return;
            }

        }
        // 如果只有1张牌，肯定出这张。如果这张牌能打到多个区域，则找牌堆顶最小的打。
        else if (useableCards.Count == 1)
        {
            Card shouldPlayCard = useableCards[0];
            List<Zone> canPlayZones = ZoneFilter.CheckCanPlayZones(shouldPlayCard);
            Zone shouldPlayZone = canPlayZones.OrderBy(z => z.GetPopPoint()).FirstOrDefault();

            self.PlayCard(shouldPlayCard, ref shouldPlayZone);
            return;
        }
        // 如果同颜色只有1，且比牌堆顶的卡大1点以上，优先出
        // todo

        // 最小的点数相同时，牌堆顶最小的出
        else
        {
            // 筛选可用卡牌中点数最小的（可能不止一张）
            List<Card> minOfHands = useableCards.Where(c => c.points == useableCards.Min(c => c.points)).ToList();
            List<Zone> canPlayZones = new List<Zone>();
            foreach (var card in minOfHands)
            {
                // 将那些点数最小的卡牌能打出的区域都纳入统计
                canPlayZones.AddRange(ZoneFilter.CheckCanPlayZones(card));
            }
            // 将能打出的区域按牌堆顶的点数大小进行排序，无卡牌时以-2计算，最后取牌堆顶最小的区域作为目标区域
            Zone shouldPlayZone = canPlayZones.OrderBy(z => z.GetPopPoint()).FirstOrDefault();
            Card shouldPlayCard = GameHelper.SelectOne(CardFilter.UsableCardsToZone(minOfHands, shouldPlayZone, self));
            
            self.PlayCard(shouldPlayCard, ref shouldPlayZone);
            return;
        }
    }


}
