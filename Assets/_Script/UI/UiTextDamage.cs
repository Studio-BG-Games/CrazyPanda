using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class UiTextDamage : MonoBehaviour
{
    //public int damageValue;

    public GameObject textScore;
    public Color32 colorStart;
    public Color32 colorEnd;
    // Start is called before the first frame update

    void OnDisable(){
        textScore.GetComponent<TextMeshProUGUI>().DOFade(1.0f, 0.1f);
        textScore.transform.DOLocalMoveY(0.0f, 0.1f);
        textScore.transform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), 0.1f);
    }

    void Start()
    {
        /*textScore.GetComponent<TextMeshProUGUI>().text = damageValue.ToString();
        textScore.GetComponent<TextMeshProUGUI>().DOFade(0.0f, 0.5f).SetDelay(0.4f);
        textScore.transform.DOLocalMoveY(200.0f, 0.5f);
        textScore.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.6f);*/
        
        
    }

    void OnEnable(){
        
        
    }

    public void GoAnim(int damageValue)
    {
        textScore.GetComponent<TextMeshProUGUI>().text = damageValue.ToString();
        textScore.GetComponent<TextMeshProUGUI>().DOFade(0.0f, 0.5f).SetDelay(0.4f);
        textScore.transform.DOLocalMoveY(200.0f, 0.5f);
        textScore.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.6f);
    }

    
}
