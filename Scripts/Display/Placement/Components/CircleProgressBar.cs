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

    /// <summary>
    /// The progress increment direction
    /// </summary>
    public bool Clockwise
    {
        get
        {
            return _clockWise;
        }
        set
        {
            _clockWise = value;
            UpdateBehavior();
        }
    }
    [Tooltip("The progress bar increment direction")]
    [SerializeField]
    private bool _clockWise;

    /// <summary>
    /// The start position
    /// </summary>
    public Image.Origin360 FillOrigin {
        get
        {
            return _fillOrigin;
        }
        set
        {
            _fillOrigin = value;
            UpdateBehavior();
        }
    }
    [Tooltip("The start position")]
    [SerializeField]
    private Image.Origin360 _fillOrigin = Image.Origin360.Bottom;

    /// <summary>
    /// The max rotation angle of the circle progress bar
    /// </summary>
    [Tooltip("The max rotation angle of the circle progress bar")]
    public int MaxAngle = 180;

    /// <summary>
    /// The max value the progress bar accepts
    /// </summary>
    [Tooltip("The max value the progress bar accepts")]
    public float MinValue = 0;

    /// <summary>
    /// The min value the progress bar accepts
    /// </summary>
    [Tooltip("The min value the progress bar accepts")]
    public float MaxValue = 1;

    /// <summary>
    /// The value of the progress
    /// </summary>
    public float Value
    {
        get
        {
            return _value;
        }
        set
        {
            _value = value;
            CheckValue();
            UpdateBehavior();
        }
    }
    [Tooltip("The value of the progress")]
    [SerializeField]
    private float _value = 0.5f;

    /// <summary>
    /// Whether the progress of the value changing is displayed in animation
    /// </summary>
    [Tooltip("Whether the progress of the value changing is displayed in animation")]
    public bool AnimatedProgress = true;

    /// <summary>
    /// The speed of the progress bar changing. Ignored when AnimatedProgress is false
    /// </summary>
    [Tooltip("The speed of the progress bar changing. Ignored when AnimatedProgress is false")]
    public float Speed = 20;


    private float currentValue = 0;

    public bool KeepIndicatorInBar = true;
    //If multiple progress bars are placed by joined together, the indicator might be overlapped by another bar's background
    //So put a space for the indicator to avoid the overlap
    private float indicatorSpace = 0.03f;

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
        CheckValue();
        UpdateBehavior();
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        //For the display in editor
        UpdateBehavior();
#endif
        if (Value != currentValue)
        {
            UpdateProgress();
        }
    }

    /// <summary>
    /// Update the progress bar view
    /// </summary>
    private void UpdateBehavior()
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
    }

    /// <summary>
    /// Update the progress bar according to the value
    /// </summary>
    private void UpdateProgress()
    {
        if (AnimatedProgress)
        {
            float delta = Speed * Time.deltaTime;
            float gap = Value - currentValue;
            if (delta > Mathf.Abs(gap))
            {
                //If move too much, then we are at the target position now
                currentValue = Value;
            }
            else
            {
                //Decide the move direction
                if (gap > 0)
                {
                    currentValue += delta;
                }
                else
                {
                    currentValue -= delta;
                }
            }
        }
        else
        {
            currentValue = Value;
        }

        //Update the progress value
        //For the display in editor
        if (Application.IsPlaying(gameObject))
        {
            SetProgress(currentValue);
        }
        else
        {
            SetProgress(Value);
        }
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
    private void SetProgress(float progress)
    {
        //Calculate the progress percentage
        float amount = (progress - MinValue) / (MaxValue - MinValue) * MaxAngle / 360;
        if (KeepIndicatorInBar)
        {
            //Set buffer for the indicator
            pbImage.fillAmount = amount * (1 - indicatorSpace * 2) + indicatorSpace * MaxAngle / 360;
        }
        else
        {
            pbImage.fillAmount = amount;
        }
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
        //The start angle
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
        //The angle for the progress
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
