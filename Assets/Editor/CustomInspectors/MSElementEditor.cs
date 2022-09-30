using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MenuScreenElement))]
public class MSElementEditor : Editor
{
    private SerializedProperty _rectProp;
    private SerializedProperty _graphicProp;
    private SerializedProperty _groupProp;

    private SerializedProperty _rectInEaseProp;
    private SerializedProperty _rectInPosProp;
    private SerializedProperty _rectInScaleProp;
    private SerializedProperty _rectOutEaseProp;
    private SerializedProperty _rectOutPosProp;
    private SerializedProperty _rectOutScaleProp;

    private SerializedProperty _graphicInEaseProp;
    private SerializedProperty _graphicInColorProp;
    private SerializedProperty _graphicInAlphaProp;
    private SerializedProperty _graphicOutEaseProp;
    private SerializedProperty _graphicOutColorProp;
    private SerializedProperty _graphicOutAlphaProp;

    private SerializedProperty _groupInEaseProp;
    private SerializedProperty _groupInAlphaProp;
    private SerializedProperty _groupOutEaseProp;
    private SerializedProperty _groupOutAlphaProp;

    private void OnEnable()
    {
        _rectProp = serializedObject.FindProperty("_rect");
        _graphicProp = serializedObject.FindProperty("_graphic");
        _groupProp = serializedObject.FindProperty("_group");

        _rectInEaseProp = serializedObject.FindProperty("_rectInTween").FindPropertyRelative("ease");
        _rectInPosProp = serializedObject.FindProperty("_rectInTween").FindPropertyRelative("anchoredPosition");
        _rectInScaleProp = serializedObject.FindProperty("_rectInTween").FindPropertyRelative("localScale");
        _rectOutEaseProp = serializedObject.FindProperty("_rectOutTween").FindPropertyRelative("ease");
        _rectOutPosProp = serializedObject.FindProperty("_rectOutTween").FindPropertyRelative("anchoredPosition");
        _rectOutScaleProp = serializedObject.FindProperty("_rectOutTween").FindPropertyRelative("localScale");

        _graphicInEaseProp = serializedObject.FindProperty("_graphicInTween").FindPropertyRelative("ease");
        _graphicInColorProp = serializedObject.FindProperty("_graphicInTween").FindPropertyRelative("color");
        _graphicInAlphaProp = serializedObject.FindProperty("_graphicInTween").FindPropertyRelative("alpha");
        _graphicOutEaseProp = serializedObject.FindProperty("_graphicOutTween").FindPropertyRelative("ease");
        _graphicOutColorProp = serializedObject.FindProperty("_graphicOutTween").FindPropertyRelative("color");
        _graphicOutAlphaProp = serializedObject.FindProperty("_graphicOutTween").FindPropertyRelative("alpha");

        _groupInEaseProp = serializedObject.FindProperty("_groupInTween").FindPropertyRelative("ease");
        _groupInAlphaProp = serializedObject.FindProperty("_groupInTween").FindPropertyRelative("alpha");
        _groupOutEaseProp = serializedObject.FindProperty("_groupOutTween").FindPropertyRelative("ease");
        _groupOutAlphaProp = serializedObject.FindProperty("_groupOutTween").FindPropertyRelative("alpha");
    }

    public override void OnInspectorGUI()
    {
        MenuScreenElement tgt = target as MenuScreenElement;

        HandleTransform(tgt);
        HandleGraphic(tgt);
        HandleCanvasGroup(tgt);
        serializedObject.ApplyModifiedProperties();

        DrawTestButtons();
    }

    private void DrawTestButtons()
    {
        GUILayout.Space(16);
        
        EditorGUI.BeginDisabledGroup(Application.isPlaying == false);
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Animate In", GUILayout.Height(48)))
        {
            (target as MenuScreenElement).AnimateIn();
        }
        if (GUILayout.Button("Animate Out", GUILayout.Height(48)))
        {
            (target as MenuScreenElement).AnimateOut();
        }

        EditorGUILayout.EndHorizontal();
        EditorGUI.EndDisabledGroup();
    }

