using UnityEngine;
using AmazingAssets.AdvancedDissolve;

// реализуем исчезновение убитого персонажа с эффектом Dissolve и его уничтожение

public class EnemyForestDeath : MonoBehaviour
{
    EnemyForestController bunnyControl;
    [SerializeField]
    private SkinnedMeshRenderer materialBunny;
    private bool noRepeate;
    
    [SerializeField]
    private AnimationCurve curveAnim;
    private float curveCurrentValue;
    private float timerCurve;

    void Start()
    {
        bunnyControl = GetComponent<EnemyForestController>();        
    }

    void Update()
    {
        if(bunnyControl.isDeadForest)
        {
            timerCurve += Time.deltaTime;
            curveCurrentValue = curveAnim.Evaluate(timerCurve); // считываем значение из кривой

            // меняем параметр в шейдере
            AdvancedDissolveProperties.Cutout.Standard.UpdateLocalProperty(
            materialBunny.material,
            AdvancedDissolveProperties.Cutout.Standard.Property.Clip,
            curveCurrentValue);

            if(curveCurrentValue > 0.98f)
                Destroy(gameObject);
        }    
    }
}
