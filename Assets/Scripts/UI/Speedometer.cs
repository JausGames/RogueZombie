using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Speedometer : MonoBehaviour
{
    private CarEngine engine;

    private float currSpeed;
    private float maxSpeed = 200f;

    private Transform needleTransform;
    private Transform speedLabelTemplateTransform;
    public Transform uiTransform;

    private List<GameObject> labels; 

    private const float MAX_NEEDLE_ROTATION = -90f;
    private const float ZERO_NEEDLE_ROTATION = 160f;

    private void Awake()
    {
        engine = GetComponentInParent<CarEngine>();
        //_rigidbody = engine.GetRigidbody();
        needleTransform = transform.Find("Image").Find("needle");
        speedLabelTemplateTransform = transform.Find("speedLabelTemplate");
        speedLabelTemplateTransform.gameObject.SetActive(false);

    }
    private void Start()
    {

        SetMaxSpeed();
        CreateSpeedLabels();
    }
    private void FixedUpdate()
    {
        GetSpeed();
        needleTransform.eulerAngles = new Vector3(needleTransform.eulerAngles.x, needleTransform.eulerAngles.y, GetSpeedRotation());
    }

    private void CreateSpeedLabels()
    {
        Debug.Log("CreateSpeedLabels, RountToInt : " + Mathf.RoundToInt(maxSpeed));
        int labelAmount = (Mathf.RoundToInt(maxSpeed) / 20);
        float totalAngleSize = ZERO_NEEDLE_ROTATION - MAX_NEEDLE_ROTATION;

        for (int i = 0; i <= labelAmount; i++)
        {
            Transform speedLabelTransform = Instantiate(speedLabelTemplateTransform, transform);
            //labels.Add(speedLabelTransform.gameObject);
            float labelSpeedNormalized = ((float)i )/ ((float)labelAmount);
            // un peu louche ce qu'il ce passe juste en dessous
            float speedLabelAngle = -(ZERO_NEEDLE_ROTATION - labelSpeedNormalized * totalAngleSize) + 8f;
            var magn = new Vector3(0, 0, -(speedLabelAngle - 95f)).magnitude;
            Debug.Log("CreateSpeedLabels, labelAmount : " + labelAmount);
            Debug.Log("CreateSpeedLabels, totalAngleSize : " + totalAngleSize);
            Debug.Log("CreateSpeedLabels, labelSpeedNormalized : " + labelSpeedNormalized);
            Debug.Log("CreateSpeedLabels, speedLabelAngle : " + speedLabelAngle);
            Debug.Log("CreateSpeedLabels, magnitude : " + magn);
            speedLabelTransform.eulerAngles = new Vector3(0, 0, -(speedLabelAngle - 95f));
            speedLabelTransform.Find("speedText").GetComponent<Text>().text = Mathf.RoundToInt(i * 20f).ToString();
            speedLabelTransform.Find("speedText").eulerAngles = Vector3.zero;
            speedLabelTransform.gameObject.SetActive(true);
        }
    }

    public float GetSpeed()
    {
        currSpeed = engine.GetSpeed();
        return currSpeed;
    }

    public float GetSpeedRotation()
    {
        float totalAngleSize =  ZERO_NEEDLE_ROTATION - MAX_NEEDLE_ROTATION;
        float normalizeSpeed = GetSpeed() / maxSpeed;
        normalizeSpeed = Mathf.Clamp(normalizeSpeed, 0f, 1.5f);
        return ZERO_NEEDLE_ROTATION - normalizeSpeed * totalAngleSize;
    }

    public void SetMaxSpeed()
    {
        maxSpeed = engine.GetMaxSpeed();
    }


}
