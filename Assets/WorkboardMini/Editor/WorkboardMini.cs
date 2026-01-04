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
        if (selected == null) 
        {
            _placeholder.style.display = DisplayStyle.Flex;
            _taskDetail.style.display = DisplayStyle.None;
            return; 
        }

        _placeholder.style.display = DisplayStyle.None;
        _taskDetail.style.display = DisplayStyle.Flex;

        var titleTextField = _taskDetail.Q<TextField>("TitleTextField");
        var statusEnumField = _taskDetail.Q<EnumField>("StatusEnumField");
        statusEnumField.Init(TaskStatus.Done);
        var priorityEnumField = _taskDetail.Q<EnumField>("PriorityEnumField");
        priorityEnumField.Init(TaskPriority.Low);
        var memoTextField = _taskDetail.Q<TextField>("MemoTextField");
        var dueTextField = _taskDetail.Q<TextField>("DueTextField");

        titleTextField.value = selected.Title;
        statusEnumField.value = selected.Status;
        priorityEnumField.value = selected.Priority;
        memoTextField.value = selected.Memo;
        dueTextField.value = selected.DueDate;

    }
}
