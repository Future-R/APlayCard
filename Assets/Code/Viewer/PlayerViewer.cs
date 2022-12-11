using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Player))]
/// <summary>
/// 这个脚本用来向玩家提供界面显示
/// </summary>
public class PlayerViewer : MonoBehaviour
{
    public BattleField battleField;

    public GameObject HandLineLayoutCenter;
    public GameObject HandCircleLayoutCenter;
    public GameObject DeckLayoutCenter;
    public GameObject GraveyardLayoutCenter;

    public GameObject OppHandLineLayoutCenter;
    public GameObject OppHandCircleLayoutCenter;
    public GameObject OppDeckLayoutCenter;
    public GameObject OppGraveyardLayoutCenter;

    public GameObject RedZoneLayoutCenter;
    public GameObject GreenZoneHandLayoutCenter;
    public GameObject BlueZoneDeckLayoutCenter;

    public float lineLayoutXOffset = 20f;

    public float circleLayoutAngle = 60f;
    //public float circleLayoutLeftAngle = 105f;
    //public float circleLayoutRightAngle = 75f;
    public float circleLayoutRadius = 100f;
    [Tooltip("阻力")]
    public float damp = 20f;
    private Player player;

    void Start()
    {
        player = GetComponent<Player>();
        EventManager.Instance.AddListener(GameEvents.HandsChange, ReCalculationHandsPosition);
        EventManager.Instance.AddListener(GameEvents.PlayCard, PlayCardPerformance);
        EventManager.Instance.AddListener(GameEvents.Discard, DiscardPerformance);
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveListener(GameEvents.HandsChange, ReCalculationHandsPosition);
        EventManager.Instance.RemoveListener(GameEvents.PlayCard, PlayCardPerformance);
        EventManager.Instance.RemoveListener(GameEvents.Discard, DiscardPerformance);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (CardMove m in battleField.cardObjectPool.Select(c => c.GetComponent<CardMove>()))
        {
            m.GetComponent<RectTransform>().localPosition += (m.targetPosition - m.GetComponent<RectTransform>().localPosition) / damp;
        }
    }

