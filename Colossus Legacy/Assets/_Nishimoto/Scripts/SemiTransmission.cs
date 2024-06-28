using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class SemiTransmission : MonoBehaviour
{
    [SerializeField] private GameObject player;     //�C���X�y�N�^�[��Ńv���C���[��ڑ����Ă���
    Vector3 tergetPosition;
    float tergetOffsetYFoot = 0.1f; //ray���΂������̃I�t�Z�b�g�i�����̕��j
    float tergetOffsetYHead = 1.6f; //ray���΂������̃I�t�Z�b�g�i�������̕��j

    public GameObject[] prevRaycast;
    public List<GameObject> raycastHitsList_ = new List<GameObject>();

    void Update()
    {
        //�J�����������Ԃ̃I�u�W�F�N�g�𔼓��������Ă���
        prevRaycast = raycastHitsList_.ToArray();   //�O�t���[���œ����ɂ��Ă���I�u�W�F�N�g�i���X�g�j��z��prevRayCast�ɏo��
        raycastHitsList_.Clear();                   //�O�t���[���œ����ɂ��Ă���I�u�W�F�N�g�i���X�g�j���������H�����H
        tergetPosition = player.transform.position; //tergetPosition��Player��position���i�[
        tergetPosition.y += tergetOffsetYFoot;      //tergetPosition��y���i���������j�ɃI�t�Z�b�g�𔽉f�B�����ł͑����̍����ɍ��킹�Ă���B�i�����̒l�����̂܂܂����Ɛ^���̏��������ɂȂ邱�Ƃ����������߃I�t�Z�b�g�����B�j
        Vector3 _difference = (tergetPosition - this.transform.position);   //�J�����ʒu��tergetPosition�ւ̃x�N�g�����擾
        RayCastHit(_difference);                    //���̃��\�b�h���Q�ƁBray���΂��ď����ɍ������̂𔼓����ɂ��āAraycastHitList�ɒǉ����Ă���B

        //�J�������������Ԃ̃I�u�W�F�N�g�𔼓��������Ă���
        tergetPosition.y += tergetOffsetYHead;      //tergetPosition��y���i���������j�ɃI�t�Z�b�g�𔽉f�B�����ł͓��̍����ɍ��킹�Ă���B
        _difference = (tergetPosition - this.transform.position);   //�J�����ʒu��tergetPosition�ւ̃x�N�g�����擾
        RayCastHit(_difference);

        //�q�b�g����GameObject�̍��������߂āA����Փ˂��Ȃ������I�u�W�F�N�g��s�����ɖ߂�
        foreach (GameObject _gameObject in prevRaycast.Except<GameObject>(raycastHitsList_))    //prevRaycast��raycastHitList_�Ƃ̍����𒊏o���Ă�B
        {
            IsHitObj noSampleMaterial = _gameObject.GetComponent<IsHitObj>();
            if (_gameObject != null)
            {
                noSampleMaterial.NotClearMaterialInvoke();
            }

        }
    }

    //ray���΂��ď����ɍ������̂𔼓����ɂ��āAraycastHitList�ɒǉ����Ă���B
    public void RayCastHit(Vector3 __difference)
    {
        Vector3 _direction = __difference.normalized;           //�J����-�^�[�Q�b�g�Ԃ̃x�N�g���̐��K�x�N�g���𒊏o

        Ray _ray = new Ray(this.transform.position, _direction);//Ray�𔭎�
        RaycastHit[] rayCastHits = Physics.RaycastAll(_ray);    //Ray�ɂ��������I�u�W�F�N�g�����ׂĎ擾

        foreach (RaycastHit hit in rayCastHits)
        {
            float distance = Vector3.Distance(hit.point, transform.position);       //�J����-ray�����������ꏊ�Ԃ̋������擾
            if (distance < __difference.magnitude)      //�J����-ray�����������ꏊ�Ԃ̋����ƃJ����-�^�[�Q�b�g�Ԃ̋������r�B�i���̔�r���s��Ȃ���Player�̉����̃I�u�W�F�N�g�������ɂȂ�B�j
            {
                IsHitObj ishitobj = hit.collider.GetComponent<IsHitObj>();
                if (
                hit.collider.tag == "Cave")          //�^�O���m�F
                {
                    ishitobj.ClearMaterialInvoke();                 //�����ɂ��郁�\�b�h���Ăяo���B
                    raycastHitsList_.Add(hit.collider.gameObject);  //hit����gameobject��ǉ�����
                }
            }
        }
    }
}
