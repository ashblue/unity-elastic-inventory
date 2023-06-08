# Unity Elastic Inventory

A Unity 3D inventory system that's designed as a micro-framework to drop in and go. Written with extendability, serialization, large scale projects, and to work with a wide range of game types. Use this library to stop constantly tweaking your inventory code and focus on making your game instead.

## Installation

Elastic Inventory is used through [Unity's Package Manager](https://docs.unity3d.com/Manual/CustomPackages.html). In order to use it you'll need to add the following lines to your `Packages/manifest.json` file. After that you'll be able to visually control what specific version of Elastic Inventory you're using from the package manager window in Unity. This has to be done so your Unity editor can connect to NPM's package registry.

```json
{
  "scopedRegistries": [
    {
      "name": "NPM",
      "url": "https://registry.npmjs.org",
      "scopes": [
        "com.fluid"
      ]
    }
  ],
  "dependencies": {
    "com.fluid.elastic-inventory": "1.0.0"
  }
}
```

## Documentation

### Custom Item Definitions

Custom item definitions can be created to add your own unique serialized fields. For example you might want a weapon with damage and range. You can do so with the following snippet.

```C#
// Sample custom item goes here
```

#### How to modify custom items at runtime

While custom inventory definitions do allow you to add your own fields. You cannot modify them at runtime. If you do so changes will be lost on save and load. You can easily get around this by referencing the item definition and modifying it with a wrapper.

For example let's say you have a weapon with modifiable slots for accessories. Instead of saving the changes to the entry or definition. You'll want to add the modifications onto your character that has the weapon. This way you can save/load the character and the weapon will still have the expected modifications.

```C#
public class WeaponInstance {
    public WeaponDefinition definition;
    public WeaponAccessoryDefinition slotA;
    public WeaponAccessoryDefinition slotB;
    public WeaponAccessoryDefinition slotC;
    
    public string Save () {
        // your logic here
    }
    
    public void Load (string data) {
        // your logic here
    }
}
```

The bottom line here is item definitions are runtime static. If you want to modify them you should write a wrapper around definition / entry instead of modifying it directly.

```C#

## Releases

Archives of specific versions and release notes are available on the [releases page](https://github.com/ashblue/unity-elastic-inventory/releases).

## Nightly Builds

To access nightly builds of the `develop` branch that are package manager friendly, you'll need to manually edit your `Packages/manifest.json` as so. 

```json
{
    "dependencies": {
      "com.fluid.elastic-inventory": "https://github.com/ashblue/unity-elastic-inventory.git#nightly"
    }
}
```

Note that to get a newer nightly build you must delete this line and any related lock data in the manifest, let Unity rebuild, then add it back. As Unity locks the commit hash for Git urls as packages.

## Development Environment

If you wish to run the development environment you'll need to install the [Node.js](https://nodejs.org/en/) version in the [.nvmrc](.nvmrc) file. The easiest way to do this is install [NVM](https://github.com/nvm-sh/nvm) and run `nvm use`. 

Once you've installed Node.js, run the following from the root once.

`npm install`

If you wish to create a build run `npm run build` from the root and it will populate the `dist` folder.

### Making Commits

All commits should be made using [Commitizen](https://github.com/commitizen/cz-cli) (which is automatically installed when running `npm install`). Commits are automatically compiled to version numbers on release so this is very important. PRs that don't have Commitizen based commits will be rejected.

To make a commit type the following into a terminal from the root.

```bash
npm run commit
```

### How To Contribute

Please see the [CONTRIBUTIONS.md](CONTRIBUTING.md) file for full details on how to contribute to this project.

---

This project was generated with [Oyster Package Generator](https://github.com/ashblue/oyster-package-generator).
