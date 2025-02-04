using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour,IObserver
{
    public GameObject enemyPrefab;  // Prefab của enemy
    public Transform[] spawnPoints; // Các điểm spawn enemy

    public int maxEnemiesInScene = 5;    // Số lượng enemy tối đa trong scene tại một thời điểm
    public int totalEnemiesToSpawn = 20; // Tổng số enemy có thể spawn trong suốt game
    public float spawnInterval = 3f;     // Khoảng thời gian giữa các lần spawn
    public int TargetNum = 0;
    public float hpEnemy;
    [SerializeField] private int currentEnemyDefeatedCount =0;
    [SerializeField] private int currentEnemyCount = 0;   // Số lượng enemy hiện tại trong scene
    [SerializeField] private int spawnedEnemiesCount = 0; // Tổng số enemy đã spawn từ đầu game
    public bool isGameover;
    void Awake()
    {
        Subject.RegisterObserver(this);
    }
    void OnDestroy()
    {
        Subject.UnregisterObserver(this);
    }
    public void OnNotify(string eventName, object eventData)
    {
        if(eventName == "enemyDie")
        {
            currentEnemyCount --;
            currentEnemyDefeatedCount ++;
        }
    }


    private void Start()
    {
        currentEnemyCount = FindObjectsOfType<Enemy>().Length;
        StartCoroutine(ManageEnemySpawning());
        isGameover = false;
    }
    void Update()
    { 
        if(isGameover) return;
        if(currentEnemyDefeatedCount == TargetNum && !TankController.Instance.isDead)
        {
            isGameover = true;
            StartCoroutine(WinAction());

        }else if(TankController.Instance.isDead && currentEnemyDefeatedCount != TargetNum )
        {
            isGameover = true;
            StartCoroutine(LoseAction());
        }
    }

    public IEnumerator WinAction()
    {
        yield return new WaitForSeconds(0.5f);
        LVManager.Instance.SaveGame();
        UIManager.Instance.OpenUI<CompleteUI>();
        Time.timeScale = 0;
    }
     public IEnumerator LoseAction()
    {
        yield return new WaitForSeconds(0.5f);
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);

    }
    private IEnumerator ManageEnemySpawning()
    {
        while (spawnedEnemiesCount < totalEnemiesToSpawn) // Chỉ spawn nếu còn enemy để spawn
        {
            if (currentEnemyCount < maxEnemiesInScene) // Chỉ spawn nếu chưa đạt giới hạn trong scene
            {
                SpawnEnemy();
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnEnemy()
    {
        if (spawnedEnemiesCount >= totalEnemiesToSpawn)
            return; // Không spawn nếu đã hết enemy dự trữ

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        enemy.GetComponent<Enemy>().Hp = hpEnemy;
        currentEnemyCount++;
        spawnedEnemiesCount++;
    }

    public void EnemyDefeated()
    {
        if (currentEnemyCount > 0)
        {
            currentEnemyCount--;
        }
    }
}
