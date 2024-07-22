# Ready Player Me Meta Movement Support

This package adds support for Meta Movement tracking, including full-body retargeting and face-tracking.
Please visit the online documentation and join our public `forums` community.

![](https://i.imgur.com/zGamwPM.png) **[Online Documentation]( https://docs.readyplayer.me/ready-player-me/integration-guides/unity )**

![](https://github.com/readyplayerme/rpm-unity-sdk-webview/assets/25016626/130b50db-d6af-4277-9da3-03172bc085eb) **[Forums](https://forum.readyplayer.me/)**

:octocat: **[GitHub Discussions]( https://github.com/readyplayerme/rpm-unity-sdk-core/discussions )**

## Requirements
- Unity Version 2021.3 or higher
- [Ready Player Me Core](https://github.com/readyplayerme/rpm-unity-sdk-core.git) - v6.3.2+
- [glTFast](https://github.com/atteneder/glTFast.git) - v5.0.0+ (included as a dependency in Ready Player Me Core)
- [Meta Movement](https://github.com/oculus-samples/Unity-Movement.git) v5.1.0+
- Before testing in VR you should Fix any issues diagnosed by the Meta Project Setup Tool by clicking on `Edit` -> ` > Project Settings` -> `Meta XR`

## Package Installation
***This assumes that your projects already meet all the requirements including the installation of all the plugins listed above***
- Copy the following GitHub URL:
  
```cs 
https://github.com/readyplayerme/rpm-unity-meta-movement-support.git
```
- Open Unity and go to `Window -> Package Manager -> Add package from git URL`
  
  ![Screenshot 2024-07-22 081415](https://github.com/user-attachments/assets/406677e8-98af-41f2-a9ad-4385032f6bae)

- Paste the URL and click `Add`
  
![Screenshot 2024-07-22 081437](https://github.com/user-attachments/assets/d725e8d0-1d23-49fa-a590-4ac84b878bf9)

## Importing the Sample
1. Import the sample by going to `Window -> Package Manager -> Ready Player Me Meta Movement Support -> Samples -> Import`
   
![Screenshot 2024-07-22 082241](https://github.com/user-attachments/assets/96e8b2fd-12f3-4b99-95b6-3548845593cd)

2. Open the scene `Assets/Samples/MetaMovement/RetargetingSample/Scenes/RetargetingSample.unity`
   
![Screenshot 2024-07-22 082310](https://github.com/user-attachments/assets/2368812f-1066-45c2-96dc-e95b1d2b7b1f)

### Running the Samples in the Editor
1. Open the scene `Assets/Samples/MetaMovement/LoaderSamples/Scenes/DynamicLoader` or `Assets/Samples/MetaMovement/LoaderSamples/Scenes/PrefabLoader`
2. Connect your Quest device via Quest Link
3. Click on the `Play` button in the Unity Editor

### Running the Samples on the device
1. Open the scene `Assets/Samples/MetaMovement/LoaderSamples/Scenes/DynamicLoader` or `Assets/Samples/MetaMovement/LoaderSamples/Scenes/PrefabLoader`
2. Add the sample scene to the build settings by going to `File -> Build Settings -> Add Open Scenes`
3. Set the build target to Android
4. Connect your Quest device via USB
5. Click on the `Build and Run` button in the Unity Editor

## Loading an RPM avatar in a custom OVR-ready scene
_This assumes that the scene is already set up with the necessary components for OVR tracking and Meta Movement_

### Dynamic Avatar Loading
1. With your scene open, drag and drop the `DynamicAvatarLoader` prefab from `Assets/Ready Player Me/MetaMovement/Prefabs` into your scene
3. Select the `DynamicAvatarLoader` object in the scene, then in the inspector, set the `Avatar URL` field in the `LoadUrlOnStart` component to the URL of the avatar you want to load

   ![Screenshot 2024-07-22 105848](https://github.com/user-attachments/assets/9cb15bd7-d37c-49d0-97ef-4b29150fef16)

4. Click on the `Play` button in the Unity Editor
5. After a small delay, your avatar will load into the scene

### Loading with AvatarPrefabLoader 
1. With your scene open, Drag and drop the `AvatarPrefabLoader` prefab from `Assets/Ready Player Me/MetaMovement/Prefabs` into your scene
2. Select the `AvatarPrefabLoader` object in the scene, then in the inspector, set the `Avatar URL` field in the `LoadUrlOnStart` component to the URL of the avatar you want to load

![Screenshot 2024-07-22 105856](https://github.com/user-attachments/assets/8ce1be70-020a-4908-8cb2-24a6c2cb8b48)

3. Click on the `Play` button in the Unity Editor
4. After a small delay, your avatar will load into the scene

## Creating your own Avatar Prefab
### Step 1: Set the correct Avatar Config
Before loading an avatar you need to set the correct avatar config. 
To do this follow these steps:
1. Open the Ready Player Me Settings window  `Tools -> Ready Player Me -> Settings` 

![Screenshot 2024-07-22 083005](https://github.com/user-attachments/assets/1dd8baec-ff3b-4566-97a0-eba2d93ab668)

2. Set the `Avatar Config` field to `Meta Avatar Config`

![Screenshot 2024-07-22 083024](https://github.com/user-attachments/assets/5a5efbbe-177f-42a8-8c8a-54df0fe1e985)

3. This will ensure that the avatar is created with the correct settings for Meta Movement and face-tracking

### Step 2: Loading an Avatar in Editor
1. Create a Ready Player Me avatar from an XR-enabled subdomain, for example, `https://dev-sdk-xr.readyplayer.me/avatar`
2. After avatar creation is complete you will get a URL to a .glb, copy that URL.
3. In Unity open the Avatar loader window by going to `Tools -> Ready Player Me -> Avatar Loader`

![Screenshot 2024-07-22 083154](https://github.com/user-attachments/assets/31beabd4-2e32-4ae7-88cb-ed54a3c5b14a)

4. Paste the URL into the `Avatar URL` field and click `Load Avatar`
5. After a small delay, the avatar should load into the scene as a new GameObject

### Step 3: Avatar Face Tracking set up & twist bones
1. Select the avatar GameObject and right-click to display the context menu
2. In the context menu, select `Ready Player Me -> Meta Movement -> Run Avatar Setup`

![Screenshot 2024-07-22 083334](https://github.com/user-attachments/assets/b982caa4-8bb4-49cc-a5f7-6191cf0d9807)

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

