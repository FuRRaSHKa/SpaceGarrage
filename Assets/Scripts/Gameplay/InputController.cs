using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private ManController currentMan;
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckClickPlace();
        }
    }

    private void CheckClickPlace()
    {
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Collider2D collusion = Physics2D.OverlapCircle(mousePos, .1f);

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
                    currentMan = currentMan == newMan ? null : newMan;
                
                return;

            case "Instrument":
                PickableObject pickable = collusion.GetComponent<PickableObject>();
                if (pickable != null && currentMan != null)
                    currentMan.MoveToInstrument(pickable);

                return;

            case "Problem":
                ProblemScript problem = collusion.GetComponent<ProblemScript>();
                if (problem != null && currentMan != null)
                    currentMan.MoveToProblem(problem);

                return;
        }


    }

}
