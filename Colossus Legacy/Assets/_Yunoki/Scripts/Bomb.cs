using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class Bomb : MonoBehaviour
{
    [SerializeField] Rigidbody m_rb;
    [SerializeField] GameObject expParticle;
    //[SerializeField] GameObject character;
    [SerializeField] MeshCollider m_meshCollider;
    [SerializeField] CapsuleCollider m_capsuleCollider;
    [SerializeField] MeshRenderer m_meshRenderer;

    [SerializeField] float GroundPos = 0.2f;

    public GameStatusManager m_gameStatusManager;
    public PlayerSoundPlay m_playerSoundPlay;

    private string m_characterTag = "Player";
    private string m_golemTag = "EnemyWeak";

    //SoundPlay m_sound;

    private Vector3 pos;

    private float m_bombTime = 3f;

    private bool m_bombFlg = false;

    private void Start()
    {
        if (!expParticle)
        {
            Debug.Log("Particle is Null");
        }

        if (!m_meshCollider)
        {
            Debug.Log("MeshCollider is Null");
        }
        else
        {
            m_meshCollider.enabled = true;
        }

        if (!m_capsuleCollider)
        {
            Debug.Log("CapsuleCollider is Null");
        }
        else
        {
            m_capsuleCollider.enabled = false;
        }

        if(!m_meshRenderer)
        {
            Debug.Log("MeshRenderer is Null");
        }
        else
        {
            m_meshRenderer.enabled = true;
        }
    }

    private void Update()
    {
        if (!m_bombFlg)
        {
            pos = transform.position;
            if (pos.y <= GroundPos) { transform.position = new Vector3(pos.x, GroundPos, pos.z); }
        }
        else
        {
            m_rb.velocity = Vector3.zero;
            m_rb.angularVelocity = Vector3.zero;
        }
    }

    private void FixedUpdate()
    {
        m_bombTime -= Time.deltaTime;
        if(m_bombTime <= 0 && !m_bombFlg)
        {
            BombAttack();
        }
    }

    private void BombAttack()
    {
        m_meshCollider.enabled = false;
        m_capsuleCollider.enabled = true;
        m_meshRenderer.enabled = false;
        m_bombFlg = true;
        m_meshRenderer.enabled = false;
        Instantiate(expParticle, transform.position, Quaternion.identity);
        m_rb.velocity = Vector3.zero;
        m_rb.angularVelocity = Vector3.zero;
        if (!m_playerSoundPlay)
        {
            Debug.LogError("Bomb: PlayerSoundPlay is Null");
        }
        else
        {
            m_playerSoundPlay.SoundSubExplosion();
        }
    }

    private void OnTriggerEnter(Collider _other)
    {
        if(m_bombFlg)
        {
            Transform targetTransform = _other.transform;
            if (targetTransform.gameObject.CompareTag(m_characterTag))
            {
                if (!m_gameStatusManager)
                {
                    Debug.Log("Bomb: GameStatusManager is Null");
                }
                else
                {
                    m_gameStatusManager.DamagePlayerBomb();
                }
            }

            if (targetTransform.gameObject.CompareTag(m_golemTag))
            {
                if (!m_gameStatusManager)
                {
                    Debug.Log("Bomb: GameStatusManager is Null");
                }
                else
                {
                    m_gameStatusManager.DamageGolemBomb();
                }
            }
        }
    }

    public void SetTime(float _time)
    {
        m_bombTime = _time;
    }
}
