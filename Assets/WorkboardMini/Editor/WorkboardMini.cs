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
            if (selectedItems == null) return;

            foreach ( var item in selectedItems)
            {
                var selected = item as TaskItem;
                if(selected != null)
                {
                    Debug.Log(selected.Title);
                }
            }
        };

        _taskListView.RefreshItems();

        // RightPane : タスク詳細表示・編集
        _rightPane = splitView.Q<VisualElement>("RightPane");

        _leftPane.Add(new Label("LEFT"));
        _rightPane.Add(new Label("RIGHT"));

        root.Add(uxmlElement);
    }
}
