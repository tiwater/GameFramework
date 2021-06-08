using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class CircleProgressBar : MonoBehaviour
{
    private int defaultIndicatorSize = 20;
    private int defaultComponentSize = 400;
    private int defaultRingWidth = 10;

    private float ratio = 1;

    public Color CircleBackgroundColor;
    public Color ProgressBarColor;
    public Color IndicatorColor;

    public RectTransform CircleBackground;
    public RectTransform ProgressBar;
    public RectTransform Indicator;

    public bool Clockwise
    {
        get
        {
            return _clockWise;
        }
        set
        {
            _clockWise = value;
            UpdateProgress();
        }
    }
    [SerializeField]
    private bool _clockWise;

    public Image.Origin360 FillOrigin {
        get
        {
            return _fillOrigin;
        }
        set
        {
            _fillOrigin = value;
            UpdateProgress();
        }
    }
    [SerializeField]
    private Image.Origin360 _fillOrigin = Image.Origin360.Bottom;
    public int MaxAngle = 180;
    public float MinValue = 0;
    public float MaxValue = 1;
    public float Value
    {
        get
        {
            return _value;
        }
        set
        {
            _value = value;
            UpdateProgress();
        }
    }
    [SerializeField]
    private float _value = 0.5f;

    private Image bgImage;
    private Image pbImage;
    private Image icImage;

    private RectTransform rectTransform;

    private void Awake()
    {
        //Init the component variables
        rectTransform = GetComponent<RectTransform>();

        bgImage = CircleBackground.GetComponent<Image>();
        pbImage = ProgressBar.GetComponent<Image>();
        icImage = Indicator.GetComponent<Image>();
    }

    // Start is called before the first frame update
    void Start()
    {

        UpdateProgress();
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        //For the display in editor
        UpdateProgress();
#endif
    }

    /// <summary>
    /// Update the progress bar
    /// </summary>
    private void UpdateProgress()
    {
        //Set the color
        bgImage.color = CircleBackgroundColor;
        pbImage.color = ProgressBarColor;
        icImage.color = IndicatorColor;
        //Set image type
        bgImage.fillClockwise = Clockwise;
        bgImage.fillOrigin = (int)FillOrigin;
        bgImage.fillAmount = MaxAngle / 360.0f;

        pbImage.fillClockwise = Clockwise;
        pbImage.fillOrigin = (int)FillOrigin;

        //Update the progress value
        SetProgress();
        SetIndicator();
    }

    /// <summary>
    /// Check whether the value of the progress is proper
    /// </summary>
    private void CheckValue()
    {

        if (MinValue > MaxValue)
        {
            Debug.LogWarning("MinValue " + MinValue + " > MaxValue: " + MaxValue);
            MinValue = MaxValue;
        }
        if (Value > MaxValue)
        {
            Debug.LogWarning("Value " + Value + " > MaxValue: " + MaxValue);
            Value = MaxValue;
        }
        if (Value < MinValue)
        {
            Debug.LogWarning("Value " + Value + " < MinValue: " + MinValue);
            Value = MinValue;
        }
    }

    /// <summary>
    /// Set the percentage of the progress bar
    /// </summary>
    private void SetProgress()
    {
        CheckValue();
        //Calculate the progress percentage
        float amount = (Value - MinValue) / (MaxValue - MinValue) * MaxAngle / 360;
        pbImage.fillAmount = amount;
    }

    /// <summary>
    /// Put the indicator to the right place
    /// </summary>
    private void SetIndicator()
    {
        //Calculate the scale
        float w = rectTransform.rect.width;
        float h = rectTransform.rect.height;

        ratio = Mathf.Min( w / defaultComponentSize, h/ defaultComponentSize);

        //Calculate the indicator angle to x axis
        float angle = 0;
        switch (FillOrigin)
        {
            case Image.Origin360.Bottom:
                angle = -90;
                break;
            case Image.Origin360.Left:
                angle = 180;
                break;
            case Image.Origin360.Top:
                angle = 90;
                break;
            default:
                break;
        }
        angle += (pbImage.fillAmount * 360) * (Clockwise ? -1 : 1);

        float r = defaultComponentSize / 2.0f - defaultRingWidth / 2.0f;

        //To radians
        angle = angle / 360.0f * 2 * Mathf.PI;

        //The position of the indicator
        float x = r * Mathf.Cos(angle) * ratio;
        float y = r * Mathf.Sin(angle) * ratio;

        //Update the indicator's position and scale
        Indicator.localPosition = new Vector3(x, y, Indicator.position.z);
        Indicator.localScale = new Vector3(ratio, ratio, 1);
    }
}
