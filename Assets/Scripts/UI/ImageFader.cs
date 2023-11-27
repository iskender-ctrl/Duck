using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageFader : MonoBehaviour
{
   public float duration = 1;
   
   private Image _image;
   private void Awake()
   {
      _image = GetComponent<Image>();
      var initColor = Color.white;
      initColor.a = 0;
      GetComponent<CanvasRenderer>().SetColor(initColor);
   }

   public void FadeTo0() => _image.CrossFadeAlpha(0,duration,true);
   
   public void FadeTo1() => _image.CrossFadeAlpha(1,duration,true);
}
