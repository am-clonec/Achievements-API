using System;
using System.IO;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AchievementsAPI.Reactor.Embedded;

public static class Extensions
{
    /// <summary>
    /// Fully reads the <paramref name="input"/> stream.
    /// </summary>
    /// <param name="input">The stream to read.</param>
    /// <returns>A byte array read from the <see cref="Stream"/>.</returns>
    // https://github.com/NuclearPowered/Reactor/blob/04100970b66d57a2dd96f6f4eae0383362e9a321/Reactor/Utilities/Extensions/StreamExtensions.cs#L125
    public static byte[] ReadFully(this Stream input)
    {
        using var ms = new MemoryStream();
        input.CopyTo(ms);
        return ms.ToArray();
    }
    
    /// <summary>
    /// Loads an asset with <paramref name="name"/> from the <paramref name="bundle"/> with the specified <typeparamref name="T"/> type.
    /// </summary>
    /// <param name="bundle">The <see cref="AssetBundle"/> to load the asset from.</param>
    /// <param name="name">The name of the asset.</param>
    /// <typeparam name="T">The type of the asset.</typeparam>
    /// <returns>The loaded asset or null if it wasn't found.</returns>
    // https://github.com/NuclearPowered/Reactor/blob/04100970b66d57a2dd96f6f4eae0383362e9a321/Reactor/Utilities/Extensions/AssetBundleExtensions.cs#L18
    public static T? LoadAsset<T>(this AssetBundle bundle, string name) where T : Object
    {
        return bundle.LoadAsset(name, Il2CppType.Of<T>())?.Cast<T>();
    }
    
    /// <summary>
    /// Stops <paramref name="obj"/> from being destroyed.
    /// </summary>
    /// <param name="obj">The object to stop from being destroyed.</param>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <returns>Passed <paramref name="obj"/>.</returns>
    // https://github.com/NuclearPowered/Reactor/blob/04100970b66d57a2dd96f6f4eae0383362e9a321/Reactor/Utilities/Extensions/UnityObjectExtensions.cs
    public static T DontDestroy<T>(this T obj) where T : Object
    {
        obj.hideFlags |= HideFlags.HideAndDontSave;

        return obj.DontDestroyOnLoad();
    }
    
    /// <summary>
    /// Stops <paramref name="obj"/> from being destroyed on load.
    /// </summary>
    /// <param name="obj">The object to stop from being destroyed on load.</param>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <returns>Passed <paramref name="obj"/>.</returns>
    // https://github.com/NuclearPowered/Reactor/blob/04100970b66d57a2dd96f6f4eae0383362e9a321/Reactor/Utilities/Extensions/UnityObjectExtensions.cs
    public static T DontDestroyOnLoad<T>(this T obj) where T : Object
    {
        Object.DontDestroyOnLoad(obj);

        return obj;
    }
    
    /// <summary>
    /// Creates a span over a <see cref="Il2CppStructArray{T}"/>.
    /// </summary>
    /// <param name="array">The array to create a span over.</param>
    /// <typeparam name="T">The type of items in the <see cref="Il2CppStructArray{T}"/>.</typeparam>
    /// <returns>A span.</returns>
    // https://github.com/NuclearPowered/Reactor/blob/04100970b66d57a2dd96f6f4eae0383362e9a321/Reactor/Utilities/Extensions/Il2CppInteropExtensions.cs#L17
    public static unsafe Span<T> ToSpan<T>(this Il2CppStructArray<T> array) where T : unmanaged
    {
        return new Span<T>(IntPtr.Add(array.Pointer, IntPtr.Size * 4).ToPointer(), array.Length);
    }
}