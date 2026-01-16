using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;

public class FireballMovement : MonoBehaviour
{
    [SerializeField] public float fireballSpeed = 10f;
    [SerializeField] public float fireballLifeTime = 5f;
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
        Destroy(gameObject);
    }

}

