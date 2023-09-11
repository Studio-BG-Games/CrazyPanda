using UnityEngine;
using DarkTonic.MasterAudio;

public class PandaAttackController : MonoBehaviour
{
    public CharacterMovementControl _charPandaControl; // основной скрипт панды - управление и анимации
    public bool noDoubleAttackLMB; //чтоб избежать двойного удара при атаке левой кнопкой
    public bool noDoubleAttackRMB; //чтоб избежать двойного удара при атаке правой кнопкой

    public int attackLMB = 3;
    public int attackRMB = 5;
    public int attackSkill1 = 5;
    public int attackSkill2 = 10;
    public int attackSkill3 = 20;
 

    public void AttackLeftBtn(){
        print("Attack_LB");

    }

    public void AttackRightBtn(){
        print("Attack_RsB");
    }

    public void AttackSkill1(){
        
    }
    void OnTriggerEnter(Collider col) 
    {
        //print("OnTriggerEnter = " + col.gameObject.name);
        if(col.tag == "enemy_woolf" && _PandaStatus.pandaAnimationPlay == "attack_LMouse1" && !noDoubleAttackLMB) // атакуем волка MouseL!  _charPandaControl.timerBlockingAttackL < 0.3f
        {            
            //transform.parent.rotation = Quaternion.LookRotation(-col.gameObject.transform.forward);// look at woolf -  fix error sword
            print("woolfContact!");
            
            if(col.gameObject.GetComponent<EnemyWoolfController>() != null)
                col.gameObject.GetComponent<EnemyWoolfController>()?.AttackFromPanda(attackLMB); // отнимаем жизни
            if(col.gameObject.GetComponent<EnemyWoolfController>() != null)
                col.gameObject.GetComponent<EnemyWoolfController>().timerIsAtakaFromPanda = 0;
            
            if(col.gameObject.GetComponent<EnemyGolemController>() != null)
                col.gameObject.GetComponent<EnemyGolemController>()?.AttackFromPanda(attackLMB); // отнимаем жизни у голема
            
            if(col.gameObject.GetComponent<EnemyBunnyController>() != null)
               col.gameObject.GetComponent<EnemyBunnyController>()?.AttackFromPanda(attackLMB); // отнимаем жизни у Bunny

            if(col.gameObject.GetComponent<EnemyForestController>() != null)
               col.gameObject.GetComponent<EnemyForestController>()?.AttackFromPanda(attackLMB); // отнимаем жизни у лесного демона
            

            noDoubleAttackLMB = true; // чтоб избежать двойных ударов
            MasterAudio.PlaySound("SwordToTarget_woolf");
        }

        if(col.tag == "enemy_woolf" && _PandaStatus.pandaAnimationPlay == "attack_RMouse1" && !noDoubleAttackRMB) // атакуем волка MouseR!  && _charPandaControl.timerBlockingSkill < 0.3f
        {            
            //transform.rotation = Quaternion.LookRotation(-col.gameObject.transform.forward);// look at woolf
            //print("woolfContact!");
            if(col.gameObject.GetComponent<EnemyWoolfController>() != null)
                col.gameObject.GetComponent<EnemyWoolfController>()?.AttackFromPanda(attackRMB); // отнимаем жизни у волка
            if(col.gameObject.GetComponent<EnemyGolemController>() != null)
                col.gameObject.GetComponent<EnemyGolemController>()?.AttackFromPanda(attackRMB); // отнимаем жизни у голема
            if(col.gameObject.GetComponent<EnemyBunnyController>() != null)
                col.gameObject.GetComponent<EnemyBunnyController>()?.AttackFromPanda(attackRMB); // отнимаем жизни у Bunny
            if(col.gameObject.GetComponent<EnemyForestController>() != null)
               col.gameObject.GetComponent<EnemyForestController>()?.AttackFromPanda(attackRMB); // отнимаем жизни у лесного демона

            noDoubleAttackRMB = true; // чтоб избежать двойных ударов
            MasterAudio.PlaySound("SwordToTarget_woolf");
        }

        //print("OnTriggerEnter = " + col.gameObject.name);
        if(col.tag == "enemy_woolf" && _PandaStatus.pandaAnimationPlay == "skill1") // атакуем волка Skill 1!  && _charPandaControl.timerBlockingSkill < 0.3f
        {            
            //transform.rotation = Quaternion.LookRotation(-col.gameObject.transform.forward);// look at woolf
            //print("woolfContact!");
            if(col.gameObject.GetComponent<EnemyWoolfController>() != null)
                col.gameObject.GetComponent<EnemyWoolfController>()?.AttackFromPanda(attackSkill1); // отнимаем жизни у волка
            if(col.gameObject.GetComponent<EnemyGolemController>() != null)
                col.gameObject.GetComponent<EnemyGolemController>()?.AttackFromPanda(attackSkill1); // отнимаем жизни у голема
            if(col.gameObject.GetComponent<EnemyBunnyController>() != null)
                col.gameObject.GetComponent<EnemyBunnyController>()?.AttackFromPanda(attackSkill1); // отнимаем жизни у Bunny
            if(col.gameObject.GetComponent<EnemyForestController>() != null)
               col.gameObject.GetComponent<EnemyForestController>()?.AttackFromPanda(attackSkill1); // отнимаем жизни у лесного демона
            MasterAudio.PlaySound("SwordToTarget_woolf");
        }

        //print("OnTriggerEnter = " + col.gameObject.name);
        if(col.tag == "enemy_woolf" && _PandaStatus.pandaAnimationPlay == "skill2" || _PandaStatus.pandaAnimationPlay == "skill2b") // атакуем волка Skill 2    && _charPandaControl.timerBlockingSkill < 0.3f
        {            
            //transform.rotation = Quaternion.LookRotation(-col.gameObject.transform.forward);// look at woolf
            //print("woolfContact!");
            if(col.gameObject.GetComponent<EnemyWoolfController>() != null)
                col.gameObject.GetComponent<EnemyWoolfController>()?.AttackFromPanda(attackSkill2); // отнимаем жизни у волка
            if(col.gameObject.GetComponent<EnemyGolemController>() != null)
                col.gameObject.GetComponent<EnemyGolemController>()?.AttackFromPanda(attackSkill2); // отнимаем жизни у голема
            if(col.gameObject.GetComponent<EnemyBunnyController>() != null)
                col.gameObject.GetComponent<EnemyBunnyController>()?.AttackFromPanda(attackSkill2); // отнимаем жизни у Bunny
            if(col.gameObject.GetComponent<EnemyForestController>() != null)
               col.gameObject.GetComponent<EnemyForestController>()?.AttackFromPanda(attackSkill2); // отнимаем жизни у лесного демона
            MasterAudio.PlaySound("SwordToTarget_woolf");
        }

        
        if(col.tag == "enemy_woolf" && _PandaStatus.pandaAnimationPlay == "skill3") // атакуем волка Skill 3  
        {            
            //transform.rotation = Quaternion.LookRotation(-col.gameObject.transform.forward);// look at woolf
            //print("woolfContact!");        
            if(col.gameObject.GetComponent<EnemyWoolfController>() != null)
                col.gameObject.GetComponent<EnemyWoolfController>()?.AttackFromPanda(attackSkill3); // отнимаем жизни у волка
            if(col.gameObject.GetComponent<EnemyGolemController>() != null)
                col.gameObject.GetComponent<EnemyGolemController>()?.AttackFromPanda(attackSkill3); // отнимаем жизни у голема
            if(col.gameObject.GetComponent<EnemyBunnyController>() != null)
                col.gameObject.GetComponent<EnemyBunnyController>()?.AttackFromPanda(attackSkill3); // отнимаем жизни у Bunny
            if(col.gameObject.GetComponent<EnemyForestController>() != null)
               col.gameObject.GetComponent<EnemyForestController>()?.AttackFromPanda(attackSkill3); // отнимаем жизни у лесного демона
            MasterAudio.PlaySound("SwordToTarget_woolf");
        }
    }


}
