using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BreakBox : MonoBehaviour
{
    void Start()
    {
       
    }
    // Update is called once per frame
    void Update()
    {
    }
    public GameObject gameobject;

    public void destroyObject()
    {
        var random = new System.Random();
        var min = -3;
        var max = 3;
        gameobject.GetComponentsInChildren<Rigidbody>().ToList().ForEach(r => {
            r.isKinematic = false;
            r.transform.SetParent(null);
            var vect = new Vector3(random.Next(min, max), random.Next(0, max), random.Next(min, max));
            r.AddForce(vect, ForceMode.Impulse);
            r.AddTorque(vect, ForceMode.Impulse);
        });
        Destroy(gameObject);
    }
    
}
