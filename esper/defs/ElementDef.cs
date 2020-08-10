﻿using esper.elements;
using esper.plugins;
using esper.setup;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace esper.defs {
    public class ElementDef : Def {
        internal int sortOrder;

        public readonly string signature;
        public readonly string name;
        public readonly bool required;

        public virtual int? size => 0;

        public ElementDef(DefinitionManager manager, JObject src, Def parent)
            : base(manager, src, parent) {
            signature = src.Value<string>("signature");
            name = src.Value<string>("name");
            required = src.Value<bool>("required");
        }

        public virtual bool ContainsSignature(string signature) {
            return false;
        }

        public virtual List<string> GetSignatures(List<string> sigs = null) {
            throw new NotImplementedException();
        }

        public virtual void SubrecordFound(
            Container container, PluginFileSource source, string sig, UInt16 size
        ) {
            throw new NotImplementedException();
        }

        public virtual Element InitElement(Container container) {
            throw new NotImplementedException();
        }

        public virtual Element ReadElement(
            Container container, PluginFileSource source, UInt16? size = null
        ) {
            throw new NotImplementedException();
        }

        public virtual Element PrepareElement(Container container) {
            throw new NotImplementedException();
        }

        public bool IsSubrecord() {
            return signature != null;
        }

        public virtual bool HasSignature(string sig) {
            return signature == sig;
        }

        public virtual string GetSortKey(Element element) {
            throw new NotImplementedException();
        }
    }
}
