using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

	protected virtual void OnEnable()
    {
        Selectable selectable = GetComponentInChildren<Selectable>();
        selectable.Select();
        EventSystem.current.SetSelectedGameObject(selectable.gameObject);
    }
}
