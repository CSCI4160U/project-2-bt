using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FirstAidKitSaver : MonoBehaviour, ISaveable
{
    [SerializeField] private Transform kitsContainer;

    [System.Serializable]
    public struct SaveData
    {
        public List<int> Indices;
        public List<bool> Active;
    }

    public void Save(string savePath)
    {
        SaveData data = new SaveData();
        data.Indices = new List<int>();
        data.Active = new List<bool>();
        foreach (Transform child in kitsContainer)
        {
            if (child.TryGetComponent(out FirstAidKit kit))
            {
				data.Indices.Add(child.GetSiblingIndex());
				data.Active.Add(child.gameObject.activeInHierarchy);
			}
        }
        File.WriteAllText(savePath + "FirstAidKits.json", JsonUtility.ToJson(data));
    }

    public void Load(string savePath)
    {
        SaveData data = JsonUtility.FromJson<SaveData>(File.ReadAllText(savePath + "FirstAidKits.json"));
        for (int i = 0; i < data.Indices.Count; i++)
        {
            kitsContainer.GetChild(data.Indices[i]).gameObject.SetActive(data.Active[i]);
        }
	}
}
