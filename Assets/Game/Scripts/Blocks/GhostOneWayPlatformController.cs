using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostOneWayPlatformController : GhostBlockController {

    OneWayTileController m_OneWayTileController;

    protected override void Start()
    {        
        m_OneWayTileController = GetComponent<OneWayTileController>();
        base.Start();
    }

    protected override void SetGhost(bool active)
    {
        base.SetGhost(active);
        m_OneWayTileController.enabled = active;
    }
}
