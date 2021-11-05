using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class ProblemSpawner : MonoBehaviour
{
    [SerializeField] private int maxActiveCount = 2;
    [SerializeField] private float maxSpawnDelay = 4;
    [SerializeField] private float minSpawnDelay = 4;
    [SerializeField] private int needToWin = 15;

    [SerializeField] private List<ProblemScript> problems;

    private int currentActive = 0;
    private IDisposable spawner;

    private void Awake()
    {
        EventManager.onProblemFixed += ProblemFixed;
    }

    private void Start()
    {
        spawner = Observable.Interval(TimeSpan.FromSeconds(UnityEngine.Random.Range(minSpawnDelay, maxSpawnDelay)))
            .TakeUntilDisable(gameObject)
            .Subscribe(_ => 
            {
                if(currentActive < maxActiveCount)
                    MakeProblem();
            });
    }

    private void MakeProblem()
    {
        int r = UnityEngine.Random.Range(0, problems.Count);
        if (problems[r].IsBroken)
            while (true)
            {
                r++;
                r %= problems.Count;
                if (!problems[r].IsBroken)
                {
                    currentActive++;
                    problems[r].BrokeIt();
                    return;
                }
            }
        currentActive++;
        problems[r].BrokeIt();
    }

    private void ProblemFixed()
    {
        needToWin--;
        currentActive--;
    }

}
