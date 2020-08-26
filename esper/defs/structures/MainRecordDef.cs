﻿using esper.data;
using esper.elements;
using esper.helpers;
using esper.plugins;
using esper.setup;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace esper.defs {
    public class MainRecordDef : MembersDef {
        public static readonly string defType = "record";

        private readonly string _signature;
        private HeaderManager headerManager => manager.headerManager;
        private UInt16 recordHeaderSize => headerManager.recordHeaderSize;

        public override string signature => _signature;
        public StructDef headerDef;
        public FormIdDef containedInDef;

        public MainRecordDef(DefinitionManager manager, JObject src)
            : base(manager, src) {
            ErrorHelpers.CheckDefProperty(src, "signature");
            _signature = src.Value<string>("signature");
            headerDef = manager.BuildMainRecordHeaderDef(src, this);
            containedInDef = (FormIdDef) JsonHelpers.Def(manager, src, "containedInElement");
        }

        private void ReadHeader(MainRecord rec, PluginFileSource source) {
            source.stream.Position = rec.bodyOffset - recordHeaderSize;
            headerManager.HeaderToStructElement(rec, source);
        }

        internal void HandleSubrecord(
            MainRecord rec, PluginFileSource source, ref int defIndex
        ) {
            var subrecord = source.currentSubrecord;
            var def = GetMemberDef(subrecord.signature, ref defIndex);
            if (def == null) {
                rec._unexpectedSubrecords.Add(subrecord);
                source.SubrecordHandled();
            } else if (def.IsSubrecord()) {
                def.ReadElement(rec, source, subrecord.dataSize);
                source.SubrecordHandled();
                defIndex++;
            } else {
                var container = (Container)def.PrepareElement(rec);
                def.SubrecordFound(container, source);
                defIndex++;
            }
        }

        private void ReadSubrecords(MainRecord rec, PluginFileSource source) {
            int defIndex = 0;
            source.StartRead(rec.dataSize);
            while (true) {
                if (!source.NextSubrecord()) break;
                HandleSubrecord(rec, source, ref defIndex);
            }
            source.EndRead();
        }

        internal void InitContainedInElements(GroupRecord group, MainRecord rec) {
            if (group == null || !group.hasRecordParent) return;
            var parentRec = group.GetParentRecord();
            var containedInDef = parentRec.mrDef.containedInDef;
            if (containedInDef == null) return;
            var element = (ValueElement)containedInDef.NewElement(rec);
            element._data = FormId.FromSource(parentRec._file, parentRec.formId);
        }

        internal void ReadElements(MainRecord rec, PluginFileSource source) {
            rec._internalElements = new List<Element>();
            rec._unexpectedSubrecords = new List<Subrecord>();
            InitContainedInElements(rec.container as GroupRecord, rec);
            ReadHeader(rec, source);
            source.WithRecordData(rec, () => {
                ReadSubrecords(rec, source);
                rec.ElementsReady();
            });
        }

        internal void InitElement(MainRecord rec) {
            foreach (var def in memberDefs) {
                if (!def.required) continue;
                if (rec._internalElements.Any(e => e.def == def)) continue;
                var e = def.NewElement(rec);
                e.Initialize();
            }
        }
    }
}
