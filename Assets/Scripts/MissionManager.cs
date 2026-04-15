using UnityEngine;
using UnityEngine.Events;

public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance { get; private set; }

    public int EnemiesDefeated { get; private set; }
    public int TotalEnemies { get; private set; }

    public bool IsMissionComplete => EnemiesDefeated >= TotalEnemies;

    public event UnityAction OnMissionComplete;
    public event UnityAction<int, int> OnMissionProgressChanged;

    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void InitializeMission(int totalEnemies)
    {
        TotalEnemies = totalEnemies;
        EnemiesDefeated = 0;
    }

    public void EnemyDefeated()
    {
        EnemiesDefeated++;
        OnMissionProgressChanged?.Invoke(EnemiesDefeated, TotalEnemies);

        CheckMissionStatus();
    }

    private void CheckMissionStatus()
    {
        if (IsMissionComplete)
        {
            Debug.Log("Mission Complete!");
            OnMissionComplete?.Invoke();
        }
    }
}
