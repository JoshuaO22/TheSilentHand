using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleporter : MonoBehaviour
{
    private enum TeleporterAction
    {
        TeleportToObject,
        ChangeScene
    }

    public static event Action<string> OnObjectiveRequested;

    [SerializeField] private TeleporterAction action = TeleporterAction.TeleportToObject;

    [SerializeField] private Transform spawnPoint;

    [SerializeField] private string sceneName;

    [SerializeField] private string objectiveText = "Kill everybody.";

    private bool isTeleporting = false;

    private void OnTriggerEnter(Collider other)
    {
        print("Player entered the teleporter trigger.");
        if (!isTeleporting && other.CompareTag("Player"))
        {
            isTeleporting = true;

            CharacterController characterController = other.GetComponent<CharacterController>();
            PlayerStats playerStats = other.GetComponentInParent<PlayerStats>();
            try
            {
                if (characterController != null)
                {
                    characterController.enabled = false;
                }

                switch (action)
                {
                    case TeleporterAction.TeleportToObject:
                        if (spawnPoint == null)
                        {
                            Debug.LogWarning("no teleport to object spawnpoint set");
                            return;
                        }

                        other.transform.SetPositionAndRotation(spawnPoint.position + Vector3.up * 1f, spawnPoint.rotation);
                        if (playerStats != null)
                        {
                            // playerStats.TakeDamage(67f);
                        }
                        break;

                    case TeleporterAction.ChangeScene:
                        if (string.IsNullOrWhiteSpace(sceneName))
                        {
                            Debug.LogWarning("scene name is empty, cannot change scene");
                            return;
                        }

                        if (GameManager.Instance != null)
                        {
                            GameManager.Instance.LoadScene(sceneName);
                        }
                        else
                        {
                            SceneManager.LoadScene(sceneName);
                        }
                        break;
                }

                OnObjectiveRequested?.Invoke(objectiveText); // TODO: refactor to let mission manager handle this
            }
            finally
            {
                if (characterController != null)
                {
                    characterController.enabled = true;
                }

                isTeleporting = false;
            }
        }
    }
}
