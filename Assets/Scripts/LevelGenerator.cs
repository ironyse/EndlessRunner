using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Templates")]
    public List<LevelTemplateController> levelTemplates;
    public List<LevelTemplateController> earlyLevelTemplates;
    public float levelTemplateWidth;

    [Header("Generator Area")]
    public Camera gameCamera;
    public float areaStartOffset;
    public float areaEndOffset;

    private const float debugLineHeight = 10f;

    private List<GameObject> spawnedLevel;
    private Dictionary<string, List<GameObject>> pool;

    private float lastGeneratedPosX;
    private float lastRemovedPosX;

    private float GetHorizontalPosStart() {
        return gameCamera.ViewportToWorldPoint(new Vector2(0f, 0f)).x + areaStartOffset;
    }

    private float GetHorizontalPosEnd() {
        return gameCamera.ViewportToWorldPoint(new Vector2(1f, 0f)).x + areaEndOffset;
    }

    void Start() {
        pool = new Dictionary<string, List<GameObject>>();
        spawnedLevel = new List<GameObject>();
        lastGeneratedPosX = GetHorizontalPosStart();
        lastRemovedPosX = lastGeneratedPosX - levelTemplateWidth;

        foreach (LevelTemplateController level in earlyLevelTemplates) {
            GenerateLevel(lastGeneratedPosX, level);
            lastGeneratedPosX += levelTemplateWidth;
        }
    }

    void Update()
    {
        while (lastGeneratedPosX < GetHorizontalPosEnd())
        {
            GenerateLevel(lastGeneratedPosX);
            lastGeneratedPosX += levelTemplateWidth;
        }

        while (lastRemovedPosX + levelTemplateWidth < GetHorizontalPosStart()) {
            lastRemovedPosX += levelTemplateWidth;
            RemoveLevel(lastRemovedPosX);
        }
    }

    void GenerateLevel(float posX, LevelTemplateController forcelevel = null)
    {
        GameObject newLevel;
        if (forcelevel)
        {
            newLevel = GenerateFromPool(forcelevel.gameObject, transform);
        }
        else
        {
            newLevel = GenerateFromPool(levelTemplates[Random.Range(0, levelTemplates.Count)].gameObject, transform);
        }

        newLevel.transform.position = new Vector2(posX, -4.35f);
        spawnedLevel.Add(newLevel);
    }

    void RemoveLevel(float posX) {
        GameObject levelToRemove = null;

        foreach (GameObject item in spawnedLevel) {
            if (item.transform.position.x == posX) {
                levelToRemove = item;
                break;
            }
        }

        if (levelToRemove != null) {
            spawnedLevel.Remove(levelToRemove);
            ReturnToPool(levelToRemove);
        }
    }

    void ReturnToPool(GameObject item) {
        if (!pool.ContainsKey(item.name)) {
            Debug.LogError("INVALID POOL ITEM");
        }
        pool[item.name].Add(item);
        item.SetActive(false);
    }

    private GameObject GenerateFromPool(GameObject item, Transform parent) {
        if (pool.ContainsKey(item.name)) {
            if (pool[item.name].Count > 0) {
                GameObject newItemFromPool = pool[item.name][0];
                pool[item.name].Remove(newItemFromPool);
                newItemFromPool.SetActive(true);
                Transform collectibles = newItemFromPool.transform.Find("Collectibles");
                if (collectibles)
                {
                    foreach (Transform collectible in collectibles)
                    {
                        collectible.gameObject.SetActive(true);
                    }
                }                
                
                return newItemFromPool;
            }
        } else {
            pool.Add(item.name, new List<GameObject>());
        }
        GameObject newItem = Instantiate(item, parent);
        newItem.name = item.name;
        return newItem;
    }    
    

    //debug
    private void OnDrawGizmos()
    {
        Vector3 areaStartPos = transform.position;
        Vector3 areaEndPos = transform.position;

        areaStartPos.x = GetHorizontalPosStart();
        areaEndPos.x = GetHorizontalPosEnd();

        Debug.DrawLine(areaStartPos + Vector3.up * debugLineHeight / 2, areaStartPos + Vector3.down * debugLineHeight / 2, Color.red);
        Debug.DrawLine(areaEndPos + Vector3.up * debugLineHeight / 2, areaEndPos + Vector3.down * debugLineHeight / 2, Color.red);
    }
}
