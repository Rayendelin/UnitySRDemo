using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Profiling;
using UnityEngine.Experimental.Rendering;
using System.Runtime.InteropServices.WindowsRuntime;
using System;
using System.Runtime.InteropServices;
using System.Collections;


namespace TENCENT.SRPLUGIN
{
    public class SRPlugin
    {
        public enum SRMode
        {
            CommonRender,
            LowResolution,
            SuperResolution
        }

        public bool hasInit = false;
        float m_ratioSR = 2;
        Vector2Int m_renderSize;
        Vector2Int m_displaySize;
        RenderTexture m_cameraTargetLR;
        RenderTexture m_cameraTargetHR;

        // Task inputs
        RenderTexture m_currentRGBALR;
        RenderTexture m_currentDepthLR;
        RenderTexture m_currentMotionLR;

        // Intermediate results
        RenderTexture m_prevRGBAHR;
        RenderTexture m_currentRGBAHR;

        // Task outputs
        RenderTexture m_upscaleRGBA;
        RenderTexture m_displayRGBA;

        // Shaders
        Material m_getMVShader;
        Material m_getDepthShader;
        ComputeShader m_EASUShader;
        ComputeShader m_RCASShader;

        // Context resources
        ScriptableRenderContext m_context;
        Camera m_camera;
        private CommandBuffer m_commandBuffer = new CommandBuffer { name = "SRPlugin CommandBuffer" };

        // Constructor and deconstructor
        public SRPlugin() {}
        ~SRPlugin() {}

        // Tool function
        private RenderTexture GetRenderTexture(in string name, int width, int height, GraphicsFormat format, bool random_write, int depth_buffer_bits = 32)
        {
            RenderTextureDescriptor RTD = new RenderTextureDescriptor(width, height, RenderTextureFormat.Default, depth_buffer_bits);
            RTD.msaaSamples = 1;
            RTD.graphicsFormat = format;
            RTD.enableRandomWrite = random_write;
            RTD.useMipMap = false;
            RTD.autoGenerateMips = false;
            RenderTexture rt = new RenderTexture(RTD);
            rt.name = name;
            rt.Create();
            return rt;
        }
        private void Submit(ScriptableRenderContext context, CommandBuffer commandBuffer)
        {
            ExecuteCommandBufferAndClear(context, commandBuffer);
            context.Submit();
        }
        private void SubmitImmediate(CommandBuffer commandBuffer)
        {
            Graphics.ExecuteCommandBuffer(commandBuffer);
            commandBuffer.Clear();
        }
        private void ExecuteCommandBufferAndClear(ScriptableRenderContext context, CommandBuffer commandBuffer)
        {
            context.ExecuteCommandBuffer(commandBuffer);
            commandBuffer.Clear();
        }

        // Init function
        public void Init(ScriptableRenderContext renderContext, Camera camera)
        {
            Debug.Log("SRPlugin init start.");

            // Only consider fixed screen resolution and super-resolution ratio now
            m_renderSize = new Vector2Int((int)(Screen.width / m_ratioSR), (int)(Screen.height / m_ratioSR));
            m_displaySize = new Vector2Int(Screen.width, Screen.height);
            hasInit = true;

            InitRenderTextureLR(m_renderSize.x, m_renderSize.y);
            InitRenderTextureHR(m_displaySize.x, m_displaySize.y);
            InitShader();
        }
        private void InitRenderTextureLR(int width, int height)
        {
            var rgba32f = GraphicsFormat.R32G32B32A32_SFloat;
            var r32ui = GraphicsFormat.R32_UInt;

            m_cameraTargetLR = GetRenderTexture("cameraTargetLR", width, height, rgba32f, false);
            m_currentRGBALR = GetRenderTexture("currentRGBALR", width, height, rgba32f, false);
            m_currentDepthLR = GetRenderTexture("currentDepthLR", width, height, rgba32f, false);
            m_currentMotionLR = Resources.Load<RenderTexture>("RenderTextures/MotionVectorLR");
        }
        private void InitRenderTextureHR(int width, int height)
        {
            var rgba32f = GraphicsFormat.R32G32B32A32_SFloat;

            m_displayRGBA = GetRenderTexture("displayRGBA", width, height, rgba32f, true);
            m_prevRGBAHR = GetRenderTexture("prevRGBAHR", width, height, rgba32f, true);
            m_currentRGBAHR = GetRenderTexture("currentRGBAHR", width, height, rgba32f, true);
            m_cameraTargetHR = GetRenderTexture("cameraTargetHR", width, height, rgba32f, true);
            m_upscaleRGBA = GetRenderTexture("upscaleRGBA", width, height, rgba32f, true);
        }
        private void InitShader()
        {
            m_EASUShader = Resources.Load<ComputeShader>("Shaders/EASU");
            m_EASUShader.SetTexture(0, "currentRGBLR", m_currentRGBALR);
            m_EASUShader.SetTexture(0, "currentRGBHR", m_upscaleRGBA);

            m_RCASShader = Resources.Load<ComputeShader>("Shaders/RCAS");
            m_RCASShader.SetTexture(0, "inputTexture", m_upscaleRGBA);
            m_RCASShader.SetTexture(0, "outputTexture", m_displayRGBA);

            m_getMVShader = new Material(Shader.Find("SRPlugin/GetMotionVector"));
            m_getDepthShader = new Material(Shader.Find("SRPlugin/GetCameraDepth"));
        }

