using System;
using NUnit.Framework;
using ObservableVariables.Contract;

namespace ObservableVariables.EditorTests
{
    public class AdditionalFunctions
    {
        [Test]
        public void ObservableVariableEvents()
        {
            bool valueStoredCallback1 = false;
            bool valueStoredCallback2 = false;
            bool valueRemovedCallback1 = false;
            bool valueRemovedCallback2 = false;
            var testData = new TestDataClass();
            testData.ValueStored = () =>
            {
                valueStoredCallback1 = true;
                valueStoredCallback2 = false;
                valueRemovedCallback1 = false;
                valueRemovedCallback2 = false;

                testData.ValueStored = () =>
                {
                    valueStoredCallback2 = true;
                    valueRemovedCallback2 = false;
                };
            };
            testData.ValueRemoved = () =>
            {
                valueRemovedCallback1 = true;
                valueStoredCallback2 = false;
                valueRemovedCallback2 = false;
                
                testData.ValueRemoved = () =>
                {
                    valueRemovedCallback2 = true;
                };
            };
            
            ObservableTestDataClass observableVariable = new();
            observableVariable.Value = testData;
            observableVariable.Value = null;
            observableVariable.Value = testData;
            observableVariable.Value = null;
            
            Assert.IsTrue(valueStoredCallback1 && valueRemovedCallback1 && valueStoredCallback2 && valueRemovedCallback2);
        }

        private class TestDataClass : IObservableVariableEventHolder<ObservableTestDataClass>
        {
            public Action ValueStored;
            public Action ValueRemoved;
            
            public void OnDataStoredInObservableVariable(ObservableTestDataClass variable)
            {
                ValueStored?.Invoke();
            }

            public void OnDataRemovedObservableVariable(ObservableTestDataClass variable)
            {
                ValueRemoved?.Invoke();
            }
        }
        
        private class ObservableTestDataClass : ObservableVariableBase<TestDataClass>{}
    }
}