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
        // Parentという名の親オブジェクトを検索
        parent = GameObject.Find("WoodBox 1");

        // 子オブジェクト達を入れる配列の初期化
        children = new Transform[parent.transform.childCount];


        for (int i = 0; i < parent.transform.childCount; i++)
        {
            children[i] = parent.transform.GetChild(i); // GetChild()で子オブジェクトを取得
            Debug.Log($"検索方法１： {i} 番目の子供は {children[i].name} です");

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
