using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class WorkboardMini : EditorWindow
{
    [SerializeField] private VisualTreeAsset m_VisualTreeAsset = default;

    [SerializeField] private WorkboardMiniData _workboardMiniData = null;
    private TaskItem _selectedTaskItem;

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
}
