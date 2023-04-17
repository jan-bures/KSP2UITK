﻿using UnityEngine;
using UnityEngine.UIElements;
using UnityObject = UnityEngine.Object;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace UitkForKsp2.API;

public static class Window
{
    private const string UIMainCanvasPath = "GameManager/Default Game Instance(Clone)/UI Manager(Clone)/Main Canvas";

    /// <summary>
    /// Creates an empty UIDocument with a pre-styled root element.
    /// </summary>
    /// <param name="root">Pre-styled root element of the UIDocument.</param>
    /// <param name="windowId">Unique ID for the game object. Autogenerated if null.</param>
    /// <param name="parent">Parent game object transform. Uses "UI Manager(Clone)/Main Canvas" if null.</param>
    /// <param name="makeDraggable">Can the window be moved by dragging?</param>
    /// <returns>New empty UIDocument.</returns>
    public static UIDocument Create(
        out VisualElement root,
        string windowId = null,
        Transform parent = null,
        bool makeDraggable = false
    )
    {
        var document = CreateInternal(windowId, parent);

        root = Element.Root();
        if (makeDraggable)
        {
            root.MakeDraggable();
        }

        document.m_RootVisualElement = root;
        document.AddRootVisualElementToTree();

        return document;
    }

    /// <summary>
    /// Creates a new UIDocument from a UXML asset.
    /// </summary>
    /// <param name="uxml">UXML asset containing the UI.</param>
    /// <param name="windowId">Unique ID for the game object. Autogenerated if null.</param>
    /// <param name="parent">Parent game object transform. Uses "UI Manager(Clone)/Main Canvas" if null.</param>
    /// <param name="makeDraggable">Can the window be moved by dragging?</param>
    /// <returns>UIDocument with the UI defined in UXML.</returns>
    public static UIDocument CreateFromUxml(
        VisualTreeAsset uxml,
        string windowId = null,
        Transform parent = null,
        bool makeDraggable = false
    )
    {
        var document = CreateInternal(windowId, parent);

        document.sourceAsset = uxml;
        document.RecreateUI();

        if (makeDraggable)
        {
            document.m_RootVisualElement.MakeDraggable();
        }

        return document;
    }

    /// <summary>
    /// Creates a new UIDocument from an existing root VisualElement. Doesn't include default root style.
    /// </summary>
    /// <param name="element">Root element of the UIDocument.</param>
    /// <param name="windowId">Unique ID for the game object. Autogenerated if null.</param>
    /// <param name="parent">Parent game object transform. Uses "UI Manager(Clone)/Main Canvas" if null.</param>
    /// <param name="makeDraggable">Can the window be moved by dragging?</param>
    /// <returns>New UIDocument with the element parameter as root.</returns>
    public static UIDocument CreateFromElement(
        VisualElement element,
        string windowId = null,
        Transform parent = null,
        bool makeDraggable = false
    )
    {
        var document = CreateInternal(windowId, parent);

        if (makeDraggable)
        {
            element.MakeDraggable();
        }

        document.m_RootVisualElement = element;
        document.AddRootVisualElementToTree();

        return document;
    }

    private static UIDocument CreateInternal(string windowId = null, Transform parent = null)
    {
        var gameObject = new GameObject(windowId ?? $"ui-{Guid.NewGuid()}");
        UnityObject.DontDestroyOnLoad(gameObject);
        gameObject.hideFlags |= HideFlags.HideAndDontSave;

        var document = gameObject.AddComponent<UIDocument>();

        document.panelSettings = UitkForKsp2Plugin.PanelSettings;
        document.enabled = true;

        if (parent == null && GameObject.Find(UIMainCanvasPath) is var uiMainCanvas)
        {
            if (uiMainCanvas != null)
            {
                parent = uiMainCanvas.transform;
            }
            else
            {
                UitkForKsp2Plugin.Logger.LogWarning($"Could not assign default parent to new window with ID {windowId}");
            }
        }

        gameObject.transform.parent = parent;
        gameObject.SetActive(true);

        return document;
    }
}