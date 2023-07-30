using UnityEngine;

public enum WireType {
    Straight,
    Poly,
    Bezier
}

public class Wire : SelectableObject {
    [Header("Parameters")]
    public SwitchBox SwitchBox;
    public WireType Type;
    public ObjectView ObjectView;
    public ElectricFieldMovingView ElectricFieldMovingView;

    [Header("Contacts")]
    public Contact StartContact;
    public WagoContact EndContact;

    public override void Start() {
        base.Start();
        ObjectView.Initialization(this);
        ElectricFieldMovingView.SetObject(this);
    }

    public override void OnHover() {
        ObjectView.OnHover(IsSelected);
    }

    public override void OnUnhover() {
        ObjectView.OnUnhover(IsSelected);
    }

    public override void Select() {
        Debug.Log(gameObject.name);
        base.Select();
        ObjectView.OnHover(IsSelected);
        ObjectView.Select();
    }

    public override void Unselect() {
        base.Unselect();
        ObjectView.OnUnhover(IsSelected);
        ObjectView.Unselect();
    }

    public override void OnMouseDrag() {
        base.OnMouseDrag();
    }

    [ContextMenu("GenerateMeshCollider")]
    public void GenerateMeshCollider() {
        // Получаем ссылки на LineRenderer и MeshCollider компоненты
        MeshCollider meshCollider = GetComponent<MeshCollider>();     
        if (meshCollider == null) {
            meshCollider = gameObject.AddComponent<MeshCollider>();
        }
        meshCollider.sharedMesh = null;

        LineRenderer lineRenderer = ObjectView.LineRenderer;
        // Проверяем, что оба компонента присутствуют
        if (lineRenderer == null || meshCollider == null) {
            Debug.LogError("LineToCollider: LineRenderer или MeshCollider не найдены!");
            return;
        }

        Mesh mesh = new Mesh();
        lineRenderer.BakeMesh(mesh, true);

        Vector3[] newArray = new Vector3[mesh.vertexCount];
        for (int i = 0; i < mesh.vertexCount; i++) {
            newArray[i] = transform.InverseTransformPoint(mesh.vertices[i]);
        }
        mesh.vertices = newArray;

        meshCollider.sharedMesh = mesh;
    }

    public void SetNewPositionStartContact() {
        ObjectView.PathElements[0].position = StartContact.transform.position;
        ObjectView.UpdatePoints();
        ElectricFieldMovingView.UpdatePoints();
    }

    public void SetNewPositionEndContact() {
        ObjectView.PathElements[ObjectView.PathElements.Length-1].position = EndContact.transform.position;
        ObjectView.UpdatePoints();
        ElectricFieldMovingView.UpdatePoints();
    }

    public void SetElectricFieldSettings(ElectricFieldSettings settings) {
        ElectricFieldMovingView.SetMaterial(settings.Material);
        ElectricFieldMovingView.SetColor(settings.Color);
    }

    public void SetElectricFieldLineSettings(ElectricFieldSettings settings) {
        ElectricFieldMovingView.SetMaterial(settings.Material);
        ElectricFieldMovingView.SetColor(settings.Color);
        ElectricFieldMovingView.SwichDirection();
    }
}
