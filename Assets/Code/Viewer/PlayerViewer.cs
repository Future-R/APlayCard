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
    public GameObject RedZoneLayoutCenter;
    public GameObject GreenZoneHandLayoutCenter;
    public GameObject BlueZoneDeckLayoutCenter;

    public float lineLayoutXOffset = 20f;
    public float circleLayoutLeftAngle = 105f;
    public float circleLayoutRightAngle = 75f;
    public float circleLayoutRadius = 10f;
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
            float startAngle = Mathf.PI * circleLayoutLeftAngle / 180f;
            float endAngle = Mathf.PI * circleLayoutRightAngle / 180f;

            for (int i = 0; i < playerHandsCount; i++)
            {
                CardMove cardMove = player.Hand[i].GetComponent<CardMove>();
                if (cardMove)
                {
                    cardMove.transform.SetParent(HandCircleLayoutCenter.transform);

                    float angle = Mathf.Lerp(startAngle, endAngle, i / (playerHandsCount - 1));
                    cardMove.targetTransform = new Vector3(Mathf.Cos(angle) * circleLayoutRadius, Mathf.Sin(angle) * circleLayoutRadius - circleLayoutRadius, 1f);
                    cardMove.gameObject.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(15f, -15f, i / (playerHandsCount - 1)));
                }
                else
                {
                    Debug.LogWarning($"找不到{player.Hand[i]}的CardMove组件！");
                }
            }
        }
    }
}
