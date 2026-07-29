using System;
using System.Collections;
using System.Collections.Generic;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.InteropTypes.Fields;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VentLib.Utilities.Attributes;

namespace AchievementsAPI
{
    [RegisterInIl2Cpp]
    public class AchievementsMenuItem(IntPtr ptr) : MonoBehaviour(ptr)
    {
        public Il2CppReferenceField<TextMeshProUGUI> nameText;
        public Il2CppReferenceField<TextMeshProUGUI> descriptionText;
        public Il2CppReferenceField<Image> iconImage;
        public Il2CppReferenceField<Image> grayscaleImage;
    }
}
