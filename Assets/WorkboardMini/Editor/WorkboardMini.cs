using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class WorkboardMini : EditorWindow
{
    [SerializeField] private VisualTreeAsset m_VisualTreeAsset = default;

    [SerializeField] private WorkboardMiniData _workboardMiniData = null;
    private TaskItem _selectedTaskItem;

    Toolbar _toolbar;
    ToolbarButton _addToolbarButton;
    ToolbarButton _deleteToolbarButton;
    ToolbarButton _duplicateToolbarButton;
    ToolbarButton _saveToolbarButton;

    VisualElement _leftPane;
    ListView _taskListView;

    VisualElement _rightPane;
    VisualElement _placeholder;
    VisualElement _taskDetail;

    [MenuItem("Tools/WorkboardMini")]
    public static void ShowExample()
    {
        WorkboardMini wnd = GetWindow<WorkboardMini>();
        wnd.titleContent = new GUIContent("WorkboardMini");
    }

    public void CreateGUI()
    {

        VisualElement root = rootVisualElement;

        VisualElement uxmlElement = m_VisualTreeAsset.Instantiate();
        
        // Toolber
        _toolbar = uxmlElement.Q<Toolbar>();
        _addToolbarButton = _toolbar.Q<ToolbarButton>("AddButton");
        _addToolbarButton.clicked += OnClickAdd;

        _duplicateToolbarButton = _toolbar.Q<ToolbarButton>("DuplicateButton");
        _duplicateToolbarButton.clicked += OnClickDuplicate;

        _deleteToolbarButton = _toolbar.Q<ToolbarButton>("DeleteButton");
        _saveToolbarButton = _toolbar.Q<ToolbarButton>("SaveButton");

        // SplitView
        TemplateContainer splitViewHostContainer = uxmlElement.Q<TemplateContainer>();
        splitViewHostContainer.style.flexGrow = 1;
        TwoPaneSplitView splitView = splitViewHostContainer.Q<TwoPaneSplitView>("MainSplitView");

        // LeftPane : タスクリスト
        _leftPane = splitView.Q<VisualElement>("LeftPane");
        _taskListView = _leftPane.Q<ListView>("TaskListView");

        _taskListView.itemsSource = _workboardMiniData.Items;

        _taskListView.makeItem = () =>
        {
            var title = new Label();
            title.name = "title";
            title.style.unityTextAlign = TextAnchor.MiddleLeft;
            title.style.flexGrow = 1;

            return title;
        };

        _taskListView.bindItem = (element, index) =>
        {
            var item = _workboardMiniData.Items[index];
            element.Q<Label>("title").text = $"{item.Title}";
        };

        _taskListView.selectionChanged += selectedItems =>
        {

            if (selectedItems == null) 
            {
                ShowSelectedTaskItemDetail(null);
                return; 
            }

            foreach ( var item in selectedItems)
            {
                var selected = item as TaskItem;
                if(selected != null)
                {
                    Debug.Log(selected.Title);
                    ShowSelectedTaskItemDetail(selected);
                    return;
                }
            }
        };

        _taskListView.RefreshItems();

        // RightPane : タスク詳細表示・編集
        _rightPane = splitView.Q<VisualElement>("RightPane");

        _placeholder = _rightPane.Q<VisualElement>("Placeholder");
        _taskDetail = _rightPane.Q<VisualElement>("TaskDetail");

        ShowSelectedTaskItemDetail(null);

        root.Add(uxmlElement);
    }

    private void ShowSelectedTaskItemDetail(TaskItem selected)
    {
        _selectedTaskItem = selected;

        if (_selectedTaskItem == null) 
        {
            _placeholder.style.display = DisplayStyle.Flex;
            _taskDetail.style.display = DisplayStyle.None;
            return; 
        }

        _placeholder.style.display = DisplayStyle.None;
        _taskDetail.style.display = DisplayStyle.Flex;

        var titleTextField = _taskDetail.Q<TextField>("TitleTextField");
        titleTextField.value = _selectedTaskItem.Title;
        titleTextField.RegisterValueChangedCallback((value) =>
        {
            _selectedTaskItem.Title = value.newValue;
        });

        var statusEnumField = _taskDetail.Q<EnumField>("StatusEnumField");
        statusEnumField.Init(TaskStatus.ToDo);
        statusEnumField.value = _selectedTaskItem.Status;
        statusEnumField.RegisterValueChangedCallback((value) =>
        {
            _selectedTaskItem.Status = (TaskStatus)value.newValue;
        });

        var priorityEnumField = _taskDetail.Q<EnumField>("PriorityEnumField");
        priorityEnumField.Init(TaskPriority.Low);
        priorityEnumField.value = _selectedTaskItem.Priority;
        priorityEnumField.RegisterValueChangedCallback((value) =>
        {
            _selectedTaskItem.Priority = (TaskPriority)value.newValue;
        });
        
        var memoTextField = _taskDetail.Q<TextField>("MemoTextField");
        memoTextField.value = _selectedTaskItem.Memo;
        memoTextField.RegisterValueChangedCallback((value) =>
        {
            _selectedTaskItem.Memo = value.newValue;
        });
        
        var dueTextField = _taskDetail.Q<TextField>("DueTextField");
        dueTextField.value = _selectedTaskItem.DueDate;
        dueTextField.RegisterValueChangedCallback((value) =>
        {
            _selectedTaskItem.DueDate = value.newValue;
        });

    }

    private void OnClickAdd()
    {
        if (_workboardMiniData == null)
        {
            Debug.LogError("WorkboardMiniData is null. Assign or load it before using Add.");
            return;
        }

        Undo.RecordObject(_workboardMiniData, "Add Task");

        int nextId = GetNextId();

        var newItem = new TaskItem
        {
            Id = nextId,
            Title = $"New Task {nextId}",
            Memo = "",
            Status = TaskStatus.ToDo,
            Priority = TaskPriority.Mid,
            DueDate = "",
            UpdatedAtTicks = System.DateTime.UtcNow.Ticks
        };

        _workboardMiniData.Items.Add(newItem);

        EditorUtility.SetDirty(_workboardMiniData);

        _taskListView?.Rebuild();

        // 追加した行を選択状態にする
        if (_taskListView != null)
        {
            int index = _workboardMiniData.Items.Count - 1;

            // selectionType が Single であること推奨
            _taskListView.SetSelection(index);
            _taskListView.ScrollToItem(index);
        }
    }
    private int GetNextId()
    {
        int max = 0;
        var items = _workboardMiniData.Items;
        for (int i = 0; i < items.Count; i++)
            if (items[i].Id > max) max = items[i].Id;

        return max + 1;
    }

    private void OnClickDuplicate()
    {
        if (_workboardMiniData == null)
        {
            Debug.LogError("WorkboardMiniData is null. Assign or load it before using Duplicate.");
            return;
        }

        if (_selectedTaskItem == null)
        {
            Debug.LogWarning("Duplicate requires a selected TaskItem.");
            return;
        }

        Undo.RecordObject(_workboardMiniData, "Duplicate Task");

        int nextId = GetNextId();

        // 複製（TaskItem の命名に合わせて全フィールドをコピー）
        var duplicated = new TaskItem
        {
            Id = nextId,
            Title = $"{_selectedTaskItem.Title} (Copy)",
            Memo = _selectedTaskItem.Memo,
            Status = _selectedTaskItem.Status,
            Priority = _selectedTaskItem.Priority,
            DueDate = _selectedTaskItem.DueDate,
            UpdatedAtTicks = System.DateTime.UtcNow.Ticks
        };

        // 元の直後に挿入（見た目が分かりやすい）
        int srcIndex = _workboardMiniData.Items.IndexOf(_selectedTaskItem);
        int insertIndex = (srcIndex >= 0) ? (srcIndex + 1) : _workboardMiniData.Items.Count;
        _workboardMiniData.Items.Insert(insertIndex, duplicated);

        // 保存対象として Dirty
        EditorUtility.SetDirty(_workboardMiniData);

        // ListView 更新（要素数が変わるので Rebuild が確実）
        _taskListView?.Rebuild();

        // 複製した行を選択してスクロール
        if (_taskListView != null)
        {
            _taskListView.SetSelection(insertIndex);
            _taskListView.ScrollToItem(insertIndex);
        }
    }

}
