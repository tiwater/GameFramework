using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameFramework.Display.Placement.Components
{
    [ExecuteInEditMode]
    public class CircleProgressBar : FloatValueWidget
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

        public bool ShowBackground {
            get {
                return _showBackground;
            }
            set
            {
                _showBackground = value;
                UpdateBehavior();
            }
        }
        [Tooltip("Whether show the background Image")]
        [SerializeField]
        private bool _showBackground = true;

        public bool ShowIndicator
        {
            get
            {
                return _showIndicator;
            }
            set
            {
                _showIndicator = value;
                UpdateBehavior();
            }
        }
        [Tooltip("Whether show the idicator")]
        [SerializeField]
        private bool _showIndicator = true;

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
        public Image.Origin360 FillOrigin
        {
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
            SetProgress(CurrentValue);
            SetIndicator();
        }

        // Update is called once per frame
        void Update()
        {
#if UNITY_EDITOR
            //For the display in editor
            UpdateBehavior();
#endif
            if (Value != CurrentValue)
            {
                UpdateProgress();
            }
        }

        protected override void OnValueChanged(float oldValue, float newValue)
        {
            base.OnValueChanged(oldValue, newValue);
            UpdateBehavior();
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

            //Enable/Disable the images
            bgImage.gameObject.SetActive(ShowBackground);
            icImage.gameObject.SetActive(ShowIndicator);
        }

        /// <summary>
        /// Update the progress bar according to the value
        /// </summary>
        private void UpdateProgress()
        {
            UpdateCurrentValue();

            //Update the progress value
            //For the display in editor
            if (Application.IsPlaying(gameObject))
            {
                SetProgress(CurrentValue);
            }
            else
            {
                SetProgress(Value);
            }
            SetIndicator();
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

            ratio = Mathf.Min(w / defaultComponentSize, h / defaultComponentSize);

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
}