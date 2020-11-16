/*************************************************************************
 *  Copyright © 2020 Great1217. All rights reserved.
 *------------------------------------------------------------------------
 *  File         :  AnnularSliderEditor.cs
 *  Description  :  Null.
 *------------------------------------------------------------------------
 *  Author       :  Great1217
 *  Version      :  0.1.0
 *  Date         :  14/11/2020
 *  Description  :  Initial development version.
 *************************************************************************/

using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(AnnularSlider), true), CanEditMultipleObjects]
public class AnnularSliderEditor : SelectableEditor
{
    private SerializedProperty _fillImage;
    private SerializedProperty _handleRect;
    private SerializedProperty _radius;
    private SerializedProperty _fillOrigin;
    private SerializedProperty _clockwise;
    private SerializedProperty _minValue;
    private SerializedProperty _maxValue;
    private SerializedProperty _value;
    private SerializedProperty _wholeNumbers;
    private SerializedProperty _maxAngle;
    private SerializedProperty _onValueChanged;
    private SerializedProperty _onBeginDragged;
    private SerializedProperty _onDragging;
    private SerializedProperty _onEndDragged;

    protected override void OnEnable()
    {
        base.OnEnable();
        _fillImage = serializedObject.FindProperty("_fillImage");
        _handleRect = serializedObject.FindProperty("_handleRect");
        _radius = serializedObject.FindProperty("_radius");
        _fillOrigin = serializedObject.FindProperty("_fillOrigin");
        _clockwise = serializedObject.FindProperty("_clockwise");
        _minValue = serializedObject.FindProperty("_minValue");
        _maxValue = serializedObject.FindProperty("_maxValue");
        _value = serializedObject.FindProperty("_value");
        _wholeNumbers = serializedObject.FindProperty("_wholeNumbers");
        _maxAngle = serializedObject.FindProperty("_maxAngle");
        _onValueChanged = serializedObject.FindProperty("_onValueChanged");
        _onBeginDragged = serializedObject.FindProperty("_onBeginDragged");
        _onDragging = serializedObject.FindProperty("_onDragging");
        _onEndDragged = serializedObject.FindProperty("_onEndDragged");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(_fillImage);
        EditorGUILayout.PropertyField(_handleRect);

        if (_fillImage.objectReferenceValue != null)
        {
            EditorGUILayout.PropertyField(_fillOrigin);
            EditorGUILayout.PropertyField(_clockwise);
            EditorGUILayout.PropertyField(_minValue);
            EditorGUILayout.PropertyField(_maxValue);
            EditorGUILayout.Slider(_value, _minValue.floatValue, _maxValue.floatValue);
            EditorGUILayout.PropertyField(_wholeNumbers);
        }

        if (_handleRect.objectReferenceValue != null)
        {
            EditorGUILayout.PropertyField(_radius);
        }

        EditorGUILayout.Slider(_maxAngle, 0f, 360f);

        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(_onValueChanged);
        EditorGUILayout.PropertyField(_onBeginDragged);
        EditorGUILayout.PropertyField(_onDragging);
        EditorGUILayout.PropertyField(_onEndDragged);

        serializedObject.ApplyModifiedProperties();
    }
}