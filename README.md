JsBridge.unity
====

A small snippet that replaces `SendMessage` in Unity+WebGL.

Why
----
* `SendMessage` can't return a value. So there's no official way to retrive values from Unity to Javascript.
* `SendMessage` can have only one parameter. Even worse, it only accepts `int`, `string` and `null`.

Usage
----

```js
JsBridge.Call("Sum", {a: 10, b: 20}, (result) => {
  console.log("10 + 20 = " + result);
});
```

```cs
JsBridge.Add("Sum", MySum);

struct MySumReqeust {
  public int a;
  public int b;
};
private int MySum(MySumRequest p) {
  return p.a + p.b;
}
```

If you think this is infficient, try this way:
```cs
private int MySum(Dictionary<string, int> p) {
  return p["a"] + p["b"];
}
```

Since C# is a strongly typed language, you have to make some effors for it. C# is not a JS.
