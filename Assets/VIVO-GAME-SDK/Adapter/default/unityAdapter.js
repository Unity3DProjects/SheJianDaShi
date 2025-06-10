/* eslint-disable */ ! function (e) {
    var t = {};

    function r(n) {
        if (t[n]) return t[n].exports;
        var o = t[n] = {
            i: n,
            l: !1,
            exports: {}
        };
        return e[n].call(o.exports, o, o.exports, r), o.l = !0, o.exports
    }
    r.m = e, r.c = t, r.d = function (e, t, n) {
        r.o(e, t) || Object.defineProperty(e, t, {
            enumerable: !0,
            get: n
        })
    }, r.r = function (e) {
        "undefined" != typeof Symbol && Symbol.toStringTag && Object.defineProperty(e, Symbol.toStringTag, {
            value: "Module"
        }), Object.defineProperty(e, "__esModule", {
            value: !0
        })
    }, r.t = function (e, t) {
        if (1 & t && (e = r(e)), 8 & t) return e;
        if (4 & t && "object" == typeof e && e && e.__esModule) return e;
        var n = Object.create(null);
        if (r.r(n), Object.defineProperty(n, "default", {
                enumerable: !0,
                value: e
            }), 2 & t && "string" != typeof e)
            for (var o in e) r.d(n, o, function (t) {
                return e[t]
            }.bind(null, o));
        return n
    }, r.n = function (e) {
        var t = e && e.__esModule ? function () {
            return e.default
        } : function () {
            return e
        };
        return r.d(t, "a", t), t
    }, r.o = function (e, t) {
        return Object.prototype.hasOwnProperty.call(e, t)
    }, r.p = "", r(r.s = 0)
}([function (e, t, r) {
    window.UnityLoader = window.UnityLoader || {
        // 目前所有文件 均不支持压缩 
        Compression: {
            identity: {
                require: function () {
                    return {}
                },
                decompress: function (e) {
                    return e
                }
            },
            
            decompress: function (e, t) {
                var r = this.identity;
                t(r.decompress(e))
            },
        },
        // md5 摘要算法  用于加载 jsframework
        Cryptography: {
            md5: function (e) {
                var t = UnityLoader.Cryptography.md5.module;
                if (!t) {
                    var r = new ArrayBuffer(16777216),
                        n = function (e, t, r) {
                            "use asm";
                            var n = new e.Uint32Array(r);

                            function o(e, t) {
                                e = e | 0;
                                t = t | 0;
                                var r = 0,
                                    o = 0,
                                    i = 0,
                                    a = 0,
                                    s = 0,
                                    d = 0,
                                    l = 0,
                                    u = 0,
                                    c = 0,
                                    f = 0,
                                    h = 0,
                                    p = 0;
                                r = n[0x80] | 0, o = n[0x81] | 0, i = n[0x82] | 0, a = n[0x83] | 0;
                                for (; t; e = e + 64 | 0, t = t - 1 | 0) {
                                    s = r;
                                    d = o;
                                    l = i;
                                    u = a;
                                    for (f = 0;
                                        (f | 0) < 512; f = f + 8 | 0) {
                                        p = n[f >> 2] | 0;
                                        r = r + (n[f + 4 >> 2] | 0) + (n[e + (p >>> 14) >> 2] | 0) + ((f | 0) < 128 ? a ^ o & (i ^ a) : (f | 0) < 256 ? i ^ a & (o ^ i) : (f | 0) < 384 ? o ^ i ^ a : i ^ (o | ~a)) | 0;
                                        h = (r << (p & 31) | r >>> 32 - (p & 31)) + o | 0;
                                        r = a;
                                        a = i;
                                        i = o;
                                        o = h
                                    }
                                    r = r + s | 0;
                                    o = o + d | 0;
                                    i = i + l | 0;
                                    a = a + u | 0
                                }
                                n[0x80] = r;
                                n[0x81] = o;
                                n[0x82] = i;
                                n[0x83] = a
                            }
                            return {
                                process: o
                            }
                        }({
                            Uint32Array: Uint32Array
                        }, null, r);
                    (t = UnityLoader.Cryptography.md5.module = {
                        buffer: r,
                        HEAPU8: new Uint8Array(r),
                        HEAPU32: new Uint32Array(r),
                        process: n.process,
                        md5: 512,
                        data: 576
                    }).HEAPU32.set(new Uint32Array([7, 3614090360, 65548, 3905402710, 131089, 606105819, 196630, 3250441966, 262151, 4118548399, 327692, 1200080426, 393233, 2821735955, 458774, 4249261313, 524295, 1770035416, 589836, 2336552879, 655377, 4294925233, 720918, 2304563134, 786439, 1804603682, 851980, 4254626195, 917521, 2792965006, 983062, 1236535329, 65541, 4129170786, 393225, 3225465664, 720910, 643717713, 20, 3921069994, 327685, 3593408605, 655369, 38016083, 983054, 3634488961, 262164, 3889429448, 589829, 568446438, 917513, 3275163606, 196622, 4107603335, 524308, 1163531501, 851973, 2850285829, 131081, 4243563512, 458766, 1735328473, 786452, 2368359562, 327684, 4294588738, 524299, 2272392833, 720912, 1839030562, 917527, 4259657740, 65540, 2763975236, 262155, 1272893353, 458768, 4139469664, 655383, 3200236656, 851972, 681279174, 11, 3936430074, 196624, 3572445317, 393239, 76029189, 589828, 3654602809, 786443, 3873151461, 983056, 530742520, 131095, 3299628645, 6, 4096336452, 458762, 1126891415, 917519, 2878612391, 327701, 4237533241, 786438, 1700485571, 196618, 2399980690, 655375, 4293915773, 65557, 2240044497, 524294, 1873313359, 983050, 4264355552, 393231, 2734768916, 851989, 1309151649, 262150, 4149444226, 720906, 3174756917, 131087, 718787259, 589845, 3951481745]))
                }
                t.HEAPU32.set(new Uint32Array([1732584193, 4023233417, 2562383102, 271733878]), t.md5 >> 2);
                for (var o = 0; o < e.length;) {
                    var i = -64 & Math.min(t.HEAPU8.length - t.data, e.length - o);
                    if (t.HEAPU8.set(e.subarray(o, o + i), t.data), o += i, t.process(t.data, i >> 6), e.length - o < 64) {
                        if (i = e.length - o, t.HEAPU8.set(e.subarray(e.length - i, e.length), t.data), o += i, t.HEAPU8[t.data + i++] = 128, i > 56) {
                            for (var a = i; a < 64; a++) t.HEAPU8[t.data + a] = 0;
                            t.process(t.data, 1), i = 0
                        }
                        for (a = i; a < 64; a++) t.HEAPU8[t.data + a] = 0;
                        var s = e.length,
                            d = 0;
                        for (a = 56; a < 64; a++, d = (224 & s) >> 5, s /= 256) t.HEAPU8[t.data + a] = ((31 & s) << 3) + d;
                        t.process(t.data, 1)
                    }
                }
                return new Uint8Array(t.HEAPU8.subarray(t.md5, t.md5 + 16))
            }
        },
        Job: {
            schedule: function (e, t, r, n, o) {
                o = o || {};
                var i = e.Jobs[t];
                if (i || (i = e.Jobs[t] = {
                        dependencies: {},
                        dependants: {}
                    }), i.callback) throw "[UnityLoader.Job.schedule] job '" + t + "' has been already scheduled";
                if ("function" != typeof n) throw "[UnityLoader.Job.schedule] job '" + t + "' has invalid callback";
                if ("object" != typeof o) throw "[UnityLoader.Job.schedule] job '" + t + "' has invalid parameters";
                i.callback = function (e, t) {
                    i.starttime = performance.now(), n(e, t)
                }, i.parameters = o, i.complete = function (r) {
                    for (var n in i.endtime = performance.now(), i.result = {
                            value: r
                        }, i.dependants) {
                        var o = e.Jobs[n];
                        o.dependencies[t] = i.dependants[n] = !1;
                        var a = "function" != typeof o.callback;
                        for (var s in o.dependencies) a = a || o.dependencies[s];
                        if (!a) {
                            if (o.executed) throw "[UnityLoader.Job.schedule] job '" + t + "' has already been executed";
                            o.executed = !0, setTimeout(o.callback.bind(null, e, o), 0)
                        }
                    }
                };
                var a = !1;
                r.forEach(function (r) {
                    var n = e.Jobs[r];
                    n || (n = e.Jobs[r] = {
                        dependencies: {},
                        dependants: {}
                    }), (i.dependencies[r] = n.dependants[t] = !n.result) && (a = !0)
                }), a || (i.executed = !0, setTimeout(i.callback.bind(null, e, i), 0))
            },
            result: function (e, t) {
                var r = e.Jobs[t];
                if (!r) throw "[UnityLoader.Job.result] job '" + t + "' does not exist";
                if ("object" != typeof r.result) throw "[UnityLoader.Job.result] job '" + t + "' has invalid result";
                return r.result.value
            }
        },
        Progress: {
            handlerQG: function () {},
            update: function (e, t, r) {
                (d = e.buildDownloadProgress[t]) || (d = e.buildDownloadProgress[t] = {
                    started: !1,
                    finished: !1,
                    lengthComputable: !1,
                    total: 0,
                    loaded: 0,
                    progress: 0
                }), "object" != typeof r || "progress" != r.type && "load" != r.type || (d.started || (d.started = !0, d.lengthComputable = r.lengthComputable, d.total = r.total), d.loaded = r.loaded, "load" == r.type && (d.finished = !0));
                var n = 0,
                    o = 0,
                    i = 0,
                    a = 0,
                    s = 0,
                    f = 0;
                if (r && r.progress) {
                    d.progress = r.progress
                }
                for (var te in e.buildDownloadProgress) {
                    var de;
                    if (!(de = e.buildDownloadProgress[te]).started) return 0;
                    i++, de.lengthComputable ? (n += de.loaded, o += de.total, a++) : de.finished || s++;
                    if (de.finished) de.progress = 100
                    f += parseFloat(de.progress);
                }
                //i-总任务量，s-已完成任务，a-正在进行中任务，o-正在进行中总所需数据量，n-正在进行中已加载数据量,f-总进度（0-100*任务量）
                var l = i ? (i - s - (o ? a * (o - n) / o : 0)) / i : 0;
                f > 0 ? e.unityInstance.onProgress(e.unityInstance, .0099 * f / i) : e.unityInstance.onProgress(e.unityInstance, .9 * l)
            }
        },

        UnityInfo:{
            unityVersion: "",
            unityPluginVersion: 0,
            unityStartTime: 0,
            unityWasmDownTime: 0,
            unityWasmLoadTime: 0,
            unityCodeSize: 0,
            unityDataSize: 0
        },

        SystemInfo: function () {
            return {
                width: screen.width ? screen.width : 0,
                height: screen.height ? screen.height : 0,
                browser: "",
                browserVersion: 5,
                mobile: true,
                os: "",
                osVersion: "",
                gpu: function () {
                    var e = document.getElementById("canvas").getContext("experimental-webgl");
                    if (e) {
                        var t = e.getExtension("WEBGL_debug_renderer_info");
                        if (t) return e.getParameter(t.UNMASKED_RENDERER_WEBGL)
                    }
                    return "-"
                }(),
                language: window.navigator.userLanguage || window.navigator.language,
                hasWebGL: function () {
                    if (!window.WebGLRenderingContext) return 0;
                    var e = document.getElementById("canvas"),
                        t = e.getContext("webgl2");
                    return t ? 2 : (t = e.getContext("experimental-webgl2")) ? 2 : (t = e.getContext("webgl")) || (t = e.getContext("experimental-webgl")) ? 1 : 0
                }(),
                hasCursorLock: (h = document.createElement("canvas"), h.requestPointerLock || h.mozRequestPointerLock || h.webkitRequestPointerLock || h.msRequestPointerLock ? 1 : 0),
                hasFullscreen: function () {
                    var e = document.createElement("canvas");
                    return (e.requestFullScreen || e.mozRequestFullScreen || e.msRequestFullscreen || e.webkitRequestFullScreen) && (-1 == i.indexOf("Safari") || a >= 10.1) ? 1 : 0
                }(),
                hasThreads: "undefined" != typeof SharedArrayBuffer,
                hasWasm: "object" == typeof WebAssembly && "function" == typeof WebAssembly.validate && "function" == typeof WebAssembly.compile,
                hasWasmThreads: function () {
                    if ("object" != typeof WebAssembly) return !1;
                    if ("undefined" == typeof SharedArrayBuffer) return !1;
                    var e = new WebAssembly.Memory({
                            initial: 1,
                            maximum: 1,
                            shared: !0
                        }),
                        t = e.buffer instanceof SharedArrayBuffer;
                    return e = null, t
                }()
            }
        }(),
        Blobs: {},
        loadCode: function (e, t, r, n) {
            var o = [].slice.call(UnityLoader.Cryptography.md5(t)).map(function (e) {
                    return ("0" + e.toString(16)).substr(-2)
                }).join(""),
                i = document.createElement("script"),
                a = (n.isModularized ? function (e) {
                    return new Blob([e], {
                        type: "application/javascript"
                    })
                } : function (e, t) {
                    return new Blob(['UnityLoader["' + t + '"]=', e], {
                        type: "text/javascript"
                    })
                })(t, o),
                s = URL.createObjectURL(a);
            UnityLoader.Blobs[s] = n, e.deinitializers.push(function () {
                delete UnityLoader.Blobs[s], delete UnityLoader[o], window.qg || document.body.removeChild(document.getElementById(o))
            }), window.qg ? (window.require(e.locateFile(n.url)), e.developmentBuild || URL.revokeObjectURL(s), r(o, a)) : "webgl.wasm.framework.unityweb" == n.url ? (i.src = "Build/webgl.wasm.framework.unityweb", i.type = "text/javascript", i.id = o, i.onload = function () {
                e.developmentBuild || URL.revokeObjectURL(s), r(o, a), delete i.onload
            }, document.body.appendChild(i)) : (i.src = s, i.id = o, i.onload = function () {
                e.developmentBuild || URL.revokeObjectURL(s), r(o, a), delete i.onload
            }, document.body.appendChild(i))
        },

        processWasmCodeJob: function (e, t) {
            e.wasmBinary = UnityLoader.Job.result(e, "downloadWasmCode"), t.complete()
        },
        processWasmFrameworkJob: function (e, t) {
            var r = UnityLoader.Job.result(e, "downloadWasmFramework");
            UnityLoader.loadCode(e, r, function (r, n) {
                e.mainScriptUrlOrBlob = n, e.isModularized && (UnityLoader[r] = UnityModule), UnityLoader[r](e), t.complete()
            }, {
                Module: e,
                url: e.wasmFrameworkUrl,
                isModularized: e.isModularized
            })
        },



        processDataJob: function (e, t) {
            var r = UnityLoader.Job.result(e, "downloadData"),
                n = new DataView(r.buffer, r.byteOffset, r.byteLength),
                o = 0,
                i = "UnityWebData1.0\0";
            if (!String.fromCharCode.apply(null, r.subarray(o, o + i.length)) == i) throw "unknown data format";
            o += i.length;
            var a = n.getUint32(o, !0);
            for (o += 4; o < a;) {
                var s = n.getUint32(o, !0);
                o += 4;
                var d = n.getUint32(o, !0);
                o += 4;
                var l = n.getUint32(o, !0);
                o += 4;
                var u = String.fromCharCode.apply(null, r.subarray(o, o + l));
                o += l;
                for (var c = 0, f = u.indexOf("/", c) + 1; f > 0; c = f, f = u.indexOf("/", c) + 1) e.FS_createPath(u.substring(0, c), u.substring(c, f - 1), !0, !0);
                e.FS_createDataFile(u, null, r.subarray(s, s + d), !0, !0, !0)
            }
            e.removeRunDependency("processDataJob"), t.complete()
        },
        downloadJob: function (e, t) {
            if (window.qg) {
                var n = e.fs,
                    o = t.parameters.url,
                    i = r(1),
                    a = o;
                if (-1 !== o.indexOf("online_mini")) {
                    var cache_key = window.qg.getStorageSync({
                        key: 'mini_wasm_cache_url_md5',
                        default: 'default'
                    })
                    a = window.qg.env.USER_DATA_PATH + "/" + cache_key
                    a += (o.indexOf("online_mini.wasm.code.unityweb") !== -1) ? '/online_mini.wasm.code.unityweb':'/online_mini.data.unityweb'
                    n.getFileInfo({
                        filePath: a,
                        success: function (e) {
                            n.readFile({
                                filePath: a,
                                encoding: "binary",
                                success: function (r) {
                                    UnityLoader.Compression.decompress(new Uint8Array(r.data), function (e) {
                                        t.complete(e)
                                    }), t.parameters.onprogress && t.parameters.onprogress({
                                        type: "progress",
                                        loaded: e.size
                                    }), t.parameters.onload && t.parameters.onload({
                                        type: "load",
                                        loaded: e.size
                                    })
                                },
                                fail: function (e,c) {
                                    UnityLoader.printError(`read file error,  filePath = ${a} msg = ${e} code = ${c}`)     
                                }
                            })
                            UnityLoader.recordWasmSize(a,e.length)
                        },
                        fail: function (e) {
                            UnityLoader.printError(`getFileInfo error,  filePath = ${a}`)
                        }
                    })
                } else if (-1 !== o.indexOf("http") || -1 !== o.indexOf("https")) {
                    var s = {
                            dataUrl: "",
                            wasmCodeUrl: ""
                        },
                        d = window.qg.env.USER_DATA_PATH + "/_quickgame_wasm_cacheFile";
                    if (n.accessSync(d)) {
                        s = JSON.parse(n.readFileSync(d, "utf8").data)
                    }
                    // try {
                    //     n.accessSync(d), s = JSON.parse(n.readFileSync(d, "utf8"))
                    // } catch (e) {}
                    var l = i.hex_md5(o),
                        u = !1;
                    a = window.qg.env.USER_DATA_PATH + "/" + l + "_quickgame_wasm_";
                    if (n.accessSync(a)) {
                        u = !0
                    } else {
                        u = !1
                    }
                    // try {
                    //     n.accessSync(a), u = !0
                    // } catch (e) {
                    //     u = !1
                    // }
                    if (u) {
                        n.getFileInfo({
                            filePath: a,
                            success: function (e) {
                                n.readFile({
                                    filePath: a,
                                    encoding: "binary",
                                    success: function (r) {
                                        UnityLoader.Compression.decompress(new Uint8Array(r.data), function (e) {
                                            t.complete(e)
                                        }), t.parameters.onprogress && t.parameters.onprogress({
                                            type: "progress",
                                            loaded: e.size
                                        }), t.parameters.onload && t.parameters.onload({
                                            type: "load",
                                            loaded: e.size
                                        })
                                    },
                                    fail: function (e,c) {
                                        UnityLoader.printError(`read file error,  filePath = ${a} msg = ${e} code = ${c}`)
                                    }
                                })
                                UnityLoader.recordWasmSize(a,e.length)
                            },
                            fail: function (e) {
                                UnityLoader.printError(`getFileInfo error,  filePath = ${a}`)
                            }
                        })
                    } else {
                        -1 != o.indexOf("wasm.code") && ("" === s.wasmCodeUrl ? s.wasmCodeUrl = a : s.wasmCodeUrl !== a && (n.unlinkSync(s.wasmCodeUrl), s.wasmCodeUrl = a), n.writeFileSync(d, JSON.stringify(s), "utf8")), -1 != o.indexOf("data.unityweb") && ("" === s.dataUrl ? s.dataUrl = a : s.dataUrl !== a && (n.unlinkSync(s.dataUrl), s.dataUrl = a), n.writeFileSync(d, JSON.stringify(s), "utf8"));
                        var downloadTask = window.qg.downloadFile({
                            url: o,
                            filePath: a,
                            success: function () {
                                n.getFileInfo({
                                    filePath: a,
                                    success: function (e) {
                                        n.readFile({
                                            filePath: a,
                                            encoding: "binary",
                                            success: function (r) {
                                                UnityLoader.Compression.decompress(new Uint8Array(r.data), function (e) {
                                                    t.complete(e)
                                                }), t.parameters.onload && t.parameters.onload({
                                                    type: "load",
                                                    loaded: e.size
                                                })
                                            },
                                            fail: function (e,c) {
                                                UnityLoader.printError(`read file error,  filePath = ${a} msg = ${e} code = ${c}`)
                                            }
                                        })
                                        UnityLoader.recordWasmSize(a,e.length)
                                    },
                                    fail: function (e) {
                                        UnityLoader.printError(`getFileInfo error,  filePath = ${a}`)
                                    }
                                })
                            },
                            fail: function (e) {
                                UnityLoader.printError(`downloadFile error,  path = ${o}`)
                            }
                        });
                        downloadTask.onProgressUpdate(function (msg) {
                            var progress = msg["progress"];
                            t.parameters.onprogress && t.parameters.onprogress({
                                type: "progress",
                                progress: progress
                            })
                        });
                    }
                } else n.getFileInfo({
                    filePath: a,
                    success: function (e) {
                        n.readFile({
                            filePath: a,
                            encoding: "binary",
                            success: function (r) {
                                UnityLoader.Compression.decompress(new Uint8Array(r.data), function (e) {
                                    t.complete(e)
                                }), t.parameters.onprogress && t.parameters.onprogress({
                                    type: "progress",
                                    loaded: e.size
                                }), t.parameters.onload && t.parameters.onload({
                                    type: "load",
                                    loaded: e.size
                                })
                            },
                            fail: function (e,c) {
                                UnityLoader.printError(`read file error,  filePath = ${a} msg = ${e} code = ${c}`)
                            }
                        })
                        UnityLoader.recordWasmSize(a,e.length)
                    },
                    fail: function (e) {
                        UnityLoader.printError(`getFileInfo error,  filePath = ${a}`)
                    }
                })
            }else{
                UnityLoader.printErr(" window has not qg")
            } 
        },
        scheduleBuildDownloadJob: function (e, t, r) {
            UnityLoader.Progress.update(e, t), UnityLoader.Job.schedule(e, t, [], UnityLoader.downloadJob, {
                url: e.resolveBuildUrl(e[r]),
                onprogress: function (r) {
                    UnityLoader.Progress.update(e, t, r)
                },
                onload: function (r) {
                    UnityLoader.Progress.update(e, t, r)
                },
                objParameters: e.companyName && e.productName && e.cacheControl && (e.cacheControl[r] || e.cacheControl.default) ? {
                    companyName: e.companyName,
                    productName: e.productName,
                    cacheControl: e.cacheControl[r] || e.cacheControl.default
                } : null
            })
        },

        printLog: function(e){
            console.log("[unity adapter] :" + e)
        },

        printError: function(e){
            console.error("[unity adapter] :" + e)
        },

        fprintLog: function(e){
            if(e.indexOf("Compressed DXT") !== -1 && e.indexOf("is not supported") !== -1){
                console.warn("[unity framework] :" + "当前游戏内纹理是DXT格式,建议使用Unity2021及以上版本将纹理压缩格式设置成ASTC，可优化游戏运行中内存从而提高流畅和稳定性")
            }else{
                console.log("[unity framework] :" + e)
            }
        },

        fprintError: function(e){
            console.error("[unity framework] :" + e)
        },

        fprintWarn: function(e){
            console.warn("[unity framework] :" + e)
        },

        recordWasmSize: function(src,size){
            if(src.indexOf("wasm.code") !== -1){
                UnityLoader.UnityInfo.unityCodeSize = size
            } else {
                UnityLoader.UnityInfo.unityDataSize = size
            }
        },

        loadModule: function (e, t) {

            var r = ["downloadWasmFramework"]; 
            
            // handle code
            UnityLoader.scheduleBuildDownloadJob(e, "downloadWasmCode", "wasmCodeUrl"), 
            UnityLoader.Job.schedule(e, "processWasmCode", ["downloadWasmCode"], UnityLoader.processWasmCodeJob), 

            r.push("processWasmCode"), 

            // handle framework
            UnityLoader.scheduleBuildDownloadJob(e, "downloadWasmFramework", "wasmFrameworkUrl"), 
            UnityLoader.Job.schedule(e, "processWasmFramework", r, UnityLoader.processWasmFrameworkJob)
                
            // handle data
            UnityLoader.scheduleBuildDownloadJob(e, "downloadData", "dataUrl"), 
            e.preRun.push(function () {
                e.addRunDependency("processDataJob"), 
                UnityLoader.Job.schedule(e, "processData", ["downloadData"], UnityLoader.processDataJob)
            })
        },
        instantiate: function (t, r) {
            function n(n) {
                var o = n.Module;
                o.canvas = document.getElementById("canvas"), 
                o.canvas.style.width = n.width, o.canvas.style.height = n.height, o.canvas.id = "#canvas";
                var i = !0;
                if (window.qg) {
                    n.Module.fs.readFile({
                        filePath: n.url,
                        encoding: "utf8",
                        success: function (e) {
                            var t = JSON.parse(e.data);
                            for (var a in t) void 0 === o[a] && (o[a] = t[a]);
                            if (o.unityVersion) {
                                var s = o.unityVersion.match(/(\d+)\.(\d+)\.(\d+)(.+)/);
                                s && (o.unityVersion = {
                                    string: o.unityVersion,
                                    version: parseInt(s[0]),
                                    major: parseInt(s[1]),
                                    minor: parseInt(s[2]),
                                    suffix: s[3]
                                })
                            }
                            if(o.unityPluginVersion){
                                UnityLoader.UnityInfo.unityVersion = o.unityVersion;
                                UnityLoader.UnityInfo.unityPluginVersion = o.unityPluginVersion;
                            }
                            o.isModularized = o.unityVersion 
                            if(o.unityVersion.version >= 2019){
                                n.onProgress(n, 0),
                                UnityLoader.loadModule(o, r.onerror)
                            }else{
                                r.onerror("unityversion is small 2019")
                                o.printErr("unityversion is small 2019")
                            }
                        },
                        fail: function (e) {
                            o.print("Could not find filePath " + n.url), 0 == document.URL.indexOf("file:") && alert("It seems your browser does not support running Unity WebGL content from file:// urls. Please upload it to an http server, or try a different browser.")
                        }
                    })
                } 
                i
            }

            function o(e) {
                return o.link = o.link || document.createElement("a"), o.link.href = e, o.link.href
            }
            void 0 === r && (r = {}), void 0 === r.onerror && (r.onerror = function (e) {
                printError(e)
            });
            var i = {
                url: t,
                onProgress: UnityLoader.Progress.handlerQG,
                Module: {
                    deinitializers: [],
                    intervals: {},
                    setInterval: function (e, t) {
                        var r = window.setInterval(e, t);
                        return this.intervals[r] = !0, r
                    },
                    clearInterval: function (e) {
                        delete this.intervals[e], window.clearInterval(e)
                    },
                    onAbort: function (e) {
                        throw void 0 !== e ? (this.print(e), this.printErr(e), e = JSON.stringify(e)) : e = "", "abort(" + e + ") at " + this.stackTrace()
                    },
                    preRun: [],
                    postRun: [],
                    print: function (e) {
                        UnityLoader.printLog(e);
                    },
                    printErr: function (e) {
                        UnityLoader.printError(e);
                    },
                    Jobs: {},
                    buildDownloadProgress: {},
                    resolveBuildUrl: function (e) {
                        if(e.indexOf("code.unityweb") != -1 || e.indexOf("data.unityweb") != -1){
                            var subUnityPkg = UnityLoader.EnvConfig.getConfig("subUnityPkg")
                            if(subUnityPkg){
                                return "/unitySubPkg/" + e;
                            }
                        }
                        return e.match(/(http|https|ftp|file):\/\//) ? e : t.substring(0, t.lastIndexOf("/") + 1) + e
                    },
                    streamingAssetsUrl: function () {
                        return UnityLoader.EnvConfig.getConfig("streamingAssetsUrl")
                    },
                    locateFile: function (e) {
                        return "buildUnity/".concat("build.wasm" == e ? this.wasmCodeUrl : e)
                    },
                    fs: window.qg ? window.qg.getFileSystemManager() : {}
                },
                SetFullscreen: function () {
                    if (i.Module.SetFullscreen) return i.Module.SetFullscreen.apply(i.Module, arguments)
                },
                SendMessage: function () {
                    if (i.Module.SendMessage) return i.Module.SendMessage.apply(i.Module, arguments)
                },
                Quit: function (e) {
                    "function" == typeof e && (i.Module.onQuit = e), i.Module.shouldQuit = !0
                }
            };
            for (var a in i.Module.unityInstance = i, i.popup = function (e, t) {
                printErr(e);
                }, i.Module.postRun.push(function () {
                    i.onProgress(i, 1), "object" == typeof r && "function" == typeof r.onsuccess && r.onsuccess(i.Module)
                }), r)
                if ("Module" == a)
                    for (var s in r[a]) i.Module[s] = r[a][s];
                else i[a] = r[a];
            return window.qg, n(i), i
        },
        EnvConfig:{
            envMap: new Map(),
            getConfig:function(type){
                var value = UnityLoader.EnvConfig.envMap.get(type)
                if (this.isEmpty(value) && 'true' === qg.accessFile({
                    uri: "/buildUnity/env.conf"
                  })) {
                  try {
                    const envConf = qg.readFileSync({
                      uri: '/buildUnity/env.conf',
                      encoding: 'utf8'
                    })
                    var envData = JSON.parse(envConf.text)
                    for (var key in envData) {
                        UnityLoader.EnvConfig.envMap.set(key,envData[key])
                    }
                    value = UnityLoader.EnvConfig.envMap.get(type)
                  } catch (error) {
                    UnityLoader.printError(" getConfig error " + JSON.stringify(error))
                  }
                }
                UnityLoader.printLog(`EnvConfig getConfig type = '${type} value = ${value}'`)
                return value
            },
            isEmpty: function (obj) {
                if (typeof obj === 'undefined' || obj == null || obj === '') {
                  return true;
                } else {
                  return false;
                }
            }
        },
    }
}, function (e, t) {
    var r = 0,
        n = "",
        o = 8;

    function i(e, t) {
        e[t >> 5] |= 128 << t % 32, e[14 + (t + 64 >>> 9 << 4)] = t;
        for (var r = 1732584193, n = -271733879, o = -1732584194, i = 271733878, a = 0; a < e.length; a += 16) {
            var c = r,
                h = n,
                p = o,
                w = i;
            r = s(r, n, o, i, e[a + 0], 7, -680876936), i = s(i, r, n, o, e[a + 1], 12, -389564586), o = s(o, i, r, n, e[a + 2], 17, 606105819), n = s(n, o, i, r, e[a + 3], 22, -1044525330), r = s(r, n, o, i, e[a + 4], 7, -176418897), i = s(i, r, n, o, e[a + 5], 12, 1200080426), o = s(o, i, r, n, e[a + 6], 17, -1473231341), n = s(n, o, i, r, e[a + 7], 22, -45705983), r = s(r, n, o, i, e[a + 8], 7, 1770035416), i = s(i, r, n, o, e[a + 9], 12, -1958414417), o = s(o, i, r, n, e[a + 10], 17, -42063), n = s(n, o, i, r, e[a + 11], 22, -1990404162), r = s(r, n, o, i, e[a + 12], 7, 1804603682), i = s(i, r, n, o, e[a + 13], 12, -40341101), o = s(o, i, r, n, e[a + 14], 17, -1502002290), r = d(r, n = s(n, o, i, r, e[a + 15], 22, 1236535329), o, i, e[a + 1], 5, -165796510), i = d(i, r, n, o, e[a + 6], 9, -1069501632), o = d(o, i, r, n, e[a + 11], 14, 643717713), n = d(n, o, i, r, e[a + 0], 20, -373897302), r = d(r, n, o, i, e[a + 5], 5, -701558691), i = d(i, r, n, o, e[a + 10], 9, 38016083), o = d(o, i, r, n, e[a + 15], 14, -660478335), n = d(n, o, i, r, e[a + 4], 20, -405537848), r = d(r, n, o, i, e[a + 9], 5, 568446438), i = d(i, r, n, o, e[a + 14], 9, -1019803690), o = d(o, i, r, n, e[a + 3], 14, -187363961), n = d(n, o, i, r, e[a + 8], 20, 1163531501), r = d(r, n, o, i, e[a + 13], 5, -1444681467), i = d(i, r, n, o, e[a + 2], 9, -51403784), o = d(o, i, r, n, e[a + 7], 14, 1735328473), r = l(r, n = d(n, o, i, r, e[a + 12], 20, -1926607734), o, i, e[a + 5], 4, -378558), i = l(i, r, n, o, e[a + 8], 11, -2022574463), o = l(o, i, r, n, e[a + 11], 16, 1839030562), n = l(n, o, i, r, e[a + 14], 23, -35309556), r = l(r, n, o, i, e[a + 1], 4, -1530992060), i = l(i, r, n, o, e[a + 4], 11, 1272893353), o = l(o, i, r, n, e[a + 7], 16, -155497632), n = l(n, o, i, r, e[a + 10], 23, -1094730640), r = l(r, n, o, i, e[a + 13], 4, 681279174), i = l(i, r, n, o, e[a + 0], 11, -358537222), o = l(o, i, r, n, e[a + 3], 16, -722521979), n = l(n, o, i, r, e[a + 6], 23, 76029189), r = l(r, n, o, i, e[a + 9], 4, -640364487), i = l(i, r, n, o, e[a + 12], 11, -421815835), o = l(o, i, r, n, e[a + 15], 16, 530742520), r = u(r, n = l(n, o, i, r, e[a + 2], 23, -995338651), o, i, e[a + 0], 6, -198630844), i = u(i, r, n, o, e[a + 7], 10, 1126891415), o = u(o, i, r, n, e[a + 14], 15, -1416354905), n = u(n, o, i, r, e[a + 5], 21, -57434055), r = u(r, n, o, i, e[a + 12], 6, 1700485571), i = u(i, r, n, o, e[a + 3], 10, -1894986606), o = u(o, i, r, n, e[a + 10], 15, -1051523), n = u(n, o, i, r, e[a + 1], 21, -2054922799), r = u(r, n, o, i, e[a + 8], 6, 1873313359), i = u(i, r, n, o, e[a + 15], 10, -30611744), o = u(o, i, r, n, e[a + 6], 15, -1560198380), n = u(n, o, i, r, e[a + 13], 21, 1309151649), r = u(r, n, o, i, e[a + 4], 6, -145523070), i = u(i, r, n, o, e[a + 11], 10, -1120210379), o = u(o, i, r, n, e[a + 2], 15, 718787259), n = u(n, o, i, r, e[a + 9], 21, -343485551), r = f(r, c), n = f(n, h), o = f(o, p), i = f(i, w)
        }
        return Array(r, n, o, i)
    }

    function a(e, t, r, n, o, i) {
        return f((a = f(f(t, e), f(n, i))) << (s = o) | a >>> 32 - s, r);
        var a, s
    }

    function s(e, t, r, n, o, i, s) {
        return a(t & r | ~t & n, e, t, o, i, s)
    }

    function d(e, t, r, n, o, i, s) {
        return a(t & n | r & ~n, e, t, o, i, s)
    }

    function l(e, t, r, n, o, i, s) {
        return a(t ^ r ^ n, e, t, o, i, s)
    }

    function u(e, t, r, n, o, i, s) {
        return a(r ^ (t | ~n), e, t, o, i, s)
    }

    function c(e, t) {
        var r = h(e);
        r.length > 16 && (r = i(r, e.length * o));
        for (var n = Array(16), a = Array(16), s = 0; s < 16; s++) n[s] = 909522486 ^ r[s], a[s] = 1549556828 ^ r[s];
        var d = i(n.concat(h(t)), 512 + t.length * o);
        return i(a.concat(d), 640)
    }

    function f(e, t) {
        var r = (65535 & e) + (65535 & t);
        return (e >> 16) + (t >> 16) + (r >> 16) << 16 | 65535 & r
    }

    function h(e) {
        for (var t = Array(), r = (1 << o) - 1, n = 0; n < e.length * o; n += o) t[n >> 5] |= (e.charCodeAt(n / o) & r) << n % 32;
        return t
    }

    function p(e) {
        for (var t = "", r = (1 << o) - 1, n = 0; n < 32 * e.length; n += o) t += String.fromCharCode(e[n >> 5] >>> n % 32 & r);
        return t
    }

    function w(e) {
        for (var t = r ? "0123456789ABCDEF" : "0123456789abcdef", n = "", o = 0; o < 4 * e.length; o++) n += t.charAt(e[o >> 2] >> o % 4 * 8 + 4 & 15) + t.charAt(e[o >> 2] >> o % 4 * 8 & 15);
        return n
    }

    function m(e) {
        for (var t = "", r = 0; r < 4 * e.length; r += 3)
            for (var o = (e[r >> 2] >> r % 4 * 8 & 255) << 16 | (e[r + 1 >> 2] >> (r + 1) % 4 * 8 & 255) << 8 | e[r + 2 >> 2] >> (r + 2) % 4 * 8 & 255, i = 0; i < 4; i++) 8 * r + 6 * i > 32 * e.length ? t += n : t += "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/".charAt(o >> 6 * (3 - i) & 63);
        return t
    }
    e.exports = {
        hex_md5: function (e) {
            return w(i(h(e), e.length * o))
        },
        b64_md5: function (e) {
            return m(i(h(e), e.length * o))
        },
        str_md5: function (e) {
            return p(i(h(e), e.length * o))
        },
        hex_hmac_md5: function (e, t) {
            return w(c(e, t))
        },
        b64_hmac_md5: function (e, t) {
            return m(c(e, t))
        },
        str_hmac_md5: function (e, t) {
            return p(c(e, t))
        }
    }
}]);