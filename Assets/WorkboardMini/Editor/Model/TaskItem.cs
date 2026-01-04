using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]public class TaskItem
{
    [SerializeField]int _id;
    [SerializeField] string _title;
    public string Title { get { return _title; } set {  _title = value; } }
    [SerializeField] string _memo;
    [SerializeField] TaskStatus _status;
    [SerializeField] TaskPriority _priority;
    [SerializeField]string _dueDate;
    long _updatedAtTicks;
}
