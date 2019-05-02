using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

using Newtonsoft.Json;

public class JsBridge : MonoBehaviour
{
    public class Packet
    {
        public int pid = 0;
    }
    public class Js2UnityPacket : Packet
    {
        public string function;
        public string message;
    }
    public class Unity2JsPacket : Packet
    {
        public string message;
    }

    [DllImport("__Internal")]
    private static extern void JsReturn(int pid, string msg);

    private static Dictionary<string, Func<int, string, Unity2JsPacket>> functions 
        = new Dictionary<string, Func<int, string, Unity2JsPacket>>();

    private static int pid = 0;
    private static GameObject gobj;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        gobj = new GameObject("JsBridge");
        GameObject.DontDestroyOnLoad(gobj);
    }

    public static void Add<TIn, TOut>(string name, Func<TIn, TOut> callback)
    {
        functions[name] = (int pid, string _in) =>
        {
            var json =
                JsonConvert.SerializeObject(
                    callback(JsonConvert.DeserializeObject<TIn>(_in)));
            return new Unity2JsPacket()
            {
                pid = pid,
                message = json
            };
        };
    }
    public static void Retrun(int pid, object obj)
    {
        JsReturn(pid, JsonConvert.SerializeObject(obj));
    }

    public void OnMessage(string msg)
    {
        var packet = JsonConvert.DeserializeObject<Js2UnityPacket>(msg);

        Func<int, string, object> func = null;
        if (functions.TryGetValue(packet.function, out func))
        {
            func(packet.pid, packet.message);
        }
        else
            Debug.LogError("[JsBridge] Unknown funcall: " + packet.function);
    }
}

using UnityEngine;

using Newtonsoft.Json;

public class JsBridge : MonoBehaviour
{
    public class Packet
    {
        public int pid = 0;
    }
    public class Js2UnityPacket : Packet
    {
        public string function;
        public string message;
    }
    public class Unity2JsPacket : Packet
    {
        public string message;
    }

    [DllImport("__Internal")]
    private static extern void JsReturn(int pid, string msg);

    private static Dictionary<string, Func<int, string, object>> functions 
        = new Dictionary<string, Func<int, string, object>>();

    private static int pid = 0;
    private static GameObject gobj;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        gobj = new GameObject("JsBridge");
        GameObject.DontDestroyOnLoad(gobj);
    }

    public static void Add<TIn, TOut>(string name, Func<int, TIn, TOut> callback)
    {
        functions[name] = (int pid, string _in) =>
        {
            return callback(pid, JsonConvert.DeserializeObject<TIn>(_in));
        };
    }
    public static void Retrun(int pid, object obj)
    {
        JsReturn(pid, JsonConvert.SerializeObject(obj));
    }

    public void OnMessage(string msg)
    {
        var packet = JsonConvert.DeserializeObject<Js2UnityPacket>(msg);

        Func<int, string, object> func = null;
        if (functions.TryGetValue(packet.function, out func))
        {
            func(packet.pid, packet.message);
        }
        else
            Debug.LogError("[JsBridge] Unknown funcall: " + packet.function);
    }
}
