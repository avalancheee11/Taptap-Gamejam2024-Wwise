using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private EquipmentStats_SO equipmentStats;
    [SerializeField] private Transform attackPosition;
    [SerializeField] private BoxCollider damageBox; // 攻击盒子
    [SerializeField] private LayerMask monsterMask;
    [SerializeField] private LayerMask collectibleMask;  // 采集物的层
    private PlayerEquipment playerEquipment;

    private PlayerInputActions playerInputActions;
    private Coroutine attackCoroutine;
    public Transform enemy { get; set; }
    [SerializeField] private PlayerSpineNode spineNode;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions(); // 初始化
        damageBox = GetComponent<BoxCollider>(); // 确保有 BoxCollider 作为攻击盒子

        if (damageBox != null)
        {
            damageBox.isTrigger = true;
        }
    }

    private void Start()
    {
        // 忽略玩家和攻击盒子的碰撞
        Collider playerCollider = GetComponent<Collider>();
        if (playerCollider != null && damageBox != null)
        {
            Physics.IgnoreCollision(playerCollider, damageBox);
        }
        playerMovement = GetComponent<PlayerMovement>();

        playerEquipment = GetComponent<PlayerEquipment>();
        spineNode.StartAni();
    }

    private void OnEnable()
    {
        playerInputActions.Enable();
        playerInputActions.Attack.ClickAttack.performed += OnAttackPerformed;
    }

    private void OnDisable()
    {
        playerInputActions.Disable();
        playerInputActions.Attack.ClickAttack.performed -= OnAttackPerformed;
    }

    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("Attack input detected!");
        Attack();
    }

    public void Attack()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
        }
        attackCoroutine = StartCoroutine(IEAttack());
    }

    private IEnumerator IEAttack()
    {
        // 禁用移动输入
        playerMovement.DisableMovementInput();

        // 检测目标并处理
        DetectAndHandleTargets();

        // 允许移动输入
        playerMovement.EnableMovementInput();
                // 动作完成后，回到idle状态
        // SetIdleState();

        yield return null;
    }

    private void DetectAndHandleTargets()
    {
        Collider[] hitObjects = Physics.OverlapSphere(damageBox.transform.position, equipmentStats.attackRange, monsterMask | collectibleMask);

        if (hitObjects.Length > 0)
        {
            foreach (Collider hit in hitObjects)
            {
                string tag = hit.tag;
                Debug.Log($"发现目标，标签：{tag}");

                if (tag == "Monster")
                {
                    if (IsWeaponEquipped())
                    {
                        StartCoroutine(HandleMonsterAttack(hit));
                    }
                    else
                    {
                        Debug.Log("未装备武器，无法攻击怪物！");
                    }
                }
                else if (tag == "Plant" || tag == "Mineral")
                {
                    if (IsToolEquipped())
                    {
                        StartCoroutine(HandleCollectible(hit, tag));
                         Debug.Log("已装备工具，准备采集！");
                    }
                    else
                    {
                        Debug.Log("未装备工具，无法采集植物或矿物！");
                    }
                }
                else if (tag == "Fish")
                {
                    if (IsFishingNetEquipped())
                    {
                        StartCoroutine(HandleCollectible(hit, tag));
                    }
                    else
                    {
                        Debug.Log("未装备渔网，无法捕捉鱼！");
                    }
                }
            }
        }
    }

    private IEnumerator HandleMonsterAttack(Collider monsterCollider)
    {
        // 播放攻击动画
        yield return PlayAttackAnimation("atkL", "atkR");

        IDamageable damageableEnemy = monsterCollider.GetComponent<IDamageable>();
        if (damageableEnemy != null)
        {
            damageableEnemy.TakeDamage(equipmentStats.damage);
            Debug.Log("伤害敌人...");
        }
    }

    private IEnumerator HandleCollectible(Collider collectibleCollider, string tag)
    {
        string actionL = "", actionR = "";

        if (tag == "Plant")
        {
            actionL = "collectPlantL";
            actionR = "collectPlantR";
        }
        else if (tag == "Mineral")
        {
            actionL = "miningL";
            actionR = "miningR";
        }
        else if (tag == "Fish")
        {
            actionL = "collectFishL";
            actionR = "collectFishR";
        }

        // 播放采集动画
        yield return PlayAttackAnimation(actionL, actionR);

        IDamageable collectible = collectibleCollider.GetComponent<IDamageable>();
        if (collectible != null)
        {
            collectible.TakeDamage(equipmentStats.damage);
            Debug.Log($"采集物品 {collectibleCollider.tag}...");
        }
    }

    private IEnumerator PlayAttackAnimation(string actionL, string actionR)
    {
        string attackAnimation = playerMovement.lastMoveDirection.x < 0 ? actionL : actionR;

        if (!string.IsNullOrEmpty(attackAnimation))
        {
            spineNode.PlayAnime(attackAnimation, false); // 动画不循环播放
        }

        // 等待动画播放完毕，假设时长为1秒
        yield return new WaitForSeconds(1f);
    }

    // 检查是否装备了武器
    private bool IsWeaponEquipped()
    {
        return playerEquipment.currentEquipment == EquipmentType.Weapon;
    }

    // 检查是否装备了工具
    private bool IsToolEquipped()
    {
        return playerEquipment.currentEquipment == EquipmentType.Tool;
    }

    // 检查是否装备了渔网
    private bool IsFishingNetEquipped()
    {
        return playerEquipment.currentEquipment == EquipmentType.FishingNet;
    }


    private void OnDrawGizmos()
    {
        if (damageBox != null)
        {
            Gizmos.color = Color.red;
            Gizmos.matrix = Matrix4x4.TRS(damageBox.transform.position, damageBox.transform.rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, damageBox.size);
        }
    }
}
