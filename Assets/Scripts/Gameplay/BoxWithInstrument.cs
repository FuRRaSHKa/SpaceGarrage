using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxWithInstrument : MonoBehaviour
{
    [SerializeField] private Instrument instrumentType;
    [SerializeField] private Sprite instrumentSprite;
    [SerializeField] private Transform instrumerntPlace;
    [SerializeField] private Transform placeForPickUp;

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

    public Sprite PickUp()
    {
        return instrumentSprite;
    }

    public void PlaceInstrument(GameObject instrument)
    {
        Destroy(instrument);
    }
}
