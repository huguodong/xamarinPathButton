
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Views.Animations;
using Java.Util.Concurrent.Atomic;
using Android.Content.Res;
using System.Collections;
using System.Collections.Generic;
using System;
using Android.Graphics.Drawables;
using Android.OS;


namespace PathbuttonLib
{

    public class SatelliteMenu : FrameLayout, Animation.IAnimationListener
    {

        private static int DEFAULT_SATELLITE_DISTANCE = 500;
        private static float DEFAULT_TOTAL_SPACING_DEGREES = 90f;
        private static Boolean DEFAULT_CLOSE_ON_CLICK = true;
        private static int DEFAULT_EXPAND_DURATION = 500;

        private Animation mainRotateRight;
        private Animation mainRotateLeft;

        private ImageView imgMain;
        private SateliteClickedListener itemClickedListener;
        private List<SatelliteMenuItem> menuItems = new List<SatelliteMenuItem>();
        private InternalSatelliteOnClickListener internalItemClickListener;
        private Dictionary<View, SatelliteMenuItem> viewToItemDic = new Dictionary<View, SatelliteMenuItem>();
        private AtomicBoolean plusAnimationActive = new AtomicBoolean(false);

        // ?? how to save/restore?
        private IDegreeProvider gapDegreesProvider = new DefaultDegreeProvider();

        //States of these variables are saved
        private Boolean rotated = false;
        private int measureDiff = 0;
        //States of these variables are saved - Also configured from XML 
        private float totalSpacingDegree = DEFAULT_TOTAL_SPACING_DEGREES;
        private int satelliteDistance = DEFAULT_SATELLITE_DISTANCE;
        private int expandDuration = DEFAULT_EXPAND_DURATION;
        private Boolean closeItemsOnClick = DEFAULT_CLOSE_ON_CLICK;


        public SatelliteMenu(Context context)
            : this(context, null, 0)
        {

            init(context, null, 0);
        }

        public SatelliteMenu(Context context, Android.Util.IAttributeSet attrs)
            : this(context, attrs, 0)
        {

            init(context, attrs, 0);
        }

        public SatelliteMenu(Context context, Android.Util.IAttributeSet attrs, int defStyle)
            : base(context, attrs, defStyle)
        {

            init(context, attrs, defStyle);
        }

        private void init(Context context, Android.Util.IAttributeSet attrs, int defStyle)
        {
            LayoutInflater.From(context).Inflate(Resource.Layout.sat_main, this, true);
            imgMain = FindViewById<ImageView>(Resource.Id.sat_main);

            if (attrs != null)
            {
                TypedArray typedArray = context.ObtainStyledAttributes(attrs, Resource.Styleable.SatelliteMenu, defStyle, 0);
                satelliteDistance = typedArray.GetDimensionPixelSize(Resource.Styleable.SatelliteMenu_satelliteDistance, DEFAULT_SATELLITE_DISTANCE);
                totalSpacingDegree = typedArray.GetFloat(Resource.Styleable.SatelliteMenu_totalSpacingDegree, DEFAULT_TOTAL_SPACING_DEGREES);
                closeItemsOnClick = typedArray.GetBoolean(Resource.Styleable.SatelliteMenu_closeOnClick, DEFAULT_CLOSE_ON_CLICK);
                expandDuration = typedArray.GetInt(Resource.Styleable.SatelliteMenu_expandDuration, DEFAULT_EXPAND_DURATION);
                //float satelliteDistance = TypedValue.applyDimension(TypedValue.COMPLEX_UNIT_DIP, 170, getResources().getDisplayMetrics());
                typedArray.Recycle();
            }


            mainRotateLeft = SatelliteAnimationCreator.createMainButtonAnimation(context);
            mainRotateRight = SatelliteAnimationCreator.createMainButtonInverseAnimation(context);

            mainRotateLeft.SetAnimationListener(this);
            mainRotateRight.SetAnimationListener(this);
            imgMain.Click += (s, e) =>
            {
                onClick();
            };
            internalItemClickListener = new InternalSatelliteOnClickListener(this);
        }



        public void OnAnimationEnd(Animation animation)
        {

        }

        public void OnAnimationRepeat(Animation animation)
        {

        }

        public void OnAnimationStart(Animation animation)
        {
            plusAnimationActive.Set(false);
        }


        private void onClick()
        {
            if (plusAnimationActive.CompareAndSet(false, true))
            {
                if (!rotated)
                {
                    imgMain.StartAnimation(mainRotateLeft);
                    foreach (SatelliteMenuItem items in menuItems)
                    {
                        items.getView().StartAnimation(items.getOutAnimation());
                    }

                }
                else
                {
                    imgMain.StartAnimation(mainRotateRight);
                    foreach (SatelliteMenuItem items in menuItems)
                    {
                        items.getView().StartAnimation(items.getInAnimation());
                    }
                }
                rotated = !rotated;
            }
        }

