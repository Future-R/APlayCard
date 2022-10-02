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
    /// ��2ץ1
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
            Debug.Log($"������{cardA}");

            Graveyard.Add(cardB);
            Hand.Remove(cardB);
            Debug.Log($"������{cardB}");

            GameHelper.DrawCards(ref Deck, ref Hand, 1);
            Debug.Log($"�鵽��{Hand.Last()}");
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// ����1������
    /// </summary>
    /// <param name="card"></param>
    /// <returns></returns>
    public bool Discard(Card card)
    {
        Graveyard.Add(card);
        Hand.Remove(card);
        Debug.Log($"������{card}");

        return true;
    }

    /// <summary>
    /// ���1������
    /// </summary>
    /// <param name="card">Ҫ������</param>
    /// <param name="targetZone">Ŀ������</param>
    /// <returns></returns>
    public bool PlayCard(Card card,ref Zone targetZone)
    {
        targetZone.cards.Add(card);
        Hand.Remove(card);
        Debug.Log($"�����{card}");
        return true;
    }
}
