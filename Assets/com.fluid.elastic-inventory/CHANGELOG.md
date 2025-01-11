# [2.0.0](https://github.com/ashblue/unity-elastic-inventory/compare/v1.0.1...v2.0.0) (2025-01-11)


### Bug Fixes

* **id repair:** null and empty string IDs are now repaired ([489ec64](https://github.com/ashblue/unity-elastic-inventory/commit/489ec648bfbe6bb109738b9abd620ad58a9b8be6))
* **inventory add:** no longer crashes when read only items are added ([afb6b44](https://github.com/ashblue/unity-elastic-inventory/commit/afb6b44f3e1f5af02eb58440fd4e50fb7357c9ea)), closes [#17](https://github.com/ashblue/unity-elastic-inventory/issues/17)
* **inventory.getall<t>():** no longer crashes when used with a custom item entry type ([c8678b0](https://github.com/ashblue/unity-elastic-inventory/commit/c8678b0ffa113957515ffc1402d11f1407d6fab1)), closes [#20](https://github.com/ashblue/unity-elastic-inventory/issues/20)
* **semantic-release:** was not picking up the main branch ([7124ae0](https://github.com/ashblue/unity-elastic-inventory/commit/7124ae0720b961b4e1e08701eacae1989eeb66e6))


### Features

* **inventory window:** categories now live display in the item table ([53193c4](https://github.com/ashblue/unity-elastic-inventory/commit/53193c4da7076173d63f93fa9d381882dbf9d0d2)), closes [#21](https://github.com/ashblue/unity-elastic-inventory/issues/21)
* **item definitions:** display name must now be implemented on all ItemDefinitionBase classes ([56e69c2](https://github.com/ashblue/unity-elastic-inventory/commit/56e69c2b0a473e2d4459472a12669a82793b4342))


### BREAKING CHANGES

* **item definitions:** Find all classes that inherit ItemDefinitionBase and add `[SerializeField] string
_displayName; public override string DisplayName => _displayName;`. To fix the error that your
classes do not implement  the display name get accessor.

## [1.0.1](https://github.com/ashblue/unity-elastic-inventory/compare/v1.0.0...v1.0.1) (2023-07-05)


### Bug Fixes

* **packages:** added in a missing package ([c8b5cf9](https://github.com/ashblue/unity-elastic-inventory/commit/c8b5cf93337ddfa3efa926f691464f479ad88d5a))
