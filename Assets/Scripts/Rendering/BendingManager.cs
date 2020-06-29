using System;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteAlways]
public class BendingManager : MonoBehaviour
{
  #region Constants

  private const string BENDING_FEATURE = "ENABLE_BENDING";

  private const string PLANET_FEATURE = "ENABLE_BENDING_PLANET";

  private static readonly int BENDING_AMOUNT_X =
    Shader.PropertyToID("_BendingAmountX");

  private static readonly int BENDING_AMOUNT_Y =
    Shader.PropertyToID("_BendingAmountY");

  #endregion


  #region Inspector

  [SerializeField]
  private bool enablePlanet = default;

  [SerializeField]
  [Range(-0.01f, 0.01f)]
  private float bendingAmountX = 0.0f;

  [SerializeField]
  [Range(-0.01f, 0.01f)]
  private float bendingAmountY = 0.0f;
  #endregion


  #region Fields

  private float _prevAmount;

  #endregion


  #region MonoBehaviour

  private void Awake ()
  {
    if ( Application.isPlaying )
      Shader.EnableKeyword(BENDING_FEATURE);
    else
      Shader.DisableKeyword(BENDING_FEATURE);

    if ( enablePlanet )
      Shader.EnableKeyword(PLANET_FEATURE);
    else
      Shader.DisableKeyword(PLANET_FEATURE);

    UpdateBendingAmountX();
    UpdateBendingAmountY();
  }

  private void OnEnable ()
  {
    if ( !Application.isPlaying )
      return;
    
    RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
    RenderPipelineManager.endCameraRendering += OnEndCameraRendering;
  }

  private void Update ()
  {
    if ( Math.Abs(_prevAmount - bendingAmountX) > Mathf.Epsilon )
      UpdateBendingAmountX();
    
    if( Math.Abs(_prevAmount - bendingAmountY) > Mathf.Epsilon )
      UpdateBendingAmountY();
  }

  private void OnDisable ()
  {
    RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
    RenderPipelineManager.endCameraRendering -= OnEndCameraRendering;
  }

  #endregion


  #region Methods

  private void UpdateBendingAmountX ()
  {
    _prevAmount = bendingAmountX;
    Shader.SetGlobalFloat(BENDING_AMOUNT_X, bendingAmountX);
  }

  private void UpdateBendingAmountY ()
  {
    _prevAmount = bendingAmountY;
    Shader.SetGlobalFloat(BENDING_AMOUNT_Y, bendingAmountY);
  }


  private static void OnBeginCameraRendering (ScriptableRenderContext ctx,
                                              Camera cam)
  {
    cam.cullingMatrix = Matrix4x4.Ortho(-99, 99, -99, 99, 0.001f, 99) *
                        cam.worldToCameraMatrix;
  }

  private static void OnEndCameraRendering (ScriptableRenderContext ctx,
                                            Camera cam)
  {
    cam.ResetCullingMatrix();
  }

  #endregion
}