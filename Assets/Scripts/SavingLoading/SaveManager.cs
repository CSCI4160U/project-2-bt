using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> saveableObjects;

    private string savePath;

    private void Start()
    {
        savePath = Application.persistentDataPath + "/SaveData/";
        Debug.Log("Save path: " + savePath);
    }

    [ContextMenu("Save All")]
    public void SaveAll()
    {
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }

        foreach (GameObject obj in saveableObjects)
        {
            foreach (ISaveable saveable in obj.GetComponents<ISaveable>())
            {
                saveable.Save(savePath);
            }
        }
    }

    [ContextMenu("Load All")]
    public void LoadAll()
    {
		foreach (GameObject obj in saveableObjects)
		{
			foreach (ISaveable saveable in obj.GetComponents<ISaveable>())
			{
				saveable.Load(savePath);
			}
		}
	}
}
