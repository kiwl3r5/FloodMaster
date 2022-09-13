using Script.Manager;
using Script.Player;
using UnityEngine;


public class CustomBullet : MonoBehaviour
{
    //Assignable
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject explosion;
    [SerializeField] private LayerMask whatIsEnemies;

    //Stats
    [Range(0f,1f)]
    [SerializeField] private float bounciness;
    [SerializeField] private bool useGravity;
    private int _numLimite;

    //Damage
    [SerializeField] private int explosionDamage;
    [SerializeField] private float explosionRange;
    [SerializeField] private float explosionForce;

    //Lifetime
    [SerializeField] private int maxCollisions;
    [SerializeField] private float maxLifetime;
    [SerializeField] private bool explodeOnTouch = true;
    private int _collisions;
    private PhysicMaterial _physicsMat;

    private void Start()
    {
        Setup();
        SimplePool.Preload(explosion,4);
    }

    private void FixedUpdate()
    {
        //When to explode:
        if (_collisions > maxCollisions) Explode();

        //Count down lifetime
        maxLifetime -= Time.deltaTime;
        if (maxLifetime <= 0)
        {
            Explode();
        }
    }
    
    private void Explode()
    {
        if (_numLimite<2)
        {
            _numLimite++;
        }

        //Check for enemies 
        Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRange, whatIsEnemies);
        foreach (var enemy in enemies)
        {
            //Get component of enemy and call Take Damage
            //enemy.GetComponent<SimpleCharacterController>().OnTakeDamage(explosionDamage);
            if (_numLimite == 1)
            {
                enemy.GetComponent<PlayerManager>().OnTakeDamage(explosionDamage);
            }

            //Add explosion force (if enemy has a rigidbody)
            if (enemy.GetComponent<Rigidbody>())
                enemy.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRange);
        }

        //Add a little delay, just to make sure everything works fine
        if (_numLimite==1)
        {
            if (explosion != null) SimplePool.Spawn(explosion, transform.position, Quaternion.identity);//Instantiate(explosion, transform.position, Quaternion.identity);
            AudioManager.Instance.Play("Eggplotion");
        }

        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        Invoke(nameof(DelayGameObj), 1.5f);
    }
    private void DelayGameObj()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Don't count collisions with other bullets
        if (collision.collider.CompareTag("Bullet")) return;

        //Count up collisions
        _collisions++;

        //Explode if bullet hits an enemy directly and explodeOnTouch is activated
        if (collision.collider.CompareTag("Player") && explodeOnTouch) Explode();
    }

    private void Setup()
    {
        //Create a new Physic material
        _physicsMat = new PhysicMaterial
        {
            bounciness = bounciness,
            frictionCombine = PhysicMaterialCombine.Minimum,
            bounceCombine = PhysicMaterialCombine.Maximum
        };
        //Assign material to collider
        GetComponent<CapsuleCollider>().material = _physicsMat;

        //Set gravity
        rb.useGravity = useGravity;
    }

    /// Just to visualize the explosion range
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }
}
