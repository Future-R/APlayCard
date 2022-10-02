using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static BattleField;

public class BattleManager : MonoBehaviour
{

    BattleField battleField;

    public GameObject cardsParentNode;


    void Start()
    {
        battleField = GetComponent<BattleField>();
        CardFilter.BF = battleField;
        ZoneFilter.BF = battleField;

        // 导入玩家
        // 暂时由配置表写死
        battleField.player1.GameInit();
        battleField.player2.GameInit();

        // 导入卡组
        DebugImportDeck();
        // 洗牌
        GameHelper.Shuffle(ref battleField.player1.Deck);
        GameHelper.Shuffle(ref battleField.player2.Deck);

        // 监听玩家触摸，进入对局
        EventManager.Instance.AddListener(GameEvents.PlayerTouch, Match);
        // 监听对局结束
        EventManager.Instance.AddListener(GameEvents.PlayerWinMatch, WinMatch);
        // 监听游戏结束
        EventManager.Instance.AddListener(GameEvents.PlayerWinGame, WinGame);

    }

    // 导入双方卡组
    public void DebugImportDeck()
    {

        battleField.player1.Deck.Clear();
        battleField.player2.Deck.Clear();

        foreach (var color in new Card.CardColor[] { Card.CardColor.红, Card.CardColor.绿, Card.CardColor.蓝 })
        {
            foreach (var num in new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 })
            {
                CreateCard(ref battleField.player1.Deck, color, num, battleField.player1);
                CreateCard(ref battleField.player2.Deck, color, num, battleField.player2);
            }
        }
    }

    public void CreateCard(ref List<Card> cards, List<Card.CardColor> colors, byte num, Player owner = null)
    {
        string cardName = $"{string.Join("-", colors)}-{num}";
        GameObject cardInstance = new GameObject(cardName, typeof(Card));
        cardInstance.transform.SetParent(cardsParentNode.transform);
        Card card = cardInstance.GetComponent<Card>();
        card.Init(colors, num);
        card.owner = owner;
        cards.Add(card);
    }
    public void CreateCard(ref List<Card> cards, Card.CardColor color, byte num, Player owner = null)
    {
        string cardName = $"{color}-{num}";
        GameObject cardInstance = (GameObject)Instantiate(Resources.Load("P_Card_Classic"), cardsParentNode.transform);


        Card card = cardInstance.GetComponent<Card>();
        card.Init(color, num);
        if (owner)
        {
            card.owner = owner;
            if (battleField.OBPlayer == owner)
            {
                cardInstance.transform.SetParent(owner.GetComponent<PlayerViewer>().DeckLayoutCenter.transform);
                //cardInstance.GetComponent<CardShow>().TurnCover();
                cardInstance.GetComponent<CardShow>().Draw();
            }
        }
        card.transform.localScale = new Vector3(0.5f, 0.5f, 1);

        cards.Add(card);
    }

    public void DrawStartingHands()
    {
        if (battleField.activePlayer.Deck.Count < 8)
        {
            // 如果当前行动玩家无牌可抓，且当前行动玩家非自己，那么显示自己（非行动玩家）胜利
            string reason = battleField.activePlayer != battleField.OBPlayer ? GameTips.WinReason.对手无牌可抓.ToString() : GameTips.LoseReason.你无牌可抓.ToString();
            this.TriggerEvent(GameEvents.PlayerWinMatch, new PlayerWinMatchEventArgs { winner = battleField.nonactivePlayer, reason = reason });
        }
        else if (battleField.nonactivePlayer.Deck.Count < 8)
        {
            string reason = battleField.nonactivePlayer != battleField.OBPlayer ? GameTips.WinReason.对手无牌可抓.ToString() : GameTips.LoseReason.你无牌可抓.ToString();
            this.TriggerEvent(GameEvents.PlayerWinMatch, new PlayerWinMatchEventArgs { winner = battleField.activePlayer, reason = reason });
        }
        GameHelper.DrawCards(ref battleField.activePlayer.Deck, ref battleField.activePlayer.Hand, 8);
        this.TriggerEvent(GameEvents.HandsChange, new HandsChangeEventArgs { currentPlayer = battleField.activePlayer });
        GameHelper.DrawCards(ref battleField.nonactivePlayer.Deck, ref battleField.nonactivePlayer.Hand, 8);
        this.TriggerEvent(GameEvents.HandsChange, new HandsChangeEventArgs { currentPlayer = battleField.nonactivePlayer });
    }

    public int CalculateScore(Player player)
    {
        List<Card> allCardsOnField = CardFilter.OnFieldCards();
        int baseScore = allCardsOnField.Where(c => c.owner == player).Count() * 100;
        return baseScore;
    }

    public void Match(object sender, EventArgs e)
    {
        // 已进入对局，移除触摸进入对局监听
        EventManager.Instance.RemoveListener(GameEvents.PlayerTouch, Match);

        battleField.initZone();

        battleField.player1.MatchInit();
        battleField.player2.MatchInit();
        battleField.round = 0;

        // 决定先后
        battleField.activePlayer = GameHelper.SelectOne(new List<Player>
        {
            battleField.player1,
            battleField.player2
        });
        if (battleField.activePlayer = battleField.player1)
        {
            battleField.nonactivePlayer = battleField.player2;
        }
        else
        {
            battleField.nonactivePlayer = battleField.player1;
        }

        // 后手贴50分
        battleField.nonactivePlayer.Score += 50;

        // 游戏开始

        // 监听玩家触摸，进入回合
        EventManager.Instance.AddListener(GameEvents.PlayerTouch, Round);
    }

    public void Round(object sender, EventArgs e)
    {
        // 已进入回合，移除触摸进入回合监听
        EventManager.Instance.RemoveListener(GameEvents.PlayerTouch, Round);
        var data = e as PlayerTouchEventArgs;
        if (data == null) return;
        battleField.round += 1;
        if (battleField.round != 1)
        {
            (battleField.nonactivePlayer, battleField.activePlayer) = (battleField.activePlayer, battleField.nonactivePlayer);
        }
        else
        {
            DrawStartingHands();
        }
        Debug.Log($"Round{battleField.round}！");
        this.TriggerEvent(GameEvents.ActionPhase, new ActionPhaseEventArgs { activePlayer = battleField.activePlayer });

        this.TriggerEvent(GameEvents.ActionEnd, new ActionPhaseEventArgs { activePlayer = battleField.activePlayer });

        Debug.Log("——————回合结束——————");

        if (battleField.nonactivePlayer.Hand.Count == 0 || battleField.round > 16)
        {
            // 算分
            battleField.activePlayer.Score += CalculateScore(battleField.activePlayer);
            battleField.nonactivePlayer.Score += CalculateScore(battleField.nonactivePlayer);
            if (battleField.activePlayer.Score > battleField.nonactivePlayer.Score)
            {
                string reason = battleField.activePlayer == battleField.OBPlayer ? GameTips.WinReason.得分胜利.ToString() : GameTips.LoseReason.分数失利.ToString();
                this.TriggerEvent(GameEvents.PlayerWinMatch, new PlayerWinMatchEventArgs { winner = battleField.activePlayer, reason = reason });
            }
            else
            {
                string reason = battleField.nonactivePlayer == battleField.OBPlayer ? GameTips.WinReason.得分胜利.ToString() : GameTips.LoseReason.分数失利.ToString();
                this.TriggerEvent(GameEvents.PlayerWinMatch, new PlayerWinMatchEventArgs { winner = battleField.nonactivePlayer, reason = reason });
            }
            Debug.LogWarning($"{battleField.activePlayer.NickName}#{battleField.activePlayer.AccountID}的分数：{battleField.activePlayer.Score}");
            Debug.LogWarning($"{battleField.nonactivePlayer.NickName}#{battleField.nonactivePlayer.AccountID}的分数：{battleField.nonactivePlayer.Score}");

            EventManager.Instance.RemoveListener(GameEvents.PlayerTouch, Round);
            if (battleField.player1.Victories > 1)
            {
                this.TriggerEvent(GameEvents.PlayerWinMatch, new PlayerWinGameEventArgs { currentPlayer = battleField.player1 });
            }
            else if (battleField.player2.Victories > 1)
            {
                this.TriggerEvent(GameEvents.PlayerWinMatch, new PlayerWinGameEventArgs { currentPlayer = battleField.player2 });
            }
            else
            {
                // 游戏未结束，对局结束，监听玩家触摸，重进对局
                EventManager.Instance.AddListener(GameEvents.PlayerTouch, Match);
            }
        }
        else
        {
            // 对局未结束，回合结束，监听玩家触摸，重进回合
            EventManager.Instance.AddListener(GameEvents.PlayerTouch, Round);
        }
    }

    public void WinGame(object sender, EventArgs e)
    {
        Player winner = (e as PlayerWinGameEventArgs).currentPlayer;
        Debug.LogWarning($"胜者{winner.name}#{winner.AccountID}！");
    }

    public void WinMatch(object sender, EventArgs e)
    {
        var data = e as PlayerWinMatchEventArgs;
        Player winner = data.winner;
        winner.Victories += 1;
        if (winner.GetComponent<PlayerViewer>() != null)
        {
            Debug.LogWarning($"{data.reason}！");
        }

    }

    private void OnDestroy()
    {

    }
}
