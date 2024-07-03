using System.Collections.Generic;

namespace Tekly.Tinker.Core
{
	public class DataItem
	{
		public string Name;
		public string Value;
		public string Color = "grey";

		public DataItem(string name, string value, string color)
		{
			Name = name;
			Value = value;
			Color = color;
		}

		public DataItem(string name, float value, string color)
		{
			Name = name;
			Value = value.ToString("0.00");
			Color = color;
		}
		
		public DataItem(string name, double value, string color)
		{
			Name = name;
			Value = value.ToString("0.00");
			Color = color;
		}

		public DataItem(string name, int value, string color)
		{
			Name = name;
			Value = value.ToString();
			Color = color;
		}

		public DataItem(string name, bool value, string color)
		{
			Name = name;
			Value = value.ToString();
			Color = color;
		}
	}
	
	public class DataList
	{
		public string Name;
		public List<DataItem> Items = new List<DataItem>();

		public DataList(string name)
		{
			Name = name;
		}
		
		public DataList Add(string name, string value, string color = "grey")
		{
			Items.Add(new DataItem(name, value, color));
			return this;
		}

		public DataList Add(string name, float value, string color = "grey")
		{
			Items.Add(new DataItem(name, value, color));
			return this;
		}
		
		public DataList Add(string name, double value, string color = "grey")
		{
			Items.Add(new DataItem(name, value, color));
			return this;
		}

		public DataList Add(string name, int value, string color = "grey")
		{
			Items.Add(new DataItem(name, value, color));
			return this;
		}

		public DataList Add(string name, bool value, string color = "grey")
		{
			Items.Add(new DataItem(name, value, color));
			return this;
		}
		
	}
}