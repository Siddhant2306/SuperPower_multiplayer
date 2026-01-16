using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;

public class FireballMovement : MonoBehaviour
{
    [SerializeField] public float fireballSpeed = 10f;
    [SerializeField] public float fireballLifeTime = 5f;
    public GameObject Explosion;
    Rigidbody fireball_rb;
    
    void Awake()
    {
       fireball_rb = gameObject.GetComponent<Rigidbody>();
    }

    void Start()
    {
        fireball_rb.linearVelocity = transform.forward * fireballSpeed;
        Destroy(gameObject, fireballLifeTime);
        Debug.Log(transform.forward);
    }
    void OnCollisionEnter(Collision collision)
    {
        Vector3 spawnPos = transform.position;
        GameObject explosionInstance = Instantiate(Explosion, spawnPos, transform.rotation);
        Destroy(gameObject);
        Destroy(explosionInstance,2f);
    }

}

