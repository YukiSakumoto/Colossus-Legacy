using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class SemiTransmission : MonoBehaviour
{
    [SerializeField] private GameObject player;     //インスペクター上でプレイヤーを接続しておく
    Vector3 tergetPosition;
    float tergetOffsetYFoot = 0.1f; //rayを飛ばす方向のオフセット（足元の方）
    float tergetOffsetYHead = 1.6f; //rayを飛ばす方向のオフセット（頭頂部の方）

    public GameObject[] prevRaycast;
    public List<GameObject> raycastHitsList_ = new List<GameObject>();

    void Update()
    {
        //カメラ→足元間のオブジェクトを半透明化している
        prevRaycast = raycastHitsList_.ToArray();   //前フレームで透明にしているオブジェクト（リスト）を配列prevRayCastに出力
        raycastHitsList_.Clear();                   //前フレームで透明にしているオブジェクト（リスト）を初期化？消去？
        tergetPosition = player.transform.position; //tergetPositionにPlayerのpositionを格納
        tergetPosition.y += tergetOffsetYFoot;      //tergetPositionのy軸（高さ方向）にオフセットを反映。ここでは足元の高さに合わせている。（足元の値をそのままいれると真下の床が透明になることがあったためオフセットした。）
        Vector3 _difference = (tergetPosition - this.transform.position);   //カメラ位置→tergetPositionへのベクトルを取得
        RayCastHit(_difference);                    //↓のメソッドを参照。rayを飛ばして条件に合うものを半透明にして、raycastHitListに追加している。

        //カメラ→頭頂部間のオブジェクトを半透明化している
        tergetPosition.y += tergetOffsetYHead;      //tergetPositionのy軸（高さ方向）にオフセットを反映。ここでは頭の高さに合わせている。
        _difference = (tergetPosition - this.transform.position);   //カメラ位置→tergetPositionへのベクトルを取得
        RayCastHit(_difference);

        //ヒットしたGameObjectの差分を求めて、今回衝突しなかったオブジェクトを不透明に戻す
        foreach (GameObject _gameObject in prevRaycast.Except<GameObject>(raycastHitsList_))    //prevRaycastとraycastHitList_との差分を抽出してる。
        {
            IsHitObj noSampleMaterial = _gameObject.GetComponent<IsHitObj>();
            if (_gameObject != null)
            {
                noSampleMaterial.NotClearMaterialInvoke();
            }

        }
    }

    //rayを飛ばして条件に合うものを半透明にして、raycastHitListに追加している。
    public void RayCastHit(Vector3 __difference)
    {
        Vector3 _direction = __difference.normalized;           //カメラ-ターゲット間のベクトルの正規ベクトルを抽出

        Ray _ray = new Ray(this.transform.position, _direction);//Rayを発射
        RaycastHit[] rayCastHits = Physics.RaycastAll(_ray);    //Rayにあたったオブジェクトをすべて取得

        foreach (RaycastHit hit in rayCastHits)
        {
            float distance = Vector3.Distance(hit.point, transform.position);       //カメラ-rayがあたった場所間の距離を取得
            if (distance < __difference.magnitude)      //カメラ-rayがあたった場所間の距離とカメラ-ターゲット間の距離を比較。（この比較を行わないとPlayerの奥側のオブジェクトも透明になる。）
            {
                IsHitObj ishitobj = hit.collider.GetComponent<IsHitObj>();
                if (
                hit.collider.tag == "Cave")          //タグを確認
                {
                    ishitobj.ClearMaterialInvoke();                 //透明にするメソッドを呼び出す。
                    raycastHitsList_.Add(hit.collider.gameObject);  //hitしたgameobjectを追加する
                }
            }
        }
    }
}
