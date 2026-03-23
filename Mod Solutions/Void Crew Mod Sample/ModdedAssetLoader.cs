using RuntimeAssets;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

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
                    BepinPlugin.Log.LogError($"[ModAssetLoader] Could not resolve DLL directory (dllPath='{dllPath}')");
                    return;
                }

                BepinPlugin.Log.LogInfo($"[ModAssetLoader] Scanning for asset bundle manifests in: {dir}");

                int loaded = 0;

                foreach (var filePath in Directory.EnumerateFiles(dir, "*", SearchOption.TopDirectoryOnly))
                {
                    var fileName = Path.GetFileName(filePath);
                    var fileExtension = Path.GetExtension(filePath);

                    // Skip the mod DLL and any file that has an extension unless it is .metem
                    if (!string.IsNullOrEmpty(fileExtension) && !(fileExtension == ".metem"))
                        continue;

                    // Skip directories just in case
                    if (!File.Exists(filePath))
                        continue;

                    try
                    {
                        var bundle = AssetBundle.LoadFromFile(filePath);
                        if (!(bool) (Object) bundle)
                        {
                            continue;
                        }

                        // Valid asset bundle, unload probe instance before real loading
                        bundle.Unload(true);

                        RuntimeAssetsAPI.LoadAssetBundle(filePath);
                        loaded++;

                        BepinPlugin.Log.LogInfo($"[ModAssetLoader] Loaded asset bundle: {fileName}");
                    }
                    catch (Exception ex)
                    {
                        BepinPlugin.Log.LogError($"[ModAssetLoader] Error while probing/loading '{fileName}': {ex}");
                    }
                }

                BepinPlugin.Log.LogInfo($"[ModAssetLoader] Loaded {loaded} asset bundle(s) from manifest pairs.");
            }
            catch (Exception e)
            {
                BepinPlugin.Log.LogError($"[ModAssetLoader] Failed loading asset bundles: {e}");
            }
        }
    }
}