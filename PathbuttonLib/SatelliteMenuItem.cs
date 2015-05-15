using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Views.Animations;
using Android.Graphics.Drawables;

namespace PathbuttonLib
{
    public class SatelliteMenuItem
    {
        private int id;
        private int imgResourceId;
        private Drawable imgDrawable;
        private ImageView view;
        private ImageView cloneView;
        private Animation outAnimation;
        private Animation inAnimation;
        private Animation clickAnimation;
        private int finalX;
        private int finalY;

        public SatelliteMenuItem(int id, int imgResourceId)
        {
            this.imgResourceId = imgResourceId;
            this.id = id;
        }

        public SatelliteMenuItem(int id, Drawable imgDrawable)
        {
            this.imgDrawable = imgDrawable;
            this.id = id;
        }

        public int getId()
        {
            return id;
        }

        public void setId(int id)
        {
            this.id = id;
        }

        public int getImgResourceId()
        {
            return imgResourceId;
        }

        public void setImgResourceId(int imgResourceId)
        {
            this.imgResourceId = imgResourceId;
        }

        public Drawable getImgDrawable()
        {
            return imgDrawable;
        }

        public void setImgDrawable(Drawable imgDrawable)
        {
            this.imgDrawable = imgDrawable;
        }

        public void setView(ImageView view)
        {
            this.view = view;
        }

        public ImageView getView()
        {
            return view;
        }

        public void setInAnimation(Animation inAnimation)
        {
            this.inAnimation = inAnimation;
        }

        public Animation getInAnimation()
        {
            return inAnimation;
        }

        public void setOutAnimation(Animation outAnimation)
        {
            this.outAnimation = outAnimation;
        }

        public Animation getOutAnimation()
        {
            return outAnimation;
        }

      public  void setFinalX(int finalX)
        {
            this.finalX = finalX;
        }

      public  void setFinalY(int finalY)
        {
            this.finalY = finalY;
        }

        int getFinalX()
        {
            return finalX;
        }

        int getFinalY()
        {
            return finalY;
        }

        public void setCloneView(ImageView cloneView)
        {
            this.cloneView = cloneView;
        }

       public ImageView getCloneView()
        {
            return cloneView;
        }

        public void setClickAnimation(Animation clickAnim)
        {
            this.clickAnimation = clickAnim;
        }

      public  Animation getClickAnimation()
        {
            return clickAnimation;
        }
    }
}