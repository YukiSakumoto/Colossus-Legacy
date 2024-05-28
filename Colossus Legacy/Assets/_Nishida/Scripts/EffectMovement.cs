using Effekseer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectMovement : MonoBehaviour
{
    EffekseerEffectAsset effect; // �G�t�F�N�g���擾����B

    // Start is called before the first frame update
    void Start()
    {
        effect = Resources.Load<EffekseerEffectAsset>("Simple_Ribbon_Sword");
    }

    public void PlayerSwordEffect()
    {
        // transform�̈ʒu�ŃG�t�F�N�g���Đ�����
        Vector3 EffectPosition = transform.position;
        EffectPosition.y += 1f;
        EffekseerHandle handle = EffekseerSystem.PlayEffect(effect, EffectPosition);

        // transform�̉�]��ݒ肷��B
        Quaternion EffectRotate = transform.rotation;
        EffectRotate *= Quaternion.Euler(-30, -90, 0);
        handle.SetRotation(EffectRotate);
    }
}
