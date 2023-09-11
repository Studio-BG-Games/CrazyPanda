using UnityEngine;
using DarkTonic.MasterAudio;
using Pathfinding;

public class CreatorGolem : MonoBehaviour
{
    public GameObject golemCreate;
    public Animator animGolemCreator;
    public AIDestinationSetter aIDestinationSetter;

    void OnTriggerEnter(Collider col)
    {
        if(col.CompareTag("Panda"))
        {   MasterAudio.PlaySound("GolemCreate");
            print("create Golem");
            animGolemCreator.SetBool("createGolem", true);
            aIDestinationSetter.target = col.transform;
        }
    }
    
    public void StartCreate()
    {
        golemCreate.SetActive(true);
        golemCreate.transform.SetParent(null);
        gameObject.SetActive(false);
    }
}
