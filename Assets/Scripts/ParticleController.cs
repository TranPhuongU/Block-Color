using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleController : MonoBehaviour
{
    public Camera cam;

    [Header("Base values (lúc FX đẹp nhất)")]
    public float baseOrthoSize = 5f;
    public float baseStartSize = 0.2f;

    ParticleSystem ps;
    ParticleSystem.MainModule main;

    void Awake()
    {
        if (!cam) cam = Camera.main;
        ps = GetComponent<ParticleSystem>();
        main = ps.main;

        // Lưu size gốc nếu quên set tay
        if (baseStartSize <= 0f)
            baseStartSize = main.startSize.constant;
    }

    void LateUpdate()
    {
        float ratio = cam.orthographicSize / baseOrthoSize;
        main.startSize = baseStartSize * ratio;
    }
}
