using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleField : MonoBehaviour
{

    public Player player1;
    public Player player2;

    public Player activePlayer;
    public Player nonactivePlayer;

    public int round;
    /// <summary>
    /// 需要显示界面的玩家
    /// </summary>
    public Player OBPlayer;

    public Zone Zone_Red = new Zone();
    public Zone Zone_Green = new Zone();
    public Zone Zone_Blue = new Zone();
    /// <summary>
    /// 游戏中所有卡片的对象池
    /// </summary>
    public List<Card> cardObjectPool = new List<Card>();


    public class Zone
    {
        public List<Card> cards = new List<Card>();
        public Card.CardColor color;

        /// <summary>
        /// 取牌堆顶点数
        /// </summary>
        /// <returns></returns>
        public int GetPopPoint()
        {
            if (cards.Count == 0)
            {
                return GameHelper.nonExist;
            }
            return cards.Last().points;
        }
    }

    public enum Phase
    {
        游戏开始阶段,

    }

    private void FixedUpdate()
    {
        if (activePlayer)
        {
            activePlayer.timer -= Time.deltaTime;
        }
    }

    /// <summary>
    /// 初始化场地
    /// </summary>
    public void initZone()
    {
        Zone_Red.color = Card.CardColor.红;
        Zone_Green.color = Card.CardColor.绿;
        Zone_Blue.color = Card.CardColor.蓝;

        // TODO：这里不应该直接清除，而应该根据持有者转移到墓地。转移后再清除（如果有无持有者的卡牌的话）
        Zone_Red.cards.Clear();
        Zone_Green.cards.Clear();
        Zone_Blue.cards.Clear();
    }

    /// <summary>
    /// 查找对应颜色的区域
    /// </summary>
    /// <param name="color">颜色</param>
    /// <returns></returns>
    public Zone FindZoneByColor(Card.CardColor color) => color switch
    {
        Card.CardColor.红 => Zone_Red,
        Card.CardColor.绿 => Zone_Green,
        Card.CardColor.蓝 => Zone_Blue,
        _ => null,
    };

    /// <summary>
    /// 获取牌堆顶点数（空是-2）
    /// </summary>
    /// <returns></returns>
    public int GetPopPoint(Zone zone)
    {
        if (null != zone || zone.cards.Count == 0)
        {
            return GameHelper.nonExist;
        }
        return zone.cards.Last().points;
    }
}
