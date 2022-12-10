using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Card))]
public class CardMove : MonoBehaviour
{
    public Vector3 targetPosition;

    public void ChangeZone(GameObject newParent, Vector3 targetVector)
    {
        gameObject.transform.SetParent(newParent.transform);
        targetPosition = targetVector;
        transform.localScale = new Vector3(0.5f, 0.5f, 1f);
        transform.localPosition = targetVector;
        transform.localRotation = Quaternion.identity;
    }
}
