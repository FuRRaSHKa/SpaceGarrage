using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class ProblemSpawner : MonoBehaviour
{
    [SerializeField] private List<ProblemScript> problems;
    [SerializeField] private List<RoundData> roundDatas;

    private int maxActiveCount = 2;
    private float maxSpawnDelay = 4;
    private float minSpawnDelay = 4;
    private int currentRound = 0;
    private float roundTime;

    private int currentActive = 0;
    private IDisposable spawner;
    private IDisposable winTimer;

    private void Awake()
    {
        EventManager.onProblemFixed += ProblemFixed;
        EventManager.onRoundEnd += OnRoundEnd;
    }

    private void InitData()
    {
        maxActiveCount = roundDatas[currentRound].maxActiveCount;
        maxSpawnDelay = roundDatas[currentRound].maxSpawnDelay;
        minSpawnDelay = roundDatas[currentRound].minSpawnDelay;
        roundTime = roundDatas[currentRound].roundTime;
        currentActive = 0;

        for (int i = 0; i < problems.Count; i++)
        {
            problems[i].Init(roundDatas[currentRound]);
        }
    }

    private void Start()
    {
        RoundStart();

        spawner = Observable.Interval(TimeSpan.FromSeconds(UnityEngine.Random.Range(minSpawnDelay, maxSpawnDelay)))
            .TakeUntilDisable(gameObject)
            .Subscribe(_ =>
            {
                if (currentActive < maxActiveCount)
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
        currentActive--;
    }

    private void OnRoundEnd(bool isWin)
    {
        spawner?.Dispose();
    }

    private void RoundStart()
    {
        InitData();

        winTimer?.Dispose();
        winTimer = Observable.Timer(TimeSpan.FromSeconds(roundTime))
            .TakeUntilDisable(gameObject)
            .Subscribe( _ => 
            {
                RoundWin();
            });
    }

    private void RoundWin()
    {
        EventManager.EndRound(true);
    }
}
