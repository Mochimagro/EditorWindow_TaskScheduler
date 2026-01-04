using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TaskItem
{
    int _id;
    string _title;
    string _memo;
    TaskStatus _status;
    TaskPriority _priority;
    string _dueDate;
    long _updatedAtTicks;
}
