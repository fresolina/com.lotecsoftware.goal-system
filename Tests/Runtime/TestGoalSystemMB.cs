using System.Collections;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace lotecsoftware.goals.tests {
    public class TestGoalSystemMBFromScene : TestGoalSystem {
        [UnitySetUp]
        public override IEnumerator SetupAssets() {
            string guid = AssetDatabase.FindAssets($"TestGoalSystemMB t:scene")[0];
            string path = AssetDatabase.GUIDToAssetPath(guid);
            yield return EditorSceneManager.LoadSceneAsyncInPlayMode(path, new LoadSceneParameters(LoadSceneMode.Single));

            _goalCompleted = false;
            _itemA = GameObject.Find("Evidence A").GetComponent<ILinkable>();
            _itemABonus = GameObject.Find("Evidence ABonus").GetComponent<ILinkable>();
            _itemB = GameObject.Find("Evidence B").GetComponent<ILinkable>();
            _itemExtra = GameObject.Find("Extra item").GetComponent<ILinkable>();
            _itemFromEvent = GameObject.Find("Item from event").GetComponent<ILinkable>();

            _goalController = GameObject.FindObjectOfType<GoalControllerMB>().GoalController;
            _goalFindA = GameObject.Find("Find Evidence A").GetComponent<GoalMB>();
            _goalFindA2 = GameObject.Find("Find Evidence A2").GetComponent<IFindItemGoal>();
            _goalLinkAToABonus = GameObject.Find("Link A to ABonus").GetComponent<GoalMB>();
            _goalLinkBToA = GameObject.Find("Link B to A").GetComponent<IConnectionGoal>();

            _goalController.GoalCompleted.AddListener((goal) => _goalCompleted = true);

            // GameObject.FindAnyObjectByType<GoalSystemTestSettings>().GoalApiAsset.Init(_goalController);
        }
    }
}
