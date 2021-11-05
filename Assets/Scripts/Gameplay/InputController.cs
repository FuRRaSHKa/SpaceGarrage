using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField] private LayerMask obstLayer;
    [SerializeField] private FollowObj weChooseIndicator;

    private ManController currentMan;
    private Camera cam;

    private bool isPlaying = false;

    private void Start()
    {
        cam = Camera.main;
        EventManager.onRoundEnd += RoundEnd;
        RoundStart();
    }

    private void Update()
    {
        if (!isPlaying)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            CheckClickPlace();
        }
    }

    private void TakeBoy(ManController boy)
    {
        currentMan = currentMan == boy ? null : boy;
        if (currentMan != null)
        {
            weChooseIndicator.gameObject.SetActive(true);
            weChooseIndicator.SetTarger(currentMan.transform);
            return;
        }

        weChooseIndicator.gameObject.SetActive(false);
    }

    private void CheckClickPlace()
    {
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Collider2D collusion = Physics2D.OverlapCircle(mousePos, .1f, ~obstLayer.value);

        if (collusion == null)
        {
            if (currentMan != null)
                currentMan.MoveTo(mousePos);

            return;
        }

        switch (collusion.tag)
        {
            case "Player":
                ManController newMan = collusion.GetComponent<ManController>();
                if (newMan != null)
                    TakeBoy(newMan);

                return;

            case "Instrument":
                BoxWithInstrument box = collusion.GetComponent<BoxWithInstrument>();
                if (box != null && currentMan != null)
                    currentMan.MoveToInstrument(box);

                return;

            case "Problem":
                ProblemScript problem = collusion.GetComponent<ProblemScript>();
                if (problem != null && currentMan != null)
                    currentMan.MoveToProblem(problem);

                return;
        }


    }

    private void RoundStart()
    {
        isPlaying = true;
    }

    private void RoundEnd(bool isWin)
    {
        isPlaying = false;
    }

}
