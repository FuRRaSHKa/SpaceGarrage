using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Instrument
{
    None,
    Hummer,
    Wrench,
    Flamethrower,
    Extinguisher, 
    Tape
}

public class ManController : MonoBehaviour
{

    [SerializeField] private Transform hand;

    private MansMovement mansMovement;
    private Instrument currentInstrument = Instrument.None;

    private bool isFixing = false;

    private void Start()
    {
        mansMovement = GetComponent<MansMovement>();
    }

    public void MoveTo(Vector2 pos)
    {
        if (isFixing)
            return;

        mansMovement.MoveTo(pos);
    }

    public void MoveToInstrument(PickableObject pickableObject)
    {
        if (isFixing)
            return;

        if (pickableObject.isIntrumentPicked)
        {
            if (currentInstrument == pickableObject.InstrumentType)
            {
                mansMovement.MoveTo(pickableObject.Position, () => 
                {
                    currentInstrument = Instrument.None;
                    pickableObject.PlaceInstrument();
                });
            }

            return;
        }

        mansMovement.MoveTo(pickableObject.Position, () =>
        {
            currentInstrument = pickableObject.InstrumentType;
            pickableObject.PickUp(hand);
        });
    }

    public void MoveToProblem(ProblemScript problem)
    {
        if (isFixing)
            return;

        mansMovement.MoveTo(problem.PosForFixing, () =>
        {
            isFixing = true;
            problem.FixIt(currentInstrument, () => isFixing = false);
        });
    }

}
