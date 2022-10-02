using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class Controller : MonoBehaviour
{
    private Player player;
    private List<string> ActionObstacles = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        ActionObstacles.Clear();
        EventManager.Instance.AddListener(GameEvents.ContorllerCoolDown, BanControl);
        //this.TriggerEvent(GameEvents.ContorllerCoolDown, new ContorllerCoolDownEventArgs { currentPlayer = player, reason = Tips.WarningTips.现在不是你的回合.ToString() });
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveListener(GameEvents.ContorllerCoolDown, BanControl);
    }

    // Update is called once per frame
    void Update()
    {
        if (ActionObstacles.Count == 0)
        {
            if (Input.touchCount > 0 || Input.GetMouseButtonDown(0))
            {
                this.TriggerEvent(GameEvents.PlayerTouch, new PlayerTouchEventArgs { activePlayer = player });
            }
        }
    }

    public void LimiterRemoval(string reason)
    {
        ActionObstacles.RemoveAll(s => s == reason);
    }

    public void BanControl(object sender, EventArgs e)
    {
        var data = e as ContorllerCoolDownEventArgs;
        if (player == data.currentPlayer)
        {
            ActionObstacles.Add(data.reason);
        }
    }
}
