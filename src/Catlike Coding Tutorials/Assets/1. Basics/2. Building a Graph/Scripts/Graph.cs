using UnityEngine;

public class Graph : MonoBehaviour
{
    [SerializeField]
    Transform pointPrefab;
    [SerializeField, Range(10,1000)]
    int resolution = 10;
    Transform[] points;
    private void Awake()
    {
        points = new Transform[resolution];
        float step = 2f / resolution;

        var position = Vector3.zero;
        var scale = Vector3.one * step * 10f;

        for (int i  = 0; i < points.Length; i++)
        {
            Transform point = points[i] = Instantiate(pointPrefab);

            point.SetParent(transform, false);

            position.x = (i + .5f) * step - 1f;
            point.localPosition = position;

            point.localScale = scale;
        }
    }
    private void Update()
    {
        float time = Time.time;
        for (int i = 0;i < points.Length;i++)
        {
            Transform point = points[i];

            Vector3 position = point.localPosition;
            position.y = Mathf.Sin(Mathf.PI * position.x * Time.time);
            point.localPosition = position;

        }
    }
}
