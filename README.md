# Ready Player Me Meta Movement Support

This package adds support for Meta Movement tracking, including full-body retargeting and face-tracking.
Please visit the online documentation and join our public `forums` community.

![](https://i.imgur.com/zGamwPM.png) **[Online Documentation]( https://docs.readyplayer.me/ready-player-me/integration-guides/unity )**

![](https://github.com/readyplayerme/rpm-unity-sdk-webview/assets/25016626/130b50db-d6af-4277-9da3-03172bc085eb) **[Forums](https://forum.readyplayer.me/)**

:octocat: **[GitHub Discussions]( https://github.com/readyplayerme/rpm-unity-sdk-core/discussions )**

## Requirements
- Unity Version 2021.3 or higher
- [Ready Player Me Core](https://github.com/readyplayerme/rpm-unity-sdk-core.git) - v7.1.0+
- [glTFast](https://github.com/atteneder/glTFast.git) - v6.0.0+ (included as a dependency in Ready Player Me Core)
- [Meta Movement](https://github.com/oculus-samples/Unity-Movement.git) v71.0.0+
- Before testing in VR you should Fix any issues diagnosed by the Meta Project Setup Tool by clicking on `Edit` -> ` > Project Settings` -> `Meta XR`

## Package Installation
***This assumes that your projects already meet all the requirements including the installation of all the plugins listed above***
- Copy the following GitHub URL:
  
```cs 
https://github.com/readyplayerme/rpm-unity-meta-movement-support.git
```
- Open Unity and go to `Window -> Package Manager -> Add package from git URL`

  ![image-package-manager-0](https://github.com/user-attachments/assets/46ff8c13-a203-419d-bb7e-55593caeb9d2)

- Paste the URL and click `Add`

![image-package-manager-1](https://github.com/user-attachments/assets/b283657d-99a1-4ce8-b5a5-df35ea0edf9b)


## Importing the Sample
1. Import the sample by going to `Window -> Package Manager -> Ready Player Me Meta Movement Support -> Samples -> Import`

![image-package-manager](https://github.com/user-attachments/assets/6fa27edc-b071-46be-b12c-07f4c4ca86bc)

### Running the Samples in the Editor
1. Open the scene `Assets/Samples/Ready Player Me Meta Movement Support/VERSION_NUMBER/LoaderSamples/Scenes/DynamicLoader` or `Assets/Samples/Ready Player Me Meta Movement Support/VERSION_NUMBER/LoaderSamples/Scenes/PrefabLoader`
2. Connect your Quest device via Quest Link
3. Click on the `Play` button in the Unity Editor

  ![image](https://github.com/user-attachments/assets/47d4d0ba-d8f1-457b-bae3-34b738184f5d)


### Running the Samples on the device
1. Open the scene `Assets/Samples/Ready Player Me Meta Movement Support/VERSION_NUMBER/LoaderSamples/Scenes/DynamicLoader` or `Assets/Samples/Ready Player Me Meta Movement Support/VERSION_NUMBER/LoaderSamples/Scenes/PrefabLoader`
2. Add the sample scene to the build settings by going to `File -> Build Settings -> Add Open Scenes`
3. Set the build target to Android
4. Connect your Quest device via USB
5. Click on the `Build and Run` button in the Unity Editor

## Loading an RPM avatar in a custom OVR-ready scene
_This assumes that the scene is already set up with the necessary components for OVR tracking and Meta Movement_

### Dynamic Avatar Loading
1. With your scene open, drag and drop the `DynamicAvatarLoader` prefab from `Packages/Ready Player Me Meta Movement Support/Runtime/Prefabs` into your scene
3. Select the `DynamicAvatarLoader` object in the scene, then in the inspector, set the `Avatar URL` field in the `LoadUrlOnStart` component to the URL of the avatar you want to load

   ![image-inspector](https://github.com/user-attachments/assets/b81a6dd8-bfd3-41eb-ac5e-afbfe404e361)


5. Click on the `Play` button in the Unity Editor
6. After a small delay, your avatar will load into the scene

### Loading with AvatarPrefabLoader 
1. With your scene open, Drag and drop the `AvatarPrefabLoader` prefab from `Packages/Ready Player Me Meta Movement Support/Runtime/Prefabs` into your scene
2. Select the `AvatarPrefabLoader` object in the scene, then in the inspector, set the `Avatar URL` field in the `LoadUrlOnStart` component to the URL of the avatar you want to load

![image-inspector-2](https://github.com/user-attachments/assets/75402fea-dbcf-42de-afe2-62c35a7dc4fe)

3. Click on the `Play` button in the Unity Editor
4. After a small delay, your avatar will load into the scene

## Creating your own Avatar Prefab
### Step 1: Set the correct Avatar Config
Before loading an avatar you need to set the correct avatar config. 
To do this follow these steps:
1. Open the Ready Player Me Settings window  `Tools -> Ready Player Me -> Settings` 

![image-toolbar](https://github.com/user-attachments/assets/b660a227-6117-4ec7-b268-522ebf2fec0e)

2. Set the `Avatar Config` field to `Meta Avatar Config`

![image-settings](https://github.com/user-attachments/assets/a6696b77-33fd-4c34-857e-e3cd457e56ea)

3. This will ensure that the avatar is created with the correct settings for Meta Movement and face-tracking

### Step 2: Loading an Avatar in Editor
1. Create a Ready Player Me avatar from an XR-enabled subdomain, for example, `https://dev-sdk-xr.readyplayer.me/avatar`
2. After avatar creation is complete you will get a URL to a .glb, copy that URL.
3. In Unity open the Avatar loader window by going to `Tools -> Ready Player Me -> Avatar Loader`

![image-loader](https://github.com/user-attachments/assets/f792901e-0ce3-4722-bbc1-a2d9e1150d16)

4. Paste the URL into the `Avatar URL` field and click `Load Avatar`
5. After a small delay, the avatar should load into the scene as a new GameObject

### Step 3: Avatar Face Tracking set up & twist bones
1. Select the avatar GameObject and right-click to display the context menu
2. In the context menu, select `Ready Player Me -> Meta Movement -> Run Avatar Setup`

![image-inspector-2](https://github.com/user-attachments/assets/c8934d0a-5784-456a-8bb3-e147345bdec7)

3. This should automatically set up the avatar with the correct settings and components including:
   - Adding a Retargeting Layer component
   - OVR Body component
   - Rig builder component
   - It will create and set up a Rig as a child of the root object
   - Adds constraints including `Full Body Deformation Constraint` and a `Retargeting constraint`
   - A `OVRFaceExpressions` component added to the root object
   - `ARKitFace` component added to each child mesh with blendshapes
   - A `TwistHierarchy` component added to the root object with the twist bones set up already

### Step 4: Saving as a prefab
With the configured avatar in the scene, there are still a few steps required to save it as a re-usable prefab.
1. Right-click on the avatar GameObject in the hierarchy and select 'Prefab -> Select Asset'. _This will show the avatar source files including a .glb and a .prefab file_
2. Move the files from this folder somewhere in your Assets folder (or anywhere outside of `Assets/Ready Player Me/Avatars` folder)
3. After moving these files you can now simply select the avatar object still in the scene and drag it anywhere into the project view to create a prefab

After creating the prefab you can now add it into any scene by dragging and dropping the prefab into the scene hierarchy.

