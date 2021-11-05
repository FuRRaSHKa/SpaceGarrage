using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoundInstaller", menuName = "Installers/Round Installer")]
public class RoundData : ScriptableObject
{
    [Header("Spawner settings")]
    public float maxSpawnDelay;
    public float minSpawnDelay;
    public int maxActiveCount;
    public float roundTime;

    [Header("Polomka settings")]
    public float timeUntilFire;
    public float timeToFix;
    public float timeToPutOutFire;
    public float timeToLoseAfterFire;
}
