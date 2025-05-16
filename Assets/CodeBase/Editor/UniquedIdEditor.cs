using System;
using System.Linq;
using CodeBase.Logic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace CodeBase.Editor
{
    [CustomEditor(typeof(UniqueId))]
    public class UniquedIdEditor: UnityEditor.Editor
    {
        private void OnEnable()
        {
            var uniqueId = (UniqueId) target; // It is necessary to rewind the type to use the ID field
            
            if(string.IsNullOrEmpty(uniqueId.Id)) Generate(uniqueId);
            
            else
            {
                UniqueId[] uniqueIds = FindObjectsOfType<UniqueId>();
                if (uniqueIds.Any(other => other != uniqueId && other.Id == uniqueId.Id)) Generate(uniqueId);
            }
        }

        private void Generate(UniqueId uniqueId)
        {
          uniqueId.Id = $"{uniqueId.gameObject.scene.name}_{Guid.NewGuid().ToString()}";
          if (!Application.isPlaying)
          {
              EditorUtility.SetDirty(uniqueId); // install as dirty, which gives unity to understand that there were changes in the object
              EditorSceneManager.MarkSceneDirty(uniqueId.gameObject.scene); // install as dirty current scene
          }
        }
    }
}