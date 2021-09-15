using UnityEngine;

public class deformation : MonoBehaviour {
    private MeshFilter mf;
    private Mesh m;
    private MeshCollider mc;
    private Vector3[] verts;
    private Vector3[] norms;
    private Vector3[] orgVerts;
    private Vector3[] orgNorms;
    [Range(0, 1)] public float maxDeformation = 1f;
    [Range(0, 10)] public float radius = 2.5f;
    private Camera cam;

    void Start() {
        mf = GetComponent<MeshFilter>();
        mc = GetComponent<MeshCollider>();
        m = mf.mesh;
        cam = Camera.main;
        print("What could possibly go wrong here...");
        ShowNormals();
    }

    private void Update() {
        //ResetMesh();
        MouseInteraction();
    }

    void MouseInteraction() {
        RaycastHit hit;
        if (Input.GetMouseButton(0) && Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit)) {
            if (hit.collider.gameObject == gameObject) {
                Vector3 point = hit.point;
                point = transform.InverseTransformPoint(point);
                DeformMesh(maxDeformation, point, radius);
            }
        }
    }

    void LoadInMesh() {
        verts = m.vertices;
        norms = m.normals;
        orgVerts = m.vertices;
        orgNorms = m.normals;
    }

    void ResetMesh() {
        m.vertices = orgVerts;
        m.normals = orgNorms;
    }

    public void DeformMesh(float maxDeformation, Vector3 center, float radius) {
        LoadInMesh();
        float distance;
        float a;
        maxDeformation *= .5f;

        for (int i = 0; i < verts.Length; i++) {
            distance = Vector3.Distance(verts[i], center);
            if (distance < radius) {
                a = Mathf.Cos(distance * Mathf.PI / radius) * maxDeformation + maxDeformation;
                verts[i] += norms[i] * a;
            }
        }

        m.vertices = verts;
        mc.sharedMesh = m;
        mc.convex = true;
    }

    public void ShowNormals() {
        LoadInMesh();
        for (int i = 0; i < verts.Length; i++) {
            Debug.DrawLine(verts[i], verts[i] + norms[i], Color.magenta, Mathf.Infinity);
        }
    }
}