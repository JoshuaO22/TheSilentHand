using System;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public static event Action<string> OnObjectiveRequested;

    [SerializeField] private Transform spawnPoint;
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

                other.transform.SetPositionAndRotation(spawnPoint.position + Vector3.up * 1f, spawnPoint.rotation);
                OnObjectiveRequested?.Invoke(objectiveText); // TODO: refactor to let mission manager handle this
                if (playerStats != null)
                {
                    playerStats.TakeDamage(67f);
                }
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
