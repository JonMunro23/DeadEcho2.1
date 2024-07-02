﻿//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using UnityEditor;

namespace GameBuilders.FPSBuilder.Core.Utilities.Editor
{
    [CustomEditor(typeof(CollisionSound))]
    public class CollisionSoundEditor : UnityEditor.Editor
    {
        private static readonly string[] m_ScriptField = { "m_Script" };

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawPropertiesExcluding(serializedObject, m_ScriptField);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
