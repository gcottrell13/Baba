using Core.Content;
using Core.Engine;
using Core.Utils;
using Editor.Saves;
using NUnit.Framework;
using Tests;

namespace EditorTests.Saves;


[TestFixture]
public class CompileMapTests
{
    [Test]
    public void Compile_ShouldWork()
    {
        var editorFormat = Newtonsoft.Json.JsonConvert.DeserializeObject<SaveFormatWorld>(WorldDataJson.saveFile)!;
        var compiledTest = CompileMap.CompileWorld(editorFormat);

        compiledTest.Inventory = new()
        {
            { ObjectTypeId.key, 10 },
            { ObjectTypeId.fish, 22 },
        };

        Assert.AreEqual(WorldDataDeserialized.expectedCompiledMap, compiledTest);
        Assert.AreEqual(WorldDataDeserialized.expectedCompiledMap.ToString(), compiledTest.ToString());
    }


}
