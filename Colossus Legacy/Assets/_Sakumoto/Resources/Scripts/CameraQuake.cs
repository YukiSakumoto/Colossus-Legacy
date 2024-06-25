using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;


public class CameraQuake : MonoBehaviour
{
    //private struct ShakeInfo
    //{
    //    public ShakeInfo(float duration, float strength, float vibrato, Vector2 randomOffset)
    //    {
    //        Duration = duration;
    //        Strength = strength;
    //        Vibrato = vibrato;
    //        RandomOffset = randomOffset;
    //    }
    //    public float Duration { get; } // 時間
    //    public float Strength { get; } // 揺れの強さ
    //    public float Vibrato { get; }  // どのくらい振動するか
    //    public Vector2 RandomOffset { get; } // ランダムオフセット値
    //}

    //private CameraFollow m_camera;

    //private ShakeInfo _shakeInfo;

    //private Vector3 _targetPosition; // 初期位置
    //private bool _isDoShake;       // 揺れ実行中か？
    //private float _totalShakeTime; // 揺れ経過時間

    //private void Start()
    //{
    //    m_camera = GetComponent<CameraFollow>();
    //}

    //private void Update()
    //{
    //    if (!_isDoShake) return;
    //    _targetPosition = m_camera.nowPos;

    //    // 揺れ位置情報更新
    //    this.transform.position = GetUpdateShakePosition(
    //        _shakeInfo,
    //        _totalShakeTime,
    //        _targetPosition);

    //    // duration分の時間が経過したら揺らすのを止める
    //    _totalShakeTime += Time.deltaTime;
    //    if (_totalShakeTime >= _shakeInfo.Duration)
    //    {
    //        _isDoShake = false;
    //        _totalShakeTime = 0.0f;
    //        // 初期位置に戻す
    //        this.transform.position = _targetPosition;
    //    }
    //}

    //private Vector3 GetUpdateShakePosition(ShakeInfo shakeInfo, float totalTime, Vector3 initPosition)
    //{
    //    // パーリンノイズ値(-1.0〜1.0)を取得
    //    var strength = shakeInfo.Strength;
    //    var randomOffset = shakeInfo.RandomOffset;
    //    var randomX = GetPerlinNoiseValue(randomOffset.x, strength, totalTime);
    //    var randomY = GetPerlinNoiseValue(randomOffset.y, strength, totalTime);

    //    // -strength ~ strength の値に変換
    //    randomX *= strength;
    //    randomY *= strength;

    //    // -vibrato ~ vibrato の値に変換
    //    var vibrato = shakeInfo.Vibrato;
    //    var ratio = 1.0f - totalTime / shakeInfo.Duration;
    //    vibrato *= ratio; // フェードアウトさせるため、経過時間により揺れの量を減衰
    //    randomX = Mathf.Clamp(randomX, -vibrato, vibrato);
    //    randomY = Mathf.Clamp(randomY, -vibrato, vibrato);

    //    // 初期位置に加える形で設定する
    //    var position = initPosition;
    //    position.x += randomX;
    //    position.y += randomY;
    //    return position;
    //}

    //private float GetPerlinNoiseValue(float offset, float speed, float time)
    //{
    //    // パーリンノイズ値を取得する
    //    // X: オフセット値 + 速度 * 時間
    //    // Y: 0.0固定
    //    var perlinNoise = Mathf.PerlinNoise(offset + speed * time, 0.0f);
    //    // 0.0〜1.0 -> -1.0〜1.0に変換して返却
    //    return (perlinNoise - 0.5f) * 2.0f;
    //}

    //// 揺れ開始
    //// <param name="duration">時間</param>
    //// <param name="strength">揺れの強さ</param>
    //// <param name="vibrato">どのくらい振動するか</param>
    //public void StartShake(float duration, float strength, float vibrato)
    //{
    //    // 揺れ情報を設定して開始
    //    var randomOffset = new Vector2(Random.Range(0.0f, 100.0f), Random.Range(0.0f, 100.0f)); // ランダム値はとりあえず0〜100で設定
    //    _shakeInfo = new ShakeInfo(duration, strength, vibrato, randomOffset);
    //    _isDoShake = true;
    //    _totalShakeTime = 0.0f;
    //}


    /// <summary>
    /// 揺れ情報
    /// </summary>
    private struct ShakeInfo
    {
        public ShakeInfo(float duration, float strength, float vibrato)
        {
            Duration = duration;
            Strength = strength;
            Vibrato = vibrato;
        }
        public float Duration { get; } // 時間
        public float Strength { get; } // 揺れの強さ
        public float Vibrato { get; }  // どのくらい振動するか
    }
    private ShakeInfo _shakeInfo;

    private CameraFollow m_camera;

    private Vector3 _targetPosition; // 初期位置
    private bool _isDoShake;       // 揺れ実行中か？
    private float _totalShakeTime; // 揺れ経過時間

    private void Start()
    {
        m_camera = GetComponent<CameraFollow>();
    }

    private void Update()
    {
        if (!_isDoShake) return;
        _targetPosition = m_camera.nowPos;

        // 揺れ位置情報更新
        gameObject.transform.position = UpdateShakePosition(
            gameObject.transform.position,
            _shakeInfo,
            _totalShakeTime,
            _targetPosition);

        // duration分の時間が経過したら揺らすのを止める
        _totalShakeTime += Time.deltaTime;
        if (_totalShakeTime >= _shakeInfo.Duration)
        {
            _isDoShake = false;
            _totalShakeTime = 0.0f;
            // 初期位置に戻す
            gameObject.transform.position = _targetPosition;
        }
    }

    /// <summary>
    /// 更新後の揺れ位置を取得
    /// </summary>
    /// <param name="currentPosition">現在の位置</param>
    /// <param name="shakeInfo">揺れ情報</param>
    /// <param name="totalTime">経過時間</param>
    /// <param name="initPosition">初期位置</param>
    /// <returns>更新後の揺れ位置</returns>>
    private Vector3 UpdateShakePosition(Vector3 currentPosition, ShakeInfo shakeInfo, float totalTime, Vector3 initPosition)
    {
        // -strength ~ strength の値で揺れの強さを取得
        var strength = shakeInfo.Strength;
        var randomX = Random.Range(-1.0f * strength, strength);
        var randomY = Random.Range(-1.0f * strength, strength);

        // 現在の位置に加える
        var position = currentPosition;
        position.x += randomX;
        position.y += randomY;

        // 初期位置-vibrato ~ 初期位置+vibrato の間に収める
        var vibrato = shakeInfo.Vibrato;
        var ratio = 1.0f - totalTime / shakeInfo.Duration;
        vibrato *= ratio; // フェードアウトさせるため、経過時間により揺れの量を減衰
        position.x = Mathf.Clamp(position.x, initPosition.x - vibrato, initPosition.x + vibrato);
        position.y = Mathf.Clamp(position.y, initPosition.y - vibrato, initPosition.y + vibrato);
        return position;
    }

    /// <summary>
    /// 揺れ開始
    /// </summary>
    /// <param name="duration">時間</param>
    /// <param name="strength">揺れの強さ</param>
    /// <param name="vibrato">どのくらい振動するか</param>
    public void StartShake(float duration, float strength, float vibrato)
    {
        // 揺れ情報を設定して開始
        _shakeInfo = new ShakeInfo(duration, strength, vibrato);
        _isDoShake = true;
        _totalShakeTime = 0.0f;
    }
}