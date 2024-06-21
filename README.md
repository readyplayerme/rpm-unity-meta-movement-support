# Ready Player Me Meta Movement Support

This package adds support for Meta Movement tracking, including fullbody retargeting and facetracking.
Please visit the online documentation and join our public `forums` community.

![](https://i.imgur.com/zGamwPM.png) **[Online Documentation]( https://docs.readyplayer.me/ready-player-me/integration-guides/unity )**

![](https://github.com/readyplayerme/rpm-unity-sdk-webview/assets/25016626/130b50db-d6af-4277-9da3-03172bc085eb) **[Forums](https://forum.readyplayer.me/)**

:octocat: **[GitHub Discussions]( https://github.com/readyplayerme/rpm-unity-sdk-core/discussions )**

## Requirements
- Unity Version 2021.3 or higher
- [Ready Player Me Core](https://github.com/readyplayerme/rpm-unity-sdk-core) - v6.3.2+
- [glTFast](https://github.com/atteneder/glTFast) - v5.0.0+ (included as dependency in Ready Player Me Core)
- [Meta Movement](https://github.com/oculus-samples/Unity-Movement.git#dev) v5.1.0+
- Before testing in VR you should Fix any issues diagnosed by the Meta Project Setup Tool by clicking on `Edit` -> ` > Project Settings` -> `Meta XR`

## Installation
- Copy the following GitHub URL: 
```cs 
https://github.com/readyplayerme/rpm-unity-meta-movement-support.git
```
- Open Unity and go to `Window` -> `Package Manager` -> `Add package from git URL`
- Paste the URL and click `Add`

## Importing the Sample
1. Import the sample by going to `Window` - > `Package Manager` -> `Ready Player Me Meta Movement Support` -> `Samples` -> `Import`
2. Open the scene `Assets/Samples/MetaMovement/RetargetingSample/Scenes/RetargetingSample.unity`

### Running the Sample in the Editor
1. Open the scene `Assets/Samples/MetaMovement/RetargetingSample/Scenes/RetargetingSample.unity`
2. Connect your Quest device via Quest Link
3. Click on the `Play` button in the Unity Editor

### Running the Sample on device
1. Open the scene `Assets/Samples/MetaMovement/RetargetingSample/Scenes/RetargetingSample.unity`
2. Add the `RetargetingSample.unity` scene to the build settings by going to `File` -> `Build Settings` -> `Add Open Scenes`
3. Set the build target to Android
4. Connect your Quest device via USB
5. Click on the `Build and Run` button in the Unity Editor

## Loading an RPM avatar in a custom OVR ready scene
1. With your scene open, Drag and drop the `MetaMovementAvatar` prefab from `Assets/Ready Player Me/MetaMovement/Prefabs` into your scene
2. Add an avatar loading component to the GameObject, for example the `BasicAvatarLoader` from the sample
3. Set the `Avatar URL` field in the `BasicAvatarLoader` component to the URL of the avatar you want to load
4. Click on the `Play` button in the Unity Editor
5. After a small delay, your avatar mesh should update

## Creating your own Avatar Prefab
### Step 1: Set correct Avatar Config
Before loading an avatar you need to set the correct avatar config. 
To do this follow these steps:
1. Open the Ready Player Me Settings window  `Tools` -> `Ready Player Me` -> `Settings` 
2. Set the `Avatar Config` field to `Meta Avatar Config`
3. This will ensure that the avatar is created with the correct settings for Meta Movement and facetracking

### Step 2: Loading an Avatar in Editor
1. Create a Ready Player Me avatar from an XR enabled subdomain, for example `https://dev-sdk-xr.readyplayer.me/avatar`
2. After avatar creation is complete you will get a URL to a .glb, copy that URL.
3. In Unity open the Avatar loader window by going to `Tools` -> `Ready Player Me` -> `Avatar Loader`
4. Paste the URL into the `Avatar URL` field and click `Load Avatar`
5. After a small delay, the avatar should load into the scene as a new GameObject

### Step 3: Meta Movement Set up
1. Select the avatar GameObject and right click to display the context menu
2. In the context menu, select `Movement Samples` -> `Body tracking` -> `Animation Retargeting (Fullbody)`
3. This should automatically set up the avatar with correct settings and components including:
   - Adding a Retargeting Layer component
   - OVR Body component
   - Rig builder component
   - It will create a Rig as a child of the root object
   - Adds contraints including Full Body Deformation Constraint and a Retargeting constraint

### Step 4: Twist bones set up
1. Add a HierarchyTwist component onto the avatar GameObject 
2. Assign the bones and values as shown in the image below

### Step 5: Avatar Face Tracking set up
1. Select the avatar GameObject and right click to display the context menu
2. In the context menu, select `Ready Player Me` -> `Meta Movement` -> `Setup FaceTracking`
3. This should automatically set up the avatar with correct settings and components including:
   - A OVRFaceExpressions component added to the root object
   - ARKitFace component added to each child mesh with Blendshapes