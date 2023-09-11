using UnityEngine;

public class DetectableObject : MonoBehaviour, IDetectableObject
{
    public event ObjectDetectedHandler OnGameObjectDetectEvent;
    public event ObjectDetectedHandler OnGameObjectDetectionReleasedEvent;


    public void Detected(GameObject detectionSource)
    {
        //print(detectionSource.name + " found " + gameObject.name);
        OnGameObjectDetectEvent?.Invoke(detectionSource, gameObject);
    }

    public void DetectionReleased(GameObject detectionSource)
    {
        //print(detectionSource.name + " delete released " + gameObject.name);
        OnGameObjectDetectionReleasedEvent?.Invoke(detectionSource, gameObject);
    }
}