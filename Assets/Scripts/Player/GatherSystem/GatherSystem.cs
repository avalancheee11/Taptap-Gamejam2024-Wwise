// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class GatherSystem
// {
//     //һЩ��������
//     public static void GatherTest(GatherItem gatherItem, PlayerGather player)
//     {
//         if (gatherItem.weaponType != player.weaponType) //�������Ͳ���
//         {
//             GatherFailed("weaponTypeWrong");
//         }else if(gatherItem.energy < player.energy) // �������㣬�޷��ɼ�
//         {
//             GatherFailed("energyNotEnough");
//         }
//         else
//         {
//             GatherSuccess(gatherItem, player);
//         }
//     }

//     public static void GatherSuccess(GatherItem gatherItem, PlayerGather player)
//     {
//         //Todo:ת���ﳯ��
//         SetLookAtRotation(gatherItem, player);

//         //Todo:���Ųɼ�����
//         PlayGatherAnimation(gatherItem, player);

//         player.energy -= gatherItem.energy;
//         Inventory.Instance.AddItems(gatherItem.rewards);

//         GatherItem.Destroy(gatherItem.gameObject);
//     }

//     public static void SetLookAtRotation(GatherItem gatherItem, PlayerGather player)
//     {

//     }
//     public static void PlayGatherAnimation(GatherItem gatherItem, PlayerGather player)
//     {

//     }
//     //Todo: ��Բɼ�ʧ�ܵ���Ӧ
//     public static void GatherFailed(string failedMessage)
//     {
//         Debug.Log("GatherFailed:" + failedMessage);
//     }
// }