    public void ReCalculationHandsPosition(object s, EventArgs e)
    {
        var args = e as HandsChangeEventArgs;
        // 如果是对手的手牌更新了，走另一个方法
        if (args.currentPlayer != player)
        {
            ReCalcOppHandsPosition(args.currentPlayer);
            return;
        }

        int playerHandsCount = player.Hand.Count;
        if (playerHandsCount == 0)
        {
            return;
        }
        // 如果手牌数量小于4，直线布局
        if (playerHandsCount < 4)
        {
            // 计算第一张牌的X偏移值
            float cardXOffset = (1 - playerHandsCount) * 0.5f * lineLayoutXOffset;
            for (int i = 0; i < playerHandsCount; i++)
            {
                CardMove cardMove = player.Hand[i].GetComponent<CardMove>();
                if (cardMove)
                {
                    cardMove.transform.SetParent(HandLineLayoutCenter.transform);
                    cardMove.targetPosition = new Vector3(cardXOffset, 0, 0);
                    // 还原角度
                    cardMove.transform.localRotation = Quaternion.identity;
                }
                else
                {
                    Debug.LogWarning($"找不到{player.Hand[i]}的CardMove组件！");
                }
                cardXOffset += lineLayoutXOffset;
            }
        }
        // 否则曲线布局
        else
        {
            float midAngle = 90f;
            // 计算牌之间的角度间隔（边界不放牌所以+1不是-1）
            float perAngle = circleLayoutAngle / (playerHandsCount + 1);
            // 计算第一张牌的角度
            float currentAngle = midAngle + (1 - playerHandsCount) * 0.5f * perAngle;

            for (int i = playerHandsCount - 1; i >= 0; i--)
            {
                CardMove cardMove = player.Hand[i].GetComponent<CardMove>();
                if (cardMove)
                {
                    cardMove.transform.SetParent(HandCircleLayoutCenter.transform);
                    cardMove.targetPosition = new Vector3(
                        Mathf.Cos(currentAngle * Mathf.PI / 180) * circleLayoutRadius,
                        Mathf.Sin(currentAngle * Mathf.PI / 180) * circleLayoutRadius,
                        1f
                        );
                    cardMove.gameObject.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0f, 0f, currentAngle - midAngle);
                    // 从左往右生成卡牌，新生成的永远在上方，使得左上角不被左侧卡牌遮挡。
                    cardMove.transform.SetAsFirstSibling();
                }
                else
                {
                    Debug.LogWarning($"找不到{player.Hand[i]}的CardMove组件！");
                }
                currentAngle += perAngle;
            }
        }
    }

    public void ReCalcOppHandsPosition(Player player)
    {
        int playerHandsCount = player.Hand.Count;
        if (playerHandsCount == 0)
        {
            return;
        }
        // 如果手牌数量小于4，直线布局
        if (playerHandsCount < 4)
        {
            // 计算第一张牌的X偏移值
            float cardXOffset = (1 - playerHandsCount) * 0.5f * lineLayoutXOffset;
            for (int i = 0; i < playerHandsCount; i++)
            {
                CardMove cardMove = player.Hand[i].GetComponent<CardMove>();
                if (cardMove)
                {
                    cardMove.transform.SetParent(OppHandLineLayoutCenter.transform);
                    cardMove.targetPosition = new Vector3(cardXOffset, 0, 0);
                    // 还原角度
                    cardMove.transform.localRotation = Quaternion.identity;
                }
                else
                {
                    Debug.LogWarning($"找不到{this.player.Hand[i]}的CardMove组件！");
                }
                cardXOffset += lineLayoutXOffset;
            }
        }
        // 否则曲线布局
        else
        {
            float midAngle = 90f;
            // 计算牌之间的角度间隔（边界不放牌所以+1不是-1）
            float perAngle = circleLayoutAngle / (playerHandsCount + 1);
            // 计算第一张牌的角度
            float currentAngle = midAngle + (1 - playerHandsCount) * 0.5f * perAngle;

            for (int i = playerHandsCount - 1; i >= 0; i--)
            {
                CardMove cardMove = player.Hand[i].GetComponent<CardMove>();
                if (cardMove)
                {
                    cardMove.transform.SetParent(OppHandCircleLayoutCenter.transform);
                    cardMove.targetPosition = new Vector3(
                        Mathf.Cos(currentAngle * Mathf.PI / 180) * circleLayoutRadius,
                        Mathf.Sin(currentAngle * Mathf.PI / 180) * circleLayoutRadius,
                        1f
                        );
                    cardMove.gameObject.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0f, 0f, currentAngle - midAngle);
                    // 从左往右生成卡牌，新生成的永远在上方，使得左上角不被左侧卡牌遮挡。
                    cardMove.transform.SetAsFirstSibling();
                }
                else
                {
                    Debug.LogWarning($"找不到{this.player.Hand[i]}的CardMove组件！");
                }
                currentAngle += perAngle;
            }
        }
    }

    public void PlayCardPerformance(object s, EventArgs e)
    {
        var args = e as PlayCard;
        // 如果出牌的是玩家，则卡牌展示方式不需要变化；否则需要展示，因为对方的手牌/牌组的牌对于我方来说是不可见的
        if (args.player != player)
        {
            args.card.GetComponent<CardShow>().Show();
        }
        CardMove cm = args.card.GetComponent<CardMove>();
        //cm.targetPosition = GetLayoutCenter(args.targetZone).transform.position;
        GameObject node = GetLayoutCenter(args.targetZone);
        //Debug.Log($"根据{args.card.colors.First()}的颜色，分配到{args.targetZone.color}的区：{node.name}");
        cm.ChangeZone(node, Vector3.zero);
    }

    public void DiscardPerformance(object s, EventArgs e)
    {
        var args = e as PlayCard;
        GameObject targetGraveyard = GraveyardLayoutCenter;
        // 如果弃牌的是玩家，则卡牌展示方式不需要变化；否则需要展示，因为对方的手牌/牌组的牌对于我方来说是不可见的
        if (args.player != player)
        {
            args.card.GetComponent<CardShow>().Show();
            // 并且这张牌也会进入对手的弃牌堆而不是你的弃牌堆
            targetGraveyard = OppGraveyardLayoutCenter;
        }
        CardMove cm = args.card.GetComponent<CardMove>();
        cm.ChangeZone(targetGraveyard, Vector3.zero);
    }

    public GameObject GetLayoutCenter(BattleField.Zone targetZone)
    {
        // 判断目标区域的颜色，并返回相应的布局中心
        return targetZone.color switch
        {
            Card.CardColor.无 => RedZoneLayoutCenter,// 这是不可能出现的情况
            Card.CardColor.红 => RedZoneLayoutCenter,
            Card.CardColor.绿 => GreenZoneHandLayoutCenter,
            Card.CardColor.蓝 => BlueZoneDeckLayoutCenter,
            _ => RedZoneLayoutCenter,// 这是不可能出现的情况
        };
    }
}
