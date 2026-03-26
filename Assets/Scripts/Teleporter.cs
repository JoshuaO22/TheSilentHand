using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    private bool isTeleporting = false;

    private void OnTriggerEnter(Collider other)
    {
        print("Player entered the teleporter trigger.");
        if (!isTeleporting && other.CompareTag("Player"))
        {
            isTeleporting = true;

            CharacterController characterController = other.GetComponent<CharacterController>();
            if (characterController != null)
                characterController.enabled = false;
            
            other.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);

            if (characterController != null)
                characterController.enabled = true;
            
            isTeleporting = false;
        }
    }
}
