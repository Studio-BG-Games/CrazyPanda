using UnityEngine;

public class Panda_Vfx : MonoBehaviour
{
    public GameObject vfxLMB;
    //public GameObject vfxRMB;
    [Space(5)]
    //public GameObject vfxDash;
    [Space(5)]
    public GameObject skill1;
    public GameObject skill2;

    void Update()
    {
        /*if(_PandaStatus.pandaAnimationPlay == "attack_LMouse1") <<<<---------- я отключил
        {
            vfxLMB.SetActive(true);
        }else
        {
            vfxLMB.SetActive(false);
        }*/

        /*if(_PandaStatus.pandaAnimationPlay == "attack_RMouse1")
        {
            vfxRMB.SetActive(true);
        }else
        {
            vfxRMB.SetActive(false);
        }*/

        /*if(_PandaStatus.pandaAnimationPlay == "dash")
        {
            vfxDash.SetActive(true);
        }else
        {
            vfxDash.SetActive(false);
        }*/

        
        /*if(_PandaStatus.pandaAnimationPlay == "skill1")   <<<<------- я отключил
        {
            skill1.SetActive(true);
        }else
        {
            skill1.SetActive(false);
        }*/

        if(_PandaStatus.pandaAnimationPlay == "skill2")
        {
            skill2.SetActive(true);
        }else
        {
            skill2.SetActive(false);
        }
    }
}