    private void HandleTransform(MenuScreenElement tgt)
    {
        #if UNITY_EDITOR
        // Top level toggle to decide if the rect will be tweened or not
        bool doRect = EditorGUILayout.Toggle("Tween RectTransform", tgt.DoRect);
        if (doRect != tgt.DoRect)
        {
            tgt.DoRect = doRect;
            EditorUtility.SetDirty(tgt);
        }

        if (doRect == false)
            return;
        
        EditorGUI.indentLevel = 2;
        EditorGUILayout.PropertyField(_rectProp);

        var inTween = tgt.RectInTween;
        bool doInTween = EditorGUILayout.Toggle("Animate In", inTween.doTween);

        if (doInTween)
        {
            DrawPositionTweenControls(inTween, _rectInEaseProp, _rectInPosProp, _rectInScaleProp);
        }

        var outTween = tgt.RectOutTween;
        bool doOutTween = EditorGUILayout.Toggle("Animate Out", outTween.doTween);

        if (doOutTween)
        {
            DrawPositionTweenControls(outTween, _rectOutEaseProp, _rectOutPosProp, _rectOutScaleProp);
        }

        if (inTween.doTween != doInTween || outTween.doTween != doOutTween)
        {
            inTween.doTween = doInTween;
            outTween.doTween = doOutTween;
            EditorUtility.SetDirty(target);
        }

        EditorGUI.indentLevel = 0;
        #endif
    }

    private void HandleGraphic(MenuScreenElement tgt)
    {
        #if UNITY_EDITOR
        // Top level toggle to decide if the graphic will be tweened or not
        bool doGraphic = EditorGUILayout.Toggle("Tween Graphic", tgt.DoGraphic);
        if (doGraphic != tgt.DoGraphic)
        {
            tgt.DoGraphic = doGraphic;
            EditorUtility.SetDirty(tgt);
        }

        if (doGraphic == false)
            return;
        
        EditorGUI.indentLevel = 2;
        EditorGUILayout.PropertyField(_graphicProp);

        var inTween = tgt.GraphicInTween;
        bool doInTween = EditorGUILayout.Toggle("Animate In", inTween.doTween);

        if (doInTween)
        {
            DrawGraphicTweenControls(inTween, _graphicInEaseProp, _graphicInColorProp, _graphicInAlphaProp);
        }

        var outTween = tgt.GraphicOutTween;
        bool doOutTween = EditorGUILayout.Toggle("Animate Out", outTween.doTween);

        if (doOutTween)
        {
            DrawGraphicTweenControls(outTween, _graphicOutEaseProp, _graphicOutColorProp, _graphicOutAlphaProp);
        }

        if (inTween.doTween != doInTween || outTween.doTween != doOutTween)
        {
            inTween.doTween = doInTween;
            outTween.doTween = doOutTween;
            EditorUtility.SetDirty(target);
        }

        EditorGUI.indentLevel = 0;
        #endif
    }

    
    private void HandleCanvasGroup(MenuScreenElement tgt)
    {
        #if UNITY_EDITOR
        // Top level toggle to decide if the CanvasGroup will be tweened or not
        bool doGroup = EditorGUILayout.Toggle("Tween CanvasGroup", tgt.DoGroup);
        if (doGroup != tgt.DoGroup)
        {
            tgt.DoGroup = doGroup;
            EditorUtility.SetDirty(tgt);
        }

        if (doGroup == false)
            return;
        
        EditorGUI.indentLevel = 2;
        EditorGUILayout.PropertyField(_groupProp);

        var inTween = tgt.GroupInTween;
        bool doInTween = EditorGUILayout.Toggle("Animate In", inTween.doTween);

        if (doInTween)
        {
            DrawCanvasGroupTweenControls(inTween, _groupInEaseProp, _groupInAlphaProp);
        }

        var outTween = tgt.GroupOutTween;
        bool doOutTween = EditorGUILayout.Toggle("Animate Out", outTween.doTween);

        if (doOutTween)
        {
            DrawCanvasGroupTweenControls(outTween, _groupOutEaseProp, _groupOutAlphaProp);
        }

        if (inTween.doTween != doInTween || outTween.doTween != doOutTween)
        {
            inTween.doTween = doInTween;
            outTween.doTween = doOutTween;
            EditorUtility.SetDirty(target);
        }

        EditorGUI.indentLevel = 0;
        #endif
    }

