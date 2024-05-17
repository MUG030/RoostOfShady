using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;


public class UISceneTool : MonoBehaviour
{
    private UIDocument uiDocument;
    private Label targetLabel;
    public string[] textArray;
    private int currentIndex = 0;

    private void Start()
    {
        // UIDocumentのインスタンスを取得
        uiDocument = GetComponent<UIDocument>();

        // ルートVisualElementを取得
        VisualElement rootVisualElement = uiDocument.rootVisualElement;

        // ラベルを取得または作成
        targetLabel = rootVisualElement.Q<Label>("SceneText");
        if (targetLabel == null)
        {
            targetLabel = new Label();
            targetLabel.name = "SceneText";
            rootVisualElement.Add(targetLabel);
        }

        // テキストの切り替えを開始
        StartCoroutine(SwitchTextCoroutine());
    }

    private IEnumerator SwitchTextCoroutine()
    {
        while (true)
        {
            // テキストを切り替える
            targetLabel.text = textArray[currentIndex];
            currentIndex = (currentIndex + 1) % textArray.Length;

            // 1.0秒待機
            yield return new WaitForSeconds(1.0f);
        }
    }
}