        private void openItems()
        {
            if (plusAnimationActive.CompareAndSet(false, true))
            {
                if (!rotated)
                {
                    imgMain.StartAnimation(mainRotateLeft);
                    foreach (SatelliteMenuItem item in menuItems)
                    {
                        item.getView().StartAnimation(item.getOutAnimation());
                    }
                }
                rotated = !rotated;
            }
        }

        private void closeItems()
        {
            if (plusAnimationActive.CompareAndSet(false, true))
            {
                if (rotated)
                {
                    imgMain.StartAnimation(mainRotateRight);
                    foreach (SatelliteMenuItem item in menuItems)
                    {
                        item.getView().StartAnimation(item.getInAnimation());
                    }
                }
                rotated = !rotated;
            }
        }
        public void addItems(List<SatelliteMenuItem> items)
        {

            foreach (var item in items)
            {
                menuItems.Add(item);
            }
            this.RemoveView(imgMain);
            TextView tmpView = new TextView(Context);
            tmpView.LayoutParameters = new FrameLayout.LayoutParams(0, 0);

            float[] degrees = getDegrees(menuItems.Count());
            int index = 0;
            foreach (SatelliteMenuItem menuItem in menuItems)
            {
                int finalX = SatelliteAnimationCreator.getTranslateX(
                        degrees[index], satelliteDistance);
                int finalY = SatelliteAnimationCreator.getTranslateY(
                        degrees[index], satelliteDistance);

                ImageView itemView = (ImageView)LayoutInflater.From(Context)
                        .Inflate(Resource.Layout.sat_item_cr, this, false);
                ImageView cloneView = (ImageView)LayoutInflater.From(Context)
                        .Inflate(Resource.Layout.sat_item_cr, this, false);
                itemView.Tag = menuItem.getId();
                cloneView.Visibility = ViewStates.Gone;
                itemView.Visibility = ViewStates.Gone;

                cloneView.SetOnClickListener(internalItemClickListener);
                cloneView.Tag = Java.Lang.Integer.ValueOf(menuItem.getId());
                FrameLayout.LayoutParams layoutParams = getLayoutParams(cloneView);
                layoutParams.BottomMargin = Math.Abs(finalY);
                layoutParams.LeftMargin = Math.Abs(finalX);
                cloneView.LayoutParameters = layoutParams;

                if (menuItem.getImgResourceId() > 0)
                {
                    itemView.SetImageResource(menuItem.getImgResourceId());
                    cloneView.SetImageResource(menuItem.getImgResourceId());
                }
                else if (menuItem.getImgDrawable() != null)
                {
                    itemView.SetImageDrawable(menuItem.getImgDrawable());
                    cloneView.SetImageDrawable(menuItem.getImgDrawable());
                }

                Animation itemOut = SatelliteAnimationCreator.createItemOutAnimation(Context, index, expandDuration, finalX, finalY);
                Animation itemIn = SatelliteAnimationCreator.createItemInAnimation(Context, index, expandDuration, finalX, finalY);
                Animation itemClick = SatelliteAnimationCreator.createItemClickAnimation(Context);

                menuItem.setView(itemView);
                menuItem.setCloneView(cloneView);
                menuItem.setInAnimation(itemIn);
                menuItem.setOutAnimation(itemOut);
                menuItem.setClickAnimation(itemClick);
                menuItem.setFinalX(finalX);
                menuItem.setFinalY(finalY);

                itemIn.SetAnimationListener(new SatelliteAnimationListener(itemView, true, viewToItemDic));
                itemOut.SetAnimationListener(new SatelliteAnimationListener(itemView, false, viewToItemDic));
                itemClick.SetAnimationListener(new SatelliteItemClickAnimationListener(this, menuItem.getId()));


                this.AddView(itemView);
                this.AddView(cloneView);
                viewToItemDic.Add(itemView, menuItem);
                viewToItemDic.Add(cloneView, menuItem);
                index++;
            }

            this.AddView(imgMain);
        }

        private float[] getDegrees(int count)
        {
            return gapDegreesProvider.getDegrees(count, totalSpacingDegree);
        }

        private void recalculateMeasureDiff()
        {
            int itemWidth = 0;
            if (menuItems.Count() > 0)
            {
                itemWidth = menuItems[0].getView().Width;
            }
            measureDiff = Java.Lang.Float.ValueOf(satelliteDistance * 0.2f).IntValue()
                    + itemWidth;
        }

