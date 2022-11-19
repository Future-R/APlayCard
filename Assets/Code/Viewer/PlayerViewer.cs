using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerViewer : MonoBehaviour
{
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
    public float damp = 20f;
    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        EventManager.Instance.AddListener(GameEvents.HandsChange, ReCalculationHandsPosition);
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveListener(GameEvents.HandsChange, ReCalculationHandsPosition);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (CardMove m in player.Hand.Select(c => c.GetComponent<CardMove>()))
        {
            m.GetComponent<RectTransform>().localPosition += (m.targetTransform - m.GetComponent<RectTransform>().localPosition) / damp;
        }
    }

    public void ReCalculationHandsPosition(object s, EventArgs e)
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
                    cardMove.transform.SetParent(HandLineLayoutCenter.transform);
                    cardMove.targetTransform = new Vector3(cardXOffset, 0, 0);
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

            for (int i = 0; i < playerHandsCount; i++)
            {
                CardMove cardMove = player.Hand[i].GetComponent<CardMove>();
                if (cardMove)
                {
                    cardMove.transform.SetParent(HandCircleLayoutCenter.transform);
                    cardMove.targetTransform = new Vector3(
                        Mathf.Cos(currentAngle * Mathf.PI / 180) * circleLayoutRadius,
                        Mathf.Sin(currentAngle * Mathf.PI / 180) * circleLayoutRadius,
                        1f
                        );
                    cardMove.gameObject.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0f, 0f, currentAngle - midAngle);
                }
                else
                {
                    Debug.LogWarning($"找不到{player.Hand[i]}的CardMove组件！");
                }
                currentAngle += perAngle;
            }
        }
    }
}
