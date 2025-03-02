using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEditor;

public class LevelSelectionManager : MonoBehaviour
{
    [SerializeField] private LevelEditorManager _levelEditorManager;
    public AllLevelsData allLevelsData;
    public Transform contentPanel;
    public GameObject levelButtonPrefab;

    private void Start()
    {
        GenerateLevelButtons();
    }

    private void GenerateLevelButtons()
    {
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }

        foreach (LevelData level in allLevelsData.levels)
        {
            CreateLevelButton(level);
        }
    }

    private void CreateLevelButton(LevelData levelData)
    {
        GameObject buttonObj = Instantiate(levelButtonPrefab, contentPanel);
        Button button = buttonObj.GetComponent<Button>();
        TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();

        if (buttonText != null)
        {
            buttonText.text = "LEVEL " + levelData.levelNumber;
        }

        button.onClick.AddListener(() => OnLevelButtonClicked(levelData));
    }

    private void OnLevelButtonClicked(LevelData levelData)
    {
        _levelEditorManager.SetLevel(levelData.levelNumber);
        Debug.Log("Seçilen Level: " + levelData.levelNumber);
    }

    // ✅ YENİ LEVEL OLUŞTURAN METOT
    public void CreateNewLevel()
    {
        // Yeni LevelData oluştur
        LevelData newLevel = ScriptableObject.CreateInstance<LevelData>();
        newLevel.levelNumber = allLevelsData.levels.Length + 1;

        // LevelData'yı asset olarak kaydet (Editör içi işlem)
        string path = $"Assets/Resources/Levels/Level_{newLevel.levelNumber}.asset";
        AssetDatabase.CreateAsset(newLevel, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        // Listeye ekle
        List<LevelData> levelsList = new List<LevelData>(allLevelsData.levels) { newLevel };
        allLevelsData.levels = levelsList.ToArray();
        EditorUtility.SetDirty(allLevelsData);

        // **Yeni butonu hemen UI'ya ekle**
        CreateLevelButton(newLevel);
    }
}
