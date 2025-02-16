using NUnit.Framework;
using ObservableVariables.BasicTypes;

namespace ObservableVariables.EditorTests
{
    public class BasicTypes
    {
        [Test]
        public void FloatValueChangedEvent()
        {
            const float targetValue = 5.3f;
            ObservableFloat floatVar = new ObservableFloat(0);
            bool hasChanged = false;
            floatVar.ValueChanged += newVal => hasChanged = newVal == targetValue;

            floatVar.Value = targetValue;
            
            Assert.IsTrue(hasChanged);
        }
        
        [Test]
        public void IntValueChangedEvent()
        {
            const int targetValue = 3;
            ObservableInt intVar = new ObservableInt(0);
            bool hasChanged = false;
            intVar.ValueChanged += newVal => hasChanged = newVal == targetValue;

            intVar.Value = targetValue;
            
            Assert.IsTrue(hasChanged);
        }
        
        [Test]
        public void BoolValueChangedEvent()
        {
            const bool targetValue = true;
            ObservableBool boolVar = new ObservableBool(false);
            bool hasChanged = false;
            boolVar.ValueChanged += newVal => hasChanged = newVal == targetValue;

            boolVar.Value = targetValue;
            
            Assert.IsTrue(hasChanged);
        }
        
        [Test]
        public void StringValueChangedEvent()
        {
            const string targetValue = "target";
            ObservableString stringVar = new ObservableString(string.Empty);
            bool hasChanged = false;
            stringVar.ValueChanged += newVal => hasChanged = newVal == targetValue;

            stringVar.Value = targetValue;
            
            Assert.IsTrue(hasChanged);
        }
        
        [Test]
        public void FloatValueSet()
        {
            const float targetValue = 5.3f;
            ObservableFloat floatVar = new ObservableFloat(0);

            floatVar.Value = targetValue;

            Assert.AreEqual(floatVar.Value, targetValue);
        }
        
        [Test]
        public void IntValueSet()
        {
            const int targetValue = 33;
            ObservableInt intVar = new ObservableInt(0);

            intVar.Value = targetValue;

            Assert.AreEqual(intVar.Value, targetValue);
        }
        
        [Test]
        public void BoolValueSet()
        {
            const bool targetValue = true;
            ObservableBool boolVar = new ObservableBool(false);

            boolVar.Value = targetValue;

            Assert.AreEqual(boolVar.Value, targetValue);
        }
        
        [Test]
        public void StringValueSet()
        {
            const string targetValue = "target";
            ObservableString stringVar = new ObservableString(string.Empty);

            stringVar.Value = targetValue;

            Assert.AreEqual(stringVar.Value, targetValue);
        }
        
        [Test]
        public void FloatValueCast()
        {
            const float targetValue = 5.3f;
            ObservableFloat floatVar = targetValue;
            Assert.AreEqual(floatVar.Value, targetValue);
        }
        
        [Test]
        public void IntValueCast()
        {
            const int targetValue = 33;
            ObservableInt intVar = targetValue;
            Assert.AreEqual(intVar.Value, targetValue);
        }
        
        [Test]
        public void BoolValueCast()
        {
            const bool targetValue = true;
            ObservableBool boolVar = targetValue;
            Assert.AreEqual(boolVar.Value, targetValue);
        }
        
        [Test]
        public void StringValueCast()
        {
            const string targetValue = "target";
            ObservableString stringVar = targetValue;
            Assert.AreEqual(stringVar.Value, targetValue);
        }
    }
}