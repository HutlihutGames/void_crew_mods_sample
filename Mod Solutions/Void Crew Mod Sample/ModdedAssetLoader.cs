using HarmonyLib;
using RuntimeAssets;
using System;
using System.IO;
using System.Reflection;
using CG;

namespace Void_Crew_Mod_Sample
{
    public class ModdedAssetLoader
    {
        public static void TryLoadAssetBundlesNextToDll()
        {
            try
            {
                var asm = Assembly.GetExecutingAssembly();

                var dllPath = !string.IsNullOrWhiteSpace(asm.Location)
                    ? asm.Location
                    : new Uri(asm.CodeBase).LocalPath;

                var dir = Path.GetDirectoryName(dllPath);
                if (string.IsNullOrWhiteSpace(dir) || !Directory.Exists(dir))
                {
                    Debug.LogError($"[ModAssetLoader] Could not resolve DLL directory (dllPath='{dllPath}')");
                    return;
                }

                Debug.Log($"[ModAssetLoader] Scanning for asset bundle manifests in: {dir}");

                int loaded = 0;

                foreach (var manifestPath in Directory.EnumerateFiles(dir, "*.manifest", SearchOption.TopDirectoryOnly))
                {
                    // Bundle file is the manifest path without ".manifest"
                    var bundlePath = manifestPath.Substring(0, manifestPath.Length - ".manifest".Length);

                    if (!File.Exists(bundlePath))
                    {
                        Debug.LogError($"[ModAssetLoader] Missing asset bundle for manifest: {manifestPath}");
                        continue;
                    }

                    RuntimeAssetsAPI.LoadAssetBundle(bundlePath);
                    loaded++;
                }

                Debug.Log($"[ModAssetLoader] Loaded {loaded} asset bundle(s) from manifest pairs.");
            }
            catch (Exception e)
            {
                Debug.LogError($"[ModAssetLoader] Failed loading asset bundles: {e}");
            }
        }
    }
}