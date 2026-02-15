using UnityEngine;
public interface ISlidables
{
    public void OnSlideSurfaceEnter(SlideSurface sliderSurface);

    public void OnSlideSurfaceExit(SlideSurface sliderSurface);
}