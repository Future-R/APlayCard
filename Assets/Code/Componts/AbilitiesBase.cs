using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilitiesBase : MonoBehaviour
{
    public string abilitiesName;
    public string description;
    public List<string> tags = new List<string>();

    private void Start()
    {
        Init();
        OnAdd();
    }

    public abstract void Init();

    public abstract void OnAdd();

    public virtual void OnRemove()
    {

    }
}
