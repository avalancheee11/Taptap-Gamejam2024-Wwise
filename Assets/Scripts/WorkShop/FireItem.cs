using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireItem : MonoBehaviour
{
    public string colorCategory;
    public string specificColor;
    public string material;
    private bool isDragging = false;
    private Vector3 offset;
    private Camera mainCamera;
    private Vector3 originalPosition;


    [SerializeField] private float returnSpeed = 5f; // 返回原位置的速度

    // 创建
    public FireItem(string colorCat, string specificClr, string mtr)
    {
        colorCategory = colorCat;
        specificColor = specificClr;
        material = mtr;
    }

    private void Start()
    {
        mainCamera = Camera.main;
        originalPosition = transform.position;
    }

    private void OnMouseDown()
    {
        isDragging = true;
        offset = transform.position - GetMouseWorldPosition();
    }

    private void OnMouseDrag()
    {
        // 一次只能拽一个，提交一个
        if (isDragging && !SellingManager.Instance.IsCustomerLeaving())
        {
            Vector3 newPosition = GetMouseWorldPosition() + offset;
            transform.position = newPosition;
        }
    }

    private void OnMouseUp()
    {
        isDragging = false;
        // 检测是否在 Customer 的范围内
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.5f);
        bool foundCustomer = false;

        foreach (Collider2D collider in colliders)
        {
            Customer customer = collider.GetComponent<Customer>();
            if (customer != null)
            {
                Debug.Log("checking...");
                customer.CheckFireItem(this);
                foundCustomer = true;
                break;
            }
        }

        if (!foundCustomer)
        {
            // 拽回 （后面要检测如果是垃圾桶的话也会被销毁，可以直接调用下面的方法）
            StartCoroutine(ReturnToOriginalPosition());
        }
    }

    private IEnumerator ReturnToOriginalPosition()
    {
        while (Vector3.Distance(transform.position, originalPosition) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, originalPosition, returnSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = originalPosition;
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = -mainCamera.transform.position.z;
        return mainCamera.ScreenToWorldPoint(mousePoint);
    }

    // 消除火焰（提交）
    public void DestroyOnSuccess()
    {
        Destroy(gameObject);
    }
}
