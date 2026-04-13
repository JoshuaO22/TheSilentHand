using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;

    private GameManager gameManager = GameManager.Instance;
    private Camera MainCamera;
    private InputAction shootAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MainCamera = Camera.main;
        shootAction = InputSystem.actions.FindAction("Attack");
        shootAction.Enable();
        shootAction.started += ctx => Shoot();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(MainCamera.transform.position, MainCamera.transform.forward, out hit, range))
        {
            Debug.Log("Hit: " + hit.transform.name);
        }
    }
}
