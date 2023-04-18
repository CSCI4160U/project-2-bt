using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnemySaver : MonoBehaviour, ISaveable
{
    [SerializeField] private Transform enemiesContainer;

    [System.Serializable]
    private struct SaveData
    {
        public List<int> Indices;
        public List<Vector3> Positions;
        public List<Vector3> Rotations;
        public List<int> States;
        public List<int> WaypointIndices;
        public List<float> AlertTimes;
        public List<Vector3> Destinations;
        public List<float> Healths;
    }

    public void Save(string savePath)
    {
        SaveData data = new SaveData();
        data.Indices = new List<int>();
        data.Positions = new List<Vector3>();
        data.Rotations = new List<Vector3>();
        data.States = new List<int>();
        data.WaypointIndices = new List<int>();
        data.AlertTimes = new List<float>();
        data.Destinations = new List<Vector3>();
        data.Healths = new List<float>();

        foreach (Transform child in enemiesContainer)
        {
            if (child.TryGetComponent(out EnemyAI enemy))
            {
                data.Indices.Add(child.GetSiblingIndex());
                data.Positions.Add(child.position);
                data.Rotations.Add(child.rotation.eulerAngles);
                data.States.Add((int)enemy.State);
                data.WaypointIndices.Add(enemy.WaypointIndex);
                data.AlertTimes.Add(enemy.AlertTime);
                data.Destinations.Add(enemy.Destination);
                data.Healths.Add(enemy.GetComponent<Health>().CurrentHealth);
            }
        }

        File.WriteAllText(savePath + "Enemies.json", JsonUtility.ToJson(data));
	}

    public void Load(string savePath)
    {
        SaveData data = JsonUtility.FromJson<SaveData>(File.ReadAllText(savePath + "Enemies.json"));
        for (int i = 0; i < data.Indices.Count; i++)
        {
            Transform child = enemiesContainer.GetChild(data.Indices[i]);
            EnemyAI enemy = child.GetComponent<EnemyAI>();
            child.position = data.Positions[i];
            child.rotation = Quaternion.Euler(data.Rotations[i]);
            enemy.Load((EnemyAI.EnemyState)data.States[i], data.WaypointIndices[i], data.AlertTimes[i], data.Destinations[i], data.Healths[i]);
        }
    }
}
