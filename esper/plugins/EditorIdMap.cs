﻿using esper.elements;
using System.Collections.Generic;

namespace esper.plugins {
    public class EditorIdMap {
        private readonly SortedDictionary<string, MainRecord> _map;

        public EditorIdMap() {
            _map = new SortedDictionary<string, MainRecord>();
        }

        public MainRecord Get(string editorId) {
            return _map[editorId];
        }

        public void Add(MainRecord rec) {
            _map[rec.editorId] = rec;
        }
    }
}