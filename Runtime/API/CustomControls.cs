using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

namespace UitkForKsp2.API;

/// <summary>
/// API for everything related to custom UI controls.
/// </summary>
[PublicAPI]
public static class CustomControls
{
    public static MethodInfo? RegisterFactory;

    static CustomControls()
    {
        RegisterFactory =
            Type.GetType("UnityEngine.UIElements.VisualElementFactoryRegistry, UnityEngine.UIElementsModule")?
                .GetMethod("RegisterFactory", BindingFlags.Static | BindingFlags.NonPublic);
        if (RegisterFactory == null)
        {
            UitkForKsp2Plugin.Logger.LogError("Unable to initialize custom controls for UITK for KSP2");
        }
    }
    
    /// <summary>
    /// Register all custom controls from the given assembly using their <see cref="IUxmlFactory"/>.
    /// </summary>
    /// <param name="assembly">The assembly to register the controls from.</param>
    public static void RegisterFromAssembly(Assembly assembly)
    {
        assembly.GetTypes()
            .Where(type => typeof(IUxmlFactory).IsAssignableFrom(type)
                           && !type.IsInterface
                           && !type.IsAbstract
                           && !type.IsGenericType)
            .ToList()
            .ForEach(type =>
            {
                if (Activator.CreateInstance(type) is IUxmlFactory factory) RegisterFactory?.Invoke(null, new object[] { factory });
            });
    }
}