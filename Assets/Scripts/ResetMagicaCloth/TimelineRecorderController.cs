using UnityEngine;
using MagicaCloth2;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Recorder;
using UnityEditor.Recorder.Input;
#endif

public class TimelineRecorderController : MonoBehaviour
{
    [Header("Dependencies")]
    public MagicaClothReset clothReset;
    public RandomSeedInitializer seedInitializer;

    [Header("Recording Settings")]
    [Tooltip("等待布料稳定的时间（秒）")]
    public float preResetDelay = 0.5f;
    public bool startRecordingOnAwake = false;

    void Awake()
    {
        if (startRecordingOnAwake)
            StartRecording();
    }

    public void StartRecording()
    {
        StartCoroutine(RecordingRoutine());
    }

    public void StopRecording()
    {
// #if UNITY_EDITOR
//         StopRecordingInternal();
// #endif
    }

    private IEnumerator RecordingRoutine()
    {
        seedInitializer.ResetSeed();
        clothReset.ResetAllCloths();
        yield return new WaitForSeconds(preResetDelay);

// #if UNITY_EDITOR
//         StartRecordingInternal();
// #endif
    }

    #region Editor-Only Recording Logic (适配 Recorder 4.0.3)
// #if UNITY_EDITOR

//     private RecorderController _recorderController;

//     private void StartRecordingInternal()
//     {
//         var settings = ScriptableObject.CreateInstance<RecorderControllerSettings>();
//         _recorderController = new RecorderController(settings);

//         var movieSettings = ScriptableObject.CreateInstance<MovieRecorderSettings>();
//         movieSettings.name = "MagicaCloth对比录制";
//         movieSettings.Enabled = true;

//         movieSettings.ImageInputSettings = new GameViewInputSettings
//         {
//             OutputWidth = 1920,
//             OutputHeight = 1080
//         };

//         movieSettings.OutputFormat = MovieRecorderSettings.VideoRecorderOutputFormat.MP4;
//         movieSettings.OutputFile = "Recordings/cloth_compare";

//         settings.AddRecorderSettings(movieSettings);

//         _recorderController.PrepareRecording();
//         _recorderController.StartRecording();
//         Debug.Log("录制已开始！");
//     }

//     private void StopRecordingInternal()
//     {
//         if (_recorderController == null)
//             return;

//         // 修复点：调用方法 IsRecording()
//         if (_recorderController.IsRecording())
//         {
//             _recorderController.StopRecording();
//             Debug.Log("录制已停止！");
//         }
//     }

// #endif
    #endregion
}