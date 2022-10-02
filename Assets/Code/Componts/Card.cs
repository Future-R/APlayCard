using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{

    public List<CardColor> colors;
    public int points;

    public enum CardColor
    {
        无,
        红,
        绿,
        蓝
    }

    public Player owner;

    public void Init(List<CardColor> SetCardColors,byte setPoints)
    {
        colors = SetCardColors;
        points = setPoints;
    }
    public void Init(CardColor SetCardColor, byte setPoints)
    {
        colors = new List<CardColor>
        {
            SetCardColor
        };
        points = setPoints;
    }
}
