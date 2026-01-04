using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]public class TaskItem
{
    [SerializeField]int _id;
    public int Id { get { return _id; } set { _id = value; } }
    public string Title { get { return _title; } set {  _title = value; } }
    [SerializeField] string _title;
    public string Memo { get { return _memo; } set { _memo = value; } }
    [SerializeField] string _memo;
    public TaskStatus Status { get { return _status; } set { _status = value; } }
    [SerializeField] TaskStatus _status;
    public TaskPriority Priority { get { return _priority; } set { _priority = value; } }
    [SerializeField] TaskPriority _priority;
    public string DueDate { get { return _dueDate; } set { _dueDate = value; } }
    [SerializeField]string _dueDate;

    public long UpdatedAtTicks { get { return _updatedAtTicks; }set { _updatedAtTicks = value; } }
    long _updatedAtTicks;
}
