const QGAudio = (window.qg.wasmsdk = window.qg.wasmsdk || {});

QGAudio._JS_ScreenOrientation_DeInit = function() {
}
QGAudio._JS_ScreenOrientation_Init = function(n) {
}
QGAudio._JS_ScreenOrientation_Lock = function(n) {
}
QGAudio._JS_Sound_Create_Channel = function(o, i) {
  const WEBAudio = unityInstance.Module.WEBAudio;
  if (0 != WEBAudio.audioWebEnabled && window.qg) {
    var e = function () {
        o && unityInstance.Module.dynCall_vi(o, [i]);
      },
      u = {
        audio: null,
        loop: !1,
        volume: 1,
        isBind: !1,
        isStop: !1,
        stopTime: 0,
        isPlay: !1,
        playTime: 0,
        isStopped: !1,
        playAudio: function (o, i) {
          this.setup(),
            o > 0
              ? ((this.audio.volume = 0),
                (this.isPlay = !0),
                (this.playTime = o))
              : ((this.audio.volume = this.volume),
                (this.audio.loop = this.loop)),
            this.audio.play(),
            0 != i && this.audio.seek(i);
        },
        setup: function () {
          if (this.audio && 0 == this.isBind) {
            this.audio.offEnded(e), this.audio.onEnded(e);
            var o = this;
            (this.isStoped = !1),
              this.audio.offTimeUpdate(null),
              this.audio.onTimeUpdate(function () {
                1 == o.isStop &&
                  parseFloat(o.audio.currentTime) >= o.stopTime &&
                  ((o.stopTime = 0),
                  (o.isStop = !1),
                  o.audio.stop(),
                  o.audio.destroy(),
                  (o.audio.src = ""),
                  (o.isBind = !1),
                  (o.isStoped = !0)),
                  !o.isStoped &&
                    1 == o.isPlay &&
                    parseFloat(o.audio.currentTime) >= o.playTime &&
                    ((o.isPlay = !1),
                    (o.playTime = 0),
                    (o.audio.volume = o.volume),
                    (o.audio.loop = o.loop));
              }),
              (this.isBind = !0);
          }
        },
        stopAudio: function (o) {
          0 == o
            ? (this.audio.offTimeUpdate(null),
              this.audio.offEnded(e),
              (this.stopTime = 0),
              (this.isStop = !1),
              this.audio.stop(),
              this.audio.destroy(),
              (this.isBind = !1))
            : ((this.stopTime = o), (this.isStop = !0));
        },
      };
    return WEBAudio.audioInstances.push(u) - 1;
  }
}
QGAudio._JS_Sound_GetLength = function(o) {
  const WEBAudio = unityInstance.Module.WEBAudio;
  if (0 == WEBAudio.audioWebEnabled) return 0;
  var i = WEBAudio.audioInstances[o];
  return i ? (window.qg ? i.bufferLength : void 0) : 0;
}
QGAudio._JS_Sound_GetLoadState = function(o) {
  const WEBAudio = unityInstance.Module.WEBAudio;
  if (0 == WEBAudio.audioWebEnabled) return 2;
  if (window.qg) {
    var i = WEBAudio.audioInstances[o],
      e = i?i.loadState:1;
    return 1 == e ? 2 : 2 == e ? 0 : 1;
  }
}
QGAudio._QG_Generate_Audio_Hash = function (buffer, o, i) {
  if (!window.qg.vTestOpts || !window.qg.vTestOpts.USE_AUDIO_HASH_NAME) {
    return "/audio" + o + i + ".mp3";
  }
  let hash = 0x811c9dc5;
  const bytes = buffer instanceof Uint8Array ? buffer : new Uint8Array(buffer);
  const len = Math.min(bytes.length, 1024); // 只取前1KB
  for (let i = 0; i < len; i++) {
    const byte = bytes[i];
    hash ^= byte;
    hash += (hash << 1) + (hash << 4) + (hash << 7) + (hash << 8) + (hash << 24);
    hash >>>= 0;
  }
  const length = 8
  let hex = hash.toString(16);
  if (hex.length < length) {
    hex = '0'.repeat(length - hex.length) + hex;
  }
  return "/audio/" + hex.slice(-length) + ".mp3";
}
QGAudio._JS_Sound_Load = function(o, i) {
  const WEBAudio = unityInstance.Module.WEBAudio;
  if (0 == WEBAudio.audioWebEnabled) return 0;
  if (window.qg) {
    var e = {
        buffer: null,
        error: !1,
        file: "",
        audio: window.qg.createInnerAudioContext(),
        bufferLength: 0,
        ptr: o,
        loadState: 0,
      },
      u = window.qg.getFileSystemManager(),
      t = WEBAudio.audioInstances.push(e) - 1;
    const HEAPU8 = unityInstance.Module.HEAPU8;
    return (
      (e.buffer = HEAPU8.buffer.slice(o, o + i)),
      (e.file = window.qg.env.USER_DATA_PATH + QGAudio._QG_Generate_Audio_Hash(e.buffer, o, i)),
      console.log("_JS_Sound_Load " + e.file),
      u.accessSync(e.file) || u.writeFileSync(e.file, e.buffer, "binary"),
      e.audio.onCanplay(function () {
        (e.bufferLength = (44100 * e.audio.duration) | 0), (e.loadState = 2);
      }),
      e.audio.onError(function (o) {
        console.log(o), (e.loadState = 1);
      }),
      (e.audio.src = e.file),
      t
    );
  }
}
QGAudio._JS_Sound_Init = function() {
  const WEBAudio = unityInstance.Module.WEBAudio;
  window.qg && (WEBAudio.audioWebEnabled = 1);
}
QGAudio._JS_Sound_Load_PCM = function(o, i, e, u) {
  const WEBAudio = unityInstance.Module.WEBAudio;
  if (0 == WEBAudio.audioWebEnabled) return 0;
  for (
    var t = {
        buffer: WEBAudio.audioContext.createBuffer(o, i, e),
        error: !1,
      },
      n = 0;
    n < o;
    n++
  ) {
    var d = (u >> 2) + i * n,
      a = t.buffer,
      s =
        a.copyToChannel ||
        function (o, i, e) {
          var u = o.subarray(0, Math.min(o.length, this.length - (0 | e)));
          this.getChannelData(0 | i).set(u, 0 | e);
        };
    s.apply(a, [unityInstance.Module.HEAPF32.subarray(d, d + i), n, 0]);
  }
  var r = WEBAudio.audioInstances.push(t) - 1;
  return r;
}
QGAudio._JS_Sound_Play = function(o, i, e, u) {
  const WEBAudio = unityInstance.Module.WEBAudio;
  if (window.qg) {
    if (
      (QGAudio._JS_Sound_Stop(i, 0),
      console.log("sound play", o, i, e, u),
      0 == WEBAudio.audioWebEnabled)
    )
      return;
    var t = WEBAudio.audioInstances[o],
      n = WEBAudio.audioInstances[i];
    if (t.buffer)
      try {
        if ((console.log("play", t.file), "" !== t.audio.src))
          (n.audio = t.audio),
            (n.audio.volume = n.volume),
            (n.audio.loop = n.loop),
            n.playAudio(parseFloat(t.audio.currentTime) + u, e);
        else {
          (t.loadState = 0),
            (t.audio = window.qg.createInnerAudioContext()),
            t.audio.onCanplay(function () {
              (t.bufferLength = (44100 * t.audio.duration) | 0),
                (t.loadState = 2);
            }),
            t.audio.onError(function (o) {
              console.log(o), (t.loadState = 1);
            }),
            (t.audio.src = t.file),
            (n.audio = t.audio),
            (n.audio.volume = n.volume),
            (n.audio.loop = n.loop);
          var d = t.loadState;
          if (0 == d)
            var a = setInterval(function () {
              0 !== t.loadState &&
                (2 == t.loadState &&
                  n.playAudio(parseFloat(t.audio.currentTime) + u, e),
                clearInterval(a));
            }, 100);
          else 2 == d && n.playAudio(parseFloat(t.audio.currentTime) + u, e);
        }
      } catch (o) {
        console.error("playBuffer error. Exception: " + o);
      }
    else console.log("Trying to play sound which is not loaded.");
  }
}
QGAudio._JS_Sound_ReleaseInstance = function(o) {
  const WEBAudio = unityInstance.Module.WEBAudio;
  WEBAudio.audioInstances[o] = null;
}
QGAudio._JS_Sound_ResumeIfNeeded = function() {
  const WEBAudio = unityInstance.Module.WEBAudio;
  0 != WEBAudio.audioWebEnabled &&
    (window.qg ||
      ("suspended" === WEBAudio.audioContext.state &&
        WEBAudio.audioContext.resume()));
}
QGAudio._JS_Sound_Set3D = function(o, i) {
  const WEBAudio = unityInstance.Module.WEBAudio;
  if (!window.qg) {
    var e = WEBAudio.audioInstances[o];
    e.threeD != i && ((e.threeD = i), e.setupPanning());
  }
}
QGAudio._JS_Sound_SetListenerOrientation = function(o, i, e, u, t, n) {
  const WEBAudio = unityInstance.Module.WEBAudio;
  0 != WEBAudio.audioWebEnabled &&
    (window.qg ||
      (WEBAudio.audioContext.listener.forwardX
        ? (WEBAudio.audioContext.listener.forwardX.setValueAtTime(
            -o,
            WEBAudio.audioContext.currentTime
          ),
          WEBAudio.audioContext.listener.forwardY.setValueAtTime(
            -i,
            WEBAudio.audioContext.currentTime
          ),
          WEBAudio.audioContext.listener.forwardZ.setValueAtTime(
            -e,
            WEBAudio.audioContext.currentTime
          ),
          WEBAudio.audioContext.listener.upX.setValueAtTime(
            u,
            WEBAudio.audioContext.currentTime
          ),
          WEBAudio.audioContext.listener.upY.setValueAtTime(
            t,
            WEBAudio.audioContext.currentTime
          ),
          WEBAudio.audioContext.listener.upZ.setValueAtTime(
            n,
            WEBAudio.audioContext.currentTime
          ))
        : WEBAudio.audioContext.listener.setOrientation(-o, -i, -e, u, t, n)));
}
QGAudio._JS_Sound_SetListenerPosition = function(o, i, e) {
  const WEBAudio = unityInstance.Module.WEBAudio;
  0 != WEBAudio.audioWebEnabled &&
    (window.qg ||
      (WEBAudio.audioContext.listener.positionX
        ? (WEBAudio.audioContext.listener.positionX.setValueAtTime(
            o,
            WEBAudio.audioContext.currentTime
          ),
          WEBAudio.audioContext.listener.positionY.setValueAtTime(
            i,
            WEBAudio.audioContext.currentTime
          ),
          WEBAudio.audioContext.listener.positionZ.setValueAtTime(
            e,
            WEBAudio.audioContext.currentTime
          ))
        : WEBAudio.audioContext.listener.setPosition(o, i, e)));
}
QGAudio._JS_Sound_SetLoop = function(o, i) {
  const WEBAudio = unityInstance.Module.WEBAudio;
  if (0 != WEBAudio.audioWebEnabled && window.qg) {
    var e = WEBAudio.audioInstances[o],
      u = 1 == i;
    (e.loop = u), null !== e.audio && (e.audio.loop = u);
  }
}
QGAudio._JS_Sound_SetLoopPoints = function(o, i, e) {
  const WEBAudio = unityInstance.Module.WEBAudio;
  if (!window.qg) {
    if (0 == WEBAudio.audioWebEnabled) return;
    var u = WEBAudio.audioInstances[o];
    (u.source.loopStart = i), (u.source.loopEnd = e);
  }
}
QGAudio._JS_Sound_SetPitch = function(o, i) {
  const WEBAudio = unityInstance.Module.WEBAudio;
  0 != WEBAudio.audioWebEnabled &&
    (window.qg ||
      WEBAudio.audioInstances[o].source.playbackRate.setValueAtTime(
        i,
        WEBAudio.audioContext.currentTime
      ));
}
QGAudio._JS_Sound_SetPosition = function(o, i, e, u) {
  const WEBAudio = unityInstance.Module.WEBAudio;
  0 != WEBAudio.audioWebEnabled &&
    (window.qg || WEBAudio.audioInstances[o].panner.setPosition(i, e, u));
}
QGAudio._JS_Sound_SetVolume = function(o, i) {
  const WEBAudio = unityInstance.Module.WEBAudio;
  if (window.qg) {
    if (0 == WEBAudio.audioWebEnabled) return;
    var e = WEBAudio.audioInstances[o];
    (e.volume = i), null !== e.audio && (e.audio.volume = i);
  }
}
QGAudio._JS_Sound_Stop = function(o, i) {
  const WEBAudio = unityInstance.Module.WEBAudio;
  if (window.qg) {
    if (0 == WEBAudio.audioWebEnabled) return;
    console.log("stop", o, i);
    var e = WEBAudio.audioInstances[o];
    e.audio &&
      (console.log(o),
      0 == i
        ? (e.stopAudio(i), e.audio.offEnded(null))
        : e.stopAudio(parseFloat(e.audio.currentTime) + i),
      (e.audio = null));
  }
}
QGAudio._JS_Sound_SetPaused = function(a, d) {
  const WEBAudio = unityInstance.Module.WEBAudio;
  if (0 != WEBAudio.audioWebEnabled && window.qg) {
    var u = WEBAudio.audioInstances[a],
      i = !u.audio || u.audio.paused;
    d != i && (d ? u.audio.pause() : u.audio.play());
  }
}