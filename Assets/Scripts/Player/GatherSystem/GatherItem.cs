using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ---- ����д�滻Ϊ GatherStats_SO�����ʵ�ʱ��ɾ�� --------
public class GatherItem : MonoBehaviour
{
    public string id;
    public string modelId; //ģ�ͻ���ͼƬ
    public float interactionRadius; //������Χ
    public Collider collider; //��ײ��
    public List<Item> rewards; //�������б�
    public float refreshProbability; //ˢ�¸���
    public float energy; //��������
    public string weaponType; //��������

}
