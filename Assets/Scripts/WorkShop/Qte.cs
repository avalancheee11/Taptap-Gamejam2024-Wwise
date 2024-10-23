using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Qte : MonoBehaviour
{
    public RectTransform containerRect;
    public RectTransform sliderRect;
    public RectTransform pointerRect;

    public float sliderSpeed = 100f;
    public float pointerSpeed = 200f;

    public bool isQTE = true;
    private int sliderDirection = 1;
    private int pointerDirection = -1;

    private void Start()
    {
        // 确保所有必要的组件都已赋值
        if (containerRect == null || sliderRect == null || pointerRect == null)
        {
            Debug.LogError("请在Inspector中设置所有必要的RectTransform引用！");
            isQTE = false;
        }

        // 设置初始位置
        SetInitialPositions();
    }

    private void Update()
    {
        // 如果在qte状态的话滑块和指针移动
        if (isQTE)
        {
            MoveSlider();
            MovePointer();

            // 如果按下空格停止
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StopQTE();
            }  
        }

        // 测试！！
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartQTE();
        }
        

    }

    // 滑块的移动
    private void MoveSlider()
    {

        float newX = sliderRect.anchoredPosition.x + sliderSpeed * sliderDirection * Time.deltaTime;
        float minX = -containerRect.rect.width / 2 + sliderRect.rect.width / 2;
        float maxX = containerRect.rect.width / 2 - sliderRect.rect.width / 2;
        
        // 检测是否到达边界
        if (newX <= minX || newX >= maxX)
        {
            // 转换方向
            sliderDirection *= -1;
            newX = Mathf.Clamp(newX, minX, maxX);
        }

        // 移动滑块
        sliderRect.anchoredPosition = new Vector2(newX, sliderRect.anchoredPosition.y);
    }

    // 指针的移动
    private void MovePointer()
    {
        float newX = pointerRect.anchoredPosition.x + pointerSpeed * pointerDirection * Time.deltaTime;
        float minX = -containerRect.rect.width / 2 + pointerRect.rect.width / 2;
        float maxX = containerRect.rect.width / 2 - pointerRect.rect.width / 2;

        if (newX <= minX || newX >= maxX)
        {
            pointerDirection *= -1;
            newX = Mathf.Clamp(newX, minX, maxX);
        }

        pointerRect.anchoredPosition = new Vector2(newX, pointerRect.anchoredPosition.y);
    }

    // 停止指针
    private void StopQTE()
    {
        isQTE = false;
        CheckSuccess();
    }

    // 可以引用的开始qte接口
    public void StartQTE()
    {
        isQTE = true;
        SetInitialPositions();
    }


    // 检测是否成功
    private void CheckSuccess()
    {
        // 判断指针是否在滑块范围内
        float sliderLeft = sliderRect.anchoredPosition.x - sliderRect.rect.width / 2;
        float sliderRight = sliderRect.anchoredPosition.x + sliderRect.rect.width / 2;
        float pointerPos = pointerRect.anchoredPosition.x;

        if (pointerPos >= sliderLeft && pointerPos <= sliderRight)
        {
            Debug.Log("Success!");
        }
        else
        {
            Debug.Log("Fail!");
        }
    }

    // 设置初始位置
    private void SetInitialPositions()
    {
        // 随机生成滑块的位置
        float sliderMinX = -containerRect.rect.width / 2 + sliderRect.rect.width / 2;
        float sliderMaxX = containerRect.rect.width / 2 - sliderRect.rect.width / 2;
        float randomSliderX = Random.Range(sliderMinX, sliderMaxX);
        sliderRect.anchoredPosition = new Vector2(randomSliderX, sliderRect.anchoredPosition.y);

        // 随机生成指针的位置
        float pointerMinX = -containerRect.rect.width / 2 + pointerRect.rect.width / 2;
        float pointerMaxX = containerRect.rect.width / 2 - pointerRect.rect.width / 2;
        float randomPointerX = Random.Range(pointerMinX, pointerMaxX);
        pointerRect.anchoredPosition = new Vector2(randomPointerX, pointerRect.anchoredPosition.y);

        // 根据初始位置设置滑块和指针的方向
        sliderDirection = randomSliderX > 0 ? -1 : 1;
        pointerDirection = randomPointerX > 0 ? -1 : 1;
    }

}
