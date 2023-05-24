using System;
using UnityEditor;
using UnityEngine.UIElements;

namespace CleverCrow.Fluid.ElasticInventory.Editors {
    public abstract class ComponentBase {
        private readonly string _path;
        protected readonly VisualElement _container;

        protected ComponentBase (VisualElement container) {
            _container = container;

            if (_path == null) {
                var stackTrace = new System.Diagnostics.StackTrace(true);
                var frame = stackTrace.GetFrame(1);
                var path = frame.GetFileName()?.Replace("\\", "/");

                if (path.Contains("/PackageCache/")) {
                    var parts = path.Split(new[] { UxmlConfig.PACKAGE_CACHE_EDITOR_ROOT }, StringSplitOptions.None);
                    _path = $"{AssetPath.BasePath}{UxmlConfig.PATH_ROOT}{UxmlConfig.PACKAGE_CACHE_EDITOR_ROOT}{parts[1]}"
                        .Replace(".cs", ".uxml");
                } else {
                    var strings = path.Split(new[] { "/Assets/" }, StringSplitOptions.None);
                    _path = $"{AssetPath.BasePath}/{strings[1]}"
                        .Replace(".cs", ".uxml");
                }
            }

            var markup = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(_path);
            markup.CloneTree(container);
        }
    }
}
