using System;

[AttributeUsage(AttributeTargets.Method)]
public class Subscribe : System.Attribute
{}

[AttributeUsage(AttributeTargets.Method)]
public class SubscribeGlobal : System.Attribute
{}