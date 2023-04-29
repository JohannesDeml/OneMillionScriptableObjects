# One Million Scriptable Objects

![Preview](./preview.png)

[![Unity 2021.3](https://img.shields.io/badge/unity-2021.3-green.svg?logo=unity&cacheSeconds=2592000)](https://unity3d.com/get-unity/download/archive)

*Test setup for Unity project with thousands of tiny scriptable objects*  

* [Unity Forum Thread](https://forum.unity.com/threads/exclude-files-from-uploading-to-accelerator.1414479/)

This repo contains a script with which you can easily batch create scriptable objects. The main branch does not contain the created objects, but just the setup. You can find the created scriptable objects on the [release pages](https://github.com/JohannesDeml/OneMillionScriptableObjects/releases) as .zip files.

## Findings

### Accelerator Memory impact

When creating 10,000 ScriptableObjects, the accelerator will increase the memory consumption of the Unity process (not of the accelerator process) by a lot:

- Without Accelerator after saving all ScriptableObjects: **1,306 MB** 
- With Accelerator after saving all ScriptableObjects: **2,895 MB**

See also [Accelerator memory impact recording on Youtube](https://youtu.be/WyYpW5fwt3g)

## License

* MIT (c) Johannes Deml - see [LICENSE](./LICENSE)