using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class Rotate : MonoBehaviour
{
    public Vector2 rot;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(rot.x, rot.y, 0);

    }
}
