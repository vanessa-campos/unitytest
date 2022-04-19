using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShipController : MonoBehaviour, IPooledObject
{
    [SerializeField] TMP_Text speedText;
    Rigidbody rb;

    public float posMinX, posMaxX, speedMin, speedMax;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnObjectSpawn()
    {
        transform.position = new Vector3(Random.Range(posMinX, posMaxX), 0, 0); 
        rb.velocity = Vector3.zero;   
    }
   
    private void FixedUpdate()
    {
        rb.AddForce(new Vector3(0, Random.Range(speedMin, speedMax) * Time.fixedDeltaTime, 0), ForceMode.Acceleration);
        float speed = rb.velocity.magnitude * 3.6f;         
        speedText.text = Mathf.Ceil(speed).ToString() + " Kph";
    }

}
