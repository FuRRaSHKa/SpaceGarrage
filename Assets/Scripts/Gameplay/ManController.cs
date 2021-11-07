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
    [SerializeField] private SpriteRenderer bodyRenderer;
    [SerializeField] private Animator animator;

    private MansMovement mansMovement;
    private Instrument currentInstrument = Instrument.None;

    private bool isFixing = false;

    private void Start()
    {
        mansMovement = GetComponent<MansMovement>();
        EventManager.onRoundEnd += RoundEnd;
    }

    public void RoundEnd(bool isWin)
    {
        isFixing = false;
        currentInstrument = Instrument.None;
        bodyRenderer.enabled = false;
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
            if (!bodyRenderer.enabled)
                bodyRenderer.enabled = true;

            currentInstrument = box.InstrumentType;
            bodyRenderer.sprite = box.PickUp();
        });
    }

    public void MoveToProblem(ProblemScript problem)
    {
        if (isFixing)
            return;

        mansMovement.MoveTo(problem.PosForFixing, () =>
        {
            isFixing = problem.FixIt(currentInstrument, () => isFixing = false);

            if (isFixing)
            {
                animator.SetTrigger("Fixing");
                 bodyRenderer.enabled = false;
            }
               
        });
    }

}
