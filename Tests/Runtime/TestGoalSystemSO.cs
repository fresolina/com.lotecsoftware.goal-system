using System.Collections;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace lotecsoftware.goals.tests {
    public class TestGoalSystemSOFromScene : TestGoalSystem {
        [UnitySetUp]
        public override IEnumerator SetupAssets() {
            _goalCompleted = false;
            _goalController = new GoalController();
            _goalController.ConnectFailed.AddListener((a, b) => Debug.Log("I have no evidence supporting that claim"));
            _goalController.GoalCompleted.AddListener((goal) => _goalCompleted = true);

            string guid = AssetDatabase.FindAssets($"TestGoalSystemSO t:scene")[0];
            string path = AssetDatabase.GUIDToAssetPath(guid);
            yield return EditorSceneManager.LoadSceneAsyncInPlayMode(path, new LoadSceneParameters(LoadSceneMode.Single));

            ILinkableItem[] items = GameObject.FindAnyObjectByType<GoalSystemTestSettings>().ItemAssets;
            _itemA = items[0];
            _itemABonus = items[1];
            _itemB = items[2];
            _itemExtra = items[3];
            _itemFromEvent = items[4];

            IGoal[] _goals = GameObject.FindAnyObjectByType<GoalSystemTestSettings>().GoalAssets;
            _goalFindA = _goals[0];
            _goalLinkAToABonus = _goals[1];
            _goalLinkBToA = _goals[2] as IConnectionGoal;
            _goalFindA2 = _goals[3];

            GameObject.FindAnyObjectByType<GoalSystemTestSettings>().GoalApiAsset.Init(_goalController);
        }
    }
}