        // Tool function
        public ref RenderTexture GetTargetTextureSR()
        {
            return ref m_cameraTargetHR;
        }
        public ref RenderTexture GetTargetTextureLR()
        {
            return ref m_cameraTargetLR;
        }
        public void PresentRenderFrameSR(ScriptableRenderContext context, Camera camera)
        {
            context.SetupCameraProperties(camera);
            m_commandBuffer.Blit(m_cameraTargetHR, camera.targetTexture);
            Submit(context, m_commandBuffer);
        }
        public void PresentRenderFrameLR(ScriptableRenderContext context, Camera camera)
        {
            context.SetupCameraProperties(camera);
            m_commandBuffer.Blit(m_cameraTargetLR, camera.targetTexture);
            Submit(context, m_commandBuffer);
        }
        public void PresentResult(ScriptableRenderContext context, Camera camera)
        {
            context.SetupCameraProperties(camera);
            m_commandBuffer.Blit(m_displayRGBA, camera.targetTexture);
            Submit(context, m_commandBuffer);
        }
        public void UpdateInputRT(ScriptableRenderContext context, Camera camera)
        {
            m_commandBuffer.Blit(null, m_currentDepthLR, m_getDepthShader);
            m_commandBuffer.Blit(null, m_currentMotionLR, m_getMVShader);
            m_commandBuffer.Blit(m_cameraTargetLR, m_currentRGBALR);
            SubmitImmediate(m_commandBuffer);
        }

        public void RunSuperResolution(ScriptableRenderContext context, Camera camera)
        {
            m_context = context;
            m_camera = camera;

            Profiler.BeginSample("SetupRender");
            SetupRender();
            Profiler.EndSample();

            Profiler.BeginSample("TestSuperResolution");
            Test(context, camera);
            Profiler.EndSample();

            Submit(m_context, m_commandBuffer);
        }

        private void Test(ScriptableRenderContext context, Camera camera)
        {
            Profiler.BeginSample("SuperResolution");
            m_EASUShader.SetFloats("renderSize", m_renderSize.x, m_renderSize.y);
            m_EASUShader.SetFloats("displaySize", m_displaySize.x, m_displaySize.y);
            m_EASUShader.Dispatch(0, m_displaySize.x / 4, m_displaySize.y / 4, 1);
            SubmitImmediate(m_commandBuffer);

            m_RCASShader.SetFloats("displaySize", m_displaySize.x, m_displaySize.y);
            m_RCASShader.Dispatch(0, m_displaySize.x / 4, m_displaySize.y / 4, 1);
            SubmitImmediate(m_commandBuffer);
            Profiler.EndSample();
        }

        private void SetupRender()
        {
            m_context.SetupCameraProperties(m_camera);
            m_commandBuffer.ClearRenderTarget(true, true, Color.clear);
            ExecuteCommandBufferAndClear(m_context, m_commandBuffer);
        }
    }
}