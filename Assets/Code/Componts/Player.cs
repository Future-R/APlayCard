using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static BattleField;

public class Player : MonoBehaviour
{
    public long AccountID;
    public string NickName;
    public int Victories = 0;
    public int Score = 0;
    public float timer = 180f;
    public List<Card> Deck = new List<Card>();
    public List<Card> Hand = new List<Card>();
    public List<Card> Graveyard = new List<Card>();

    private void Start()
    {
        //EventManager.Instance.AddListener(GameEvents.HandsCountChange,)
    }

    public void GameInit()
    {
        MatchInit();
        Deck.Clear();
    }

    public void MatchInit()
    {
        Score = 0;
        Hand.Clear();
        Graveyard.Clear();
    }

    /// <summary>
    /// 弃2抓1
    /// </summary>
    /// <param name="cardA"></param>
    /// <param name="cardB"></param>
    /// <returns></returns>
    public bool Buy(Card cardA, Card cardB)
    {
        if (Hand.Count > 1 && Deck.Count > 0)
        {
            Graveyard.Add(cardA);
            Hand.Remove(cardA);
            Debug.Log($"弃置了{cardA}");

            Graveyard.Add(cardB);
            Hand.Remove(cardB);
            Debug.Log($"弃置了{cardB}");

            GameHelper.DrawCards(ref Deck, ref Hand, 1);
            Debug.Log($"抽到了{Hand.Last()}");

            this.TriggerEvent(GameEvents.HandsChange, new HandsChangeEventArgs { currentPlayer = this });
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 丢弃1张手牌
    /// </summary>
    /// <param name="card"></param>
    /// <returns></returns>
    public bool Discard(Card card)
    {
        Graveyard.Add(card);
        Hand.Remove(card);
        Debug.Log($"弃置了{card}");

        this.TriggerEvent(GameEvents.HandsChange, new HandsChangeEventArgs { currentPlayer = this });
        return true;
    }

    /// <summary>
    /// 打出1张手牌
    /// </summary>
    /// <param name="card">要出的牌</param>
    /// <param name="targetZone">目标区域</param>
    /// <returns></returns> 
    public bool PlayCard(Card card, ref Zone targetZone)
    {
        targetZone.cards.Add(card);
        Hand.Remove(card);

        Debug.Log($"打出了{card}");

        this.TriggerEvent(GameEvents.PlayCard, new PlayCard { player = this, card = card, targetZone = targetZone });
        this.TriggerEvent(GameEvents.HandsChange, new HandsChangeEventArgs { currentPlayer = this });
        return true;
    }
}
