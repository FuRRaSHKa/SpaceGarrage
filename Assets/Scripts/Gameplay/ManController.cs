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
    private GameObject instrument;

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

    public void MoveToInstrument(BoxWithInstrument box)
    {
        if (isFixing)
            return;

        mansMovement.MoveTo(box.Position, () =>
        {
            if (instrument != null)
                Destroy(instrument);
            currentInstrument = box.InstrumentType;
            instrument = box.PickUp(hand);
        });

        return;
    }

    public void MoveToProblem(ProblemScript problem)
    {
        if (isFixing)
            return;

        mansMovement.MoveTo(problem.PosForFixing, () =>
        {
            isFixing = problem.FixIt(currentInstrument, () => isFixing = false);

            if (instrument != null && isFixing)
                Destroy(instrument);
        });
    }

}
