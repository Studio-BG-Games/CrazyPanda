using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// класс находит и подписывается на объекты которые прикасаются с коллайдерами, а так же отписывается из них
// записывание и отписывание через List путем вызова событий

[RequireComponent(typeof(DetectableObject))] 
public class DetectorForEnemy : MonoBehaviour
{
    private IDetectableObject _detectableObject;

    void Awake()
    {
        _detectableObject = GetComponent<IDetectableObject>();
    }
    
    void OnEnable() // подписываемся на события
    {
        _detectableObject.OnGameObjectDetectEvent += OnGameObjectDetect;
        _detectableObject.OnGameObjectDetectionReleasedEvent += OnGameObjectDetectionReleased;
    }

    void OnDisable() // отписываемся от события
    {
        _detectableObject.OnGameObjectDetectEvent -= OnGameObjectDetect;
        _detectableObject.OnGameObjectDetectionReleasedEvent -= OnGameObjectDetectionReleased;
    }

    // вызывавется при входе нового объекта в зону триггера
    private void OnGameObjectDetect(GameObject source, GameObject detectedObject) 
    {
        print("Кто вошел=" + detectedObject.name + "  Кто обнаружил=" + source.name);
    }

    // вызывавется при выходе объекта из зоны триггера
    private void OnGameObjectDetectionReleased(GameObject source, GameObject detectedObject)
    {
        print("Кто вышел=" + detectedObject.name + "  Кто обнаружил выход=" + source.name);
    }


}
