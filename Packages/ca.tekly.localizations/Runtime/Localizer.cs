﻿using System;
using System.Collections.Generic;
using Tekly.Common.Utils;
using Tekly.Logging;

namespace Tekly.Localizations
{
    public class Localizer : Singleton<Localizer, ILocalizer>, ILocalizer
    {
        private readonly Dictionary<string, LocalizationString> m_strings = new Dictionary<string, LocalizationString>();

        private readonly TkLogger m_logger = TkLogger.Get<Localizer>();
        
        private readonly (string, object)[] m_emptyData = Array.Empty<(string, object)>();
        private readonly ArraysPool<object> m_objectArrayPool = new ArraysPool<object>();
        
        public void Clear()
        {
            m_strings.Clear();
        }
        
        public void AddData(LocalizationData localizationData)
        {
            foreach (var dataString in localizationData.Strings) {
                LocalizationStringifier.Stringify(dataString.Format, out var outFormat, out var outKeys);
                m_strings[dataString.Id] = new LocalizationString(dataString.Id, outFormat, outKeys);
            }
        }

        public string Localize(string id)
        {
            if (m_strings.TryGetValue(id, out var locString)) {
                if (locString.Keys != null && locString.Keys.Length > 0) {
                    return Localize(locString, m_emptyData);
                }
                return locString.Format;
            }

            m_logger.Error("Failed to find localization ID: [{id}]", ("id", id));
            
            return $"[{id}]";
        }

        public string Localize(string id, (string, object)[] data)
        {
            if (m_strings.TryGetValue(id, out var locString)) {
                return Localize(locString, data);
            }
            
            m_logger.Error("Failed to find localization ID: [{id}]", ("id", id));
            return $"[{id}]";
        }

        public string Localize(LocalizationString locString, (string, object)[] data)
        {
            var formattingData = m_objectArrayPool.Get(locString.Keys.Length);
            ToFormattedArray(formattingData, data, locString.Keys);
            var text = string.Format(locString.Format, formattingData);
                
            m_objectArrayPool.Return(formattingData);

            return text;
        }

        private void ToFormattedArray(object[] outObjects, (string, object)[] data, string[] keys)
        {
            for (var index = 0; index < keys.Length; index++) {
                outObjects[index] = GetData(keys[index], data);
            }
        }

        private object GetData(string key, (string, object)[] data)
        {
            if (key[0] == '$') {
                return Localize(key);
            }
            
            foreach (var (dataKey, dataValue) in data) {
                if (dataKey == key) {
                    return dataValue;
                }
            }
            
            m_logger.Error("Failed to find data with key: [{key}]", ("key", key));

            return $"[{key}]";
        }
    }
}