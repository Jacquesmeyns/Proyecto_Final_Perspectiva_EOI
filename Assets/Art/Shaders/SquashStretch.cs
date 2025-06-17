using System;
using UnityEngine;
using UnityEngine.Assertions.Must;

namespace Art.Shaders
{
    public class SquashStretch : MonoBehaviour
    {
         [Header("Referencias")]
    public Transform visualChild;
    public Transform colliderChild;
    public Material deformationMaterial;
    private Rigidbody rb;
    
    [Header("Configuración")]
    [Range(0, 1)] public float deformationStrength = 0f;
    [Range(0, 5)] public float debugDeformation = 0f;
    [Range(0, 5)] public float debugDirectionX = 0f;
    [Range(0, 5)] public float debugDirectionY = 1f;
    [Range(0, 5)] public float debugDirectionZ = 0f;

    private Vector3 lastScale = Vector3.one;
    private bool isRuntime = false;

    void OnEnable()
    {
        isRuntime = Application.isPlaying;
    }

    private void Start()
    {
        rb = colliderChild.GetComponent<Rigidbody>();
        //deformationMaterial = new Material(deformationMaterial);
    }

    void Update()
    {
        // Solo aplicar en tiempo de ejecución a menos que estemos debuggeando
        if (!isRuntime && debugDeformation <= 0f) return;
        
        //Aplicar deformación al shader
        deformationMaterial.SetFloat("_DeformationAmount", rb.linearVelocity.magnitude * deformationStrength);
        deformationMaterial.SetVector("_DeformationDirection", -rb.linearVelocity.normalized);
        
        // Obtener los parámetros actuales del shader
        float deformation = isRuntime ? 
            deformationMaterial.GetFloat("_DeformationAmount") : 
            debugDeformation;
            
        Vector3 direction = isRuntime ?
            deformationMaterial.GetVector("_DeformationDirection") :
            new Vector3(debugDirectionX, debugDirectionY, debugDirectionZ);

        // Calcular nueva escala para ambos hijos
        //Vector3 newScale = CalculateDeformedScale(deformation, direction);
        
        // Aplicar la misma escala a ambos hijos
        //if(visualChild != null) 
        //    visualChild.localScale = newScale;
            
        //if(colliderChild != null) 
        //    colliderChild.localScale = newScale;
    }

    Vector3 CalculateDeformedScale(float deformation, Vector3 direction)
    {
        direction.Normalize();
        
        // Valores base (ajustar según necesidad)
        float stretchFactor = 1f + deformation * 0.5f;
        float squashFactor = 1f - deformation * 0.3f;
        
        // Matriz de escala basada en dirección
        Matrix4x4 scaleMatrix = Matrix4x4.identity;
        
        // Componente principal de estiramiento
        scaleMatrix.m00 = Mathf.Lerp(1f, stretchFactor, Mathf.Abs(direction.x));
        scaleMatrix.m11 = Mathf.Lerp(1f, stretchFactor, Mathf.Abs(direction.y));
        scaleMatrix.m22 = Mathf.Lerp(1f, stretchFactor, Mathf.Abs(direction.z));
        
        // Componentes de aplastamiento perpendicular
        float avgSquash = (squashFactor * (3f - (
            Mathf.Abs(direction.x) + 
            Mathf.Abs(direction.y) + 
            Mathf.Abs(direction.z)))) / 2f;
            
        scaleMatrix.m00 *= Mathf.Lerp(squashFactor, 1f, Mathf.Abs(direction.x));
        scaleMatrix.m11 *= Mathf.Lerp(squashFactor, 1f, Mathf.Abs(direction.y));
        scaleMatrix.m22 *= Mathf.Lerp(squashFactor, 1f, Mathf.Abs(direction.z));
        
        // Convertir matriz a escala
        return new Vector3(scaleMatrix.m00, scaleMatrix.m11, scaleMatrix.m22);
    }

    #if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (!isRuntime && debugDeformation > 0f)
        {
            Gizmos.color = Color.cyan;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireSphere(Vector3.zero, 1f);
            
            Vector3 dir = new Vector3(
                debugDirectionX, 
                debugDirectionY, 
                debugDirectionZ).normalized;
                
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(Vector3.zero, dir * (1f + debugDeformation));
        }
    }
    #endif
    }
}