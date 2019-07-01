# UnityPhotoBooth
Tools for making photo booth games with Unity. 📷

<p align="center">
<img src="https://github.com/GimChuang/UnityPhotoBooth/blob/master/readme_information/Sample_UnityPhotoBooth.gif" width="200"/>
</p>

This project is aimed at building a photo booth game with several tools I wrote. ✨ **For more information, please refer to those individual pages**:
- [UnityWebcamTool](https://github.com/GimChuang/UnityWebcamTool) - Simple scripts to control webcam.
- [UnityPhotoTool](https://github.com/GimChuang/UnityPhotoTool) - Scripts for taking screenshots with countdown animation and chroma keying.
- [UnityFileIOUtility](https://github.com/GimChuang/UnityFileIOUtility) - Scripts to process some basic file I/O (eg. save texture as .jpg).
- [UnityWebTool](https://github.com/GimChuang/UnityWebTool) - Scripts to upload pictures to your server and generate QR codes with the pictures' url.
 
There are 2 sample scenes in this project:
- **Test_PhotoBooth** simply opens webcam, applies the WebCamTexture to a "chroma-keyable" material (press D to open the panel of chroma key settings). Countdown and flash animation will be played when you click the "Take Photo" button. The photo will be saved to your desired path.

- **Test_PhotoBooth_Web** adds simple functionality of choosing background for chroma-keying, uploading photo, and getting QR codes. 
 
 
Notes
---
- This project uses Git submodules [gm_WebcamTool](https://github.com/GimChuang/gm_WebcamTool), [gm_PhotoTool](https://github.com/GimChuang/gm_PhotoTool),  [gm_FileIOUtility](https://github.com/GimChuang/gm_FileIOUtility), and [gm_WebTool](https://github.com/GimChuang/gm_WebTool). If you want to clone this reposiory, you need to call
```
git clone --recurse-submodules <URL>
``` 
- This project is developed with Unity 2018.2.8f1 on Windows 10. NOT tested on Mac OS X.
- Currently I don't have a green screen in my house, so keying function in the above gif is made with After Effects. It's just for demonstration. 😉


License
---
MIT


Acknowledgement
---
This project uses [ProcAmp by Keijiro](https://github.com/keijiro/ProcAmp) and [HSV Color Picker by judah4](https://github.com/judah4/HSV-Color-Picker-Unity) to achieve chroma keying. Also uses  [PlayerPrefsX by Eric Haines](http://wiki.unity3d.com/index.php/ArrayPrefs2#C.23_-_PlayerPrefsX.cs) to save colors with *PlayerPrefs*! 🙇‍♀️

