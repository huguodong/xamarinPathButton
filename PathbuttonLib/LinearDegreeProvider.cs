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

namespace PathbuttonLib
{
    public class LinearDegreeProvider : IDegreeProvider
    {
        public float[] getDegrees(int count, float totalDegrees)
        {
            if (count < 1)
            {
                return new float[] { };
            }

            if (count == 1)
            {
                return new float[] { 45 };
            }

            float[] result = null;
            int tmpCount = count - 1;

            result = new float[count];
            float delta = totalDegrees / tmpCount;

            for (int index = 0; index < count; index++)
            {
                int tmpIndex = index;
                result[index] = tmpIndex * delta;
            }

            return result;
        }
    }

}