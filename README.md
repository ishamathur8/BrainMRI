# 🧠 Brain MRI Visualization in Augmented Reality (AR)

This Unity-based mobile application transforms 2D brain MRI images into an interactive 3D visualization using Augmented Reality (AR). Built for Android, the app allows users to explore the human brain in a hands-on, immersive way—ideal for medical students, educators, and patient communication.

## 📱 Features

- 📸 **Image Tracking**: Detects MRI scans and overlays a 3D brain model on top.
- 🔁 **Rotation & Interaction**: Rotate the brain or highlight specific regions.
- 🔊 **Audio Feedback**: Listen to region-specific descriptions with a tap.
- 🎚️ **Transparency Slider**: Control visibility of non-selected brain areas.
- 🧠 **Exploded View**: Visual separation of brain regions for better understanding.
- 🎙️ **Voice Command Ready** *(Planned)*: Future support for voice interaction.
- ☁️ **Mobile-Optimized**: Built and tested for Android devices with ARCore support.

## 🛠 Tech Stack

- Unity (2022.3.x)
- AR Foundation
- Blender (for model optimization)
- 3D Slicer (for MRI segmentation)
- Android SDK, NDK, IL2CPP

## 📂 Folder Structure

```
├── Assets/              # Project assets (scripts, models, prefabs, UI)
├── Packages/            # Package dependencies
├── ProjectSettings/     # Unity project settings
├── .gitignore           # Standard Unity .gitignore
├── README.md            # Project documentation
```

> ❗**Note**: `Library/`, `obj/`, `Temp/`, and build files like `.apk` are excluded from this repository.

## 🚀 Getting Started

1. Clone the repo:
   ```bash
   git clone https://github.com/your-username/MRI-Brain-AR.git
   ```

2. Open the project in Unity Hub (2022.3.x or later).

3. Make sure Android Build Support, ARCore XR Plugin, and AR Foundation are installed via Package Manager.

4. Connect your Android phone and build the APK:
   - Go to `File > Build Settings`
   - Select Android > `Build & Run`

5. Point your phone camera at a reference MRI image to start the AR experience!

## 📸 Reference Dataset

This project uses the **MNI152 T1-weighted MRI atlas**, available at:  
https://www.bic.mni.mcgill.ca/ServicesAtlases/ICBM152NLin2009  
*(Fonov et al., 2009)*

## 📄 License

This project is for educational and academic demonstration purposes. For commercial use or redistribution, please contact the author.
