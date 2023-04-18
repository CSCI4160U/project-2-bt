using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable
{
    void Save(string savePath);
    void Load(string savePath);
}
