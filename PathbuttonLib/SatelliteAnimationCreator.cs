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



namespace PathbuttonLib
{

    public class SatelliteAnimationCreator
    {

        public static Animation createItemInAnimation(Context context, int index, long expandDuration, int x, int y)
        {
            RotateAnimation rotate = new RotateAnimation(720, 0,
                    Dimension.RelativeToSelf, 0.5f,
                     Dimension.RelativeToSelf, 0.5f);

            rotate.SetInterpolator(context,Resource.Animation.sat_item_in_rotate_interpolator);
            rotate.Duration=expandDuration;

            TranslateAnimation translate = new TranslateAnimation(x, 0, y, 0);


            long delay = 250;
            if (expandDuration <= 250)
            {
                delay = expandDuration / 3;
            }

            long duration = 400;
            if ((expandDuration - delay) > duration)
            {
                duration = expandDuration - delay;
            }

            translate.Duration=duration;
            translate.StartOffset=delay;

            translate.SetInterpolator(context,Resource.Animation.sat_item_anticipate_interpolator );

            AlphaAnimation alphaAnimation = new AlphaAnimation(1f, 0f);
            long alphaDuration = 10;
            if (expandDuration < 10)
            {
                alphaDuration = expandDuration / 10;
            }
            alphaAnimation.Duration=alphaDuration;
            alphaAnimation.StartOffset=(delay + duration) - alphaDuration;

            AnimationSet animationSet = new AnimationSet(false);
            animationSet.FillAfter=false;
            animationSet.FillBefore=true;
            animationSet.FillEnabled=true;

            animationSet.AddAnimation(alphaAnimation);
            animationSet.AddAnimation(rotate);
            animationSet.AddAnimation(translate);


            animationSet.StartOffset=30 * index;
            animationSet.Start();
            animationSet.StartNow();
            return animationSet;
        }

        public static Animation createItemOutAnimation(Context context, int index, long expandDuration, int x, int y)
        {

            AlphaAnimation alphaAnimation = new AlphaAnimation(0f, 1f);
            long alphaDuration = 60;
            if (expandDuration < 60)
            {
                alphaDuration = expandDuration / 4;
            }
            alphaAnimation.Duration=alphaDuration;
            alphaAnimation.StartOffset=0;


            TranslateAnimation translate = new TranslateAnimation(0, x, 0, y);

            translate.StartOffset=0;
            translate.Duration = expandDuration;
            translate.SetInterpolator(context,Resource.Animation.sat_item_overshoot_interpolator);

            RotateAnimation rotate = new RotateAnimation(0f, 360f,
                    Dimension.RelativeToSelf, 0.5f,
                     Dimension.RelativeToSelf, 0.5f);

            rotate.SetInterpolator(context,Resource.Animation.sat_item_out_rotate_interpolator);

            long duration = 100;
            if (expandDuration <= 150)
            {
                duration = expandDuration / 3;
            }

            rotate.Duration=expandDuration - duration;
            rotate.StartOffset=duration;

            AnimationSet animationSet = new AnimationSet(false);
            animationSet.FillAfter=false;
            animationSet.FillBefore=true;
            animationSet.FillEnabled=true;

            //animationSet.addAnimation(alphaAnimation);
            //animationSet.addAnimation(rotate);
            animationSet.AddAnimation(translate);

            animationSet.StartOffset=30 * index;

            return animationSet;
        }

        public static Animation createMainButtonAnimation(Context context)
        {
            return AnimationUtils.LoadAnimation(context,Resource.Animation.sat_main_rotate_left);
        }

        public static Animation createMainButtonInverseAnimation(Context context)
        {
            return AnimationUtils.LoadAnimation(context, Resource.Animation.sat_main_rotate_right);
        }

        public static Animation createItemClickAnimation(Context context)
        {
            return AnimationUtils.LoadAnimation(context, Resource.Animation.sat_item_anim_click);
        }


        public static int getTranslateX(float degree, int distance)
        {
            return Java.Lang.Double.ValueOf(distance * Math.Cos(Java.Lang.Math.ToRadians(degree))).IntValue();
        }

        public static int getTranslateY(float degree, int distance)
        {
            return Java.Lang.Double.ValueOf(-1 * distance * Math.Sin(Java.Lang.Math.ToRadians(degree))).IntValue();
        }

    }

}