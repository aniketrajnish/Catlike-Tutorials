using UnityEngine;

public class GPUGraph : MonoBehaviour
{
    public enum TransitionMode { Cycle, Random }

    [SerializeField, Range(10,1000)]
    int resolution = 10;

    [SerializeField]
    FunctionLibrary.FunctionName function, transitionFunction;
    [SerializeField]
    TransitionMode transitionMode;

    [SerializeField, Min(0f)]
    float functionDuration = 1f, transitionDuration = 1f;

    [SerializeField]
    ComputeShader computeShader;

    [SerializeField]
    Material material;

    [SerializeField]
    Mesh mesh;

    float duration;
    bool transitioning;

    ComputeBuffer positionsBuffer;

    static readonly int
        positionsId = Shader.PropertyToID("_Positions"),
        resolutionsId = Shader.PropertyToID("_Resolution"),
        stepId = Shader.PropertyToID("_Step"),
        timeId = Shader.PropertyToID("_Time");

    private void OnEnable()
    {
        positionsBuffer = new ComputeBuffer(resolution * resolution, 3*4);
    }
    private void OnDisable()
    {
        positionsBuffer.Release();
        positionsBuffer = null;
    }

    private void Update()
    {
        duration += Time.deltaTime;
        if (transitioning)
        {
            if (duration >= transitionDuration)
            {
                duration -= transitionDuration;
                transitioning = false;
            }
        }
        else if (duration >= functionDuration)
        {
            duration -= functionDuration;
            transitioning = true;
            transitionFunction = function;
            PickNextFunction();
        }

        UpdateFunctionOnGPU();
    }
    void PickNextFunction()
    {
        function = transitionMode == TransitionMode.Cycle ?
            FunctionLibrary.GetNextFunctionName(function) :
            FunctionLibrary.GetRandomFunctionNameOtherThan(function);
    }
    void UpdateFunctionOnGPU()
    {
        float step = 2f / resolution;
        int groups = Mathf.CeilToInt(resolution / 8f);

        computeShader.SetInt(resolutionsId, resolution);
        computeShader.SetFloat(stepId, step);
        computeShader.SetFloat(timeId, Time.time);

        computeShader.SetBuffer(0, positionsId, positionsBuffer);
        computeShader.Dispatch(0,groups, groups, 1);

        material.SetBuffer(positionsId, positionsBuffer);
        material.SetFloat(stepId, step);

        var bounds = new Bounds(Vector3.zero, Vector3.one * (2f + 2f/resolution));
        Graphics.DrawMeshInstancedProcedural(mesh, 0, material, bounds, positionsBuffer.count);
    }
}
