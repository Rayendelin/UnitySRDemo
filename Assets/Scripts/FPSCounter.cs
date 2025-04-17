using UnityEngine;

public class FPS : MonoBehaviour
{
    public TMPro.TMP_Text m_fpsText;
    private float m_lastUpdateShowTime = 0f;
    private readonly float m_updateTime = 0.1f;
    private int m_frames = 0;
    private float m_frameDeltaTime = 0;
    private float m_FPS = 0;

    private void Awake()
    {
        //Application.targetFrameRate = 1200;
    }
    void Start()
    {
        m_lastUpdateShowTime = Time.realtimeSinceStartup;
        m_fpsText = GetComponent<TMPro.TMP_Text>();
    }

    void Update()
    {
        m_frames++;
        if (Time.realtimeSinceStartup - m_lastUpdateShowTime >= m_updateTime)
        {
            m_FPS = m_frames / (Time.realtimeSinceStartup - m_lastUpdateShowTime);
            m_frameDeltaTime = (Time.realtimeSinceStartup - m_lastUpdateShowTime) / m_frames;
            m_frames = 0;
            m_lastUpdateShowTime = Time.realtimeSinceStartup;
            m_fpsText.SetText(m_FPS.ToString("F1") + " FPS (" + (m_frameDeltaTime * 1000).ToString("F1") + " ms)");
        }
    }
}