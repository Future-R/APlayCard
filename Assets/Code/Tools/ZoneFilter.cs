using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BattleField;

public class ZoneFilter
{
    public static BattleField BF;

    /// <summary>
    /// 获取场上三个区域
    /// </summary>
    /// <returns></returns>
    public static List<Zone> GetField() => new List<Zone>
        {
            BF.Zone_Blue,
            BF.Zone_Green,
            BF.Zone_Red
        };

    /// <summary>
    /// 检查这张牌能打到哪些区域
    /// </summary>
    /// <param name="card"></param>
    /// <returns></returns>
    public static List<Zone> CheckCanPlayZones(Card card)
    {
        List<Zone> zones = new List<Zone>();
        foreach (var zone in GetField())
        {
            if (card.colors.Contains(zone.color))
            {
                zones.Add(zone);
            }
        }
        return zones;
    }
}
