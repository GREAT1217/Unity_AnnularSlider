/*************************************************************************
 *  Copyright © 2021 Great1217. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  Test.cs
 *  Description  :  Null.
 *------------------------------------------------------------------------
 *  Author       :  Great1217
 *  Version      :  0.1.0
 *  Date         :  13/04/2021
 *  Description  :  Initial development version.
 *************************************************************************/

using UnityEngine;
using UnityEngine.UI;

public class AngleCalculator : MonoBehaviour
{
    [System.Serializable]
    public class KeyPoint
    {
        [SerializeField] private Text _textS0;
        [SerializeField] private Text _textS1;
        [SerializeField] private Text _textO0;
        [SerializeField] private Text _textO1;
        private int _angleS0;
        private int _angleS1;
        private int _angleO0;
        private int _angleO1;

        public void Standard_ResetAngle(int index, bool clockwise)
        {
            var angle = index * 90; //0~270
            SetAngle(angle, out _angleS0, out _angleS1, clockwise);


            _textS0.text = _angleS0.ToString();
            _textS1.text = _angleS1.ToString();
        }

        public void Operation_ResetAngle(int index, bool clockwise)
        {
            var angle = index * 90; //0~270
            SetAngle(angle, out _angleO0, out _angleO1, clockwise);

            _textO0.text = _angleO0.ToString();
            _textO1.text = _angleO1.ToString();
        }

        public void Operation_AddAngleOffset(int offset, bool clockwise)
        {
            var angle = ClampAngle(_angleO0 + offset);
            SetAngle(angle, out _angleO0, out _angleO1, clockwise);

            _textO0.text = _angleO0.ToString();
            _textO1.text = _angleO1.ToString();
        }

        private void SetAngle(int angle, out int angleA, out int angleB, bool clockwise)
        {
            if (angle == 0 || angle == 360)
            {
                if (clockwise)
                {
                    angleA = 360;
                    angleB = 0;
                }
                else
                {
                    angleA = 0;
                    angleB = 360;
                }
            }
            else
            {
                angleA = angleB = angle;
            }
        }
    }

    private const int PointCount = 4;
    private readonly int[] PointOrderStandard = {0, 1, 2, 3};
    private readonly int[] PointOrderOperation = {0, 1, 2, 3};

    [SerializeField] private KeyPoint[] _points = new KeyPoint[PointCount];

    [Header("standard")] [SerializeField] private Toggle _clockwiseS;
    [SerializeField] private Text _clockwiseSText;
    [SerializeField] private Toggle _right;
    [SerializeField] private Toggle _bottom;
    [SerializeField] private Toggle _left;
    [SerializeField] private Toggle _top;

    [Header("operation")] [SerializeField] private Toggle _clockwiseO;
    [SerializeField] private Text _clockwiseOText;
    [SerializeField] private Button _add90;
    [SerializeField] private Button _add180;
    [SerializeField] private Button _minus90;
    [SerializeField] private Button _minus180;
    [SerializeField] private Button _reset;
    [SerializeField] private Text _debug;

    private Toggle _currentOriginPoint;

    private void Start()
    {
        _clockwiseS.onValueChanged.AddListener(Standard_ResetClockWise);
        _right.onValueChanged.AddListener(b =>
        {
            _currentOriginPoint = _right;
            Standard_ResetOriginPoint(b, PointOrderStandard[0]);
        });
        _bottom.onValueChanged.AddListener(b =>
        {
            _currentOriginPoint = _bottom;
            Standard_ResetOriginPoint(b, PointOrderStandard[1]);
        });
        _left.onValueChanged.AddListener(b =>
        {
            _currentOriginPoint = _left;
            Standard_ResetOriginPoint(b, PointOrderStandard[2]);
        });
        _top.onValueChanged.AddListener(b =>
        {
            _currentOriginPoint = _top;
            Standard_ResetOriginPoint(b, PointOrderStandard[3]);
        });

        _clockwiseO.onValueChanged.AddListener(Operation_ResetClockWise);
        _add90.onClick.AddListener(() => Operation_AddAngleOffset(90));
        _add180.onClick.AddListener(() => Operation_AddAngleOffset(180));
        _minus90.onClick.AddListener(() => Operation_AddAngleOffset(-90));
        _minus180.onClick.AddListener(() => Operation_AddAngleOffset(-180));
        _reset.onClick.AddListener(Operation_ResetAngle);

        _right.isOn = true;
        Operation_ResetAngle();
    }

    /// <summary>
    /// 标准角度-重置顺时针或逆时针
    /// </summary>
    /// <param name="clockwise"></param>
    private void Standard_ResetClockWise(bool clockwise)
    {
        _clockwiseSText.text = clockwise ? "Clockwise" : "Anticlockwise";

        //重置顺序
        for (int i = 0; i < PointCount; i++)
        {
            PointOrderStandard[i] = GetClockwiseIndex(PointOrderStandard[i]);
        }

        //重置起点
        _currentOriginPoint.onValueChanged.Invoke(true);
        //重置角度
        Operation_ResetAngle();
    }

    /// <summary>
    /// 标准角度-重置起点
    /// </summary>
    /// <param name="set"></param>
    /// <param name="index"></param>
    private void Standard_ResetOriginPoint(bool set, int index)
    {
        if (!set) return;

        var offset = PointCount - index;
        for (int i = 0; i < PointCount; i++)
        {
            var offsetIndex = (i + offset) % PointCount;
            _points[PointOrderStandard[i]].Standard_ResetAngle(offsetIndex, _clockwiseS.isOn);
        }
    }

    /// <summary>
    /// 操作角度-重置顺时针或逆时针
    /// </summary>
    /// <param name="clockwise"></param>
    private void Operation_ResetClockWise(bool clockwise)
    {
        _clockwiseOText.text = clockwise ? "Clockwise" : "Anticlockwise";

        //重置顺序
        for (int i = 0; i < PointCount; i++)
        {
            PointOrderOperation[i] = GetClockwiseIndex(PointOrderOperation[i]);
        }

        //重置角度
        Operation_ResetAngle();
    }

    /// <summary>
    /// 操作角度-重置角度
    /// 重置回最原始（起点为右侧）的角度
    /// </summary>
    private void Operation_ResetAngle()
    {
        for (int i = 0; i < PointCount; i++)
        {
            _points[i].Operation_ResetAngle(PointOrderOperation[i], _clockwiseO.isOn);
        }

        _debug.text = "";
    }

    /// <summary>
    /// 操作角度-添加角度偏移
    /// </summary>
    /// <param name="offset"></param>
    private void Operation_AddAngleOffset(int offset)
    {
        for (int i = 0; i < PointCount; i++)
        {
            _points[i].Operation_AddAngleOffset(offset, _clockwiseO.isOn);
        }

        if (offset > 0)
        {
            _debug.text += "+";
        }

        _debug.text += offset.ToString();
    }

    /// <summary>
    /// 顺时针和逆时针互转
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    private static int GetClockwiseIndex(int index)
    {
        return (PointCount - index) % PointCount;
    }

    /// <summary>
    /// 限制角度
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    private static int ClampAngle(int angle)
    {
        if (angle < 0)
        {
            angle += 360;
        }
        else if (angle > 360)
        {
            angle -= 360;
        }

        return angle;
    }
}