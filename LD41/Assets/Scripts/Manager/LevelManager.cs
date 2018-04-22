using Assets.Scripts.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    public static LevelManager Instance;

    public const float DelayStart = 2f;
    public const float DelayBetween = 12f;

    public int EnemyCount = 0;

    public Spawner Spawner;

    public List<Enemy> EnemiesL1;
    public List<Enemy> EnemiesL2;
    public List<Enemy> EnemiesL3;

    public Level Level { get; private set; }

    private bool _waitingToSpawn = false;
    private bool _spawnImmediately = false;

    void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartLevel(int level)
    {
        Level = Levels.GetLevel(level);

        Enemy.Speed = Level.EnemySpeed;

        StartCoroutine(spawnNextWave(DelayStart));
    }

    public void EnemyDefeated()
    {
        EnemyCount--;

        if (EnemyCount <= 0)
        {
            if (Level.Waves.Count == 0)
                GameManager.Instance.LevelFinished();
            else if (_waitingToSpawn)
                _spawnImmediately = true;

        }
    }

    private void spawnWave()
    {
        if (Level.Waves.Count == 0)
        {
            if (Level.Number == 3 && EnemyCount > 0)
            {
                Level.Waves.Add(new Wave(
"X11X11X" +
"X11211X"
                    ));
            }
            else
            {
                return;
            }
        }

        Wave wave = Level.Waves[0];
        Level.Waves.RemoveAt(0);

        foreach (WaveEntry entry in wave.GetEntries())
        {
            spawnEntry(entry);
        }

        StartCoroutine(spawnNextWave(DelayBetween));
    }
    private void spawnEntry(WaveEntry entry)
    {
        List<Enemy> availableEnemies = null;
        switch (entry.EnemyLevel)
        {
            case 1:
                availableEnemies = EnemiesL1;
                break;
            case 2:
                availableEnemies = EnemiesL2;
                break;
            case 3:
                availableEnemies = EnemiesL3;
                break;
        }

        spawn(availableEnemies[Random.Range(0, availableEnemies.Count)], entry.X, entry.Y);
    }    
    private void spawn(Enemy enemy,int x,int y)
    {
        EnemyCount++;

        Spawner spawner = Instantiate(Spawner, transform.parent);
        spawner.transform.position = new Vector2(-3 + x, 4 - y);
        spawner.EnemyTemplate = () => enemy;
    }

    private IEnumerator spawnNextWave(float delay)
    {
        _waitingToSpawn = true;

        float passed = 0f;
        while (passed < delay && !_spawnImmediately)
        {
            passed += Time.deltaTime;
            yield return null;
        }

        _waitingToSpawn = false;
        _spawnImmediately = false;
        spawnWave();
    }
}
