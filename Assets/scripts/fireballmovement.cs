using Unity.VisualScripting;
using UnityEngine;

public class FireballMovement : MonoBehaviour
{
    [SerializeField] private float fireballSpeed = 10f;
    [SerializeField] private float fireballLifeTime = 5f;

    void Start()
    {
        this.gameObject.GetComponent<Rigidbody>();
    }
    void Update()
    {
        
    }

}

