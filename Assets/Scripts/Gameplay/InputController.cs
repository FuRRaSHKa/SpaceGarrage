using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField] private LayerMask obstLayer;
    [SerializeField] private FollowObj weChooseIndicator;
    [SerializeField] private string pathChooseSound;
    [SerializeField] private string pathWalkSound;

    private ManController currentMan;
    private Camera cam;

    private bool isPlaying = false;

    private void Start()
    {
        cam = Camera.main;
        EventManager.onRoundEnd += RoundEnd;
        EventManager.onRoundStart += RoundStart;
        RoundStart();
    }

    private void Update()
    {
        if (!isPlaying)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            LeftClick();
        } 
        else if (Input.GetMouseButtonDown(1))
        {
            RightClick();
        }

    }

    private void TakeBoy(ManController boy)
    {
        currentMan = boy;
        if (currentMan != null)
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/ui_select");
            weChooseIndicator.gameObject.SetActive(true);
            weChooseIndicator.SetTarger(currentMan.transform);
            return;
        }

        weChooseIndicator.gameObject.SetActive(false);
    }

    private void LeftClick()
    {
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Collider2D collusion = Physics2D.OverlapCircle(mousePos, .1f, ~obstLayer.value);

        if (collusion == null)
        {
            TakeBoy(null);
            return;
        }

        if (collusion.CompareTag("Player"))
        {
            ManController newMan = collusion.GetComponent<ManController>();
            if (newMan != null)
                TakeBoy(newMan);

            return;
        }
    }

    private void RightClick()
    {
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Collider2D collusion = Physics2D.OverlapCircle(mousePos, .1f, ~obstLayer.value);

        if (collusion == null)
        {
            if (currentMan != null)
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/ui_approve");
                currentMan.MoveTo(mousePos);
            }
            return;
        }

        switch (collusion.tag)
        {             

            case "Instrument":
                BoxWithInstrument box = collusion.GetComponent<BoxWithInstrument>();
                if (box != null && currentMan != null)
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/ui_approve");
                    currentMan.MoveToInstrument(box);
                }
                   

                return;

            case "Problem":
                ProblemScript problem = collusion.GetComponent<ProblemScript>();
                if (problem != null && currentMan != null)
                {
                    FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/ui_approve");
                    currentMan.MoveToProblem(problem);
                }
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
