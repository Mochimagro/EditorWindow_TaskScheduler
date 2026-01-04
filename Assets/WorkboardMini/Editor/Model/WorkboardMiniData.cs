using Codice.CM.WorkspaceServer.Lock;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkboardMiniData : ScriptableObject
{
    [SerializeField]List<TaskItem> _items = new List<TaskItem>();
    public List<TaskItem> Items { get { return _items; } set { _items = value; } }
    int _nextId;
}
