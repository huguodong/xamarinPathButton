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

    public interface IDegreeProvider
    {
        float[] getDegrees(int count, float totalDegrees);
       
    }
}