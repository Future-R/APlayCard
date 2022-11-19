using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ZonePosition")]
public class ZonePosition : ScriptableObject
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
}
