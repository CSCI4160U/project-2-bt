using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class ObjectiveSystem : MonoBehaviour, ISaveable
{
    [System.Serializable]
    public class StateTransition
    {
        public string Message;
        public int RequiredState;
        public int NewState;
    }

    [SerializeField] private List<StateTransition> stateTransitions;
    [SerializeField] private List<string> stateTitles;
    [SerializeField] private List<string> stateDescriptions;
    [SerializeField] private UnityEvent<int> onStateChanged;

    private int state = 0;

    public int State { get => state; }
    public List<string> StateTitles { get => stateTitles; }
    public List<string> StateDescriptions { get => stateDescriptions; }
    public UnityEvent<int> OnStateChanged { get => onStateChanged; }

    public void TriggerDetected(string message)
    {
        foreach (StateTransition transition in stateTransitions)
        {
            if (state == transition.RequiredState && message == transition.Message)
            {
                state = transition.NewState;
                onStateChanged.Invoke(state);
            }
        }
    }

    [System.Serializable]
    private struct SaveData
    {
        public int State;
    }

	public void Save(string savePath)
	{
        SaveData data = new SaveData();
        data.State = state;
        File.WriteAllText(savePath + "ObjectiveSystem.json", JsonUtility.ToJson(data));
	}

	public void Load(string savePath)
	{
        SaveData data = JsonUtility.FromJson<SaveData>(File.ReadAllText(savePath + "ObjectiveSystem.json"));
        state = data.State;
        onStateChanged.Invoke(state);
	}
}
