﻿using esper.elements;
using esper.setup;
using Newtonsoft.Json.Linq;
using System;

namespace esper.defs.TES5 {
    public class CTDATypeFormat : FormatDef {
        public static string defType = "CtdaTypeFormat";

        private static FlagsDef ctdaTypeFlags;
        private static EnumDef ctdaTypeEnum;

        public CTDATypeFormat(DefinitionManager manager, JObject src)
            : base(manager, src) {
            ctdaTypeFlags = (FlagsDef) manager.ResolveDef("CtdaTypeFlags");
            ctdaTypeEnum = (EnumDef) manager.ResolveDef("CtdaTypeEnum");
        }

        public override string DataToValue(ValueElement element, dynamic data) {
            string type = ctdaTypeEnum.DataToValue(element, data & 0xE0);
            string flags = ctdaTypeFlags.DataToValue(element, data & 0x1F);
            return flags != "" ? $"{type} / {flags}" : type;
        }

        public override dynamic ValueToData(ValueElement element, string value) {
            // TODO
            throw new NotImplementedException();
        }
    }
}
