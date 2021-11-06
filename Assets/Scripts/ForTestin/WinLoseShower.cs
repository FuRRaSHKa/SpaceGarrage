using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WinLoseShower : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    private void Start()
    {
        EventManager.onRoundEnd += ShowResult;
    }

    private void ShowResult(bool isWin)
    {

        if (isWin)
        {
            text.text = "Win";
            text.color = Color.green;
            return;
        }

        text.text = "Lose";
        text.color = Color.red;
        return;
    }
}
