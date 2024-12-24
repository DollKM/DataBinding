Json部分的解析使用，需要导入Newtonsoft.JSON包
并且在项目初始化时使用：
```csharp
JsonConvert.DefaultSettings = () => new JsonSerializerSettings
{
    Converters = new List<JsonConverter> {
            new DataConverter(),
        },
    ContractResolver = new ReadOnlyPropertyContractResolver()
};
```
