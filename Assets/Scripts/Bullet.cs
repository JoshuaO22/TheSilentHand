using System.Collections.Generic;
using UnityEngine;

// From: https://www.youtube.com/watch?v=0jGL5_DFIo8
public class Bullet : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject explosion;
    public LayerMask whatIsEnemies;

    [Range(0f, 1f)]
    public float bounciness;
    public bool useGravity;

    public int explosionDamage;
    public float explosionRange;
    public float explosionForce;

    public int maxCollisions;
    public float maxLifetime;
    public bool explodeOnTouch = true;

    int collisions;
    PhysicsMaterial physics_mat;
    private bool exploding = false;

#if UNITY_EDITOR
    LineRenderer lineRenderer;
    private List<Vector3> positions = new List<Vector3>();
#endif

    private void Start()
    {
        // Create a new Physic material
        physics_mat = new PhysicsMaterial
        {
            bounciness = bounciness,
            frictionCombine = PhysicsMaterialCombine.Minimum,
            bounceCombine = PhysicsMaterialCombine.Maximum
        };
        // Assign material to collider
        GetComponentInChildren<SphereCollider>().material = physics_mat;

        //Set gravity
        rb.useGravity = useGravity;

#if UNITY_EDITOR
        // Add a LineRenderer component
        lineRenderer = gameObject.AddComponent<LineRenderer>();

        // Set the material
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));

        // Set the color
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.green;

        // Set the width
        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.2f;
#endif
    }

    // Make sure to only put code here that could be effected by physics
    private void FixedUpdate()
    {
#if UNITY_EDITOR
        // Store the current position
        positions.Add(transform.position);

        // Update the LineRenderer with the new positions
        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());
#endif

        // When to explode:
        if (collisions > maxCollisions) Explode();

        // Count down lifetime
        maxLifetime -= Time.deltaTime;
        if (maxLifetime <= 0) Explode();
    }

    private void Explode()
    {
        if (exploding == true) return;
        exploding = true;

        // Instantiate explosion
        if (explosion != null)
        {
            // Debug.Log("Bullet: Exploding at position " + transform.position);
            GameObject expInstance = Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(expInstance, 3f); // TODO: refactor later
        }

        // Check for enemies 
        Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRange, whatIsEnemies);
        for (int i = 0; i < enemies.Length; i++)
        {
            // Get component of enemy and call Take Damage
            Debug.Log($"Bullet: Exploding and damaging enemy {enemies[i].name} at distance {Vector3.Distance(transform.position, enemies[i].transform.position):F2} units.");
            enemies[i].GetComponentInParent<Enemy>().TakeDamage(explosionDamage);

            // Add explosion force (if enemy has a rigidbody)
            if (enemies[i].GetComponent<Rigidbody>())
                enemies[i].GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRange);
        }

        // Add a little delay, just to make sure everything works fine
        Invoke(nameof(Delay), 0.05f);
    }
    private void Delay()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Bullet: Collided with {collision.collider.name}. Collision count: {collisions + 1}/{maxCollisions}.");
        //Don't count collisions with other bullets and player
        if (collision.collider.CompareTag("Bullet") || collision.collider.CompareTag("Player")) return;

        //Count up collisions
        collisions++;

        //Explode if bullet hits an enemy directly and explodeOnTouch is activated
        if (collision.collider.CompareTag("Enemy") && explodeOnTouch) Explode();
    }

    /// Visualize the explosion range
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }
}
