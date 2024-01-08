using UnityEngine;

public class ComplexGraph : MonoBehaviour
{
    [SerializeField]
    Transform pointPrefab;
    [SerializeField, Range(10,1000)]
    int resolution = 10;
    [SerializeField]
    FunctionLibrary.FunctionName function;
    Transform[] points;
    private void Awake()
    {
        points = new Transform[resolution * resolution];
        float step = 2f / resolution;

        var scale = Vector3.one * step;

        for (int i  = 0, x = 0, z = 0; i < points.Length; i++, x++)
        {
            if (x == resolution)
            {
                x = 0;
                z++;
            }

            Transform point = points[i] = Instantiate(pointPrefab);

            point.SetParent(transform, false);

            point.localScale = scale;
        }
    }
    private void Update()
    {
        float time = Time.time;
        float step = 2f / resolution;
        float v = 0.5f * step - 1f;
        for (int i = 0, x = 0, z = 0; i < points.Length; i++, x++)
        {
            if (x == resolution)
            {
                x = 0;
                z += 1;
                v = (z + 0.5f) * step - 1f;
            }
            float u = (x + 0.5f) * step - 1f;
            points[i].localPosition = FunctionLibrary.GetFunction(function)(u, v, time);

        }
    }
}
