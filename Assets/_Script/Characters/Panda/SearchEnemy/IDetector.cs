using UnityEngine;


    public delegate void ObjectDetectedHandler(GameObject source, GameObject detectedObject);

    public interface IDetector 
    {
        event ObjectDetectedHandler OnGameObjectDetectedEvent; // мы обнаружили объект
        event ObjectDetectedHandler OnGameObjectDetectionReleasedEvent; // обнаруженный объект пропал
        
        // обнаружить объект
        void Detect(IDetectableObject detectableObject); 
        void Detect(GameObject detectedObject);

        // убрать из списка обнаруженных
        void ReleaseDetection(IDetectableObject detectableObject);
        void ReleaseDetection(GameObject detectedObject);
        
    
}