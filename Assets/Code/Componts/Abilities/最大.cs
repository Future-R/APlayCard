using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 最大 : AbilitiesBase
{
    public override void Init()
    {
        abilitiesName = GetType().Name;
        description = "这张牌的点数无穷大。";
        tags.Add("点数变化");
    }

    public override void OnAdd()
    {
        throw new System.NotImplementedException();
    }
}
