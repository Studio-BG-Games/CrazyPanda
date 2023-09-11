using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour, IDetector
{
    public event ObjectDetectedHandler OnGameObjectDetectedEvent;
    public event ObjectDetectedHandler OnGameObjectDetectionReleasedEvent;

    public GameObject[] detectedObjects => _detectedObjects.ToArray(); // для доступа к списку обнаруженных объектов из других скриптов
    private List<GameObject> _detectedObjects = new List<GameObject>(); // список объектов которые были найдены

    public void Detect(IDetectableObject detectableObject)
    {
        if(!_detectedObjects.Contains(detectableObject.gameObject)) // если нет этого объекта
        {
            detectableObject.Detected(gameObject); // сообщаем какой объект обнаружили
            _detectedObjects.Add(detectableObject.gameObject); // добавляем в список этот объект

            OnGameObjectDetectedEvent?.Invoke(source:gameObject, detectableObject.gameObject); // сообщаем кому то что объект обнаружен и какой именно
        }
    }

    public void Detect(GameObject detectedObject)
    {
         if(!_detectedObjects.Contains(detectedObject)) // если нет этого объекта
        {
            _detectedObjects.Add(detectedObject); // добавляем в список этот объект

            OnGameObjectDetectedEvent?.Invoke(source:gameObject, detectedObject); // сообщаем кому то что объект обнаружен и какой именно
        }
    }

    public void ReleaseDetection(IDetectableObject detectableObject)
    {
        if(_detectedObjects.Contains(detectableObject.gameObject)) // если нет этого объекта
        {
            detectableObject.DetectionReleased(gameObject); // сообщаем какой объект обнаружили
            _detectedObjects.Remove(detectableObject.gameObject); // добавляем в список этот объект

            OnGameObjectDetectionReleasedEvent?.Invoke(source:gameObject, detectableObject.gameObject); // сообщаем кому то что объект удален и какой именно
        }
    }

    public void ReleaseDetection(GameObject detectedObject)
    {
        print("release...");
        if(_detectedObjects.Contains(detectedObject)) // если нет этого объекта
        {
            _detectedObjects.Remove(detectedObject); // сообщаем какой объект удалили
            print("release2...");

            OnGameObjectDetectionReleasedEvent?.Invoke(source:gameObject, detectedObject); // сообщаем кому то что объект удален и какой именно
        }
    }


    void OnTriggerEnter(Collider col) 
    {
        if(IsColliderDetectableObject(col, out var detectedObject)) // проверка является ли объект обнаруживаемым
        {
            Detect(detectedObject); //записываем в список
        }
    }
    void OnTriggerExit(Collider col)
    {
        if(IsColliderDetectableObject(col, out var detectedObject)) // проверка является ли объект обнаруживаемым
        {
            ReleaseDetection(detectedObject); //записываем в список
        }
    }

    private bool IsColliderDetectableObject(Collider coll, out IDetectableObject detectedObject) // проверяем есть ли подходящий объект
    {
        detectedObject = coll.GetComponentInParent<IDetectableObject>(); // проверяем есть ли нужный коллайдер в дочерних объектах
        //detectedObject = coll.GetComponent<IDetectableObject>(); // проверяем есть ли нужный коллайдер на объекте родительском

        

        return detectedObject != null;
    }
}
