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

    public class DefaultDegreeProvider : IDegreeProvider
    {
        public float[] getDegrees(int count, float totalDegrees)
        {
            if (count < 1)
            {
                return new float[] { };
            }

            float[] result = null;
            int tmpCount = 0;
            if (count < 4)
            {
                tmpCount = count + 1;
            }
            else
            {
                tmpCount = count - 1;
            }

            result = new float[count];
            float delta = totalDegrees / tmpCount;

            for (int index = 0; index < count; index++)
            {
                int tmpIndex = index;
                if (count < 4)
                {
                    tmpIndex = tmpIndex + 1;
                }
                result[index] = tmpIndex * delta;
            }

            return result;
        }
    }

}