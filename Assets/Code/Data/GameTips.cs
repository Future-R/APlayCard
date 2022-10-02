using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTips
{
    public enum WarningTips
    {
        现在不是你的回合
    }

    public enum WinReason
    {
        得分胜利,
        对手无牌可抓,
        你解开了封印
    }

    public enum LoseReason
    {
        分数失利,
        你无牌可抓,
        对手解开了封印
    }
}
