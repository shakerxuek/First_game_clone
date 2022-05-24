using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    float _speed = 20f;
    public Rigidbody _rigidbody;
    Vector3 _velocity;
    // Start is called before the first frame update
    Renderer _renderer;
    void Start()
    {
        _rigidbody=GetComponent<Rigidbody>();
        _renderer=GetComponent<Renderer>();
        Invoke("Launch",0.5f);
    }
    public void Launch()
    {
        _rigidbody.velocity=Vector3.up * _speed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _rigidbody.velocity = _rigidbody.velocity.normalized *_speed; 
        _velocity=_rigidbody.velocity;

        if(!_renderer.isVisible)
        {
            GameManager.Instance.Balls--;
            Destroy(gameObject); 
        }

        if(Input.anyKeyDown)
        {
            Invoke("Launch",0.5f);
        }
               
    }
     
    private void OnCollisionEnter(Collision collision) 
    {
        _rigidbody.velocity = Vector3.Reflect(_velocity, collision.contacts[0].normal);     
    }
}