    private void DrawPositionTweenControls(MenuScreenElement.RectTweenValues tweenVals, SerializedProperty easeProp, SerializedProperty posProp, SerializedProperty scaleProp)
    {
        EditorGUI.indentLevel = 4;

        float tweenTime = DrawTimingBox(easeProp, tweenVals.time);

        // ANCHORED POSITION
        var position = EditorGUI.PrefixLabel(EditorGUILayout.GetControlRect(), new GUIContent("Position"));
        var positionRect = new Rect(position.x-60, position.y, position.width - 10, position.height);
        var doPosRect = new Rect(position.x + position.width - 120, position.y, 120, position.height);

        bool doPos = EditorGUI.Toggle(doPosRect, tweenVals.doAnchoredPosition);

        if (doPos)
            EditorGUI.PropertyField(positionRect, posProp, GUIContent.none);

        // LOCAL SCALE
        position = EditorGUI.PrefixLabel(EditorGUILayout.GetControlRect(), new GUIContent("Scale"));
        var scaleRect = new Rect(position.x-60, position.y, position.width - 10, position.height);
        var doScaleRect = new Rect(position.x + position.width - 120, position.y, 120, position.height);

        bool doScale = EditorGUI.Toggle(doScaleRect, tweenVals.doScale);

        if (doScale)
            EditorGUI.PropertyField(scaleRect, scaleProp, GUIContent.none);

        EditorGUILayout.Space(16);

        // SO DIRTY
        if (tweenVals.doAnchoredPosition != doPos || tweenVals.doScale != doScale || tweenTime != tweenVals.time)
        {
            tweenVals.time = tweenTime;
            tweenVals.doScale = doScale;
            tweenVals.doAnchoredPosition = doPos;
            EditorUtility.SetDirty(target);
        }

        EditorGUI.indentLevel = 2;
    }

    private void DrawGraphicTweenControls(MenuScreenElement.GraphicTweenValues tweenVals, SerializedProperty easeProp, SerializedProperty colorProp, SerializedProperty alphaProp)
    {
        EditorGUI.indentLevel = 4;

        float tweenTime = DrawTimingBox(easeProp, tweenVals.time);
        bool doColor = DrawColorBox(colorProp, tweenVals.doColor);
        bool doAlpha = DrawAlphaBox(alphaProp, tweenVals.doAlpha);

        EditorGUILayout.Space(16);

        // SO DIRTY
        if (tweenVals.doColor != doColor || tweenVals.doAlpha != doAlpha || tweenTime != tweenVals.time)
        {
            tweenVals.time = tweenTime;
            tweenVals.doAlpha = doAlpha;
            tweenVals.doColor = doColor;
            EditorUtility.SetDirty(target);
        }

        EditorGUI.indentLevel = 2;
    }

    
    private void DrawCanvasGroupTweenControls(MenuScreenElement.CanvasGroupTweenValues tweenVals, SerializedProperty easeProp, SerializedProperty alphaProp)
    {
        EditorGUI.indentLevel = 4;

        float tweenTime = DrawTimingBox(easeProp, tweenVals.time);
        bool doAlpha = DrawAlphaBox(alphaProp, tweenVals.doAlpha);

        EditorGUILayout.Space(16);

        // SO DIRTY
        if (tweenVals.doAlpha != doAlpha || tweenTime != tweenVals.time)
        {
            tweenVals.time = tweenTime;
            tweenVals.doAlpha = doAlpha;
            EditorUtility.SetDirty(target);
        }

        EditorGUI.indentLevel = 2;
    }

    private float DrawTimingBox(SerializedProperty easeProp, float oldValue)
    {
        Rect position = EditorGUI.PrefixLabel(EditorGUILayout.GetControlRect(), new GUIContent("Timing"));
        var easeRect = new Rect(position.x-60, position.y, position.width - 10, position.height);
        var timeRect = new Rect(position.x + position.width - 120, position.y, 120, position.height);

        EditorGUI.PropertyField(easeRect, easeProp, GUIContent.none);
        float tweenTime = EditorGUI.FloatField(timeRect, oldValue);

        return tweenTime;
    }

    private bool DrawColorBox(SerializedProperty colorProp, bool checkBox)
    {
        var position = EditorGUI.PrefixLabel(EditorGUILayout.GetControlRect(), new GUIContent("Color"));
        var colorRect = new Rect(position.x-60, position.y, position.width - 10, position.height);
        var doColorRect = new Rect(position.x + position.width - 120, position.y, 120, position.height);

        bool doColor = EditorGUI.Toggle(doColorRect, checkBox);

        if (doColor)
            EditorGUI.PropertyField(colorRect, colorProp, GUIContent.none);

        return doColor;
    }

    private bool DrawAlphaBox(SerializedProperty alphaProp, bool checkBox)
    {
        var position = EditorGUI.PrefixLabel(EditorGUILayout.GetControlRect(), new GUIContent("Alpha"));
        var alphaRect = new Rect(position.x-60, position.y, position.width - 10, position.height);
        var doAlphaRect = new Rect(position.x + position.width - 120, position.y, 120, position.height);

        bool doAlpha = EditorGUI.Toggle(doAlphaRect, checkBox);

        if (doAlpha)
            EditorGUI.PropertyField(alphaRect, alphaProp, GUIContent.none);

        return doAlpha;
    }
}