//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using GameBuilders.FPSBuilder.Editor.Utility;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

namespace FPSBuilder.EnvironmentPack.Editor
{
    [CustomEditor(typeof(InteractiveDoor))]
    public class InteractiveDoorEditor : UnityEditor.Editor
    {
        private SerializedProperty m_TargetTransform;

        private SerializedProperty m_OpenedPosition;
        private SerializedProperty m_OpenedRotation;

        private SerializedProperty m_ClosedPosition;
        private SerializedProperty m_ClosedRotation;

        private SerializedProperty m_Open;
        private SerializedProperty m_RequiresAnimation;
        private SerializedProperty m_FadeOutAtEnd;
        private SerializedProperty m_FadeOutDuration;
        private SerializedProperty m_DelayBeforeFadeOut;
        private SerializedProperty m_Duration;
        private SerializedProperty m_Cost;
        private SerializedProperty m_OtherDoorsToOpen;
        //private SerializedProperty m_HighlightSphereRadius;

        private void OnEnable()
        {
            m_TargetTransform = serializedObject.FindProperty("m_TargetTransform");
            //m_HighlightSphereRadius = serializedObject.FindProperty("highlightSphereRadius");
            m_OpenedPosition = serializedObject.FindProperty("m_OpenedPosition");
            m_OpenedRotation = serializedObject.FindProperty("m_OpenedRotation");
            m_ClosedPosition = serializedObject.FindProperty("m_ClosedPosition");
            m_ClosedRotation = serializedObject.FindProperty("m_ClosedRotation");
            m_Open = serializedObject.FindProperty("m_Open");
            m_RequiresAnimation = serializedObject.FindProperty("m_RequiresAnimation");
            m_Duration = serializedObject.FindProperty("m_Duration");
            m_Cost = serializedObject.FindProperty("m_Cost");
            m_OtherDoorsToOpen = serializedObject.FindProperty("otherDoorsToOpen");
            m_FadeOutAtEnd = serializedObject.FindProperty("m_FadeOutAtEnd");
            m_FadeOutDuration = serializedObject.FindProperty("m_FadeOutDuration");
            m_DelayBeforeFadeOut = serializedObject.FindProperty("m_DelayBeforeFadeOut");
        }

        public override void OnInspectorGUI()
        {
            //Update the serializedProperty - always do this in the beginning of OnInspectorGUI
            serializedObject.Update();

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(m_TargetTransform);

            //EditorGUILayout.Space();
            //EditorGUILayout.PropertyField(m_HighlightSphereRadius);

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(m_Duration);

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(m_Cost);

            EditorGUILayout.Space();
            using (new EditorGUILayout.VerticalScope(Styling.background))
            {
                EditorGUILayout.PropertyField(m_OpenedPosition);
                EditorGUILayout.PropertyField(m_OpenedRotation);

                EditorGUILayout.Space();
                using (new EditorGUILayout.HorizontalScope())
                {
                    GUILayout.FlexibleSpace();

                    if (GUILayout.Button("Set Position & Rotation", Styling.leftButton))
                    {
                        m_OpenedPosition.vector3Value = ((Transform)m_TargetTransform.objectReferenceValue).localPosition;
                        m_OpenedRotation.vector3Value = ((Transform)m_TargetTransform.objectReferenceValue).localEulerAngles;
                    }

                    if (GUILayout.Button("Reset", Styling.rightButton))
                    {
                        m_OpenedPosition.vector3Value = Vector3.zero;
                        m_OpenedRotation.vector3Value = Vector3.zero;
                    }

                    GUILayout.FlexibleSpace();
                }
            }


            EditorGUILayout.Space();
            using (new EditorGUILayout.VerticalScope(Styling.background))
            {
                EditorGUILayout.PropertyField(m_ClosedPosition);
                EditorGUILayout.PropertyField(m_ClosedRotation);

                EditorGUILayout.Space();
                using (new EditorGUILayout.HorizontalScope())
                {
                    GUILayout.FlexibleSpace();

                    if (GUILayout.Button("Set Position & Rotation", Styling.leftButton))
                    {
                        m_ClosedPosition.vector3Value = ((Transform)m_TargetTransform.objectReferenceValue).localPosition;
                        m_ClosedRotation.vector3Value = ((Transform)m_TargetTransform.objectReferenceValue).localEulerAngles;
                    }

                    if (GUILayout.Button("Reset", Styling.rightButton))
                    {
                        m_ClosedPosition.vector3Value = Vector3.zero;
                        m_ClosedRotation.vector3Value = Vector3.zero;
                    }

                    GUILayout.FlexibleSpace();
                }
            }

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(m_Open, new GUIContent("Start Open"));
            EditorGUILayout.PropertyField(m_RequiresAnimation);
            EditorGUILayout.PropertyField(m_FadeOutAtEnd);
            EditorGUILayout.PropertyField(m_FadeOutDuration);
            EditorGUILayout.PropertyField(m_DelayBeforeFadeOut);

            EditorGUILayout.PropertyField(m_OtherDoorsToOpen);

            serializedObject.ApplyModifiedProperties();
        }
    }
}