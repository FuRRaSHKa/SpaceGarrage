using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class ProblemSpawner : MonoBehaviour
{
    [SerializeField] private List<ProblemScript> problems;
    [SerializeField] private List<RoundData> roundDatas;
    [SerializeField] private CutSceneManager cutSceneManager;
    [SerializeField] private UITimer uITimer;
    [SerializeField] private SecondCutScene second;
    [SerializeField] private ThirdCutScene third;
    [SerializeField] private FourthCutScene fourthCut;

    private FMOD.Studio.EventInstance musicInstance;
    private FMOD.Studio.EventInstance ambienceInstance;

    private int maxActiveCount = 2;
    private float maxSpawnDelay = 4;
    private float minSpawnDelay = 4;
    private int currentRound = 0;
    private float roundTime;

    private int currentActive = 0;
    private IDisposable spawner;

    bool isEnded = false;

    private void InitData()
    {
        maxActiveCount = roundDatas[currentRound].maxActiveCount;
        maxSpawnDelay = roundDatas[currentRound].maxSpawnDelay;
        minSpawnDelay = roundDatas[currentRound].minSpawnDelay;
        roundTime = roundDatas[currentRound].roundTime;
        currentActive = 0;
        isEnded = false;

        for (int i = 0; i < problems.Count; i++)
        {
            problems[i].Init(roundDatas[currentRound]);
        }

        StartSpawner();
    }

    private void Start()
    {
        EventManager.onProblemFixed += ProblemFixed;
        EventManager.onRoundEnd += OnRoundEnd;
        EventManager.onRoundStart += RoundStart;

        musicInstance = FMODUnity.RuntimeManager.CreateInstance("event:/Music/main_music");
        ambienceInstance = FMODUnity.RuntimeManager.CreateInstance("event:/Ambience/main_ambience");

        musicInstance.start();
        ambienceInstance.start();
    }

    private void StartSpawner()
    {
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
        if (!problems[r].EnableToSpawn)
        {
            int t = r;
            while (true)
            {
                t++;
                t %= problems.Count;
                if (problems[t].EnableToSpawn)
                {
                    currentActive++;
                    problems[t].BrokeIt();
                    return;
                }

                if (t == r)
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
        if (isEnded)
            return;

        isEnded = true;
        uITimer.StopTimer();

        if (isWin)
            if(currentRound < roundDatas.Count)
                switch (currentRound) 
                {
                    case 1:
                        second.StarScene();
                        break;
                    case 2:
                        third.StarScene();
                        break;
                } 
            else
                fourthCut.StartScene();
        else

        musicInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        musicInstance.release();

        ambienceInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        ambienceInstance.release();
        cutSceneManager.StartLose();

        spawner?.Dispose();
    }

    private void RoundStart()
    {
        InitData();
        isEnded = false;

        uITimer.PlayTimer(roundTime, RoundWin);
    }

    private void RoundWin()
    {
        currentRound++;
        EventManager.EndRound(true);
    }
}
