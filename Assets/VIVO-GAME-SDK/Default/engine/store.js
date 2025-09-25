/* eslint-disable */
! function (e) {
    var t = {};

    function s(n) {
        if (t[n]) return t[n].exports;
        var r = t[n] = {
            i: n,
            l: !1,
            exports: {}
        };
        return e[n].call(r.exports, r, r.exports, s), r.l = !0, r.exports
    }
    s.m = e, s.c = t, s.d = function (e, t, n) {
        s.o(e, t) || Object.defineProperty(e, t, {
            enumerable: !0,
            get: n
        })
    }, s.r = function (e) {
        "undefined" != typeof Symbol && Symbol.toStringTag && Object.defineProperty(e, Symbol.toStringTag, {
            value: "Module"
        }), Object.defineProperty(e, "__esModule", {
            value: !0
        })
    }, s.t = function (e, t) {
        if (1 & t && (e = s(e)), 8 & t) return e;
        if (4 & t && "object" == typeof e && e && e.__esModule) return e;
        var n = Object.create(null);
        if (s.r(n), Object.defineProperty(n, "default", {
                enumerable: !0,
                value: e
            }), 2 & t && "string" != typeof e)
            for (var r in e) s.d(n, r, function (t) {
                return e[t]
            }.bind(null, r));
        return n
    }, s.n = function (e) {
        var t = e && e.__esModule ? function () {
            return e.default
        } : function () {
            return e
        };
        return s.d(t, "a", t), t
    }, s.o = function (e, t) {
        return Object.prototype.hasOwnProperty.call(e, t)
    }, s.p = "", s(s.s = 0)
}([function (e, t, s) {
    "use strict";
    s.r(t);
    var n = class {
        constructor(e) {
            this.fs = localStorage
        }
        read(e) {
            var t = this.fs.getItem("dbs");
            return t ? JSON.parse(t) : null
        }
        write(e) {
            var t = this.read(e);
            t || (t = {});
            const {
                name: s,
                callback: n = (e => e),
                indexNames: r,
                storeDatas: o,
                ...i
            } = e, {
                indexNames: a = {},
                ...c
            } = t[s] || {};
            window.qg && o && (Object.keys(o).forEach(e => {
                const t = o[e];
                Object.keys(t).forEach(e => {
                    t[e].contents && t[e].contents instanceof Int8Array && (localStorage.setArrayBufferItem(e, t[e].contents), t[e].contents = e)
                })
            }), t[s].storeDatas = o), t[s] = {
                ...c,
                ...i,
                storeDatas: o,
                indexNames: {
                    ...a,
                    ...r
                }
            }, this.fs.setItem("dbs", JSON.stringify(t)), n(!0)
        }
        append() {}
    };
    var r = class {
        constructor(e = []) {
            this.contains = function (e) {
                return !!~this.item.indexOf(e)
            }, this.item = e, this.length = this.item.length
        }
    };
    var o = class {
        constructor(e) {
            if (this.direction = "next", this._storeDatas = e.storeDatas.slice(), this.request = e.request || null, this.source = e.source || null, this._storeDatas.length > 0) {
                const e = this._storeDatas.shift();
                this.key = e[this.source.name], this.primaryKey = e.primaryKey
            }
        }
        continue () {
            if (this._storeDatas.length > 0) {
                const e = this._storeDatas.shift();
                this.key = e[this.source.name], this.primaryKey = e.primaryKey, this.request.onsuccess({
                    target: {
                        result: this
                    }
                })
            } else this.request.onsuccess({
                target: {
                    result: null
                }
            })
        }
    };
    var i = class {
        constructor(e) {
            this.name = e.name, this.keyPath = e.keyPath, this.multiEntry = !1, this.unique = e.unique || !1, this.objectStore = e.objectStore
        }
        get(e) {}
        getKey(e) {}
        getAll(e, t) {}
        getAllKeys(e, t) {}
        count(e) {}
        openCursor(e) {}
        openKeyCursor() {
            const e = this.objectStore._iDBRequest;
            console.log('indexdb store')
            return setTimeout(() => {
                if (e.onsuccess) {
                    const t = (new n).read(),
                        {
                            source: {
                                name: s
                            },
                            result: r
                        } = e,
                        i = t[r.name].storeDatas ? t[r.name].storeDatas[s] : [],
                        a = [];
                    Object.keys(i).forEach(e => {
                        a.push(i[e])
                    }), e.onsuccess({
                        target: {
                            result: a.length > 0 ? new o({
                                source: this,
                                request: e,
                                storeDatas: a
                            }) : null
                        }
                    })
                }
            }, 10), e
        }
    };
    var a = class {
        constructor(e, t) {
            this.name = e, this.keyPath = t.keyPath || "", this.indexNames = t.indexNames || new r, this.transaction = t.transaction, this.autoIncrement = t.autoIncrement || !1, this._iDBRequest = new h(this.transaction.db.name, this.transaction.db.version, {
                source: this
            }), this._fs = new n
        }
        put(e, t) {
            const {
                db: s
            } = this.transaction, {
                storeDatas: n = {}
            } = s;
            n[`${this.name}`] || (n[`${this.name}`] = {}), Object.keys(e).forEach(t => {
                e[t] instanceof Date && "timestamp" == t && (e[t] = e[t].getTime())
            });
            let r = n[`${this.name}`][t];
            if (r) {
                const {
                    contents: t
                } = r;
                t && Object.keys(t).length > e.contents.length && (e.contents = t)
            }
            return r = {
                ...r,
                ...e,
                primaryKey: t
            }, n[`${this.name}`][t] = r, s.storeDatas = n, this._fs.write({
                ...s
            }), setTimeout(() => {
                console.warn('indexdb执行')
                this._iDBRequest.onsuccess && this._iDBRequest.onsuccess({
                    target: {
                        result: null
                    }
                })
            }, 0), this._iDBRequest
        }
        add(e, t) {
            return this.put(e, t)
        }
        delete(e) {
            const {
                db: t
            } = this.transaction, {
                storeDatas: s = {}
            } = t, n = s[`${this.name}`];
            return n && n[e] ? (window.qg && n[e].contents && localStorage.removeItem(e), delete n[e], this._fs.write({
                ...t
            }), setTimeout(() => {
                this._iDBRequest.onsuccess && this._iDBRequest.onsuccess({
                    target: {
                        result: null
                    }
                })
            }, 0)) : setTimeout(() => {
                this._iDBRequest.onerror && this._iDBRequest.onerror({
                    preventDefault: () => {}
                })
            }, 0), this._iDBRequest
        }
        clear() {}
        get(e) {
            const {
                db: t
            } = this.transaction, {
                storeDatas: s = {}
            } = t;
            if (s[`${this.name}`] && s[`${this.name}`][e]) {
                const t = this._iDBRequest.result = s[`${this.name}`][e];
                if (t.contents)
                    if (window.qg && "string" == typeof t.contents) {
                        const e = t.contents;
                        t.contents = localStorage.getArrayBufferItem(e)
                    } else {
                        const {
                            contents: e
                        } = t;
                        t.contents = new Uint8Array(Object.values(e))
                    } if (t.timestamp) {
                    const {
                        timestamp: e
                    } = t;
                    t.timestamp = new Date(e)
                }
                setTimeout(() => {
                    console.warn('indexdb do get')
                    this._iDBRequest.onsuccess && this._iDBRequest.onsuccess({
                        target: {
                            result: t
                        }
                    })
                }, 0)
            } else this._iDBRequest.onerror && this._iDBRequest.onerror({
                error: `${this.name} not found | ${e} not found`,
                preventDefault() {}
            });
            return this._iDBRequest
        }
        getKey(e) {}
        getAll(e, t) {}
        getAllKeys(e, t) {}
        count(e) {}
        index(e) {
            return new i({
                name: e,
                keyPath: e,
                objectStore: this
            })
        }
        createIndex(e, t, s) {
            this.indexNames.item.push(e), (new n).write({
                name: this.transaction.db.name,
                indexNames: {
                    [`${this.name}`]: {
                        keyPath: this.keyPath,
                        indexNames: this.indexNames
                    }
                }
            })
        }
        deleteIndex(e) {}
    };
    var c = class extends EventTarget {
        constructor(e) {
            super(), this.objectStoreNames = e.objectStoreNames, this.mode = null, this.db = e.db, this.error = e.error || null, this.abort = e.abort || null
        }
        objectStore(e) {
            if (!this.objectStoreNames.contains(e)) throw "NotFoundError";
            var t;
            return this.db.indexNames && ~this.db.indexNames[e] && (t = this.db.indexNames[e]), new a(e, {
                transaction: this,
                indexNames: t ? t.indexNames : new r,
                keyPath: t ? t.keyPath : ""
            })
        }
    };
    var u = class extends EventTarget {
        constructor(e, t, s) {
            super(), this.name = e, this.version = t, this.objectStoreNames = new r, this.onabort = null, this.onclose = null, this.onerror = null, this.onversionchange = null;
            var o = new n,
                i = o.read(),
                a = i && i[e];
            const {
                callback: u = (e => e),
                initTransaction: h
            } = s;
            h && (h = new c({
                db: this
            })), setTimeout(() => {
                if (!a || a.version < t) o.write({
                    name: e,
                    version: t,
                    callback: e => {
                        u(e)
                    }
                });
                else {
                    if (a.version < t) throw "The value of version is less than exist version";
                    u(!1)
                }
            }, 0)
        }
        close() {}
        createObjectStore(e, t) {
            var s = new a(e, {
                ...t,
                transaction: new c({
                    db: this
                })
            });
            return this.objectStoreNames.item.push(e), setTimeout(() => {
                (new n).write({
                    name: this.name,
                    objectStoreNames: this.objectStoreNames
                })
            }, 0), s
        }
        deleteObjectStore(e) {}
        transaction(e, t) {
            var s = (new n).read(),
                o = s && s[this.name],
                i = o.objectStoreNames && o.objectStoreNames.item || {},
                a = [];
            return ("string" == typeof e ? [e] : e).forEach(e => {
                if (i.length > 0 && -1 === i.indexOf(e)) throw "The specified object store was not found.";
                a.push(e)
            }), new c({
                db: {
                    ...this,
                    ...o
                },
                objectStoreNames: new r(a)
            })
        }
    };
    var h = class extends EventTarget {
        constructor(e, t, s = {}) {
            super(), this.transaction = s.transaction || null, this.result = new u(e, t, {
                ...s,
                initTransaction: this.transaction
            }), this.error = null, this.source = s.source || null, this.readyState = s.readyState || null, this.onerror = null, this.onsuccess = null
        }
    };
    var l = class extends h {
        constructor(e, t, s) {
            super(e, t, s), this.onblocked = null, this.onupgradeneeded = null
        }
    };
    var m = class {
        constructor() {
            this.timer = null
        }
        open(e, t) {
            var s = new l(e, t, {
                callback: e => {
                    e && s.onupgradeneeded && setTimeout(() => {
                        s.onupgradeneeded({
                            target: s
                        })
                    }, 0), s.onsuccess && setTimeout(() => {
                        s.onsuccess({
                            target: s
                        })
                    }, 0)
                }
            });
            return s
        }
        databases() {}
        deleteDatabase() {}
        cmp(e, t) {}
    };
    delete window.indexedDB, 
    //window.indexedDB = new m, 
    console.log("store version: ", "0.0.4")
}]);