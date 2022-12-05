﻿using Extism.Sdk.Native;

using System.Reflection;
using System.Text;

Console.WriteLine($"Extism version {Context.GetExtismVersion()}");
var x = Context.SetExtismLogFile("log.log", LogLevel.Trace);

using var context = new Context();

var binDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
var wasm = await File.ReadAllBytesAsync(Path.Combine(binDirectory, "code.wasm"));
using var plugin = context.CreatePlugin(wasm, withWasi: true);

Console.Write("Input: ");
var input = Console.ReadLine() ?? "";

var response = plugin.CallFunction("count_vowels", Encoding.UTF8.GetBytes(input));
if (response == 0)
{
    var output = Encoding.UTF8.GetString(plugin.OutputData());
    Console.WriteLine(output);
}
else
{
    Console.WriteLine($"Status code: {response}");
    Console.WriteLine($"Context error: {context.GetError()}");
    Console.WriteLine($"Plugin error: {plugin.GetError()}");
}