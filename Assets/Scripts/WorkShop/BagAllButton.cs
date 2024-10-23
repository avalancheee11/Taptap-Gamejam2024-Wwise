using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagAllButton : Singleton<BagAllButton>
{
    // Start is called before the first frame update
    public Sprite defaultSprite;
    private Button button;
    private Image buttonImage;
    public Sprite unselectedSprite;

    // 引用其他两个按钮
    public Button button1;
    public Button button2;
    void Start()
    {
        button = GetComponent<Button>();
        buttonImage = GetComponent<Image>();;
        buttonImage.sprite = defaultSprite;
    }

    public void OpenBageImage()
    {
        buttonImage.sprite = defaultSprite;
    }

    public void ChangeSourceImage()
    {
        buttonImage.sprite = unselectedSprite;
    }

}
