package md5be0af9e3c421112295ae1b622f60c3a9;


public class MyWidget
	extends android.appwidget.AppWidgetProvider
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Images.MyWidget, Images, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", MyWidget.class, __md_methods);
	}


	public MyWidget () throws java.lang.Throwable
	{
		super ();
		if (getClass () == MyWidget.class)
			mono.android.TypeManager.Activate ("Images.MyWidget, Images, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
