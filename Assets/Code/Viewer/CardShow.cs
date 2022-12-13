using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Card))]
public class CardShow : MonoBehaviour
{
    public GameObject WhiteEdge;
    public GameObject CardBack;
    public GameObject BlackShadow;
    public GameObject R;
    public GameObject G;
    public GameObject B;
    public GameObject PointForward;
    public GameObject PointBackward;

    private Card card;

    public ShowStatus showStatus = ShowStatus.隐藏;

    public enum ShowStatus
    {
        公开可见,
        仅自己可见,
        不可见,
        隐藏
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Hide()
    {
        new List<GameObject> { WhiteEdge, CardBack, BlackShadow, R, G, B, PointForward, PointBackward }
        .ForEach(obj => obj.SetActive(false));
    }

    public void TurnCover()
    {
        Hide();
        CardBack.SetActive(true);
        BlackShadow.SetActive(true);
    }

    public void Show()
    {
        card = GetComponent<Card>();
        Hide();

        foreach (var item in new List<GameObject> { WhiteEdge, BlackShadow, PointForward, PointBackward })
        {
            item.SetActive(true);
        }

        PointForward.GetComponent<Text>().text = card.points.ToString();
        PointBackward.GetComponent<Text>().text = card.points.ToString();

        switch (card.colors.FirstOrDefault())
        {
            case Card.CardColor.红:
                R.SetActive(true);
                break;
            case Card.CardColor.绿:
                G.SetActive(true);
                break;
            case Card.CardColor.蓝:
                B.SetActive(true);
                break;
            default:
                break;
        }
    }
}
