namespace CleverCrow.Fluid.ElasticInventory.Testing {
    public static class A {
        public static ItemDefinitionBuilder ItemDefinition () {
            return new ItemDefinitionBuilder();
        }

        public static ItemEntryReadOnlyBuilder ItemEntryReadOnly () {
            return new ItemEntryReadOnlyBuilder();
        }

        public static ItemEntryBuilder ItemEntry () {
            return new ItemEntryBuilder();
        }
    }
}
