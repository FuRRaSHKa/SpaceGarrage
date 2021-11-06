using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : MonoBehaviour
{
    [SerializeField] private Instrument instrumentType;
    [SerializeField] private Transform instrument;
    [SerializeField] private Transform instrumerntPlace;
    [SerializeField] private Transform placeForPickUp;

    public Transform Instrument
    {
        get
        {
            return instrument;
        }
    }

    public Instrument InstrumentType
    {
        get
        {
            return instrumentType;
        }
    }

    public Vector3 Position
    {
        get
        {
            return placeForPickUp.position;
        }
    }

    public Vector3 PositionForIstrument
    {
        get
        {
            return instrumerntPlace.position;
        }
    }

    public void PickUp(Transform hand)
    {
        instrument.SetParent(hand);
        instrument.localPosition = Vector3.zero;
    }

    public void PlaceInstrument()
    {
        instrument.SetParent(instrumerntPlace);
        instrument.localPosition = Vector3.zero;
    }

}
