namespace CleverCrow.Fluid.ElasticInventory.Editors {
    // @TODO Long term this should be a globally accessible overridable file
    // that drives a micro-framework for UXML components
    public static class UxmlConfig {
        /// <summary>
        /// Where your UXML files are located after the Assets/ folder
        /// </summary>
        public static string PATH_ROOT = "com.fluid.elastic-inventory";

        /// <summary>
        /// Declare where editor code is stored in the package cache (for loading UXML files from a package)
        /// Value must at least be "/" to be valid
        /// </summary>
        public static string PACKAGE_CACHE_EDITOR_ROOT = "/Editor/";
    }
}
