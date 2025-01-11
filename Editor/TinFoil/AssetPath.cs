using UnityEditor;
using UnityEngine;

namespace CleverCrow.Fluid.ElasticInventory.Editors {
    public class AssetPath {
        private static string _basePath;

        public static string BasePath {
            get {
                if (_basePath != null) return _basePath;

                if (AssetDatabase.IsValidFolder($"Assets/{UxmlConfig.PATH_ROOT}")) {
                    _basePath = "Assets/";
                    return _basePath;
                }

                if (AssetDatabase.IsValidFolder($"Packages/{UxmlConfig.PATH_ROOT}")) {
                    _basePath = "Packages/";
                    return _basePath;
                }

                Debug.LogError("Asset root could not be found");

                return null;
            }
        }
    }
}
