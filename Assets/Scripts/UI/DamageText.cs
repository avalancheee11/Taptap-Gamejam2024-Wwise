using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private TextMeshProUGUI damageTMP;

    public void SetDamageText(float damage){
        damageTMP.text = damage.ToString();
    }

    // call this method inside the text animation(event)
    public void DestroyText(){
        Destroy(gameObject);
    }

}
