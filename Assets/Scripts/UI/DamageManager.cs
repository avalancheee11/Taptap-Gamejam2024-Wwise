using UnityEngine;

public class DamageManager : MonoBehaviour
{
    public static DamageManager Instance;

   [Header("Config")]
   [SerializeField] private DamageText damageTextPrefab;

    private void Awake() {

        Instance = this;
    }

   public void ShowDamageText(float damageAmount, Transform parent){
        DamageText text = Instantiate(damageTextPrefab, parent);
        //modify the text position
        text.transform.position += Vector3.right * 0.5f;
        text.SetDamageText(damageAmount);

   }

}
