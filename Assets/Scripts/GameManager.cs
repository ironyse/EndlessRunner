using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Character{

    public string Name;
    public RuntimeAnimatorController characterAnimator;
    public Sprite sprite;    
    public int UnlockCost;
    public bool Unlocked;
}

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
            }
            return _instance;
        }
    }

    [SerializeField] private List<Character> _characters;
    public List<Character> GetCharacters() { return _characters; }

    private Character _selectedCharacter;
    public static RuntimeAnimatorController selectedPlayerAnimator;

    public Transform CharPanel;
    public PlayerCharacterController CharPrefab;    

    public void SetSelectedChar(Character chr){
        _selectedCharacter = chr;
        selectedPlayerAnimator = _selectedCharacter.characterAnimator;
    }

    private void Awake(){
        if (_instance){
            Destroy(gameObject);
        } else {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        Load();
    }   

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (scene.name=="Main Menu")
        {
            CharPanel = GameObject.Find("Content").transform;
            AddCharacters();    
        }
    }

    void AddCharacters(){        
        foreach(Character item in _characters){
            GameObject obj = Instantiate(CharPrefab.gameObject, CharPanel, false);
            PlayerCharacterController playerChar = obj.GetComponent<PlayerCharacterController>();
            playerChar.SetCharacter(item);

        }
    }    

    public void UnlockCharacter(string name)
    {        
        Character playerChar = _characters.Find(c => c.Name == name);
        if (playerChar != null && !playerChar.Unlocked){
            playerChar.Unlocked = true;   
        }
    }

    // Save & load progress
    public void Save()
    {
        List<string> characterNames = new List<string>();

        foreach (Character playerChar in _characters)
        {
            if (playerChar.Unlocked) characterNames.Add(playerChar.Name);
        }

        SaveObject saveObject = new SaveObject
        {
            diamondAmount = CurrencyData.diamondAmount,
            highScore = ScoreData.highScore,
            characterNames = characterNames,
        };

        string json = JsonUtility.ToJson(saveObject);
        File.WriteAllText(Application.dataPath + "/save.txt", json);
    }

    void Load()
    {
        if (File.Exists(Application.dataPath + "/save.txt"))
        {
            string saveString = File.ReadAllText(Application.dataPath + "/save.txt");
            SaveObject saveObject = JsonUtility.FromJson<SaveObject>(saveString);

            ScoreData.highScore = saveObject.highScore;
            CurrencyData.diamondAmount = saveObject.diamondAmount;

            foreach (string charName in saveObject.characterNames)
            {
                UnlockCharacter(charName);
            }

        }
    }

    private class SaveObject
    {
        public int diamondAmount;
        public int highScore;
        public List<string> characterNames;
    }
}

