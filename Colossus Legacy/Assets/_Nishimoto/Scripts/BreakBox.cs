using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBox : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        destroyObject();
    }
    public void destroyObject()
    {
        var random = new System.Random();
        var min = -3;
        var max = 3;

        GameObject parent;

        Transform[] children;
        Rigidbody rb = this.GetComponent<Rigidbody>();
        // Parent�Ƃ������̐e�I�u�W�F�N�g������
        parent = GameObject.Find("WoodBox 1");

        // �q�I�u�W�F�N�g�B������z��̏�����
        children = new Transform[parent.transform.childCount];


        for (int i = 0; i < parent.transform.childCount; i++)
        {
            children[i] = parent.transform.GetChild(i); // GetChild()�Ŏq�I�u�W�F�N�g���擾
            Debug.Log($"�������@�P�F {i} �Ԗڂ̎q���� {children[i].name} �ł�");

            for(int j = 0; j < children[i].childCount; j++)
            {
                rb.isKinematic = false;
                rb.transform.SetParent(null);
              //  j.gameObject.AddComponent<AutoDestroy>().time = 2f;
                var vect = new Vector3(random.Next(min, max), random.Next(0, max), random.Next(min, max));
                rb.AddForce(vect, ForceMode.Impulse);
                rb.AddTorque(vect, ForceMode.Impulse);

            }
        }
        /*
        gameObject.GetComponentsInChildren<Rigidbody>().ToList().ForEach(r => {
            r.isKinematic = false;
            r.transform.SetParent(null);
            r.gameObject.AddComponent<AutoDestroy>().time = 2f;
            var vect = new Vector3(random.Next(min, max), random.Next(0, max), random.Next(min, max));
            r.AddForce(vect, ForceMode.Impulse);
            r.AddTorque(vect, ForceMode.Impulse);
        });
        Destroy(gameObject);*/
    }
}
