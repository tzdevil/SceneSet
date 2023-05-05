#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace tzdevil.SceneSet
{
    [CreateAssetMenu(fileName = "Scene Set", menuName = "Create New/Scene Setx", order = 52)]
    public class SceneSetSO : ScriptableObject
    {
        [field: SerializeField] public SceneAsset MainScene { get; set; }
        [field: Space(10)]
        [field: SerializeField] public List<SceneAsset> AdditiveScenes { get; set; }

        private static List<string> _scenePaths = new();

        private void OnValidate()
        {
            var mainScene = AssetDatabase.GetAssetPath(MainScene);
            var additiveScenes = AdditiveScenes.Select(s => AssetDatabase.GetAssetPath(s)).ToList();

            _scenePaths.Add(mainScene);
            _scenePaths.AddRange(additiveScenes);
        }

        [OnOpenAsset]
        public static bool OnDoubleClickSceneSet(int instanceID, int line)
        {
            var target = EditorUtility.InstanceIDToObject(instanceID);

            if (target is not SceneSetSO)
                return false;

            if (_scenePaths.Count == 0)
                return false;

            // Load scenes.
            EditorSceneManager.OpenScene(_scenePaths[0], OpenSceneMode.Single);
            for (int i = 0; i < _scenePaths.Count; i++)
            {
                var sceneId = _scenePaths[i];
                EditorSceneManager.OpenScene(sceneId, OpenSceneMode.Additive);
            }

            return false;
        }
    }
}
#endif