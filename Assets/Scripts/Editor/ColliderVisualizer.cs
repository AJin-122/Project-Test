using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public class ColliderVisualizer
{
    private const string MENU_PATH = "Tools/콜라이더 시각화 켜기/끄기 _%#C";
    
    // 시각화 활성화 상태를 저장할 변수
    private static bool isEnabled
    {
        get => EditorPrefs.GetBool("ColliderVisualizer_Enabled", false);
        set 
        {
            EditorPrefs.SetBool("ColliderVisualizer_Enabled", value);
            Menu.SetChecked(MENU_PATH, value);
            Debug.Log($"콜라이더 시각화 {(value ? "활성화" : "비활성화")} 되었습니다.");
        }
    }

    static ColliderVisualizer()
    {
        SceneView.duringSceneGui += OnSceneGUI;
        Menu.SetChecked(MENU_PATH, isEnabled);
    }

    [MenuItem(MENU_PATH)]
    private static void ToggleVisualization()
    {
        isEnabled = !isEnabled;
        SceneView.RepaintAll();
    }

    static void OnSceneGUI(SceneView sceneView)
    {
        if (!isEnabled) return;

        // 씬 뷰가 업데이트될 때마다 실행
        sceneView.Repaint();

        // 현재 프리팹 스테이지 확인
        var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
        
        Collider2D[] colliders;
        if (prefabStage != null)
        {
            // 프리팹 스테이지에서는 프리팹의 루트 오브젝트에서 검색
            colliders = prefabStage.prefabContentsRoot.GetComponentsInChildren<Collider2D>();
        }
        else
        {
            // 일반 씬에서는 전체 검색
            colliders = Object.FindObjectsByType<Collider2D>(FindObjectsSortMode.None);
        }
        
        foreach (var collider in colliders)
        {
            if (!collider.enabled) continue;

            // 콜라이더 타입에 따라 다른 색상 사용
            Color color = Color.green;
            if (collider is BoxCollider2D) color = new Color(0, 1, 0, 0.5f);
            else if (collider is CircleCollider2D) color = new Color(1, 0, 0, 0.5f);
            else if (collider is PolygonCollider2D) color = new Color(0, 0, 1, 0.5f);

            // 기존 핸들 색상 저장
            Color oldColor = Handles.color;
            Handles.color = color;

            // 콜라이더 타입에 따라 적절한 시각화
            if (collider is BoxCollider2D boxCollider)
            {
                Vector3 center = boxCollider.transform.TransformPoint(boxCollider.offset);
                Vector3 size = boxCollider.size;
                Matrix4x4 rotationMatrix = Matrix4x4.TRS(
                    center,
                    boxCollider.transform.rotation,
                    boxCollider.transform.lossyScale
                );

                using (new Handles.DrawingScope(rotationMatrix))
                {
                    Handles.DrawWireCube(Vector3.zero, size);
                }
            }
            else if (collider is CircleCollider2D circleCollider)
            {
                Vector3 center = circleCollider.transform.TransformPoint(circleCollider.offset);
                float radius = circleCollider.radius * Mathf.Max(
                    Mathf.Abs(circleCollider.transform.lossyScale.x),
                    Mathf.Abs(circleCollider.transform.lossyScale.y)
                );
                Handles.DrawWireDisc(center, Vector3.forward, radius);
            }

            // 핸들 색상 복구
            Handles.color = oldColor;
        }
    }
} 