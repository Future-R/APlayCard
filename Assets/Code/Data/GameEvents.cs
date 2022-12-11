using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameEvents
{
    public const string PlayerTouch = nameof(PlayerTouch);
    public const string ActionPhase = nameof(ActionPhase);
    public const string ActionEnd = nameof(ActionEnd);
    public const string MatchStartPhase = nameof(MatchStartPhase);
    public const string RoundStartPhase = nameof(RoundStartPhase);
    public const string PlayerWinGame = nameof(PlayerWinGame);
    public const string PlayerWinMatch = nameof(PlayerWinMatch);
    public const string ContorllerCoolDown = nameof(ContorllerCoolDown);
    public const string HandsChange = nameof(HandsChange);
    public const string PlayCard = nameof(PlayCard);
    public const string Discard = nameof(Discard);
    //public const string ZoneCardsCountChange = nameof(ZoneCardsCountChange);
}

/// <summary>
/// 玩家点击
/// </summary>
public class PlayerTouchEventArgs : EventArgs
{
    /// <summary>
    /// 玩家
    /// </summary>
    public Player activePlayer;
}

/// <summary>
/// 行动开始
/// </summary>
public class ActionPhaseEventArgs : EventArgs
{
    /// <summary>
    /// 玩家
    /// </summary>
    public Player activePlayer;
}

/// <summary>
/// 行动（出牌/弃牌/购牌）结束
/// </summary>
public class ActionEndEventArgs : EventArgs
{
    /// <summary>
    /// 玩家
    /// </summary>
    public Player currentPlayer;
}

public class MatchStartEventArgs : EventArgs
{
    
}

public class RoundStartEventArgs : EventArgs
{

}

public class PlayerWinGameEventArgs : EventArgs
{
    /// <summary>
    /// 玩家
    /// </summary>
    public Player currentPlayer;
}

public class PlayerWinMatchEventArgs : EventArgs
{
    /// <summary>
    /// 玩家
    /// </summary>
    public Player winner;
    public string reason;
}

public class ContorllerCoolDownEventArgs : EventArgs
{
    /// <summary>
    /// 玩家
    /// </summary>
    public Player currentPlayer;
    /// <summary>
    /// 等待原因
    /// </summary>
    public string reason;
}
public class HandsChangeEventArgs : EventArgs
{
    /// <summary>
    /// 玩家
    /// </summary>
    public Player currentPlayer;
}

public class PlayCard : EventArgs
{
    /// <summary>
    /// 玩家
    /// </summary>
    public Player player;
    /// <summary>
    /// 卡牌
    /// </summary>
    public Card card;
    /// <summary>
    /// 目标区域
    /// </summary>
    public BattleField.Zone targetZone;
}

public class Discard : EventArgs
{
    /// <summary>
    /// 玩家
    /// </summary>
    public Player player;
    /// <summary>
    /// 卡牌
    /// </summary>
    public Card card;
}