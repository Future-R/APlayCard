using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHelper
{
    #region 固定配置（我给你写死了）
    /// <summary>
    /// 区域没牌时，当前区域点数应该返回这个
    /// </summary>
    public const int nonExist = -2;
    /// <summary>
    /// 【最小】能力生效时，那张牌的点数应该是这个
    /// </summary>
    public const int minPoint = -1;
    /// <summary>
    /// 【最大】能力生效时，那张牌的点数应该是这个
    /// </summary>
    public const int maxPoint = 256;
    #endregion

    /// <summary>
    /// 在闭区间内获取一个int类型随机值
    /// </summary>
    /// <param name="minimum">最小值(包含)</param>
    /// <param name="maximum">最大值(包含)</param>
    /// <returns></returns>
    public static int GetRandomNumber(int minimum, int maximum) => new System.Random(Guid.NewGuid().GetHashCode()).Next(minimum, maximum + 1);

    /// <summary>
    /// 从list里随机取一项<br />
    /// 建议在 <b>需要足够随机的地方</b> 使用这个方法代替Random.Range()
    /// IL2CPP不支持dynamic，如果之后要打64位包（上TapPlay）的话得把这里重构……
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public static dynamic SelectOne(IList list)
    {
        if (list.Count == 1)
        {
            return list[0];
        }
        return list[GetRandomNumber(0, list.Count - 1)];
    }

    /// <summary>
    /// 不重复随机抽取
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public static List<T> SelectAny<T>(List<T> list, int count)
    {
        if (list.Count <= count)
        {
            return list;
        }
        List<T> result = new List<T>();
        while (result.Count < count)
        {
            var temp = list[GetRandomNumber(0, list.Count - 1)];
            if (!result.Contains(temp))
            {
                result.Add(temp);
            }
        }
        return result;
    }

    /// <summary>
    /// 洗牌
    /// </summary>
    /// <typeparam name="T">一般是Card</typeparam>
    /// <param name="list"></param>
    public static void Shuffle<T>(ref List<T> list)
    {
        int currentIndex;
        T tempValue;
        for (int i = list.Count - 1; i >= 0; i--)
        {
            currentIndex = UnityEngine.Random.Range(0, i + 1);
            tempValue = list[currentIndex];
            list[currentIndex] = list[i];
            list[i] = tempValue;
        }
    }

    public static bool DrawCards<T>(ref List<T> originList, ref List<T> toList, int num)
    {
        if (originList == toList)
        {
            return false;
        }
        else
        {
            if (originList.Count < num)
            {
                return false;
            }
            else
            {
                toList.AddRange(originList.GetRange(0, num));
                originList.RemoveRange(0, num);
                return true;
            }
        }
    }
    public static bool ForceDrawCards<T>(ref List<T> originList, ref List<T> toList, int num)
    {
        if (originList == toList)
        {
            return false;
        }
        else
        {
            if (originList.Count < num)
            {
                toList.AddRange(originList);
                originList.Clear();
                return true;
            }
            else
            {
                toList.AddRange(originList.GetRange(0, num));
                originList.RemoveRange(0, num);
                return true;
            }
        }
    }

}
