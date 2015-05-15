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
using Java.Lang;

namespace PathbuttonLib
{
   public class ArrayDegreeProvider : IDegreeProvider {
	private float[] degrees;
	
	public ArrayDegreeProvider(float[] degrees) {
		this.degrees = degrees;
	}
	
	public float[] getDegrees(int count, float totalDegrees){
		if(degrees == null || degrees.Length != count){

            throw new IllegalArgumentException("Provided delta degrees and the action count are not the same.");
        }
		return degrees; 
	}
}

}