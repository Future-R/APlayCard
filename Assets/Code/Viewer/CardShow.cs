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

    public ShowStatus showStatus = ShowStatus.����;

    public enum ShowStatus
    {
        �����ɼ�,
        ���Լ��ɼ�,
        ���ɼ�,
        ����
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Hide()
    {
        foreach (var item in new List<GameObject> { WhiteEdge, CardBack, BlackShadow, R, G, B, PointForward, PointBackward })
        {
            item.SetActive(false);
        }
    }

    public void TurnCover()
    {
        Hide();
        CardBack.SetActive(true);
    }

    public void Draw()
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
            case Card.CardColor.��:
                R.SetActive(true);
                break;
            case Card.CardColor.��:
                G.SetActive(true);
                break;
            case Card.CardColor.��:
                B.SetActive(true);
                break;
            default:
                break;
        }
    }
}
