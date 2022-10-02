using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Card))]
public class CardMove : MonoBehaviour
{
    public Vector3 targetTransform;

    public void ChangeZone(GameObject newParent,Vector3 targetVector)
    {
        gameObject.transform.SetParent(newParent.transform);
        targetTransform = targetVector;
    }
}
