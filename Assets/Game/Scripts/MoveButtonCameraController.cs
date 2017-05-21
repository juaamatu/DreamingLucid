using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Petro Pitkänen, Juho Turpeinen
/// Moves Camera to position and out
/// </summary>
public class MoveButtonCameraController : MonoBehaviour {

    private FollowCameraController cameraRig;

    void Start () {
        cameraRig = FindObjectOfType<FollowCameraController>();
    }
	
	/// <summary>
    /// Moves camera to transform
    /// </summary>
    public void MoveCamera()
    {
        
            cameraRig.MoveCamera(transform);
        
    }
    
    /// <summary>
    /// Moves camera back from transform
    /// </summary>
    public void MoveCameraBack()
    {
        
            cameraRig.MoveCameraBack();
          
    }

    void Update()
    {
        
    }
}
