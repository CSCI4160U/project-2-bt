using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tooltips : MonoBehaviour
{
	[SerializeField] private GameObject textPrefab;

    public void Add(string text)
    {
        GameObject newText = Instantiate(textPrefab);
        newText.GetComponent<TMP_Text>().text = text;
        newText.transform.SetParent(transform);
    }

    public void Clear()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
