#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Project.Editor.ManagedRef
{
    /// <summary>
    /// Reusable drawer for List/Array of [SerializeReference] base types.
    /// It gives each element a "Type" dropdown and draws the fields of the chosen subclass.
    /// </summary>
    public sealed class ManagedRefListDrawer
    {
        private readonly UnityEngine.Object _target;
        private readonly SerializedObject _so;
        private readonly SerializedProperty _listProp;
        private readonly Type _baseType;
        private readonly string _header;

        private readonly ReorderableList _list;
        private readonly Type[] _concreteTypes;
        private readonly GUIContent[] _typeLabels;

        public ManagedRefListDrawer(UnityEngine.Object target, SerializedObject so, SerializedProperty listProp, Type baseType, string header)
        {
            _target = target;
            _so = so;
            _listProp = listProp;
            _baseType = baseType;
            _header = header;

            (_concreteTypes, _typeLabels) = ManagedRefTypeCache.GetConcreteTypes(baseType);

            _list = new ReorderableList(_so, _listProp, draggable: true, displayHeader: true, displayAddButton: true, displayRemoveButton: true);

            _list.drawHeaderCallback = rect => EditorGUI.LabelField(rect, _header);

            _list.onAddDropdownCallback = (rect, list) =>
            {
                var menu = new GenericMenu();
                menu.AddItem(new GUIContent("None (null)"), false, () =>
                {
                    AddNewElement(null);
                });

                for (int i = 0; i < _concreteTypes.Length; i++)
                {
                    var t = _concreteTypes[i];
                    menu.AddItem(new GUIContent(t.Name), false, () =>
                    {
                        AddNewElement(t);
                    });
                }

                menu.DropDown(rect);
            };

            _list.elementHeightCallback = index =>
            {
                var el = _listProp.GetArrayElementAtIndex(index);
                if (el.managedReferenceValue == null)
                    return EditorGUIUtility.singleLineHeight + 10;

                // Type line + full property height
                float h = EditorGUIUtility.singleLineHeight; // type popup line
                h += 6;
                h += EditorGUI.GetPropertyHeight(el, includeChildren: true);
                h += 10;
                return h;
            };

            _list.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                var el = _listProp.GetArrayElementAtIndex(index);

                rect.y += 2;
                rect.height = EditorGUIUtility.singleLineHeight;

                DrawTypePopup(rect, el);

                rect.y += EditorGUIUtility.singleLineHeight + 6;

                if (el.managedReferenceValue == null)
                {
                    EditorGUI.HelpBox(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight + 6),
                        "Null reference. Pick a Type above.", MessageType.Info);
                    return;
                }

                EditorGUI.PropertyField(rect, el, GUIContent.none, includeChildren: true);
            };
        }

        public void DoLayout()
        {
            _so.Update();
            _list.DoLayoutList();
            _so.ApplyModifiedProperties();
        }

        private void DrawTypePopup(Rect rect, SerializedProperty element)
        {
            EditorGUI.BeginProperty(rect, GUIContent.none, element);

            var currentType = element.managedReferenceValue?.GetType();
            int currentIndex = GetIndex(currentType);

            var left = new Rect(rect.x, rect.y, 44, rect.height);
            var right = new Rect(rect.x + 48, rect.y, rect.width - 48, rect.height);

            EditorGUI.LabelField(left, "Type");

            int newIndex = EditorGUI.Popup(right, currentIndex, _typeLabels);
            if (newIndex != currentIndex)
            {
                Type newType = (newIndex <= 0) ? null : _concreteTypes[newIndex - 1]; // 0 is None
                SetElementType(element, newType);
            }

            EditorGUI.EndProperty();
        }

        private int GetIndex(Type currentType)
        {
            if (currentType == null) return 0; // None
            for (int i = 0; i < _concreteTypes.Length; i++)
                if (_concreteTypes[i] == currentType)
                    return i + 1;
            return 0;
        }

        private void AddNewElement(Type type)
        {
            Undo.RecordObject(_target, $"Add {_baseType.Name}");
            int i = _listProp.arraySize;
            _listProp.InsertArrayElementAtIndex(i);
            var el = _listProp.GetArrayElementAtIndex(i);
            el.managedReferenceValue = CreateInstance(type);
            _so.ApplyModifiedProperties();
            EditorUtility.SetDirty(_target);
        }

        private void SetElementType(SerializedProperty element, Type type)
        {
            Undo.RecordObject(_target, $"Change {_baseType.Name} Type");
            element.managedReferenceValue = CreateInstance(type);
            _so.ApplyModifiedProperties();
            EditorUtility.SetDirty(_target);
        }

        private object CreateInstance(Type type)
        {
            if (type == null) return null;

            // ManagedReference needs a concrete instance. Prefer parameterless constructors.
            try
            {
                return Activator.CreateInstance(type);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to create instance of {type.FullName}. Ensure it has a public parameterless constructor.\n{e}");
                return null;
            }
        }
    }

    internal static class ManagedRefTypeCache
    {
        // Cache per baseType
        private static readonly Dictionary<Type, (Type[] types, GUIContent[] labels)> Cache = new();

        public static (Type[] types, GUIContent[] labels) GetConcreteTypes(Type baseType)
        {
            if (Cache.TryGetValue(baseType, out var cached))
                return cached;

            // Use Unity's TypeCache for speed
            var list = TypeCache.GetTypesDerivedFrom(baseType)
                .Where(t =>
                    t.IsClass &&
                    !t.IsAbstract &&
                    !t.IsGenericType &&
                    t.GetCustomAttribute<SerializableAttribute>() != null &&
                    typeof(UnityEngine.Object).IsAssignableFrom(t) == false // don't allow ScriptableObject/MonoBehaviour here
                )
                .OrderBy(t => t.Name)
                .ToArray();

            var labels = new GUIContent[list.Length + 1];
            labels[0] = new GUIContent("None (null)");
            for (int i = 0; i < list.Length; i++)
                labels[i + 1] = new GUIContent(list[i].Name);

            Cache[baseType] = (list, labels);
            return (list, labels);
        }
    }
}
#endif
