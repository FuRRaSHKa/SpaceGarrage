using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class ProblemSpawner : MonoBehaviour
{
    [SerializeField] private int maxActiveCount = 2;
    [SerializeField] private float spawnDelay = 0;
    [SerializeField] private int needToWin = 15;

    [SerializeField] private List<ProblemScript> problems;

    private int currentActive = 0;

    private void Awake()
    {
        EventManager.onProblemFixed += ProblemFixed;
    }

    private void Start()
    {
        problems[0].BrokeIt();
    }

    private void MakeProblem()
    {
        int r = Random.Range(0, problems.Count);
        if (problems[r].IsBroken)
            while (true)
            {
                r++;
                r %= problems.Count;
                if (!problems[r].IsBroken)
                {
                    problems[r].BrokeIt();
                    return;
                }
            }

        problems[r].BrokeIt();
    }

    private void ProblemFixed()
    {
        needToWin--;
        currentActive--;
    }

}
