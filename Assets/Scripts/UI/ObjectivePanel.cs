using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectivePanel : MonoBehaviour
{
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;
    [SerializeField] private ObjectiveSystem objectiveSystem;

    private void Start()
    {
        ObjectiveStateChanged(objectiveSystem.State);
	}

    public void ObjectiveStateChanged(int state)
    {
        title.text = objectiveSystem.StateTitles[state];
        description.text = objectiveSystem.StateDescriptions[state];
    }
}
