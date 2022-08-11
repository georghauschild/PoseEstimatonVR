## Overview
This is an Unity project for human pose estimation on one VR player. The human pose estimation in only calculated via camera. There are no sensor data which form the body. Even the visible head movement is not controlled via VR headset tracker, but only from the AI model which gets the data from the camera. This may be subject of change, since this method cause major stuttering and imprecise line of sight.

https://user-images.githubusercontent.com/37111215/184219074-1d199728-1884-4545-a565-aac3aef45274.mp4

## Hardware requirements and software
- RGB camera (e.g. Smartphone Camera)
- VR Headset (e.g. Meta Quest 2)
- Unity version 2020.3.14f1
- Unity XR framework for standardized VR commands.

## Installation
Copy the project on your PC.
Follow the first 3 installment steps from https://github.com/digital-standard/ThreeDPoseUnityBarracuda
Download the Resnet34 onx model here:
https://digital-standard.com/threedpose/models/Resnet34_3inputs_448x448_20200609.onnx

## Restrictions and problems
- Body deformities and stuttering may occur
- Walking is (yet) not possible without desync between VR headset and body estimation
- no multi person tracking


## Credits
This project is build on top of https://github.com/digital-standard/ThreeDPoseUnityBarracuda by Digital- Standard Co., Ltd.
