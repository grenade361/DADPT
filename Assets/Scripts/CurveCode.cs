using UnityEngine;

public class CurveCode : MonoBehaviour
{
    [SerializeField] private float curvature = 2.0f;
    [SerializeField] private float trimming = 0.3f;

    private void Update()
    {
        Shader.SetGlobalFloat("_Curvature", curvature);
        Shader.SetGlobalFloat("_Trimming", trimming);
    }
}
