using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    void Start() {
        // Destroy the bullet after 10 seconds if it does not hit any object.
        Destroy(gameObject, 10.0f);
    }
    
    private void OnCollisionEnter(Collision collision) {
        IDamageable obj = collision.gameObject.GetComponent<IDamageable>();
        if (obj != null) {
            obj.TakeDamage();
        }
        Destroy(gameObject);
    }
}
