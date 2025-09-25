using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QGMiniGame
{
    public class QGReplaceRules
    {
        public static ReplaceRule manifestRule =
        new ReplaceRule()
        {
            oldStr = "\"type\": \"game\"",
            newStr = "\"type\": \"game\",\"subpackages\": [{\"name\": \"unitySubPkg\",\"root\": \"unitySubPkg/\"}]"
        };
        public static ReplaceRule[] jsonConfigRules = {
        new ReplaceRule()
        {
           oldStr="webgl.data.unityweb",
           newStr="online_mini.data.unityweb"
        },
        new ReplaceRule()
        {
           oldStr="webgl.wasm.code.unityweb",
           newStr="online_mini.wasm.code.unityweb"
        }
        };

        public static ReplaceRule[] frameworkRules = {
        new ReplaceRule()
        {
           oldStr="console.log",
           newStr="window.UnityLoader.fprintLog"
        },
        new ReplaceRule()
        {
           oldStr="console.warn",
           newStr="window.UnityLoader.fprintWarn"
        },
        new ReplaceRule()
        {
           oldStr="console.error",
           newStr="window.UnityLoader.fprintError"
        },
        new ReplaceRule()
        {
           oldStr="IDBFS.storeLocalEntry\\(path,entry,done\\)",
           newStr="if(window[\"qg\"]){IDBFS.storeLocalEntry(entry[\"primaryKey\"],entry,done)}else{IDBFS.storeLocalEntry(path,entry,done)}"
        },
        new ReplaceRule()
        {
           oldStr="function stringToUTF8Array\\(str,outU8Array,outIdx,maxBytesToWrite\\)\\{",
           newStr="function stringToUTF8Array(str,outU8Array,outIdx,maxBytesToWrite){if(str==undefined){return 0}"
        },
         new ReplaceRule()
        {
           oldStr="function lengthBytesUTF8\\(str\\)\\{",
           newStr="function lengthBytesUTF8(str){if(str==undefined){return 0}"
        },
        new ReplaceRule()
        {
           oldStr="function receiveInstantiatedSource\\(output\\)\\{",
           newStr="var start,end,__performance;if(window[\"qg\"]){__performance=window[\"qg\"].getPerformance()}else{__performance=window[\"performance\"]}function receiveInstantiatedSource(output){end=__performance.now();if(window[\"qg\"]){window[\"qg\"].setWasmTaskCompile(false)}console.log(\"wasm compile time :\",end-start);"
        },
         new ReplaceRule()
        {
           oldStr="function instantiateArrayBuffer\\(receiver\\)\\{getBinaryPromise\\(\\)\\.then\\(\\(function\\(binary\\)\\{",
           newStr="function instantiateArrayBuffer(receiver){getBinaryPromise().then((function(binary){start=__performance.now();if(window[\"qg\"]){window[\"qg\"].setWasmTaskCompile(true)}"
        },
          new ReplaceRule()
        {
           oldStr="assert\\(ret,\"IDBFS used, but indexedDB not supported\"\\)\\;",
           newStr=""
        },
          new ReplaceRule()
        {
           oldStr="self\\[\"performance\"\\]",
           newStr="window[\"performance\"]"
        },
          new ReplaceRule()
        {
           oldStr=@"var http=Module.companyName[\s\S]*?:new XMLHttpRequest;",
           newStr="var http = new XMLHttpRequest;"
        },
          new ReplaceRule()
        {
           oldStr=@"var WEBAudio={[\s\S]*?};",
           newStr="function QG_JS_Sound_Create_Channel(o,i){if(0!=WEBAudio.audioWebEnabled&&window.qg){var e=function(){o&&dynCall(\"vi\",o,[i])},u={audio:null,loop:!1,volume:1,isBind:!1,isStop:!1,stopTime:0,isPlay:!1,playTime:0,isStopped:!1,playAudio:function(o,i){this.setup(),o>0?(this.audio.volume=0,this.isPlay=!0,this.playTime=o):(this.audio.volume=this.volume,this.audio.loop=this.loop),this.audio.play(),0!=i&&this.audio.seek(i)},setup:function(){if(this.audio&&0==this.isBind){this.audio.offEnded(e),this.audio.onEnded(e);var o=this;this.isStoped=!1,this.audio.offTimeUpdate(null),this.audio.onTimeUpdate(function(){1==o.isStop&&parseFloat(o.audio.currentTime)>=o.stopTime&&(o.stopTime=0,o.isStop=!1,o.audio.stop(),o.audio.destroy(),o.audio.src=\"\",o.isBind=!1,o.isStoped=!0),!o.isStoped&&1==o.isPlay&&parseFloat(o.audio.currentTime)>=o.playTime&&(o.isPlay=!1,o.playTime=0,o.audio.volume=o.volume,o.audio.loop=o.loop)}),this.isBind=!0}},stopAudio:function(o){0==o?(this.audio.offTimeUpdate(null),this.audio.offEnded(e),this.stopTime=0,this.isStop=!1,this.audio.stop(),this.audio.destroy(),this.isBind=!1):(this.stopTime=o,this.isStop=!0)}};return WEBAudio.audioInstances.push(u)-1}}function QG_JS_Sound_GetLength(o){if(0==WEBAudio.audioWebEnabled)return 0;var i=WEBAudio.audioInstances[o];return i?window.qg?i.bufferLength:void 0:0}function QG_JS_Sound_GetLoadState(o){if(0==WEBAudio.audioWebEnabled)return 2;if(window.qg){var i=WEBAudio.audioInstances[o],e=i.loadState;return 1==e?2:2==e?0:1}}function QG_JS_Sound_Load(o,i){if(0==WEBAudio.audioWebEnabled)return 0;if(window.qg){var e={buffer:null,error:!1,file:\"\",audio:window.qg.createInnerAudioContext(),bufferLength:0,ptr:o,loadState:0},u=window.qg.getFileSystemManager(),t=WEBAudio.audioInstances.push(e)-1;return e.file=window.qg.env.USER_DATA_PATH+\"/audio\"+o+i+\".mp3\",console.log(\"_JS_Sound_Load \"+e.file),e.buffer=HEAPU8.buffer.slice(o,o+i),u.accessSync(e.file)||u.writeFileSync(e.file,e.buffer,\"binary\"),e.audio.onCanplay(function(){e.bufferLength=44100*e.audio.duration|0,e.loadState=2}),e.audio.onError(function(o){console.log(o),e.loadState=1}),e.audio.src=e.file,t}}function QG_JS_Sound_Init(){window.qg&&(WEBAudio.audioWebEnabled=1)}function QG_JS_Sound_Load_PCM(o,i,e,u){if(0==WEBAudio.audioWebEnabled)return 0;for(var t={buffer:WEBAudio.audioContext.createBuffer(o,i,e),error:!1},n=0;n<o;n++){var d=(u>>2)+i*n,a=t.buffer,s=a.copyToChannel||function(o,i,e){var u=o.subarray(0,Math.min(o.length,this.length-(0|e)));this.getChannelData(0|i).set(u,0|e)};s.apply(a,[HEAPF32.subarray(d,d+i),n,0])}var r=WEBAudio.audioInstances.push(t)-1;return r}function QG_JS_Sound_Play(o,i,e,u){if(window.qg){if(QG_JS_Sound_Stop(i,0),console.log(\"sound play\",o,i,e,u),0==WEBAudio.audioWebEnabled)return;var t=WEBAudio.audioInstances[o],n=WEBAudio.audioInstances[i];if(t.buffer)try{if(console.log(\"play\",t.file),\"\"!==t.audio.src)n.audio=t.audio,n.audio.volume=n.volume,n.audio.loop=n.loop,n.playAudio(parseFloat(t.audio.currentTime)+u,e);else{t.loadState=0,t.audio=window.qg.createInnerAudioContext(),t.audio.onCanplay(function(){t.bufferLength=44100*t.audio.duration|0,t.loadState=2}),t.audio.onError(function(o){console.log(o),t.loadState=1}),t.audio.src=t.file,n.audio=t.audio,n.audio.volume=n.volume,n.audio.loop=n.loop;var d=t.loadState;if(0==d)var a=setInterval(function(){0!==t.loadState&&(2==t.loadState&&n.playAudio(parseFloat(t.audio.currentTime)+u,e),clearInterval(a))},100);else 2==d&&n.playAudio(parseFloat(t.audio.currentTime)+u,e)}}catch(o){console.error(\"playBuffer error. Exception: \"+o)}else console.log(\"Trying to play sound which is not loaded.\")}}function QG_JS_Sound_ReleaseInstance(o){WEBAudio.audioInstances[o]=null}function QG_JS_Sound_ResumeIfNeeded(){0!=WEBAudio.audioWebEnabled&&(window.qg||\"suspended\"===WEBAudio.audioContext.state&&WEBAudio.audioContext.resume())}function QG_JS_Sound_Set3D(o,i){if(!window.qg){var e=WEBAudio.audioInstances[o];e.threeD!=i&&(e.threeD=i,e.setupPanning())}}function QG_JS_Sound_SetListenerOrientation(o,i,e,u,t,n){0!=WEBAudio.audioWebEnabled&&(window.qg||(WEBAudio.audioContext.listener.forwardX?(WEBAudio.audioContext.listener.forwardX.setValueAtTime(-o,WEBAudio.audioContext.currentTime),WEBAudio.audioContext.listener.forwardY.setValueAtTime(-i,WEBAudio.audioContext.currentTime),WEBAudio.audioContext.listener.forwardZ.setValueAtTime(-e,WEBAudio.audioContext.currentTime),WEBAudio.audioContext.listener.upX.setValueAtTime(u,WEBAudio.audioContext.currentTime),WEBAudio.audioContext.listener.upY.setValueAtTime(t,WEBAudio.audioContext.currentTime),WEBAudio.audioContext.listener.upZ.setValueAtTime(n,WEBAudio.audioContext.currentTime)):WEBAudio.audioContext.listener.setOrientation(-o,-i,-e,u,t,n)))}function QG_JS_Sound_SetListenerPosition(o,i,e){0!=WEBAudio.audioWebEnabled&&(window.qg||(WEBAudio.audioContext.listener.positionX?(WEBAudio.audioContext.listener.positionX.setValueAtTime(o,WEBAudio.audioContext.currentTime),WEBAudio.audioContext.listener.positionY.setValueAtTime(i,WEBAudio.audioContext.currentTime),WEBAudio.audioContext.listener.positionZ.setValueAtTime(e,WEBAudio.audioContext.currentTime)):WEBAudio.audioContext.listener.setPosition(o,i,e)))}function QG_JS_Sound_SetLoop(o,i){if(0!=WEBAudio.audioWebEnabled&&window.qg){var e=WEBAudio.audioInstances[o],u=1==i;e.loop=u,null!==e.audio&&(e.audio.loop=u)}}function QG_JS_Sound_SetLoopPoints(o,i,e){if(!window.qg){if(0==WEBAudio.audioWebEnabled)return;var u=WEBAudio.audioInstances[o];u.source.loopStart=i,u.source.loopEnd=e}}function QG_JS_Sound_SetPitch(o,i){0!=WEBAudio.audioWebEnabled&&(window.qg||WEBAudio.audioInstances[o].source.playbackRate.setValueAtTime(i,WEBAudio.audioContext.currentTime))}function QG_JS_Sound_SetPosition(o,i,e,u){0!=WEBAudio.audioWebEnabled&&(window.qg||WEBAudio.audioInstances[o].panner.setPosition(i,e,u))}function QG_JS_Sound_SetVolume(o,i){if(window.qg){if(0==WEBAudio.audioWebEnabled)return;var e=WEBAudio.audioInstances[o];e.volume=i,null!==e.audio&&(e.audio.volume=i)}}function QG_JS_Sound_Stop(o,i){if(window.qg){if(0==WEBAudio.audioWebEnabled)return;console.log(\"stop\",o,i);var e=WEBAudio.audioInstances[o];e.audio&&(console.log(o),0==i?(e.stopAudio(i),e.audio.offEnded(null)):e.stopAudio(parseFloat(e.audio.currentTime)+i),e.audio=null)}}function QG_JS_Sound_SetPaused(a,d){if(0!=WEBAudio.audioWebEnabled&&window.qg){var u=WEBAudio.audioInstances[a],i=!u.audio||u.audio.paused;d!=i&&(d?u.audio.pause():u.audio.play())}}var WEBAudio={audioInstanceIdCounter:0,audioInstances:[],audioContext:null,audioWebEnabled:0};"
        },
          new ReplaceRule()
       {
          oldStr=": *_JS_Sound_Create_Channel",
          newStr=":QG_JS_Sound_Create_Channel"
       },
       new ReplaceRule()
       {
          oldStr=": *_JS_Sound_GetLength",
          newStr=":QG_JS_Sound_GetLength"
       },new ReplaceRule()
       {
          oldStr =": *_JS_Sound_GetLoadState",
          newStr=":QG_JS_Sound_GetLoadState"
       },new ReplaceRule()
       {
          oldStr=": *_JS_Sound_Init",
          newStr=":QG_JS_Sound_Init"
       },new ReplaceRule()
       {
          oldStr=": *_JS_Sound_Load",
          newStr=":QG_JS_Sound_Load"
       },new ReplaceRule()
       {
          oldStr=": *_JS_Sound_Load_PCM",
          newStr=":QG_JS_Sound_Load_PCM"
       },new ReplaceRule()
       {
          oldStr=": *_JS_Sound_Play",
          newStr=":QG_JS_Sound_Play"
       },new ReplaceRule()
       {
          oldStr=": *_JS_Sound_ReleaseInstance",
          newStr=":QG_JS_Sound_ReleaseInstance"
       }, new ReplaceRule()
       {
          oldStr=": *_JS_Sound_ResumeIfNeeded",
          newStr=":QG_JS_Sound_ResumeIfNeeded"
       },new ReplaceRule()
       {
          oldStr=": *_JS_Sound_Set3D",
          newStr=":QG_JS_Sound_Set3D"
       },new ReplaceRule()
       {
          oldStr=": *_JS_Sound_SetListenerOrientation",
          newStr=":QG_JS_Sound_SetListenerOrientation"
       },new ReplaceRule()
       {
           oldStr=": *_JS_Sound_SetListenerPosition",
          newStr=":QG_JS_Sound_SetListenerPosition"
       },
       new ReplaceRule()
       {
          oldStr=": *_JS_Sound_SetLoop",
          newStr=":QG_JS_Sound_SetLoop"
       },new ReplaceRule()
       {
          oldStr=": *_JS_Sound_SetLoopPoints",
          newStr=":QG_JS_Sound_SetLoopPoints"
       },

         new ReplaceRule()
       {
          oldStr=": *_JS_Sound_SetPitch",
          newStr=":QG_JS_Sound_SetPitch"
       },new ReplaceRule()
       {
          oldStr=": *_JS_Sound_SetPosition",
          newStr=":QG_JS_Sound_SetPosition"
       },new ReplaceRule()
       {
          oldStr=": *_JS_Sound_SetVolume",
          newStr=":QG_JS_Sound_SetVolume"
       },new ReplaceRule()
       {
          oldStr=": *_JS_Sound_Stop",
          newStr=":QG_JS_Sound_Stop"
       },new ReplaceRule()
       {
          oldStr=": *_JS_Sound_SetPaused",
          newStr=":QG_JS_Sound_SetPaused"
       }

#if UNITY_2020_1_OR_NEWER
       ,new ReplaceRule()
       {
         oldStr="Module.SystemInfo",
         newStr="UnityLoader.SystemInfo"
       },new ReplaceRule()
       {
         oldStr="function unityFramework",
         newStr="var UnityModule = function unityFramework"
       },new ReplaceRule()
       {
         oldStr="Module.streamingAssetsUrl",
         newStr="Module.streamingAssetsUrl()"
       }
#endif

#if UNITY_2021_1_OR_NEWER
       ,new ReplaceRule()
       {
         oldStr=": *_JS_ScreenOrientation_DeInit",
         newStr=":QG_JS_ScreenOrientation_DeInit"
       },new ReplaceRule()
       {
         oldStr=": *_JS_ScreenOrientation_Init",
         newStr=":QG_JS_ScreenOrientation_Init"
       },new ReplaceRule()
       {
         oldStr=": *_JS_ScreenOrientation_Lock",
         newStr=":QG_JS_ScreenOrientation_Lock"
       }
       ,new ReplaceRule()
       {
         oldStr="var JS_ScreenOrientation_callback=0;",
         newStr="function QG_JS_ScreenOrientation_DeInit(){}function QG_JS_ScreenOrientation_Init(n){}function QG_JS_ScreenOrientation_Lock(n){}var JS_ScreenOrientation_callback=0;"
       },new ReplaceRule()
       {
         oldStr=@"var domElement=specialHTMLTargets[\s\S]*?:undefined\);",
         newStr="var domElement=Module[\"canvas\"];"
       },new ReplaceRule()
       {
         oldStr=@"function *getRandomDevice *\( *\) *{",
         newStr="function getRandomDevice(){ if(window[\"qg\"])return function () {return Math.random() * 256 | 0};"
       },new ReplaceRule()
       {
         oldStr="var abortController=new AbortController;",
         newStr="var abortController=new XMLHttpRequest;abortController.open(_method,_url,true);"
       },new ReplaceRule()
       {
         oldStr="var kWebRequestOK=0",
         newStr="var body=new Uint8Array(body);wrProgress={total:body.length,loaded:body.length,chunk:body};HandleProgress(response,wrProgress);var kWebRequestOK=0;"
       },new ReplaceRule()
       {
         oldStr=@"var fetchImpl=Module.fetchWithProgress;[\s\S]*?kWebErrorUnknown\)}}\)",
         newStr="wr.responses[requestId] = abortController;requestOptions.timeout&&requestOptions.timeout>=0&&(abortController.timeout=requestOptions.timeout);Object.keys(requestOptions.init.headers).map(r=>{abortController.setRequestHeader(r,requestOptions.init.headers[r])}),abortController.responseType=\"arraybuffer\",abortController.onload=function(){abortController.status<400?HandleSuccess(abortController,abortController.response):(HandleError(abortController.statusText,abortController.status),window.UnityLoader.fprintError(\" xml request error msg = \"+abortController.statusText+\" code = \"+abortController.status))},abortController.onerror=function(){HandleError(abortController.statusText?abortController.statusText:\"\",abortController.status)},postData?abortController.send(postData):abortController.send();"
       },new ReplaceRule()
       {
         oldStr=@"function HandleProgress[\s\S]*?e.loaded,e.total,0,0]\)}}",
         newStr="function HandleProgress(e,i){if(!onprogress) {return;};if(i.chunk){var n=getTempBuffer(i.chunk.length);HEAPU8.set(i.chunk,n),dynCall(\"viiiiii\",onprogress,[arg,e.status,i.loaded,i.total,n,i.chunk.length])}else dynCall(\"viiiiii\",onprogress,[arg,e.status,i.loaded,i.total,0,0])}"
       },new ReplaceRule()
       {
         oldStr=@"function jsWebRequestGetResponseHeaderString[\s\S]*?return headers",
         newStr="function jsWebRequestGetResponseHeaderString(e){var r=wr.responses[e];return r?r.headerString?r.headerString:r.getAllResponseHeaders()?r.getAllResponseHeaders():\"\":\"\""
       },new ReplaceRule()
       {
         oldStr=@"if\(length\>0\)[\s\S]*?body=new Blob\(\[postData\]\)}",
         newStr="var postData;length>0&&(postData=HEAPU8.subarray(ptr,ptr+length));"
       },new ReplaceRule()
       {
         oldStr="\\|\\|abortController.signal.aborted",
         newStr=""
       }
#elif TUANJIE_2022_3_OR_NEWER
        ,new ReplaceRule()
       {
         oldStr=": *_JS_ScreenOrientation_DeInit",
         newStr=":QG_JS_ScreenOrientation_DeInit"
       },new ReplaceRule()
       {
         oldStr=": *_JS_ScreenOrientation_Init",
         newStr=":QG_JS_ScreenOrientation_Init"
       },new ReplaceRule()
       {
         oldStr=": *_JS_ScreenOrientation_Lock",
         newStr=":QG_JS_ScreenOrientation_Lock"
       }
       ,new ReplaceRule()
       {
         oldStr="var JS_ScreenOrientation_callback=0;",
         newStr="function QG_JS_ScreenOrientation_DeInit(){}function QG_JS_ScreenOrientation_Init(n){}function QG_JS_ScreenOrientation_Lock(n){}var JS_ScreenOrientation_callback=0;"
       },new ReplaceRule()
       {
         oldStr=@"var domElement=specialHTMLTargets[\s\S]*?:undefined\);",
         newStr="var domElement=Module[\"canvas\"];"
       },new ReplaceRule()
       {
         oldStr=@"function *getRandomDevice *\( *\) *{",
         newStr="function getRandomDevice(){ if(window[\"qg\"])return function () {return Math.random() * 256 | 0};"
       },new ReplaceRule()
       {
         oldStr="var abortController=new AbortController;",
         newStr="var abortController=new XMLHttpRequest;abortController.open(_method,_url,true);"
       },new ReplaceRule()
       {
         oldStr="var kWebRequestOK=0",
         newStr="var body=new Uint8Array(body);wrProgress={total:body.length,loaded:body.length,chunk:body};HandleProgress(response,wrProgress);var kWebRequestOK=0;"
       },new ReplaceRule()
       {
         oldStr=@"var fetchImpl=Module.fetchWithProgress;[\s\S]*?kWebErrorUnknown\)}}\)",
         newStr="wr.responses[requestId] = abortController;requestOptions.timeout&&requestOptions.timeout>=0&&(abortController.timeout=requestOptions.timeout);Object.keys(requestOptions.init.headers).map(r=>{abortController.setRequestHeader(r,requestOptions.init.headers[r])}),abortController.responseType=\"arraybuffer\",abortController.onload=function(){abortController.status<400?HandleSuccess(abortController,abortController.response):(HandleError(abortController.statusText,abortController.status),window.UnityLoader.fprintError(\" xml request error msg = \"+abortController.statusText+\" code = \"+abortController.status))},abortController.onerror=function(){HandleError(abortController.statusText?abortController.statusText:\"\",abortController.status)},postData?abortController.send(postData):abortController.send();"
       },new ReplaceRule()
       {
         oldStr=@"function HandleProgress[\s\S]*?e.loaded,e.total,0,0]\)}}",
         newStr="function HandleProgress(e,i){if(!onprogress) {return;}if(i.chunk){var n=getTempBuffer(i.chunk.length);HEAPU8.set(i.chunk,n),dynCall(\"viiiiii\",onprogress,[arg,e.status,i.loaded,i.total,n,i.chunk.length])}else dynCall(\"viiiiii\",onprogress,[arg,e.status,i.loaded,i.total,0,0])}"
       },new ReplaceRule()
       {
         oldStr=@"function jsWebRequestGetResponseHeaderString[\s\S]*?return headers",
         newStr="function jsWebRequestGetResponseHeaderString(e){var r=wr.responses[e];return r?r.headerString?r.headerString:r.getAllResponseHeaders()?r.getAllResponseHeaders():\"\":\"\""
       },new ReplaceRule()
       {
         oldStr=@"if\(length\>0\)[\s\S]*?body=new Blob\(\[postData\]\)}",
         newStr="var postData;length>0&&(postData=HEAPU8.subarray(ptr,ptr+length));"
       },new ReplaceRule()
       {
         oldStr="\\|\\|abortController.signal.aborted",
         newStr=""
       }
#endif

#if UNITY_2022
       ,new ReplaceRule()
       {
         oldStr="unityFramework",
         newStr="UnityModule"
       }
#endif
        };
    }
}
