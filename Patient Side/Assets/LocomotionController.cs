﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LocomotionController : MonoBehaviour
{
    public XRController rightTeleportRay;
    public InputHelpers.Button teleportationActivationButton;
    public float activationThreshold = 0.1f;

    public XRRayInteractor rightInteractorRay;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = new Vector3();
        Vector3 norm = new Vector3();
        int index = 0;
        bool validTarget = false;


        if(rightTeleportRay)
        {
            bool isRightInteractorRayHovering = rightInteractorRay.TryGetHitInfo(ref pos, ref norm, ref index, ref validTarget); 
            rightTeleportRay.gameObject.SetActive(CheckIfActivated(rightTeleportRay) && !isRightInteractorRayHovering);            
        }
    }

    public bool CheckIfActivated(XRController controller)
    {
        InputHelpers.IsPressed(controller.inputDevice, teleportationActivationButton, out bool isActivated, activationThreshold);
        return isActivated;
    }
}
