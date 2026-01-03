using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class WorkboardMini : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

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
        var left = splitView.Q<VisualElement>("LeftPane");
        var right = splitView.Q<VisualElement>("RightPane");

        left.Add(new Label("LEFT"));
        right.Add(new Label("RIGHT"));

        root.Add(uxmlElement);
    }
}
