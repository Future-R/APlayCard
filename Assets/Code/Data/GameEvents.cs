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
    //public const string ZoneCardsCountChange = nameof(ZoneCardsCountChange);
}

/// <summary>
/// ��ҵ��
/// </summary>
public class PlayerTouchEventArgs : EventArgs
{
    /// <summary>
    /// ���
    /// </summary>
    public Player activePlayer;
}

/// <summary>
/// �ж���ʼ
/// </summary>
public class ActionPhaseEventArgs : EventArgs
{
    /// <summary>
    /// ���
    /// </summary>
    public Player activePlayer;
}

/// <summary>
/// �ж�������/����/���ƣ�����
/// </summary>
public class ActionEndEventArgs : EventArgs
{
    /// <summary>
    /// ���
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
    /// ���
    /// </summary>
    public Player currentPlayer;
}

public class PlayerWinMatchEventArgs : EventArgs
{
    /// <summary>
    /// ���
    /// </summary>
    public Player winner;
    public string reason;
}

public class ContorllerCoolDownEventArgs : EventArgs
{
    /// <summary>
    /// ���
    /// </summary>
    public Player currentPlayer;
    /// <summary>
    /// �ȴ�ԭ��
    /// </summary>
    public string reason;
}
public class HandsChangeEventArgs : EventArgs
{
    /// <summary>
    /// ���
    /// </summary>
    public Player currentPlayer;
}