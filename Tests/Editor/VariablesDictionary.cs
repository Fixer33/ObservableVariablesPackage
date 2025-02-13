using System.ComponentModel;
using NUnit.Framework;
using ObservableVariables.BasicTypes;
using ObservableVariables.Unity;
using UnityEngine;

namespace ObservableVariables.EditorTests
{
    public class VariablesDictionary
    {
        [Test]
        public void CreatingNewValueVariableAndRetrievingIt()
        {
            var variable = Variables.Get<ObservableBool>(VarKey.TestKey1);

            try
            {
                Assert.NotNull(variable);
            }
            finally
            {
                Variables.Tests_Clear();
            }
        }
        
        [Test]
        public void CreatingNewReferenceVariableAndRetrievingIt()
        {
            var variable = Variables.Get<ObservableGameObject>(VarKey.TestKey1);
            
            try
            {
                Assert.NotNull(variable);
            }
            finally
            {
                Variables.Tests_Clear();
            }
        }
        
        [Test]
        public void CreateNewValueVariableAndSetValue()
        {
            const float targetVal = 5.3f;
            var variable = Variables.Get<ObservableFloat>(VarKey.TestKey1);
            variable.Value = targetVal;

            try
            {
                Assert.NotNull(variable.Value == targetVal);
            }
            finally
            {
                Variables.Tests_Clear();
            }
        }
        
        [Test]
        public void CreateNewReferenceVariableAndSetValue()
        {
            GameObject go = new GameObject();
            var variable = Variables.Get<ObservableGameObject>(VarKey.TestKey1);
            variable.Value = go;

            try
            {
                Assert.IsTrue(go == variable.Value);
            }
            finally
            {
                Object.DestroyImmediate(go);
                Variables.Tests_Clear();
            }
        }
        
        [Test]
        public void RetrievingCorrectValuesForDifferentKeys()
        {
            const int targetVal = 33;
            
            var variable1 = Variables.Get<ObservableInt>(VarKey.TestKey1);
            var variable2 = Variables.Get<ObservableInt>(VarKey.TestKey2);
            variable1.Value = targetVal;

            try
            {
                Assert.IsTrue(variable1.Value == targetVal && variable2.Value == 0);
            }
            finally
            {
                Variables.Tests_Clear();
            }
        }
        
        [Test]
        public void WrongVariableTypeForKey_ThrowsException()
        {
            var variableRef1 = Variables.Get<ObservableInt>(VarKey.TestKey1);

            try
            {
                Assert.Throws<InvalidEnumArgumentException>(() => Variables.Get<ObservableFloat>(VarKey.TestKey1));
            }
            finally
            {
                Variables.Tests_Clear();
            }
        }
        
        [Test]
        public void RetrievingSameValueVariableForSameKey()
        {
            const int targetVal = 33;
            
            var variableRef1 = Variables.Get<ObservableInt>(VarKey.TestKey1);
            variableRef1.Value = targetVal;
            var variableRef2 = Variables.Get<ObservableInt>(VarKey.TestKey1);

            try
            {
                Assert.IsTrue(variableRef2.Value == targetVal);
            }
            finally
            {
                Variables.Tests_Clear();
            }
        }
        
        [Test]
        public void RetrievingSameReferenceVariableForSameKey()
        {
            GameObject targetVal = new GameObject();
            
            var variableRef1 = Variables.Get<ObservableGameObject>(VarKey.TestKey1);
            variableRef1.Value = targetVal;
            var variableRef2 = Variables.Get<ObservableGameObject>(VarKey.TestKey1);

            try
            {
                Assert.IsTrue(variableRef2.Value == targetVal);
            }
            finally
            {
                Variables.Tests_Clear();
            }
        }

        private enum VarKey
        {
            TestKey1,
            TestKey2,
        }
    }
}