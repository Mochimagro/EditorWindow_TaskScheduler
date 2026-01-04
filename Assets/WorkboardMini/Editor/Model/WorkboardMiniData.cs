using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkboardMiniData : ScriptableObject
{
    [SerializeField]List<TaskItem> _items = new List<TaskItem>();
    int _nextId;
}
