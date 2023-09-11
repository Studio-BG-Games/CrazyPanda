using UnityEngine;


public interface IDetectableObject
    {
        event ObjectDetectedHandler OnGameObjectDetectEvent; // меня обнаружили
        event ObjectDetectedHandler OnGameObjectDetectionReleasedEvent; // меня потеряли

        GameObject gameObject{ get; }

        void Detected(GameObject detectionSource); // тот кто обнаружил объект
        void DetectionReleased(GameObject detectionSource); // кого обнаружили
    }

