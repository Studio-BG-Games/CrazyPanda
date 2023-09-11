using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UICameraControlPosition : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform targetArround; // цель вокруг которой происходят перемещения

    public float distance;
    float yPos; // позиция камеры по высоте (средняя кнопка мышки)

    [Range(0.01f, 200.0f)]
    public float speedX = 0.5f;
    [Range(0.01f, 200.0f)]
    public float speedY = 0.5f;

    [Range(0.01f, 3.0f)]
    public float speedXAutorotate = 0.5f;

    public float smoothTime = 2f;
    public float rotationYAxis = 0.0f;
    public float rotationXAxis = 0.0f;
    float velocityX = 0.0f;
    float velocityY = 0.0f;
    [Space(5)]
    public float yMinLimit = 5;
    public float yMaxLimit = 90;

    public bool autorotate;
    public bool homePositiom; // если да камера возвращается к исходной позиции (для окон)

    Vector2 deltaTouch; // дельта свайпов для EventSystems
    float distancePitch; // расстояние между пальцами

    private float timeBefor; // время предыдущего Update
    private float myDeltaTime; // свой Deltatime т.к. при паузе он равен 0
    

    

    void Start(){
        yPos = targetArround.position.y;
    }


    void Update()
    {
        
        if (Input.GetMouseButton(0))
        { // вращение мышкой


            /*velocityX -= speedX * Input.GetTouch(0).deltaPosition.x * 0.02f;
            velocityY -= speedY * Input.GetTouch(0).deltaPosition.y * 0.02f;
*/
            //print(Input.GetTouch(0).deltaPosition);

            velocityX -= speedX * Input.GetAxis("Mouse X");
            //velocityY -= speedY * Input.GetAxis("Mouse Y");
        }else{ // автовращение
            if (autorotate && !homePositiom) 
            { // автовращение
                velocityY += speedXAutorotate * 0.01f; // скорость вращения
                transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.localEulerAngles.z);
                
                rotationYAxis = Mathf.Lerp(rotationYAxis, 0, Time.deltaTime / 6); // возврат врашения камеры по вертикали в середину
				distance = Mathf.Lerp(distance, 140, Time.deltaTime / 6);
			}
        }

        /*if (homePositiom) {
            rotationXAxis = Mathf.Lerp(rotationXAxis, 0, Time.deltaTime * 2); // возврат врашения камеры по горизонтали в середину
            rotationYAxis = Mathf.Lerp(rotationYAxis, 0, Time.deltaTime * 2); // возврат врашения камеры по вертикали в середину

            distance = Mathf.Lerp(distance, 140, Time.deltaTime * 2); // возврат дистанции камеры от цели
            yPos = Mathf.Lerp(yPos, 33, Time.deltaTime * 2); // высота цели и камеры (средняя мышка)
        }*/


        //rotationYAxis += velocityY;
        rotationXAxis -= velocityX;
/*
        if (targetArround.position.y < 5)
            rotationYAxis = ClampAngle(rotationYAxis, 5, 90);
        if ((targetArround.position.y >= 5) && (targetArround.position.y < 10))
            rotationYAxis = ClampAngle(rotationYAxis, -5, 90);
        if (targetArround.position.y >= 10)
            rotationYAxis = ClampAngle(rotationYAxis, -10, 90);
*/

        Quaternion toRotation = Quaternion.Euler(rotationYAxis, rotationXAxis, 0);
        Quaternion rotation = toRotation;
        //print("E = " + rotation);
        transform.rotation = rotation;

        transform.position = transform.rotation * new Vector3(0, 0, -distance) + targetArround.position; // zoom/out
        
        
        

        myDeltaTime = Time.time - timeBefor;

        velocityX = Mathf.Lerp(velocityX, 0, Time.unscaledDeltaTime * smoothTime);
        //velocityY = Mathf.Lerp(velocityY, 0, Time.deltaTime * smoothTime);
        timeBefor = Time.time;

        // дистанция до цели
        //print(" distance = " +distance);
        /*    distance -= Input.GetAxis("Mouse ScrollWheel") * 3.0f;
        if (distance < 0.5f && targetArround.position.y < 36) distance = 2;
        if (distance < 25 && targetArround.position.y > 36) distance = 1;
        //if (distance > 200) distance = 10;*/


        // смещение цели
        /*if (Input.GetMouseButton(2)){ // midle button mouse
            yPos -= Input.GetAxis("Mouse Y");
            if (targetArround.position.y < 0.5f)
                yPos = 0.5f;
            if (targetArround.position.y > 100.0f)
                yPos = 100.0f;            
        }
        targetArround.position = new Vector3(targetArround.position.x, yPos, targetArround.position.z);*/

          
          /*if(Input.touchCount == 2) // если да выполняем зум
          {
              Zoom();
          }else if (distancePitch != 0) distancePitch = 0;*/
    }

    /*void Zoom()
    {
        Vector2 touch1 = Input.GetTouch(0).position;
        Vector2 touch2 = Input.GetTouch(1).position;

        if(distancePitch == 0)
            distancePitch = Vector2.Distance(touch1, touch2);

        float delta = Vector2.Distance(touch1, touch2) - distancePitch;

        transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, distancePitch * Time.deltaTime);
        distancePitch = Vector2.Distance(touch1, touch2);
    }*/


    float ClampAngle(float angle, float min, float max) {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }




    public void OnBeginDrag(PointerEventData eventData) // IBeginDragHandler
    {        
        print("START DRAG");
    }
    public void OnDrag(PointerEventData eventData) // IDragHandler
    {
        deltaTouch = Input.GetTouch(0).deltaPosition;

        if(Mathf.Abs(deltaTouch.x) > Mathf.Abs(deltaTouch.y))
        {
            if(deltaTouch.x > 0)
                print("Right");
                else
                print("Left");
        }
        else
        {
             if(deltaTouch.y > 0)
                print("Up");
                else
                print("Down");
        }
       print("DRAG");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        print("END DRAG");
    }
    
}
