using Assets.Scripts.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

    public static LevelManager Instance;

    public Spawner Spawner;

    public List<Enemy> EnemiesL1;
    public List<Enemy> EnemiesL2;
    public List<Enemy> EnemiesL3;

    public Level Level { get; private set; }

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
        spawnWave();
    }

    private void spawnWave()
    {
        if (Level.Waves.Count == 0)
        {
            if (Level.Number == 3)
            {
                Level.Waves.Add(new Wave(
"X11X11X" +
"X11211X"
                    ));
            }
            else
            {
                GameManager.Instance.LevelFinished();
                return;
            }
        }

        Wave wave = Level.Waves[0];
        Level.Waves.RemoveAt(0);

        foreach (WaveEntry entry in wave.GetEntries())
        {
            spawnEntry(entry);
        }

        StartCoroutine(spawnNextWave());
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
        Spawner spawner = Instantiate(Spawner, transform.parent);
        spawner.transform.position = new Vector2(-3 + x, 4 - y);
        spawner.EnemyTemplate = () => enemy;
    }

    private IEnumerator spawnNextWave()
    {
        yield return new WaitForSeconds(12f);
        spawnWave();
    }
}