        private void resetItems()
        {
            if (menuItems.Count() > 0)
            {
                List<SatelliteMenuItem> items = new List<SatelliteMenuItem>(
                        menuItems);
                menuItems.Clear();
                this.RemoveAllViews();
                addItems(items);
            }
        }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
            recalculateMeasureDiff();

            int totalHeight = imgMain.Height + satelliteDistance + measureDiff;
            int totalWidth = imgMain.Width + satelliteDistance + measureDiff;
            SetMeasuredDimension(totalWidth, totalHeight);

        }


        private class SatelliteItemClickAnimationListener : Java.Lang.Object, Animation.IAnimationListener
        {
            private SatelliteMenu menuRef;
            private int tag;

            public SatelliteItemClickAnimationListener(SatelliteMenu menu, int tag)
            {
                this.menuRef = menu;
                this.tag = tag;
            }

            public void OnAnimationEnd(Animation animation)
            {

            }

            public void OnAnimationRepeat(Animation animation)
            {

            }

            public void OnAnimationStart(Animation animation)
            {
                SatelliteMenu menu = menuRef;
                if (menu != null && menu.closeItemsOnClick)
                {
                    menu.close();
                    if (menu.itemClickedListener != null)
                    {
                        menu.itemClickedListener.eventOccured(tag);
                    }
                }
            }
        }

        private class SatelliteAnimationListener : Java.Lang.Object, Animation.IAnimationListener
        {
            private View viewRef;
            private Boolean isInAnimation;
            private Dictionary<View, SatelliteMenuItem> viewToItemDic;

            public SatelliteAnimationListener(View view, Boolean isIn, Dictionary<View, SatelliteMenuItem> viewToItemDic)
            {
                this.viewRef = view;
                this.isInAnimation = isIn;
                this.viewToItemDic = viewToItemDic;
            }
            public void OnAnimationEnd(Animation animation)
            {
                if (viewRef != null)
                {
                    View view = viewRef;
                    if (view != null)
                    {
                        SatelliteMenuItem menuItem = viewToItemDic[view];

                        if (isInAnimation)
                        {
                            menuItem.getView().Visibility = ViewStates.Gone;
                            menuItem.getCloneView().Visibility = ViewStates.Gone;
                        }
                        else
                        {
                            menuItem.getCloneView().Visibility = ViewStates.Visible;
                            menuItem.getView().Visibility = ViewStates.Gone;
                        }
                    }
                }
            }

            public void OnAnimationRepeat(Animation animation)
            {

            }

            public void OnAnimationStart(Animation animation)
            {
                if (viewRef != null)
                {
                    View view = viewRef;
                    if (view != null)
                    {
                        SatelliteMenuItem menuItem = viewToItemDic[view];
                        if (isInAnimation)
                        {
                            menuItem.getView().Visibility = ViewStates.Visible;
                            menuItem.getCloneView().Visibility = ViewStates.Gone;
                        }
                        else
                        {
                            menuItem.getCloneView().Visibility = ViewStates.Gone;
                            menuItem.getView().Visibility = ViewStates.Visible;
                        }
                    }
                }
            }
        }

        public Dictionary<View, SatelliteMenuItem> getViewToItemMap()
        {
            return viewToItemDic;
        }

        private static FrameLayout.LayoutParams getLayoutParams(View view)
        {
            return (FrameLayout.LayoutParams)view.LayoutParameters;
        }

        private  class InternalSatelliteOnClickListener : Java.Lang.Object,View.IOnClickListener
        {
            private SatelliteMenu menuRef;

            public InternalSatelliteOnClickListener(SatelliteMenu menu)
            {
                this.menuRef = menu;
            }
            public void OnClick(View v)
            {
                SatelliteMenu menu = menuRef;
                if (menu != null)
                {
                    SatelliteMenuItem menuItem = menu.getViewToItemMap()[v];

                    v.StartAnimation(menuItem.getClickAnimation());
                }
            }
        }

        /**
         * Sets the click listener for satellite items.
         * 
         * @param itemClickedListener
         */

        public void setOnItemClickedListener(SateliteClickedListener itemClickedListener)
        {
            this.itemClickedListener = itemClickedListener;
        }


        /**
         * Defines the algorithm to define the gap between each item. 
         * Note: Calling before adding items is strongly recommended. 
         * 
         * @param gapDegreeProvider
         */
        public void setGapDegreeProvider(IDegreeProvider gapDegreeProvider)
        {
            this.gapDegreesProvider = gapDegreeProvider;
            resetItems();
        }

