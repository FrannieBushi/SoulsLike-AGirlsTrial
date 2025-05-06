using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
    
    public float secondsToDestroy;
    public bool hasAnimationDestruction;

    void Start()
    {
        if(! hasAnimationDestruction)
        {
            Destroy(gameObject, secondsToDestroy);    
        }
        
    }

    public void destroyObject()
    {
        Destroy(gameObject);
    }

}
