package fantasy.configuration;

import java.util.*;

import org.jdom2.*;
import org.jdom2.output.*;


import fantasy.*;

import fantasy.xserialization.*;

public class SettingsBase implements INotifyPropertyChanged
{
	
	

	public void renew(String xml) throws Exception
	{
		
		
		XSerializer ser = new XSerializer(this.getClass());
		Element e = JDomUtils.parseElement(xml);
		ser.deserialize(e, this);
		
		this.onPropertyChanged(null);
		
	}


	public String toXml() throws Exception
	{
		
		XSerializer ser = new XSerializer(this.getClass());
		Element newSettings = new Element("settings", Namespace.getNamespace(Consts.SettingsNameSpace));
		newSettings.setAttribute("type",this.getClass().getName());
		
		ser.serialize(newSettings, this);
		
		
		String rs = new XMLOutputter(Format.getPrettyFormat()).outputString(newSettings);
		return rs;

	}

	
    private HashSet<IPropertyChangedListener> _listeners = new HashSet<IPropertyChangedListener>();


	protected void onPropertyChanged(String name)
	{
		PropertyChangedEvent e = new PropertyChangedEvent(this, name);
		synchronized(_listeners)
		{
		for(IPropertyChangedListener l : this._listeners)
		{
			l.propertyChanged(e);
		}
		}
	}




	public final void save() throws Exception
	{
		SettingStorage.save(this);
	}


	@Override
	public void AddPropertyChangedListener(IPropertyChangedListener listener) {
	
		synchronized(_listeners)
		{
		    this._listeners.add(listener);
		}
	}


	@Override
	public void RemovePropertyChangedListener(IPropertyChangedListener listener) {
		synchronized(_listeners)
		{
		    this._listeners.remove(listener);
		}
		
	}
}