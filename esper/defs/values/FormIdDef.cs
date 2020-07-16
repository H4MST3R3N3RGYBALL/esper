﻿using esper.data;
using esper.elements;
using esper.plugins;
using esper.parsing;
using esper.setup;
using System;
using Newtonsoft.Json.Linq;

namespace esper.defs.values {
    public class FormIdDef : ValueDef {
        public static string defType = "formId";
        public new int size { get => 4; }

        public FormIdDef(DefinitionManager manager, JObject src, Def parent = null)
            : base(manager, src, parent) { }

        public new DataContainer ReadData(PluginFileSource source) {
            UInt32 data = source.reader.ReadUInt32();
            byte ordinal = (byte)(data >> 24);
            var targetPlugin = source.plugin.OrdinalToFile(ordinal, false);
            var formId = data & 0xFFFFFF;
            return new FormIdData(targetPlugin, formId);
        }

        public new DataContainer DefaultData() {
            return new FormIdData(null, 0);
        }

        public new string GetValue(ValueElement element) {
            // TODO: make form ID serializers
            return "";
        }

        public new void SetValue(ValueElement element, string value) {
            // TODO: make form ID parsers
        }
    }
}