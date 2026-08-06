using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawnManager : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {

        [System.Serializable]
        public class EnemyInfo
        {
            public GameObject EnemyPref;
            public int number;
            public GameObject Path;
            public GameObject positon_spawn;
        }

        public EnemyInfo[] enemyType;


        public Image[] iconSkull;
    }
    public Wave[] waves;
    [Header("Time After Spawn 1 Enemy")]
    public float time_to_spawn_1_enemy;
    //   [Header("Time After Spawn 1 Wave")]
    //  public float time_between_2_wave;
    [Header("Time After Spawn 1 Type Enemy")]
    public float time_to_different_Type_Enemy;

    public float countToNextWave;
    public bool firstTime, finishrespawn;
    public float _time { get; set; }
    private int CurrentWave, MaxWave;
    public static WaveSpawnManager _instance;
    public Text _waveText;

    private List<GameObject> TotalEnemy = new List<GameObject>();
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    void Start()
    {
        _waveText = GameObject.Find("WaveText").GetComponent<Text>();
        Manager.isFinishing = false;
        MaxWave = waves.Length;
        CurrentWave = 0;
        firstTime = true;
        finishrespawn = true;
        foreach (Image _image in waves[CurrentWave].iconSkull)
        {
            _image.gameObject.SetActive(true);
            _image.fillAmount = 1;
        }
        _waveText.text = "Wave " + CurrentWave.ToString() + "/" + MaxWave.ToString();
    }
    private void RemoveNull()
    {
        for (int i = 0; i < TotalEnemy.Count; i++)
        {
            if (TotalEnemy[i] == null)
            {
                TotalEnemy.RemoveAt(i);
            }
        }
    }
    void Update()
    {
        if (CurrentWave == MaxWave)
        {
            RemoveNull();
        }
        if (firstTime == true || finishrespawn == false || (CurrentWave >= MaxWave)) return;
        else
        {
            _time += Time.deltaTime;
            if (CurrentWave < MaxWave)
            {
                foreach (Image _image in waves[CurrentWave].iconSkull)
                {
                    if (_image.gameObject.activeSelf == false)
                    {
                        _image.gameObject.SetActive(true);
                        _image.gameObject.GetComponent<Circle_Spawn>().Active_Direction();
                    }
                    _image.fillAmount = _time / countToNextWave;
                }
            }
            if (_time >= countToNextWave)
            {
                _time = 0;
                finishrespawn = false;
                if (CurrentWave < MaxWave)
                {
                    foreach (Image _image in waves[CurrentWave].iconSkull)
                    {
                        _image.gameObject.GetComponent<Circle_Spawn>().Disactive_Direction();
                        _image.gameObject.SetActive(false);
                    }
                }
                StartCoroutine(Spawn());
            }
        }
    }
    // gia su maxwave =2
    private IEnumerator Spawn()
    {
        if (CurrentWave > MaxWave)
        { yield break; }


        CurrentWave++;
        yield return new WaitForSeconds(0.2f);
        _waveText.text = "Wave " + CurrentWave.ToString() + "/" + MaxWave.ToString();
        int numberofEnemy = waves[CurrentWave - 1].enemyType.Length;
        //   print("So loai enemy la=" + numberofEnemy);
        for (int i = 0; i < numberofEnemy; i++)
        {
            //   print("CurrentWave=" + CurrentWave);
            int number = waves[CurrentWave - 1].enemyType[i].number;
            //   print("Number=" + number);
            for (int j = 0; j < number; j++)
            {
                Instance_Enemy(waves[CurrentWave - 1].enemyType[i].EnemyPref, waves[CurrentWave - 1].enemyType[i].Path, waves[CurrentWave - 1].enemyType[i].positon_spawn);
                yield return new WaitForSeconds(time_to_spawn_1_enemy);
            }
            yield return new WaitForSeconds(time_to_different_Type_Enemy);
        }

        // yield return new WaitForSeconds(time_between_2_wave);
        finishrespawn = true;
    }

    private void Instance_Enemy(GameObject enemyPref, GameObject path, GameObject position)
    {
        GameObject enemy = Instantiate(enemyPref, position.transform.position, Quaternion.identity);
        if (enemy.GetComponent<EnemyController>() != null)
        {
            enemy.GetComponent<EnemyController>().Path = path;
        }
        else if (enemy.GetComponent<FlyController>() != null)
        {
            enemy.GetComponent<FlyController>().Path = path;
        }
        if (CurrentWave == MaxWave)
        {
            if (!TotalEnemy.Contains(enemy))
            {
                TotalEnemy.Add(enemy);
            }
            InvokeRepeating("CheckEnemyToWin", 0, 1f);
            //  CheckEnemyToWin();
        }
    }
    public void SpawnNow()
    {
        _time = 0;
        firstTime = false;
        finishrespawn = false;
        foreach (Image _image in waves[CurrentWave].iconSkull)
        {
            _image.gameObject.GetComponent<Circle_Spawn>().Disactive_Direction();
            _image.gameObject.SetActive(false);

        }
        StartCoroutine(Spawn());
    }
    private void CheckEnemyToWin()
    {
        if (TotalEnemy.Count == 0)
        {
            Invoke("WinPanel", 2f);
        }
    }
    private void WinPanel()
    {
        Manager._instance.WinGame();
    }
}
