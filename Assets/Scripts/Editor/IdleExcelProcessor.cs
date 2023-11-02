using System.Collections.Generic;
using System.Linq;
using Tekly.Sheets.Data;
using Tekly.Sheets.Excel;
using Tekly.Sheets.Processing;
using Tekly.Webster;
using TeklySample.Game.Generators;
using TeklySample.Game.Items;
using TeklySample.Game.Worlds;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace TeklySample.Editor
{
	[CreateAssetMenu]
	public class IdleExcelProcessor : ExcelSheetProcessor
	{
		[SerializeField] private string m_assetIdKey = "Id";
		
		public override void Process(AssetImportContext ctx, Dictionary<string, DataObject> sheets)
		{
			var idKey = new PathKey(m_assetIdKey);
			Frameline.BeginEvent("Start", "IdleExcelProcessor");
			
			ProcessSheet<ItemBalance>(ctx, sheets["Items"], idKey);
			ProcessSheet<GeneratorBalance>(ctx, sheets["Generators"], idKey);
			ProcessSheet<WorldBalance>(ctx, sheets["Worlds"], idKey);
			
			Frameline.EndEvent("Start", "IdleExcelProcessor");
		}

		private void ProcessSheet<T>(AssetImportContext ctx, DataObject sheetData, PathKey idKey) where T: ScriptableObject
		{
			Frameline.BeginEvent($"Process: [{typeof(T).Name}]", "IdleExcelProcessor");
			
			foreach (var data in sheetData.Object.Values.Cast<DataObject>()) {
				var objectId = data.Object[idKey].ToString();

				var asset = CreateInstance<T>();
				asset.name = objectId;
				
				JsonExt.PopulateObject(data.ToJson(), asset);
				ctx.AddObjectToAsset(objectId, asset);
			}
			Frameline.EndEvent($"Process: [{typeof(T).Name}]", "IdleExcelProcessor");
		}
	}
}