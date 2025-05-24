// #define UNITY_EDITOR

using System.Reflection;
using PropertyGenerators.Sample;

namespace PropertyGenerators.Tests;

[TestClass]
public class SampleEntityTests
{
        [TestMethod]
        public void IdProperty_ReturnsDefaultValue()
        {
            var entity = new SampleEntity();
            
            var type = typeof(SampleEntity);
            var idProperty = type.GetProperty("Id");
            Assert.IsNotNull(idProperty, "Id property should exist.");
            var value = idProperty.GetValue(entity);
            Assert.AreEqual(0, value, "Id property should return the default value of 0.");
        }

#if UNITY_EDITOR
        [TestMethod]
        public void NameProperty_GetterAndSetter_WorkCorrectly()
        {
            var entity = new SampleEntity();
            var type = typeof(SampleEntity);
            var nameProp = type.GetProperty("Name");
            Assert.IsNotNull(nameProp, "Name property should exist in UNITY_EDITOR mode.");
            
            // Check initial value
            var initial = nameProp.GetValue(entity) as string;
            Assert.AreEqual("Sample", initial, "Name property should initially be 'Sample'.");

            // Set new value if writable
            if (nameProp.CanWrite)
            {
                nameProp.SetValue(entity, "NewSample");
                var updated = nameProp.GetValue(entity) as string;
                Assert.AreEqual("NewSample", updated, "Name property should update to 'NewSample'.");
            }
            else
            {
                Assert.Fail("Name property should be writable in UNITY_EDITOR mode.");
            }
        }

        [TestMethod]
        public void NameAsdProperty_GetterAndSetter_WorkCorrectly()
        {
            var entity = new SampleEntity();
            var type = typeof(SampleEntity);
            var nameAsdProp = type.GetProperty("NameAsd");
            Assert.IsNotNull(nameAsdProp, "NameAsd property should exist in UNITY_EDITOR mode.");
            
            var initial = nameAsdProp.GetValue(entity) as string;
            Assert.AreEqual("Asd", initial, "NameAsd property should initially be 'Asd'.");

            if (nameAsdProp.CanWrite)
            {
                nameAsdProp.SetValue(entity, "NewAsd");
                var updated = nameAsdProp.GetValue(entity) as string;
                Assert.AreEqual("NewAsd", updated, "NameAsd property should update to 'NewAsd'.");
            }
            else
            {
                Assert.Fail("NameAsd property should be writable in UNITY_EDITOR mode.");
            }
        }

        [TestMethod]
        public void NameAsdfProperty_InEditorMode_IsWritable()
        {
            var entity = new SampleEntity();
            var type = typeof(SampleEntity);
            var nameAsdfProp = type.GetProperty("NameAsdf");
            Assert.IsNotNull(nameAsdfProp, "NameAsdf property should exist in UNITY_EDITOR mode.");
            
            var initial = nameAsdfProp.GetValue(entity) as string;
            Assert.AreEqual("Asdf", initial, "NameAsdf property should initially be 'Asdf'.");

            if (nameAsdfProp.CanWrite)
            {
                nameAsdfProp.SetValue(entity, "NewAsdf");
                var updated = nameAsdfProp.GetValue(entity) as string;
                Assert.AreEqual("NewAsdf", updated, "NameAsdf property should update to 'NewAsdf' in editor mode.");
            }
            else
            {
                Assert.Fail("NameAsdf property should be writable in UNITY_EDITOR mode.");
            }
        }
#else
        [TestMethod]
        public void NameAndNameAsdProperties_AreNotAvailableInNonEditorMode()
        {
            var type = typeof(SampleEntity);
            var nameProp = type.GetProperty("Name");
            var nameAsdProp = type.GetProperty("NameAsd");
            Assert.IsNull(nameProp, "Name property should not exist in non-editor mode.");
            Assert.IsNull(nameAsdProp, "NameAsd property should not exist in non-editor mode.");
        }

        [TestMethod]
        public void NameAsdfProperty_InNonEditorMode_IsReadOnly()
        {
            var entity = new SampleEntity();
            var type = typeof(SampleEntity);
            var nameAsdfProp = type.GetProperty("NameAsdf");
            Assert.IsNotNull(nameAsdfProp, "NameAsdf property should exist in non-editor mode.");
            
            var initial = nameAsdfProp.GetValue(entity) as string;
            Assert.AreEqual("Asdf", initial, "NameAsdf should initially be 'Asdf' in non-editor mode.");
            Assert.IsFalse(nameAsdfProp.CanWrite, "NameAsdf property should be read-only in non-editor mode.");
        }
#endif
}