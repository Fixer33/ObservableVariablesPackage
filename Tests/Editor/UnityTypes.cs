using NUnit.Framework;
using ObservableVariables.Unity;
using UnityEngine;

namespace ObservableVariables.EditorTests
{
    public class UnityTypes
    {
        private enum VarKey
        {
            TestKey
        }
        
        [Test]
        public void Vector3ValueChangedEvent()
        {
            Vector3 targetValue = new Vector3(3.1f, 5, 0);
            ObservableVector3 vectorVar = new ObservableVector3(Vector3.zero);
            bool hasChanged = false;
            vectorVar.ValueChanged += newVal => hasChanged = newVal == targetValue;

            vectorVar.Value = targetValue;
            
            Assert.IsTrue(hasChanged);
        }
        
        [Test]
        public void Vector2ValueChangedEvent()
        {
            Vector2 targetValue = new Vector3(3.1f, 5);
            ObservableVector2 vectorVar = new ObservableVector2(Vector2.zero);
            bool hasChanged = false;
            vectorVar.ValueChanged += newVal => hasChanged = newVal == targetValue;

            vectorVar.Value = targetValue;
            
            Assert.IsTrue(hasChanged);
        }
        
        [Test]
        public void GameObjectValueChangedEvent()
        {
            GameObject targetValue = new GameObject();
            ObservableGameObject goVar = new ObservableGameObject(null);
            bool hasChanged = false;
            goVar.ValueChanged += newVal => hasChanged = newVal == targetValue;

            goVar.Value = targetValue;
            
            try
            {
                Assert.IsTrue(hasChanged);
            }
            finally
            {
                GameObject.DestroyImmediate(targetValue);
            }
        }
        
        [Test]
        public void Vector3ValueSet()
        {
            Vector3 targetValue = new Vector3(3.1f, 5, 0);
            ObservableVector3 vectorVar = new ObservableVector3(Vector3.zero);

            vectorVar.Value = targetValue;

            Assert.IsTrue(vectorVar.Value == targetValue);
        }
        
        [Test]
        public void Vector2ValueSet()
        {
            Vector2 targetValue = new Vector3(3.1f, 5);
            ObservableVector2 vectorVar = new ObservableVector2(Vector2.zero);

            vectorVar.Value = targetValue;

            Assert.IsTrue(vectorVar.Value == targetValue);
        }
        
        [Test]
        public void GameObjectValueSet()
        {
            GameObject targetValue = new GameObject();
            ObservableGameObject goVar = new ObservableGameObject(null);

            goVar.Value = targetValue;

            try
            {
                Assert.IsTrue(goVar.Value == targetValue);
            }
            finally
            {
                GameObject.DestroyImmediate(targetValue);
            }
        }
        
        [Test]
        public void Vector3ValueCast()
        {
            Vector3 targetValue = new Vector3(3.1f, 5, 0);
            ObservableVector3 vectorVar = targetValue;
            
            Assert.IsTrue(vectorVar.Value == targetValue);
        }
        
        [Test]
        public void Vector2ValueCast()
        {
            Vector2 targetValue = new Vector3(3.1f, 5);
            ObservableVector2 vectorVar = targetValue;
            
            Assert.IsTrue(vectorVar.Value == targetValue);
        }
        
        [Test]
        public void GameObjectValueCast()
        {
            GameObject targetValue = new GameObject();
            ObservableGameObject goVar = targetValue;
            
            try
            {
                Assert.IsTrue(goVar.Value == targetValue);
            }
            finally
            {
                GameObject.DestroyImmediate(targetValue);
            }
        }

        [Test]
        public void Vector2ShortcutMethod()
        {
            ObservableVector2 var1 = Variables.GetVector2(VarKey.TestKey);
            ObservableVector2 var2 = Variables.GetVector2(VarKey.TestKey);
            try
            {
                Assert.AreSame(var1, var2);
            }
            finally
            {
                Variables.Tests_Clear();
            }
        }
        
        [Test]
        public void Vector3ShortcutMethod()
        {
            ObservableVector3 var1 = Variables.GetVector3(VarKey.TestKey);
            ObservableVector3 var2 = Variables.GetVector3(VarKey.TestKey);
            try
            {
                Assert.AreSame(var1, var2);
            }
            finally
            {
                Variables.Tests_Clear();
            }
        }
        
        [Test]
        public void GameObjectShortcutMethod()
        {
            ObservableGameObject var1 = Variables.GetGameObject(VarKey.TestKey);
            ObservableGameObject var2 = Variables.GetGameObject(VarKey.TestKey);
            try
            {
                Assert.AreSame(var1, var2);
            }
            finally
            {
                Variables.Tests_Clear();
            }
        }
    }
}