        /**
         * Defines the total space between the initial and final item in degrees.
         * Note: Calling before adding items is strongly recommended.
         * 
         * @param totalSpacingDegree The degree between initial and final items. 
         */
        public void setTotalSpacingDegree(float totalSpacingDegree)
        {
            this.totalSpacingDegree = totalSpacingDegree;
            resetItems();
        }

        /**
         * Sets the distance of items from the center in pixels.
         * Note: Calling before adding items is strongly recommended.
         * 
         * @param distance the distance of items to center in pixels.
         */
        public void setSatelliteDistance(int distance)
        {
            this.satelliteDistance = distance;
            resetItems();
        }

        /**
         * Sets the duration for expanding and collapsing the items in miliseconds. 
         * Note: Calling before adding items is strongly recommended.
         * 
         * @param expandDuration the duration for expanding and collapsing the items in miliseconds.
         */
        public void setExpandDuration(int expandDuration)
        {
            this.expandDuration = expandDuration;
            resetItems();
        }

        /**
         * Sets the image resource for the center button.
         * 
         * @param resource The image resource.
         */
        public void setMainImage(int resource)
        {
            this.imgMain.SetImageResource(resource);
        }

        /**
         * Sets the image drawable for the center button.
         * 
         * @param resource The image drawable.
         */
        public void setMainImage(Drawable drawable)
        {
            this.imgMain.SetImageDrawable(drawable);
        }

        /**
         * Defines if the menu shall collapse the items when an item is clicked. Default value is true. 
         * 
         * @param closeItemsOnClick
         */
        public void setCloseItemsOnClick(Boolean closeItemsOnClick)
        {
            this.closeItemsOnClick = closeItemsOnClick;
        }

        /**
         * The listener class for item click event. 
         * @author Siyamed SINIR
         */
        public interface SateliteClickedListener
        {
            /**
             * When an item is clicked, informs with the id of the item, which is given while adding the items. 
             * 
             * @param id The id of the item. 
             */
           void eventOccured(int id);
        }

        /**
         * Expand the menu items. 
         */
        public void expand()
        {
            openItems();
        }

        /**
         * Collapse the menu items
         */
        public void close()
        {
            closeItems();
        }

        protected override IParcelable OnSaveInstanceState()
        {
            IParcelable superState = base.OnSaveInstanceState();
            SavedState ss = new SavedState(superState);
            ss.rotated = rotated;
            ss.totalSpacingDegree = totalSpacingDegree;
            ss.satelliteDistance = satelliteDistance;
            ss.measureDiff = measureDiff;
            ss.expandDuration = expandDuration;
            ss.closeItemsOnClick = closeItemsOnClick;
            return ss;
        }

        protected override void OnRestoreInstanceState(IParcelable state)
        {
            SavedState ss = (SavedState)state;
            rotated = ss.rotated;
            totalSpacingDegree = ss.totalSpacingDegree;
            satelliteDistance = ss.satelliteDistance;
            measureDiff = ss.measureDiff;
            expandDuration = ss.expandDuration;
            closeItemsOnClick = ss.closeItemsOnClick;
            base.OnRestoreInstanceState(state);
        }


        public class SavedState : BaseSavedState, IParcelable, IParcelableCreator
        {
            public Boolean rotated;
            public float totalSpacingDegree;
            public int satelliteDistance;
            public int measureDiff;
            public int expandDuration;
            public Boolean closeItemsOnClick;

            public SavedState(IParcelable superState) : base(superState) { }

            public SavedState(Parcel IN)
                : base(IN)
            {


                rotated = Convert.ToBoolean(Java.Lang.Boolean.ValueOf(IN.ReadString()));
                totalSpacingDegree = IN.ReadFloat();
                satelliteDistance = IN.ReadInt();
                measureDiff = IN.ReadInt();
                expandDuration = IN.ReadInt();
                closeItemsOnClick = Convert.ToBoolean(Java.Lang.Boolean.ValueOf(IN.ReadString()));
            }

            public int DescribeContents()
            {
                return 0;
            }

            public void writeToParcel(Parcel Out, int flags)
            {
               
            }  
            public void WriteToParcel(Parcel dest, ParcelableWriteFlags flags)
            {
                dest.WriteString(Java.Lang.Boolean.ToString(Convert.ToBoolean(rotated)));
                dest.WriteFloat(totalSpacingDegree);
                dest.WriteInt(satelliteDistance);
                dest.WriteInt(measureDiff);
                dest.WriteInt(expandDuration);
                dest.WriteString(Java.Lang.Boolean.ToString(Convert.ToBoolean(closeItemsOnClick)));
            }

            public Java.Lang.Object CreateFromParcel(Parcel source)
            {
                return new SavedState(source);
            }

            public Java.Lang.Object[] NewArray(int size)
            {
                return new SavedState[size];
            }
        }
    }

}