using UnityEditor;
using UnityEngine;

public class CreateAssetBundles
{
    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        // Adjust the path to your font asset
        string fontPath = "Assets/Fonts/misaki_gothic SDF.asset";

        AssetImporter importer = AssetImporter.GetAtPath(fontPath);
        if (importer == null)
        {
            Debug.LogError($"Font not found at: {fontPath}");
            return;
        }

        // Assign a bundle name
        importer.assetBundleName = "misaki_font";

        // Output folder (outside Assets)
        string outputPath = "AssetBundles/StandaloneWindows64";
        if (!System.IO.Directory.Exists(outputPath))
            System.IO.Directory.CreateDirectory(outputPath);

        // Build
        BuildPipeline.BuildAssetBundles(outputPath, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);

        // Clean up (optional)
        importer.assetBundleName = null;

        AssetDatabase.Refresh();
        Debug.Log("AssetBundle build completed.");
    }
}